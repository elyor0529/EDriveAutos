using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace Edrive.CsvParserService
{
	public class Helpers
	{
		/// <summary>
		/// Unzip the zip file
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="zipFileName"></param>
		/// <param name="unzippedFileName"> </param>
		/// <returns></returns>
		public static void UnzipFile(string filePath, string zipFileName, string unzippedFileName)
		{
			using(var s = new ZipInputStream(File.OpenRead(Path.Combine(filePath, zipFileName))))
			{
				ZipEntry zipEntry;
				while((zipEntry = s.GetNextEntry()) != null)
				{
					string directoryName = Path.GetDirectoryName(zipEntry.Name);
					string fileName = Path.GetFileName(zipEntry.Name);

					if(!string.IsNullOrEmpty(directoryName))
					{
						Directory.CreateDirectory(Path.Combine(filePath, directoryName));
					}

					if(fileName != String.Empty)
					{
						FileStream streamWriter = null;

						try
						{
							streamWriter = File.Create(unzippedFileName);

							int size = 2048;
							byte[] data = new byte[size];
							while(true)
							{
								size = s.Read(data, 0, data.Length);
								if(size > 0)
								{
									streamWriter.Write(data, 0, size);
								}
								else
									break;
							}
						}
						finally
						{
							if(streamWriter != null)
							{
								streamWriter.Flush();
								streamWriter.Close();
								streamWriter.Dispose();
							}
						}
					}
				}
			}
		}

		public static void ZipFile(string srcFile, string destinationFile)
		{
			using(FileStream fileStreamIn = new FileStream(srcFile, FileMode.Open, FileAccess.Read))
			using(FileStream fileStreamOut = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
			using(ZipOutputStream zipOutStream = new ZipOutputStream(fileStreamOut))
			{
				int size = 2048;
				byte[] buffer = new byte[size];
				ZipEntry entry = new ZipEntry(Path.GetFileName(srcFile));
				zipOutStream.PutNextEntry(entry);
				
				do
				{
					size = fileStreamIn.Read(buffer, 0, buffer.Length);
					zipOutStream.Write(buffer, 0, size);
				} 
				while (size > 0);
			}
		}

		public static void CreateCarfaxCsvFile(IEnumerable<string> data, string fileName, int operationType)
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				string filePath = Path.Combine(Settings.Default.CarfaxPath, fileName);
				
				if(File.Exists(filePath))
					File.Delete(filePath);

				using(StreamWriter sw = new StreamWriter(filePath, false))
				{
					foreach (var item in data)
					{
						sb.Append(item);
						sb.Append(Environment.NewLine);
					}

					sw.Write(sb.ToString());
				}

				Logger.WriteLog("Start uploading to CARFAX file: {0}", fileName);
				UploadCarfaxData(filePath, operationType);
			}
			catch(Exception ex)
			{
				Logger.WriteLog("Error occurred while creating a carfax csv file: {0}", ex.Message);
			}
		}

		public static void UploadCarfaxData(string filePath, int operationType)
		{
			//FTPAddress = "ftp://173.248.133.242/files/CARFAX_Testing";
			//username = "edrive_henisha";
			//password = "edrive2324";

			const string ftpAddress = "ftp://ftp.carfax.com";
			const string username = "EDRIVEAUTO";
			const string password = "8883163374";

			using(FileStream stream = File.OpenRead(filePath))
			{
				ServiceManager manager = new ServiceManager();
				Stream reqStream = null;
				string message;

				try
				{
					FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpAddress + "/" + Path.GetFileName(filePath));

					request.Method = WebRequestMethods.Ftp.UploadFile;
					request.Credentials = new NetworkCredential(username, password);
					request.UsePassive = true;
					request.UseBinary = true;
					request.KeepAlive = false;
					
					byte[] buffer = new byte[stream.Length];
					stream.Read(buffer, 0, buffer.Length);
					reqStream = request.GetRequestStream();
					reqStream.Write(buffer, 0, buffer.Length);
					message = "File Uploaded Successfully.";
					manager.InsertCarfaxLog(message, DateTime.Now, operationType, 1, 0);
					Logger.WriteLog("Successfully uploaded the CARFAX File: {0}", filePath);
				}
				catch(Exception ex)
				{
					message = "error";
					manager.InsertCarfaxLog(message, DateTime.Now, operationType, 0, 0);
					Logger.WriteLog("Error occurred while uploading the CARFAX File: {0}", filePath);
					Logger.WriteLog("Carfax upload error message: {0}", ex.Message);
					Logger.WriteLog("Carfax upload error stack trace: {0}", ex.StackTrace);
				}
				finally
				{
					if(reqStream != null) reqStream.Dispose();
				}
			}
		}

		public static void DownloadCarfaxData()
		{
			string result = string.Empty;

			try
			{
				ServiceManager manager = new ServiceManager();
				string fileName = string.Format("edriveauto_cfx_{0}_return_file.txt", DateTime.Now.ToString("MMddyyyy"));
				
				result = DownloadFile(0, fileName);
				
				if(result != string.Empty)
				{
					manager.InsertCarfaxLog(result, DateTime.Now, 3, 1, 0);
					Logger.WriteLog("Successfully downloaded the CARFAX file: {0}", fileName);

					string ansProduct = manager.UpdateFromCarfax(Path.Combine(Settings.Default.CarfaxPath, fileName), 0);

					if(ansProduct != string.Empty)
					{
						manager.InsertCarfaxLog(ansProduct, DateTime.Now, 4, 1, 0);
						Logger.WriteLog("Successfully updated the products from CARFAX file: {0}", fileName);
					}
					else
					{
						ansProduct = "Products not updated successfully";
						manager.InsertCarfaxLog(ansProduct, DateTime.Now, 4, 0, 0);
						Logger.WriteLog("Error occurred while updating the products from the CARFAX file: {0}", fileName);
					}
				}
				else
				{
					result = "File not downloaded successfully.";
					manager.InsertCarfaxLog(result, DateTime.Now, 3, 0, 0);
				}
			}
			catch(Exception ex)
			{
				Logger.WriteLog("Error occurred while downloading from CARFAX: {0}", ex.Message);
			}
		}

		public static void SetFileAcccessToEveryone(string filePath)
		{
			try
			{
				FileInfo file = new FileInfo(filePath);

				FileSecurity access = file.GetAccessControl();
				SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
				access.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.ReadAndExecute, AccessControlType.Allow));
				file.SetAccessControl(access);
			}
			catch(Exception ex)
			{
				Logger.WriteLog("Error while setting access rules for file: {0}", filePath);
			}
		}

		private static string DownloadFile(int customerId, string fileName)
		{
			ServiceManager manager = new ServiceManager();

			try
			{
				const string ftpAddress = "ftp://ftp.carfax.com/";
				const string username = "EDRIVEAUTO_get";
				const string password = "8883163374";
				string message = string.Empty;
				byte[]  downloadedData = new byte[0];
				string filePath = string.Empty;
				string ftpFileName = Path.Combine(ftpAddress, fileName);
				
				//Create FTP request
				//Note: format is ftp://server.com/file.ext
				FtpWebRequest request = FtpWebRequest.Create(ftpFileName) as FtpWebRequest;

				//Get the file size first (for progress bar)
				request.Method = WebRequestMethods.Ftp.GetFileSize;
				request.Credentials = new NetworkCredential(username, password);
				request.UsePassive = true;
				request.UseBinary = true;
				request.KeepAlive = true; //don't close the connection

				int dataLength = (int)request.GetResponse().ContentLength;

				//Now get the actual data
				request = FtpWebRequest.Create(ftpFileName) as FtpWebRequest;
				request.Method = WebRequestMethods.Ftp.DownloadFile;
				request.Credentials = new NetworkCredential(username, password);
				request.UsePassive = true;
				request.UseBinary = true;
				request.KeepAlive = false; //close the connection when done


				//Streams
				FtpWebResponse response = request.GetResponse() as FtpWebResponse;
				Stream reader = response.GetResponseStream();

				//Download to memory
				//Note: adjust the streams here to download directly to the hard drive
				MemoryStream memStream = new MemoryStream();
				byte[] buffer = new byte[1024]; //downloads in chuncks

				while(true)
				{

					//Try to read the data
					int bytesRead = reader.Read(buffer, 0, buffer.Length);

					if(bytesRead == 0)
					{
						break;
					}
					else
					{
						//Write the downloaded data
						memStream.Write(buffer, 0, bytesRead);
					}
				}

				//Convert the downloaded stream to a byte array
				downloadedData = memStream.ToArray();

				//Clean up
				reader.Close();
				memStream.Close();
				response.Close();
				
				if(downloadedData != null && downloadedData.Length != 0)
				{
					//String ss = System.Configuration.ConfigurationManager.AppSettings["CarfaxPath"].ToString();
					filePath = Path.Combine(Settings.Default.CarfaxPath, fileName);
					//Write the bytes to a file
					FileStream newFile = new FileStream(filePath, FileMode.Create);
					newFile.Write(downloadedData, 0, downloadedData.Length);
					newFile.Close();

				}

				message = "Data downloaded successfully.";

				return message;
			}
			catch(Exception ex)
			{
				manager.InsertCarfaxLog(ex.Message, DateTime.Now, 1, 1, customerId);
				Logger.WriteLog("Error occurred while downloading the CARFAX file: {0}", ex.Message);

				return string.Empty;
			}
		}
	}
}