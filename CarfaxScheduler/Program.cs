using System;
using System.Threading;

namespace CarfaxScheduler
{
	public class Program
	{
		public static void Main(string[] args)
		{
			StartScheduler();
		}

		public static void StartScheduler()
		{
			//Start every 6 hours
			while(true)
			{
				ServiceManager manager = new ServiceManager();

				Logger.WriteLog("Starting Carfax Scheduler...");

				Helpers.DownloadCarfaxData();//Download carfax file and update products.
				manager.SaveCarfaxDealersFile();//Dealers' file is saved and uploaded to Carfax
				manager.SaveCarfaxVehiclesFile();//Vehicles' file is saved and uploaded to Carfax

				Logger.WriteLog("Finished Checking from Carfax.");
				Logger.WriteLog("---------------------------------------------------------------------------------------");

				Thread.Sleep(new TimeSpan(0, 0, Settings.Default.CarfaxCheckIntervalMinutes, 0));//wait for 6 hours before next check
			}
		}
	}
}
