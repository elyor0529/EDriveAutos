using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edrive.CsvParserService.NadaUsedCars;

namespace Edrive.CsvParserService
{
	public class ServiceManager
	{
		private EdriveEntities _context;

		public List<FTP_DIRECTORIES> GetFtpFolders()
		{
			using(_context = new EdriveEntities())
			{
				var query = _context.FtpDirectories();

				return query.ToList();
			}
		}

		public void SaveCarfaxDealersFile()
		{
			using(_context = new EdriveEntities())
			{
				string fileName = String.Format("EDA_dealerlist_{0}.txt", DateTime.Now.ToString("MMddyyyy"));
				var data = _context.CarfaxDealerList().Select(c => c.Column1).ToList();

				Helpers.CreateCarfaxCsvFile(data, fileName, 1);
			}
		} 

		public void SaveCarfaxVehiclesFile()
		{
			using(_context = new EdriveEntities())
			{
				string fileName = String.Format("edriveauto _cfxiicr_{0}.txt", DateTime.Now.ToString("MMddyyyy"));
				var data = _context.CarfaxVehicleList().Select(c => c.Column1).ToList();
				
				Helpers.CreateCarfaxCsvFile(data, fileName, 2);
			}
		}

		public void UpdateFromNada()
		{
			//((pageNumber - 1) * pageSize)
			//SELECT Price_Current, VIN from [VEHICLE.TEMP]
			const int pageSize = 200;
			int totalCount = GetTotalNewProductsCount();

			if(totalCount == 0)
			{
				return;
			}

			const string queryFormat = @"SELECT top {0} CAST(PriceCurrent as money)  as PriceCurrent, Vin FROM (
							SELECT ISNULL(Price_Current, 0.00) as PriceCurrent, VIN, ROW_NUMBER() OVER(order by Price_Current) as row 
							from [VEHICLE.TEMP]) as tmp
							where tmp.row > {1}
							order by tmp.row";
			
			int totalPages = (int) Math.Ceiling(totalCount/(pageSize*1.00));

			try
			{
				for (int i = 1; i <= totalPages; i++)
				{
					using (_context = new EdriveEntities())
					{
						string query = String.Format(queryFormat, pageSize, (i - 1)*pageSize);
						var vehicles = _context.ExecuteStoreQuery<VehicleItem>(query).ToList();

						UpdateNada(vehicles);
					}
				}
			}
			catch(Exception ex)
			{
				Logger.WriteLog("Error occurred while updating products from NADA: {0}", ex.Message);
				Logger.WriteLog("Stack Trace: {0}", ex.StackTrace);
			}
		}

		public int GetTotalNewProductsCount()
		{
			int totalCount;
			const string countQuery = @"select COUNT(*) as total from [VEHICLE.TEMP]";

			using(_context = new EdriveEntities())
			{
				totalCount = _context.ExecuteStoreQuery<int>(countQuery).FirstOrDefault();
			}

			return totalCount;
		}

		public int GetProductsToUpdate()
		{
			using(_context = new EdriveEntities())
			{
				int total = 0;
				try
				{
					_context.CommandTimeout = 86400;//24 hours
					total = _context.CsvNightlyBatch().FirstOrDefault().GetValueOrDefault(0);
				}
				catch(Exception ex)
				{
					Logger.WriteLog("Error occurred while retrieving products from the stored procedure");
					Logger.WriteLog("Error Message: {0}", ex.Message);
					Logger.WriteLog("Stack Trace: {0}", ex.StackTrace);
				}

				return total;
			}
		} 
		
		public string UpdateFromCarfax(string fileName, int customerId)
		{
			string message = string.Empty;

			try
			{
				using(_context = new EdriveEntities())
				{
					_context.ProductCarfaxReport(fileName);
				}

				message = "Products updated successfully";
			}
			catch(Exception ex)
			{
				InsertCarfaxLog(ex.Message, DateTime.Now, 1, 1, customerId);
				Logger.WriteLog("Carfax File Name: {0}", fileName);
				Logger.WriteLog("Error occurred while updating products from CARFAX: {0}", ex.Message);
				Logger.WriteLog("Stack Trace", ex.StackTrace);				
			}

			return message;
		}

		public void InsertCarfaxLog(string logmsg, DateTime logdate, int status, int success, int customerId)
		{
			using(_context = new EdriveEntities())
			{
				ED_CarfaxLogDetail newCarFaxObj = new ED_CarfaxLogDetail
				{
					LogMsg = logmsg,
					CreateBy = customerId,
					CreateOn = DateTime.Now,
					Status = status,
					Success = success
				};

				try
				{
					_context.ED_CarfaxLogDetail.AddObject(newCarFaxObj);
					_context.SaveChanges();
				}
				catch
				{
					//Logger.WriteLog("Error occurred while logging Carfax");
				}
			}
		}

		private void UpdateNada(List<VehicleItem> vehicles)
		{
			if(vehicles == null || vehicles.Count == 0)
				return;

			using(UsedCarsSoapClient nadaService = new UsedCarsSoapClient())
			{
				int counter = 0;

				StringBuilder sb = new StringBuilder();
				const string queryFormat = @" update Product set ISQUALIFIED = {0}, NADACheckedOn = GETDATE() where VIN = '{1}'; ";

				foreach(var car in vehicles)
				{
					if(counter % 300 == 0 && counter > 0)
					{
						_context.CommandTimeout = 21600;
						_context.ExecuteStoreCommand(sb.ToString());
						sb = new StringBuilder();
					}

					int qualified = 0;

					try
					{
						var nadaAuto = nadaService.GetUsedCars("EdriveAutos", "ed12uc20", car.Vin);

						if(nadaAuto != null) //vehicle not found in NADA
						{
							var nadaPriceMin = nadaAuto.PriceMinMax.Values.Where(it => it.Min > 0).Average(it => it.Min);
							var nadaPriceMax = nadaAuto.PriceMinMax.Values.Where(it => it.Max > 0).Average(it => it.Max);

							if(car.PriceCurrent > (decimal)nadaPriceMin - 500 && car.PriceCurrent < (decimal)nadaPriceMax + 500)
							{
								qualified = 1;
							}
						}
					}
					catch
					{
						qualified = 0;
					}

					sb.AppendFormat(queryFormat, qualified, car.Vin);
					counter++;
				}

				//write the rest of the records
				if(sb.Length > 0)
				{
					_context.CommandTimeout = 21600;
					_context.ExecuteStoreCommand(sb.ToString());
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