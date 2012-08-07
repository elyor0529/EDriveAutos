using System;
using System.Linq;

namespace CarfaxScheduler
{
	public class ServiceManager
	{
		private EDriveAutosDevEntities _context;

		public void SaveCarfaxDealersFile()
		{
			using(_context = new EDriveAutosDevEntities())
			{
				_context.CommandTimeout = 200000000;
				string fileName = String.Format("EDA_dealerlist_{0}.txt", DateTime.Now.ToString("MMddyyyy"));
				var data = _context.CarfaxDealerList().Select(c => c.Column1).ToList();

				Helpers.CreateCarfaxCsvFile(data, fileName, 1);
			}
		}

		public void SaveCarfaxVehiclesFile()
		{
			using(_context = new EDriveAutosDevEntities())
			{
				_context.CommandTimeout = 200000000;
				string fileName = String.Format("edriveauto _cfxiicr_{0}.txt", DateTime.Now.ToString("MMddyyyy"));
				var data = _context.CarfaxVehicleList().Select(c => c.Column1).ToList();

				Helpers.CreateCarfaxCsvFile(data, fileName, 2);
			}
		}

		public string UpdateFromCarfax(string fileName, int customerId)
		{
			string message = string.Empty;

			try
			{
				using(_context = new EDriveAutosDevEntities())
				{
					_context.CommandTimeout = 200000000;
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
				Logger.WriteLog("Innner Exception Message", ex.InnerException.Message);
				Logger.WriteLog("Innner Exception Stack Trace", ex.InnerException.StackTrace);
			}

			return message;
		}

		public void InsertCarfaxLog(string logmsg, DateTime logdate, int status, int success, int customerId)
		{
			using(_context = new EDriveAutosDevEntities())
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
	}
}