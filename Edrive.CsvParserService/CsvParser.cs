using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Edrive.CsvParserService
{
	public partial class CsvParser : ServiceBase
	{
		private CancellationTokenSource _tokenSource;
		private readonly Timer _timer;
		private readonly Timer _carfaxTimer;
		private readonly Stopwatch _timePassed;
		private Dictionary<string, string> _csvFilePath;
		private Task _nadaTask = null;
		private Task _productsTask = null;
		private Task _checkFolderTask = null;
		private Task _archiveFilesTask = null;
		private readonly ServiceManager _manager;

		public CsvParser()
		{
			InitializeComponent();

			this.CanStop = true;
			this.CanPauseAndContinue = true;
			this.AutoLog = true;

			_manager = new ServiceManager();
			_csvFilePath = new Dictionary<string, string>();
			_tokenSource = new CancellationTokenSource();
			_timePassed = new Stopwatch();
			_timer = new Timer(Settings.Default.TimerIntervalMilliseconds);
			_timer.Elapsed += TimerOnElapsed;
			_carfaxTimer = new Timer(Settings.Default.CarfaxTimerIntervalMilliseconds);
			_carfaxTimer.Elapsed += CarfaxTimerOnElapsed;
		}

		private void CarfaxTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			//Check if Carfax has uploaded any files and save to database.
			Task.Factory.StartNew(Helpers.DownloadCarfaxData);
		}

		private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			_timePassed.Start();

			CheckTimePassed();//Check if it's taking too long. Cancel if true.
			
			try
			{
				//Wait until NADA updates the average prices for products.
				//Wait until products are updated from csv
				if(!(IsNadaTaskFinished() && IsProductsTaskFinished()))
					return;

				if(!IsArchiveFilesTaskFinished())//Check if archiving csv files has finished. If not wait.
					return;

				if(!IsCheckFolderTaskFinished())//check if unzipping csv files finished. If not wait. 
					return;
				
				_tokenSource = new CancellationTokenSource();
				
				_checkFolderTask = new Task(CheckFolders);
				_checkFolderTask.Start();
				_checkFolderTask.Wait();

				if(_csvFilePath.Count == 0) //return if no files detected.
					return;
				
				if(IsProductsTaskFinished() && IsNadaTaskFinished())
				{
					SaveNewProductDetails();
				}

				if(_productsTask != null)
					if(_productsTask.IsCompleted || _productsTask.IsCanceled || _productsTask.IsFaulted)
					{
						Logger.WriteLog("Start archiving files...");
						
						if(_csvFilePath.Count == 0)
							Logger.WriteLog("No files to archive.");

						_archiveFilesTask = new Task(() => ArchiveFiles(_csvFilePath));
						_archiveFilesTask.Start();
						_archiveFilesTask.Wait();
					}
			}
			catch(Exception ex)
			{
				Logger.WriteLog("Error Occurred:\n{0}", ex.Message);
				Logger.WriteLog("Stack Trace\n{0}", ex.StackTrace);
			}
			finally
			{
				_timePassed.Reset();
				
				if(_csvFilePath.Count > 0 && IsNadaTaskFinished() && IsProductsTaskFinished())
					Logger.WriteLog("Finished Parsing.");
			}
		}

		protected override void OnStart(string[] args)
		{
			_timer.Start();
			_carfaxTimer.Start();
			Logger.WriteLog(String.Format("------------Service started at: {0}--------------", DateTime.Now));
		}

		protected override void OnStop()
		{
			_timer.Stop();
			_carfaxTimer.Stop();
			Logger.WriteLog(String.Format("------------Service stopped at: {0}--------------", DateTime.Now));
		}

		#region Private Methods

		private bool IsNadaTaskFinished()
		{
			bool result = true;

			if(_nadaTask != null)
				if(!_nadaTask.IsCompleted && !_nadaTask.IsCanceled && !_nadaTask.IsFaulted)
					result = false;

			return result;
		}

		private bool IsProductsTaskFinished()
		{
			bool result = true;

			if(_productsTask != null)
				if(!_productsTask.IsCompleted && !_productsTask.IsCanceled && !_productsTask.IsFaulted)
					result = false;

			return result;
		}

		private bool IsCheckFolderTaskFinished()
		{
			bool result = true;

			if(_checkFolderTask != null)
				if(!_checkFolderTask.IsCompleted && !_checkFolderTask.IsCanceled && !_checkFolderTask.IsFaulted)
					result = false;

			return result;
		}

		private bool IsArchiveFilesTaskFinished()
		{
			bool result = true;

			if(_archiveFilesTask != null)
				if(!_archiveFilesTask.IsCompleted && !_archiveFilesTask.IsCanceled && !_archiveFilesTask.IsFaulted)
					result = false;

			return result;
		}

		private void CheckTimePassed()
		{
			if(_timePassed.ElapsedMilliseconds == Settings.Default.CancelTaskAfterMilliseconds)
			{
				try
				{
					_tokenSource.Cancel(true);//cancel all tasks
					Logger.WriteLog("Started cancelling tasks...");
				}
				catch(Exception ex)
				{
					Logger.WriteLog("Tasks cancellation exception: {0}", ex.Message);
				}
				finally
				{
					Logger.WriteLog("Tasks force stopped after not responding for {0} minutes", Settings.Default.CancelTaskAfterMilliseconds / 60000);

					_timePassed.Reset();

					ArchiveFiles(_csvFilePath);
				}
			}
		}

		private void ArchiveFiles(Dictionary<string, string> csvFilePath)
		{
			try
			{
				foreach (var i in csvFilePath)
				{
					try
					{
						string backupFile = Path.Combine(i.Value, new FileInfo(i.Key).Name);
						string zipFileName = backupFile.Replace(".csv", ".zip");

						Helpers.SetFileAcccessToEveryone(i.Key);
						File.Move(i.Key, backupFile);

						if (File.Exists(zipFileName))
						{
							Helpers.SetFileAcccessToEveryone(zipFileName);
							File.Delete(zipFileName);
						}
						Helpers.SetFileAcccessToEveryone(backupFile);
						Helpers.ZipFile(backupFile, zipFileName);
						File.Delete(backupFile);
					}
					catch (Exception ex)
					{
						Logger.WriteLog("Error occurred while archiving the file: {0}", i);
						Logger.WriteLog("Archiving Error Message: {0}", ex.Message);
						Logger.WriteLog("Archiving Stack Trace: {0}", ex.StackTrace);
					}
				}
			}
			finally
			{
				_csvFilePath = new Dictionary<string, string>();
			}
		}

		private void SaveNadaQualified(CancellationToken cancellationToken)
		{
			if(cancellationToken.IsCancellationRequested)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}
			
			try
			{
				_manager.UpdateFromNada();
				Logger.WriteLog("Finished Checking NADA prices.");
			}
			catch
			{
				Logger.WriteLog("Error occurred while checking the NADA prices.");
			}
		}
		
		private void CheckFolders()
		{
			var ftpDirectories = _manager.GetFtpFolders();

			foreach(var d in ftpDirectories)
			{
				if(!Directory.Exists(d.PATH))
				{
					Logger.WriteLog("No such folder exists: '{0}'", d.PATH);
					continue;
				}

				PrepareCsvFiles(_csvFilePath, d.PATH);
			}
		}

		private void PrepareCsvFiles(Dictionary<string, string> csvFilePath, string directoryPath)
		{
			var fileList = Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".zip") || s.EndsWith(".csv")).ToList();

			if(!fileList.Any()) //no file to grab
			{
				//Logger.WriteLog("No files under the folder: '{0}'", directoryPath);
				return;
			}

			foreach(var file in fileList)
			{
				string unzippedFileName = Path.Combine(Path.GetDirectoryName(file), string.Format("vehicle-data-{0}{1}", DateTime.Now.ToString("yyyyMMdd"), ".csv"));

				if(Path.GetExtension(file).ToLower() == ".zip")
				{
					try
					{
						Helpers.UnzipFile(directoryPath, new FileInfo(file).Name, unzippedFileName);
						File.Delete(file);
					}
					catch(Exception ex)
					{
						Logger.WriteLog("Error occurred while unzipping the file: {0}", file);
						Logger.WriteLog("Upzipping Error Message: {0}", ex.Message);
						Logger.WriteLog("Upzipping Stack Trace: {0}", ex.StackTrace);
					}
				}
				else
				{
					try
					{
						Helpers.SetFileAcccessToEveryone(unzippedFileName);//set the access rights
						File.Move(file, unzippedFileName);
					}
					catch(Exception ex)
					{
						Logger.WriteLog("Error occurred while moving the file: {0}", file);
						Logger.WriteLog("Moving File Error Message: {0}", ex.Message);
						Logger.WriteLog("Moving File Stack Trace: {0}", ex.StackTrace);
					}
				}

				Helpers.SetFileAcccessToEveryone(unzippedFileName);//reset the access rights
				string backupFolder = Path.Combine(directoryPath, "bak");

				csvFilePath.Add(unzippedFileName, backupFolder);

				if(!Directory.Exists(backupFolder))
					Directory.CreateDirectory(backupFolder);
			}
		}

		private void SaveNewProductDetails()
		{
			try
			{
				int totalCount = 0;

				Logger.WriteLog("Starting the procedure...");
				_productsTask = new Task(() => totalCount = _manager.GetProductsToUpdate());
				_productsTask.Start();
				_productsTask.Wait();

				if(totalCount > 0)
				{
					Logger.WriteLog("{0} new Products returned by the Procedure", totalCount);

					Task.Factory.StartNew(_manager.SaveCarfaxDealersFile); //Dealer's file is saved and uploaded to Carfax
					Task.Factory.StartNew(_manager.SaveCarfaxVehiclesFile); //Vehicles' file is saved and uploaded to Carfax
					_nadaTask = Task.Factory.StartNew(() => SaveNadaQualified(_tokenSource.Token), _tokenSource.Token);
				}
				else
				{
					Logger.WriteLog("No new vehicles detected.");
				}
			}
			catch(Exception ex)
			{
				//throws exception when no vehicles returned
				Logger.WriteLog("Procedure returned no new products.");
				Logger.WriteLog("Error occurred while saving product details: {0}", ex.Message);
				Logger.WriteLog("Stack Trace: {0}", ex.StackTrace);
			}
		}

		#endregion
	}
}
