using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NadaScheduler.NadaUsedCars;

namespace NadaScheduler
{
	class Program
	{
		private static EdriveEntities _context;

		static void Main(string[] args)
		{
			UpdateFromNada();
		}

		public static void UpdateFromNada()
		{
			int totalCount = 0;
            DateTime dt = DateTime.UtcNow;

            Console.WriteLine("Begin NADA update");

			while(true)
			{
				List<VehicleItem> newVehicles = new List<VehicleItem>();

				using(_context = new EdriveEntities())
				{
					try
					{
						newVehicles = _context.VehiclesToProcess().ToList().Select(c => new VehicleItem
						{
						    PriceCurrent =
						        c.Price_Current.GetValueOrDefault(0),
						    Vin = c.VIN
						}).ToList();
					}
					catch(Exception ex)
					{
						Console.WriteLine("Error occurred while retrieving a list of products: {0}", ex.Message);
						Console.WriteLine("Stack Trace: {0}", ex.StackTrace);
					}
				}

				totalCount += newVehicles.Count;

				if(!newVehicles.Any())
				{
					Console.WriteLine("Finished processing all vehicles. Total processed vehicles' count: {0}", totalCount);
					break;
				}
				else
				{
					UpdateNada(newVehicles);
                    Console.WriteLine("Finished processing {0} vehicles. Time to process: {1}", totalCount, (DateTime.UtcNow - dt).ToString(@"M/d/yyyy hh:mm:ss tt"));
                    dt = DateTime.UtcNow;
                }
			}
		}

		private static void UpdateNada(List<VehicleItem> vehicles)
		{
			if(vehicles == null || vehicles.Count == 0)
				return;

			using(UsedCarsSoapClient nadaService = new UsedCarsSoapClient())
			{
				int counter = 0;

				StringBuilder sb = new StringBuilder();
                const string updateQualified = @" update [Product.STAGING] set ISQUALIFIED = 1, Deleted = 0, SavingAmount = {0}, QualifyPrice = {1}, AverageRetailPrice = {2},UpdatedOn = GETDATE(), NADACheckedOn = GETDATE(), City_Fuel = {3}, Highway_Fuel = {4} where VIN = '{5}'; ";
                const string updateNotQualified = @" update [Product.STAGING] set ISQUALIFIED = 0, Deleted = 1, SavingAmount = null, QualifyPrice = null, AverageRetailPrice = null, NADACheckedOn = GETDATE() where VIN = '{0}'; ";

                foreach(var car in vehicles)
				{
					/*if(counter % vehicles.Count == 0 && counter > 0)
					{
						using(_context = new EdriveEntities())
						{
							_context.CommandTimeout = 21600;
							_context.ExecuteStoreCommand(sb.ToString());
							sb = new StringBuilder();
						}
					}*/

                    decimal savings = 0;
                    decimal qualifyPrice = 0;
                    string hwyMpg = "";
                    string cityMpg = "";

					try
					{
						var nadaAuto = nadaService.GetUsedCars("EdriveAutos", "ed12uc20", car.Vin);

						if(nadaAuto != null) //vehicle not found in NADA
						{
                            var nadaPrice = nadaAuto.PriceMinMax.Values.Where(it => it.Min > 0 && it.Type == MinMaxType.AverageRetailPrice).Max(it => it.Min);

                            qualifyPrice = (nadaPrice - 500);
                            savings = nadaPrice - car.PriceCurrent;
                            hwyMpg = nadaAuto.UsedCars.First().HighwayFuelHigh.ToString();
                            cityMpg = nadaAuto.UsedCars.First().CityFuelHigh.ToString();

                            if (car.PriceCurrent <= qualifyPrice)
                            {
                                sb.AppendFormat(updateQualified, savings, qualifyPrice, nadaPrice, cityMpg, hwyMpg,  car.Vin);

                            }
                            else
                            {
                                sb.AppendFormat(updateNotQualified, car.Vin);
                            }
						}
					}
					catch
					{
                        sb.AppendFormat(updateNotQualified, car.Vin);
					}

					counter++;
				}

				//write the rest of the records
				if(sb.Length > 0)
				{
					using(_context = new EdriveEntities())
					{
						_context.CommandTimeout = 21600;
						_context.ExecuteStoreCommand(sb.ToString());
					}
                    sb = new StringBuilder();
				}
			}
		}
	}

	public class VehicleItem
	{
		public decimal PriceCurrent { get; set; }
		public string Vin { get; set; }
	}
}
