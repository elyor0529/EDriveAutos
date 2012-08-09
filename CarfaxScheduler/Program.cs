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
			ServiceManager manager = new ServiceManager();

			Logger.WriteLog("Starting Carfax Scheduler...");

			Helpers.DownloadCarfaxData();//Download carfax file and update products.
			manager.SaveCarfaxDealersFile();//Dealers' file is saved and uploaded to Carfax
			manager.SaveCarfaxVehiclesFile();//Vehicles' file is saved and uploaded to Carfax

			Logger.WriteLog("Finished Checking from Carfax.");
			Logger.WriteLog("---------------------------------------------------------------------------------------");
		}
	}
}
