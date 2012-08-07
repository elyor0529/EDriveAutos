using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using CsvHelper;
using EdriveService.BAL;
using EdriveService.DataContract;
using EdriveService.NADA_UsedCars;
using EdriveService.Nada_UsedCarPrices;
using ICSharpCode.SharpZipLib.Zip;


namespace EdriveService
{
	public class Edrive_Service : IEdrive_Service
	{

		#region "Variables"
		public String ServiceCSVFilePath
		{
			get
			{
				return System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\EdriveDataCsv";
			}
		}
		EdriveService.edriveautoEntities _edriveEntity = new edriveautoEntities();

		public Edrive_Service()
		{
			_edriveEntity.CommandTimeout = 1200;
		}

		public enum ProductStatus { JunkRecord = 1, ExcetionInValidateMethod = 2, NewRecordInserted = 3, RecordUpdated = 4, PriceValidationFailed = 5, NadaWebServiceError = 6, Price_CastingError = 7, RecordDeleted }
		public enum csvColumns
		{
			Type, Stock, VIN, Year, Make, Model, Trim, Free_Text, Body, Mileage, Price_Current,
			Reserved, Price_Wholesale, Price_Cost, Title, Condition, Exterior, Interior, Doors,
			Engine, Transmission, Fuel_Type, Drive_Type, Options, Warranty, Description, Pics,
			Date_In_Stock, DealerEmail, DealerName, DealerCompany, DealerAddress, DealerCity,
			DealerState, DealerZip, DealerTelephone
		}

		#endregion
		#region "RajMethods"

		public void WEBProxy1()
		{
			try
			{
				WebProxy pr;
				NetworkCredential cr;
				cr = new NetworkCredential(ConfigurationManager.AppSettings["NetworkUserName"].ToString(), ConfigurationManager.AppSettings["NetworkPassword"].ToString());
				pr = new WebProxy(ConfigurationManager.AppSettings["WebProxyServer"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["WebProxyPort"]));
				pr.Credentials = cr;
				WebRequest.DefaultWebProxy = pr;

			}
			catch(System.Web.Services.Protocols.SoapException se)
			{
			}


		}
		/// <summary>
		/// Validate Car's Value specified by its VIN using third party services and update the Status Variable with result
		/// </summary>
		/// <param name="VIN"></param>
		/// <param name="price"></param>
		/// <param name="Status"></param>
		/// <returns>If product is Validate successfully then true else false</returns>
		private bool ValidRecord(String VIN, Decimal price, ref ProductStatus Status)
		{
			try
			{
				UsedCars oUsedCarService = new UsedCars();
				UsedCarResultSet oUsedCarsResultSet = oUsedCarService.GetUsedCars("EdriveAutos", "ed12uc20", VIN.ToString());
				NADA_UsedCars.UsedCar uc = oUsedCarsResultSet.UsedCars.FirstOrDefault();
				if(uc != null)
				{
					//Nirav - 16-Aug-11
					//Client wants Clean AverageRetailPrice now
					//Decimal Five_PPrice = Convert.ToDecimal((uc.AverageRetailPrice) * 0.15);
					//Decimal newAverageTradeInPrice = Convert.ToDecimal(uc.AverageRetailPrice) + Five_PPrice;
					decimal? nadaPriceMin = (decimal?)oUsedCarsResultSet.PriceMinMax.Values.Where(it => it.Min > 0).Average(it => it.Min);
					decimal? nadaPriceMax = (decimal?)oUsedCarsResultSet.PriceMinMax.Values.Where(it => it.Max > 0).Average(it => it.Max);

					//                    Decimal Five_PPrice = Convert.ToDecimal((uc.AverageRetailPrice));
					//                    Decimal newAverageTradeInPrice = Five_PPrice;

					//if (price + 3000 <= newAverageTradeInPrice) //Nirav - 28/04/11 - to import all data for demo purpose. --Commented by henisha rathod 8-9-2011 4:16pm
					//if (true) //Nirav - 28/04/11 - to import all data for demo purpose.
					if(price >= nadaPriceMin - 500 && price <= nadaPriceMax + 500)//Added by henisha rathod 8-9-2011 4:16pm
					{
						//Nirav - 27/04/11 - to check import routine
						//wr.WriteLine(VIN + "," + price.ToString() + "," + uc.AverageRetailPrice.ToString());
						return true;
					}
					else
					{
						//Nirav - 27/04/11 - to check import routine
						//wr.WriteLine(VIN + "," + price.ToString() + "," + uc.AverageRetailPrice.ToString());
						Status = ProductStatus.PriceValidationFailed; //Price validation failed
						return false;
					}
				}

				else
				{
					//wr.WriteLine(VIN + "," + price + ",0");
					Status = ProductStatus.NadaWebServiceError; //NADA Webservice error
					return false;
				}
			}
			catch(Exception ex)
			{
				CreateLogFiles err = new CreateLogFiles();
				err.ErrorLog(ex);
				Status = ProductStatus.ExcetionInValidateMethod;
				return false;
			}
		}

		//------------------------------------------------------------
		//-------------------------------WindowService------------------
		//-------------------------------------------------------
		#region "Window Services Methods"
		/// <summary>
		/// Updates And Add records to existing DB from GetAoto third part csv files
		/// </summary>
		public string GetDataFromGetAuto()
		{

			var aultechFolderPath = ConfigurationManager.AppSettings["GetAutoFolderPath"];
			try
			{
				//delete old records--from created one month before
				using(edriveautoEntities _entity = new edriveautoEntities())
				{
					var PrevMonth = DateTime.Now.AddMonths(-1);
					var lstDelRecords = _entity.ED_DataImportLog.Where(m => m.CreatedDate < PrevMonth);
					foreach(var item in lstDelRecords)
					{
						_entity.ED_DataImportLog.DeleteObject(item);
					}
					_entity.SaveChanges();
				}

				var fileList = Directory.GetFiles(aultechFolderPath, "*.zip");

				if(!fileList.Any()) //no file to grab
				{
					return String.Format("No files in GetAuto Folder path is: '{0}'", aultechFolderPath);
				}
				var log = "";
				foreach(var zipFile in fileList)
				{
					//unzip file and return path to cvs file
					var unzipFIlePath = UnzipFile(aultechFolderPath, new FileInfo(zipFile).Name);

					log += Data_Import_Update(unzipFIlePath) + "\n";
					//delete cvs file
					File.Delete(unzipFIlePath);
					//delete zip file
					File.Move(zipFile, ConfigurationManager.AppSettings["GetAutoFolderPathBak"]);
				}
				return "All files in GetAuto Folder is grap success, files count is " + fileList.Count() + "\n Information about work is: \n" + log;
				//WEBProxy1();
				// for testing By Rajeev
				//                DownloadFromFTP_Unzip("69.167.162.50", "/", DateTime.Today.Year.ToString().Substring(2, 2) 
				//                    + DateTime.Today.Month.ToString().PadLeft(2, '0') + (DateTime.Today.Day).ToString().PadLeft(2, '0') + ".zip", this.ServiceCSVFilePath + "\\GetAuto\\", "edrive_will", "R12*2_12DF12");

				// for live
				//DownloadFromFTP_Unzip("69.167.162.50", "/", DateTime.Today.Year.ToString().Substring(2, 2) + DateTime.Today.Month.ToString().PadLeft(2, '0') + (DateTime.Today.Day).ToString().PadLeft(2, '0') + ".zip", this.ServiceCSVFilePath + "\\GetAuto\\", "edrive_will", "R12*2_12DF12");

			}
			catch(Exception ex)
			{
				CreateLogFiles Err = new CreateLogFiles();
				Err.ErrorLog(ex);
				return ex.Message + " ||| " + ex.StackTrace;
			}
		}
		/// <summary>
		/// Updates And Add records to existing DB from third part csv files
		/// </summary>
		public string GetDataFromSchumacher()
		{
			var schumacherFolderPath = ConfigurationManager.AppSettings["SchumacherFolderPath"];
			try
			{
				//delete old records--from created one month before
				using(edriveautoEntities _entity = new edriveautoEntities())
				{
					var PrevMonth = DateTime.Now.AddMonths(-1);
					var lstDelRecords = _entity.ED_DataImportLog.Where(m => m.CreatedDate < PrevMonth);
					foreach(var item in lstDelRecords)
					{
						_entity.ED_DataImportLog.DeleteObject(item);
					}
					_entity.SaveChanges();
				}

				var fileList = Directory.GetFiles(schumacherFolderPath, "*.zip");

				if(!fileList.Any()) //no file to grab
				{
					return String.Format("No files in Schumacher Folder path is: '{0}'", schumacherFolderPath);
				}
				var log = "";
				foreach(var zipFile in fileList)
				{
					//unzip file and return path to cvs file
					var unzipFIlePath = UnzipFile(schumacherFolderPath, new FileInfo(zipFile).Name);

					log = Data_Import_Update(unzipFIlePath) + "\n";
					//delete cvs file
					File.Delete(unzipFIlePath);
					//delete zip file
					File.Move(zipFile, ConfigurationManager.AppSettings["SchumacherFolderPathBak"]);
				}
				return "All files in Schumacher Folder is grap success, files count is " + fileList.Count() + "\n Information about work is: \n" + log;
				//WEBProxy1();
				//DownloadFromFTP_Unzip("69.167.162.50", "/", DateTime.Today.Year.ToString().Substring(2, 2) + DateTime.Today.Month.ToString().PadLeft(2, '0') + (DateTime.Today.Day).ToString().PadLeft(2, '0') + ".zip", this.ServiceCSVFilePath + "\\Schumacher\\", "edrive_netlook", "SA_FER12*2");
			}
			catch(Exception ex)
			{
				CreateLogFiles Err = new CreateLogFiles();
				Err.ErrorLog(ex);
				return ex.Message + " ||| " + ex.StackTrace;
			}

		}

		public void GetDataFromEBizAuto()
		{
			try
			{
				DownloadFromFTP_Unzip("69.167.162.50", "/", DateTime.Today.Year.ToString().Substring(2, 2) + DateTime.Today.Month.ToString().PadLeft(2, '0') + (DateTime.Today.Day).ToString().PadLeft(2, '0') + ".csv", this.ServiceCSVFilePath + "\\eBizAuto\\", "edrive_colin", "F#22SA_FSD12");
			}
			catch(Exception ex)
			{
				CreateLogFiles Err = new CreateLogFiles();
				Err.ErrorLog(ex);
			}

		}
		/// <summary>
		/// Updates And Add records to existing DB from third part csv files
		/// </summary>
		public string GetDataFromAULtec()
		{
			var aultechFolderPath = ConfigurationManager.AppSettings["AultechFolderPath"];
			//delete old records--from created one month before
			try
			{
				using(edriveautoEntities _entity = new edriveautoEntities())
				{
					var PrevMonth = DateTime.Now.AddMonths(-1);
					var lstDelRecords = _entity.ED_DataImportLog.Where(m => m.CreatedDate < PrevMonth);
					foreach(var item in lstDelRecords)
					{
						_entity.ED_DataImportLog.DeleteObject(item);
					}
					_entity.SaveChanges();
				}

				var fileList = Directory.GetFiles(aultechFolderPath, "*.zip");

				if(!fileList.Any()) //no file to grab
				{
					return String.Format("No files in Aultech Folder path is : '{0}'", aultechFolderPath);
				}
				var log = "";
				foreach(var zipFile in fileList)
				{
					//unzip file and return path to cvs file
					var unzipFIlePath = UnzipFile(aultechFolderPath, new FileInfo(zipFile).Name);

					log = Data_Import_Update(unzipFIlePath) + "\n";
					//delete cvs file
					File.Delete(unzipFIlePath);
					//delete zip file
					File.Move(zipFile, ConfigurationManager.AppSettings["AultechFolderPathBak"]);
				}

				// WEBProxy1();
				//                DownloadFromFTP_Unzip("69.167.162.50", 
				//                    "/", 
				//                    DateTime.Today.Year.ToString().Substring(2, 2) +
				//                    DateTime.Today.Month.ToString().PadLeft(2, '0')
				//                    + (DateTime.Today.AddDays(-1).Day).ToString().PadLeft(2, '0') + ".zip", this.ServiceCSVFilePath + "\\AULtec\\", "edrive_aultec", "SDF#22SA_FHW");
				// check and remove those products for whick price validation using naada service gets falls
				//Qualify_All_Products();//
				return "All files in Aultech Folder is grap success, files count is " + fileList.Count() + "\n Information about work is: \n" + log;
			}
			catch(Exception ex)
			{
				CreateLogFiles Err = new CreateLogFiles();
				Err.ErrorLog(ex);
				return ex.Message + " ||| " + ex.StackTrace;
			}
		}
		/// <summary>
		/// Updates And Add records to existing DB from third part csv files
		/// </summary>
		public string GetDataFromAutoBase()
		{
			var autobaseFolderPath = ConfigurationManager.AppSettings["AutobaseFolderPath"];
			try
			{
				//delete old records--from created one month before
				using(edriveautoEntities _entity = new edriveautoEntities())
				{
					var PrevMonth = DateTime.Now.AddMonths(-1);
					var lstDelRecords = _entity.ED_DataImportLog.Where(m => m.CreatedDate < PrevMonth);
					foreach(var item in lstDelRecords)
					{
						_entity.ED_DataImportLog.DeleteObject(item);
					}
					_entity.SaveChanges();
				}

				var fileList = Directory.GetFiles(autobaseFolderPath, "*.zip");

				if(!fileList.Any()) //no file to grab
				{
					return String.Format("No files in Autobase Folder path is '{0}'", autobaseFolderPath);
				}
				var log = "";
				foreach(var zipFile in fileList)
				{
					//unzip file and return path to cvs file
					var unzipFIlePath = UnzipFile(autobaseFolderPath, new FileInfo(zipFile).Name);

					log += Data_Import_Update(unzipFIlePath) + "\n";
					//delete cvs file
					File.Delete(unzipFIlePath);
					//delete zip file
					File.Move(zipFile, ConfigurationManager.AppSettings["AutobaseFolderPathBak"]);
				}
				return "All files in Autobase Folder is grap success, files count is " + fileList.Count() + "\n Information about work is: \n" + log;

				//                DownloadFromFTP_Unzip("69.167.162.50", "/", "data-" + DateTime.Today.Year.ToString()
				//                    + DateTime.Today.Month.ToString().PadLeft(2, '0') +
				//                    (DateTime.Today.Day - 1).ToString().PadLeft(2, '0') + ".zip", this.ServiceCSVFilePath + "\\AutoBase\\", "edrive_david", "2SA_FSD12#2");
			}
			catch(Exception ex)
			{
				CreateLogFiles Err = new CreateLogFiles();
				Err.ErrorLog(ex);
				return ex.Message + " ||| " + ex.StackTrace;
			}


		}

		/// <summary>
		/// This method check if cdn images exist on server. if All images have been remove it will return "false"
		/// if even one image is valid it will return "true"
		/// </summary>
		/// <param name="pics"></param>
		/// <returns></returns>
		private static bool UrlExists(string pics)
		{

			if(String.IsNullOrEmpty(pics))
				return false;
			try
			{
				String[] imgUrl = pics.Split(';');
				Boolean flag = false;
				foreach(var url in imgUrl)
				{
					try
					{
						new System.Net.WebClient().DownloadData(url);
						flag = true;

					}
					catch(Exception ex)
					{
						break;
					}

				}
				return flag;
			}
			catch(System.Net.WebException e)
			{
				if(((System.Net.HttpWebResponse)e.Response).StatusCode == System.Net.HttpStatusCode.NotFound)
					return false;
				else
					throw;
			}
		}

		private static string ValidatePics(string pics)
		{
			var urlToPhotoDefault = "http://edriveautos.com/Content/Images/Dealer/photo-comming-soon.jpg";

			if(String.IsNullOrEmpty(pics))
			{
				return urlToPhotoDefault;
			}

			String[] imgUrl = pics.Split(';');
			for(var i = 0; i < imgUrl.Count(); i++)
			{
				try
				{
					new WebClient().DownloadData(imgUrl[i]);
				}
				catch(Exception ex)
				{
					imgUrl[i] = urlToPhotoDefault;
				}
			}

			return String.Join(";", imgUrl);
		}
		/// <summary>
		/// This method check whether products are qualifed if not deltes the products
		/// </summary>

		public void Qualify_All_Products()
		{
			try
			{
				using(edriveautoEntities _entity = new edriveautoEntities())
				{
					SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
					try
					{
						SqlCommand cmd = new SqlCommand("Select Productid,pics,vin,Price_Current from product   where deleted=0   order by Productid desc", con);
						if(con.State == ConnectionState.Closed)
							con.Open();
						SqlDataReader dr = cmd.ExecuteReader();
						DataTable tb = new DataTable();
						tb.Load(dr);
						if(tb.Rows.Count > 0)
						{
							for(int i = 0; i < tb.Rows.Count; i += 1000)
							{
								ThreadProcess tp = new ThreadProcess(i + 1000, tb);
								Thread ts = new Thread(tp.Process);
								ts.Start();

							}


						}
					}
					catch(Exception ex)
					{
					}
					finally
					{
						if(con.State == ConnectionState.Open)
							con.Close();

					}
				}

			}
			catch(Exception ex)
			{
				CreateLogFiles Err = new CreateLogFiles();
				Err.ErrorLog(ex);
			}
		}
		/// <summary>
		///  to recover those products that are deleted and and have valid price
		/// </summary>
		public void Qualify_All_Products_to_RecoverDeletedProducts()
		{
			try
			{
				using(edriveautoEntities _entity = new edriveautoEntities())
				{
					SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
					try
					{
						SqlCommand cmd = new SqlCommand("Select Productid,pics,vin,Price_Current from product   where deleted=1   order by Productid desc", con);
						if(con.State == ConnectionState.Closed)
							con.Open();
						SqlDataReader dr = cmd.ExecuteReader();
						DataTable tb = new DataTable();
						tb.Load(dr);
						if(tb.Rows.Count > 0)
						{
							for(int i = 0; i < tb.Rows.Count; i += 1000)
							{
								ThreadProcess tp = new ThreadProcess(i + 1000, tb);
								Thread ts = new Thread(tp.RecoverDeleted);
								ts.Start();

							}


						}
					}
					catch(Exception ex)
					{
					}
					finally
					{
						if(con.State == ConnectionState.Open)
							con.Close();

					}
				}

			}
			catch(Exception ex)
			{
				CreateLogFiles Err = new CreateLogFiles();
				Err.ErrorLog(ex);
			}
		}

		private void Pricen_Image_Validation(SqlConnection con, SqlDataReader dr)
		{
			string pics = dr["pics"].ToString();
			string vin = dr["vin"].ToString();
			Int32 Productid = Convert.ToInt32(dr["Productid"]);
			decimal Price_Current = Convert.ToDecimal(dr["Price_Current"]);
			ProductStatus Status = ProductStatus.JunkRecord;
			Boolean IsPriceValid = ValidRecord(vin, Convert.ToDecimal(Price_Current), ref Status);
			Boolean IsImageUrlExist = UrlExists(pics);
			Boolean delProduct = false;
			if(IsPriceValid == false)
			{
				if(Status == ProductStatus.PriceValidationFailed || String.IsNullOrEmpty(pics) || (UrlExists(pics) == false))
				{
					delProduct = true;
				}
			}
			else
				if(IsImageUrlExist == false)
				{
					delProduct = true;
				}

			if(delProduct)
			{
				SqlCommand cmdDelete = new SqlCommand("Update product set deleted=1 where productid=" + Productid.ToString(), con);
				cmdDelete.ExecuteNonQuery();

			}
		}

		//public void Qualify_All_Products2()
		//{
		//    try
		//    {
		//        using (edriveautoEntities _entity = new edriveautoEntities())
		//        {
		//           var s= _entity.Product.OrderBy(m=>m.ProductId).Skip(1).Take(10);
		//           var p = s.ToList();
		//            var totalCars=_entity.Product.Where(m => m.Deleted == false).Count();
		//            Int32 RecordsSkipped=0;
		//            Int32 PageSized=100;
		//            List<Product> lstProducts;
		//            while(RecordsSkipped<=totalCars)
		//            {
		//                lstProducts = _entity.Product.Where(m => m.Deleted == false).OrderByDescending(m => m.ProductId).Skip(RecordsSkipped).Take(PageSized).ToList();
		//                foreach (var prdt in lstProducts)
		//                {
		//                    ProductStatus Status = ProductStatus.JunkRecord;
		//                    Boolean IsPriceValid=ValidRecord(prdt.VIN, Convert.ToDecimal(prdt.Price_Current), ref Status);
		//                    Boolean IsImageUrlExist=UrlExists( prdt.Pics);
		//                    if (IsPriceValid == false)
		//                    {
		//                        if (Status == ProductStatus.PriceValidationFailed || String.IsNullOrEmpty(prdt.Pics) || ( UrlExists( prdt.Pics)==false))
		//                        {
		//                            prdt.Deleted = true;
		//                            prdt.UpdatedOn = DateTime.Now;
		//                            _entity.SaveChanges();
		//                        }
		//                    }
		//                    else
		//                    if (IsImageUrlExist == false)
		//                    {
		//                         prdt.Deleted = true;
		//                            prdt.UpdatedOn = DateTime.Now;
		//                            _entity.SaveChanges();
		//                    }

		//                }


		//                _entity.SaveChanges();
		//                RecordsSkipped+=PageSized;
		//            }



		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        CreateLogFiles Err = new CreateLogFiles();
		//        Err.ErrorLog(ex);
		//    }

		//}

		/// <summary>
		/// Download csv file from third party ftp credential
		/// </summary>
		/// <param name="ftphost"></param>
		/// <param name="ftpfilepath"></param>
		/// <param name="ftpFileName"></param>
		/// <param name="outputFilePath"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		private void DownloadFromFTP_Unzip(string ftphost, string ftpfilepath, string ftpFileName, string outputFilePath, string userName, string password)
		{

			ftpfilepath += ftpFileName;
			string outputFilePathWithFileName = outputFilePath + ftpFileName;

			if(!File.Exists(outputFilePathWithFileName))
			{
				string ftpfullpath = "ftp://" + ftphost + ftpfilepath;
				FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);

				ftp.Credentials = new NetworkCredential(userName, password);
				ftp.KeepAlive = true;
				ftp.UseBinary = true;
				ftp.Method = WebRequestMethods.Ftp.DownloadFile;

				FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
				Stream responseStream = response.GetResponseStream();

				FileStream writeStream = new FileStream(outputFilePathWithFileName, FileMode.Create, FileAccess.Write);
				int Length = 256;
				byte[] buffer = new byte[Length];
				int byteRead = responseStream.Read(buffer, 0, Length);
				while(byteRead > 0)
				{
					writeStream.Write(buffer, 0, byteRead);
					byteRead = responseStream.Read(buffer, 0, Length);
				}
				writeStream.Close();
				responseStream.Close();
				response.Close();
				if(Path.GetExtension(outputFilePathWithFileName).ToLower() == ".zip")
				{
					outputFilePathWithFileName = UnzipFile(outputFilePath, ftpFileName);
				}
				Data_Import_Update(outputFilePathWithFileName);
				File.Delete(outputFilePathWithFileName);
			}
			else
			{

				return;
			}
		}



		public void UploadOnCarfax()
		{
			try
			{
				GetDealerInfo Carfax = new GetDealerInfo();
				string ans = string.Empty;
				DateTime dateFinal, date = new DateTime();
				date = DateTime.Now;
				dateFinal = date.AddDays(1);

				//for All Dealer Information


				ans = Carfax.GenerateCSVFiles("EDA_dealerlist_", "GetAllDealerInfo", 0);
				if(ans != string.Empty)
				{
					Carfax.insertCarfaxLog(ans, dateFinal, 1, 1, 0);
				}
				else
				{
					Carfax.insertCarfaxLog(ans, dateFinal, 1, 0, 0);
				}

				//for Dealer And Product Vin No 

				ans = Carfax.GenerateCSVFiles("edriveauto_cfxiicr_", "GetAllDealerIDAndVIN", 0);
				if(ans != string.Empty)
				{
					Carfax.insertCarfaxLog(ans, dateFinal, 2, 1, 0);
				}
				else
				{
					Carfax.insertCarfaxLog(ans, dateFinal, 2, 0, 0);
				}
			}
			catch(Exception ex)
			{
				CreateLogFiles Err = new CreateLogFiles();
				Err.ErrorLog(ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>

		public void DownloadFromCarfax()
		{
			try
			{
				GetDealerInfo Carfax = new GetDealerInfo();
				DateTime dtd, dtdnew = new DateTime();
				string fileName = string.Empty;
				string ans = string.Empty;

				dtd = DateTime.Now;//-- by rajeev uncommne if site is live
				//  dtd = DateTime.Now.AddDays(-3); //--- by rajeev added for testing delete this line when live
				//fileName = "edriveauto_cfx_05102012_return_file.txt"; //--added for testing, delete when site is live
				fileName = "edriveauto_cfx_" + (dtd).ToString("MMddyyyy") + "_return_file.txt";// uncomment for live site
				ans = Carfax.downloadFile("ftp://ftp.carfax.com/", fileName, "EDRIVEAUTO_get", "8883163374", 0);


				if(ans != string.Empty)
				{
					Carfax.insertCarfaxLog(ans, DateTime.Now, 3, 1, 0);
				}
				else
				{
					ans = "File not downloaded successfully.";
					Carfax.insertCarfaxLog(ans, DateTime.Now, 3, 0, 0);
				}

				string ansProduct = Carfax.updateAllProduct(ServiceCSVFilePath + "\\Carfax\\" + fileName, 0);


				if(ansProduct != string.Empty)
				{
					Carfax.insertCarfaxLog(ansProduct, DateTime.Now, 4, 1, 0);
				}
				else
				{
					ansProduct = "Products not updated successfully";
					Carfax.insertCarfaxLog(ansProduct, DateTime.Now, 4, 0, 0);
				}
			}
			catch(Exception ex)
			{
				CreateLogFiles Err = new CreateLogFiles();
				Err.ErrorLog(ex);
			}

		}


		/// <summary>
		/// Impport records from downloaded  csv file (by DownloadFromFTP_Unzip) into DB
		/// </summary>
		/// <param name="csvFilePath"></param>
		public string Data_Import_Update(String csvFilePath)
		{

			try
			{
				// csvFilePath = "C:\\Projects\\VEHICLES.CSV";//--delete this line after testing
				if(Path.GetExtension(csvFilePath).ToLower() == ".csv")
				{
					// StreamReader sr = new StreamReader(csvFilePath);--comment add for DAta table method

					try
					{
						{//---for data table code--
							return CopyDataToDestination(csvFilePath, Path.GetFileName(csvFilePath));
						}
						//---for stream reader code    CopyDataToDestination(sr, Path.GetFileName(csvFilePath));

					}
					catch(Exception ex)
					{
						return ex.Message;
					}
					//finally
					//{
					//    if (sr != null)
					//        sr.Close();
					//}


				}

				else
				{

				}

				return String.Empty;
			}
			catch(Exception ex)
			{
				CreateLogFiles Err = new CreateLogFiles();
				Err.ErrorLog(ex);
				return ex.Message;
			}
		}

		/// <summary>
		/// it return the datatable from csv file by using the give csv file path
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public DataTable ImportCsvFile(string filename)
		{


			FileInfo file = new FileInfo(filename);


			using(OleDbConnection con =
					new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + file.DirectoryName + "\";Extended Properties='text;HDR=Yes;FMT=Delimited(,)';"))
			{
				using(OleDbCommand cmd = new OleDbCommand(string.Format("SELECT * FROM [{0}]", file.Name), con))
				{
					con.Open();
					// Using a DataTable to process the data
					using(OleDbDataAdapter adp = new OleDbDataAdapter(cmd))
					{
						DataTable tbl = new DataTable("MyTable");
						adp.Fill(tbl);
						return tbl;
					}
				}
			}
		}
		//
		//        void csv_ParseError(object sender, ParseErrorEventArgs e)
		//        {
		//            // if the error is that a field is missing, then skip to next line
		//            if (e.Error is MissingFieldCsvException)
		//            {
		//                Console.Write("--MISSING FIELD ERROR OCCURRED");
		//                e.Action = ParseErrorAction.AdvanceToNextLine;
		//            }
		//        }

		private string CopyDataToDestination(string filePath, string fileName)
		{
			string vin = String.Empty;

			try
			{
				string log = "";
				int addCount = 0;
				int updateCount = 0;
				int deleteCount = 0;
				int deleteCount1 = 0;
				//List<Product> lstProduct = new List<Product>();
				//table.ReadLine();//to read header rows
				using(CsvReader csv = new CsvReader(new StreamReader(filePath)))
				{
					//int fieldCount = item.FieldCount;
					var recordsList = csv.GetRecords<CsvRecord>().ToList();

					var filldsCount = csv.FieldHeaders.Count();

					if(filldsCount != 36)
					{
						return "Incorrect file struct, file must contains 35 fields, but file contains " + filldsCount + "file name is: " + filePath + ". File will be deleted.";
					}

					foreach(CsvRecord record in recordsList)
					{
						ProductStatus Status = ProductStatus.JunkRecord;

						try
						{
							vin = record.Vin;

							if(vin.Length > 0 && record.PriceCurrent.Length > 0 && record.PriceCurrent != "0" && record.DealerEmail.Length > 0)
							{
								decimal Price_Current = 0;
								Boolean PriceCastingSuccess = true;

								try
								{
									Price_Current = decimal.Parse(record.PriceCurrent);
								}
								catch
								{
									Status = ProductStatus.Price_CastingError;
									PriceCastingSuccess = false;
								}

								record.Pics = ValidatePics(record.Pics);

								if(PriceCastingSuccess) //--for live
								{

									Product objProduct = _edriveEntity.Product.Where(m => m.VIN == vin).SingleOrDefault();

									if(objProduct == null)
									{
										objProduct = new Product();
										Status = ProductStatus.NewRecordInserted;
									}
									else
									{
										Status = ProductStatus.RecordUpdated;
									}


									//objProduct.=item["Make"]
									Product_Make _prdMake;
									string Make = record.Make;
									// hard coded checking of make "Mercedes-Benz"
									// if it is "Mercedes-Benz" then change this to "Mercedes Benz"
									if(Make.Trim().ToLower() == "Mercedes-Benz".ToLower())
									{
										Make = "Mercedes Benz";
									}

									//end of hard coded checking of make "Mercedes-Benz"

									_prdMake = _edriveEntity.Product_Make.Where(m => m.Make == Make).SingleOrDefault();
									if(_prdMake == null)
									{
										_prdMake = new Product_Make();
										_prdMake.Make = Make;
										_edriveEntity.AddToProduct_Make(_prdMake);
										_edriveEntity.SaveChanges();
									}


									var Model = record.Model;

									var prdModel = _edriveEntity.Product_Model.Where(m => m.ModeLName == Model && m.MakeID == _prdMake.id).SingleOrDefault();
									if(prdModel == null)
									{
										prdModel = new Product_Model();
										prdModel.ModeLName = Model;
										prdModel.MakeID = _prdMake.id;
										_edriveEntity.AddToProduct_Model(prdModel);
										_edriveEntity.SaveChanges();
									}


									objProduct.Trim = record.Trim;
									objProduct.Free_Text = record.FreeText;

									var Body = record.Body;

									var prdBody = _edriveEntity.Product_Body.Where(m => m.Body == Body).SingleOrDefault();

									if(prdBody == null)
									{
										prdBody = new Product_Body();
										prdBody.Body = Body;
										_edriveEntity.AddToProduct_Body(prdBody);
										//_edriveEntityEntity.SaveChanges();
									}
									var Type = record.Type;
									Product_Type prdType = _edriveEntity.Product_Type.Where(m => m.Type == Type).SingleOrDefault();
									if(prdType == null)
									{
										prdType = new Product_Type();
										prdType.Type = Type;
										_edriveEntity.AddToProduct_Type(prdType);
										//_edriveEntityEntity.SaveChanges();
									}


									double wholeSale = 0;
									bool abc = double.TryParse(record.PriceWholesale, out wholeSale);

									double Price = 0;

									bool _price = double.TryParse(record.PriceCost, out Price);
									objProduct.Type = prdType.id;
									Decimal PriceCurrent = 0;
									if(String.IsNullOrEmpty(record.PriceCurrent) == false)
										PriceCurrent = Convert.ToDecimal(record.PriceCurrent);

									objProduct.Price_Current = PriceCurrent;
									objProduct.Price_WholeSale = wholeSale;
									objProduct.Reserved = Convert.ToString(record.Reserved);
									objProduct.Stock = record.Stock;
									int Mileage;

									if(Int32.TryParse(record.Mileage, out Mileage))
									{
										objProduct.Mileage = Mileage;
									}


									objProduct.Model = prdModel.id; //---------------
									objProduct.Body = prdBody.id; //---------------
									objProduct.VIN = record.Vin;
									objProduct.Year = Convert.ToInt32(record.Year);
									objProduct.Name = record.Make + " " + record.Model + " " + objProduct.VIN + " " + objProduct.Year + " " + record.DealerCity;
									objProduct.Price_Cost = Price;
									objProduct.Title = "";

									objProduct.Condition = record.Condition;
									objProduct.Exterior_Color = record.Exterior;
									objProduct.Interior_Color = record.Interior;
									Int32? Doors = String.IsNullOrEmpty(record.Doors) ? 0 : Convert.ToInt32(record.Doors);
									objProduct.Doors = Doors;
									objProduct.Engine = record.Engine;
									objProduct.Transmission = record.Tramsission;
									objProduct.Fuel_Type = record.FuelType;
									objProduct.Drive_Type = record.DriveType;
									//objProduct.Options = item[(Int32)csvColumns.Options].ToString();
									objProduct.Warranty = (record.Warranty.ToLower() == "yes");
									objProduct.Description = record.Description;
									objProduct.Pics = record.Pics;

									if(String.IsNullOrEmpty(record.DateInStock.Trim()) == false)
									{
										objProduct.Date_in_Stock = Convert.ToDateTime(record.DateInStock);
									}
									else
									{
										objProduct.Date_in_Stock = DateTime.Now;
									}

									objProduct.CreatedOn = DateTime.Now;
									objProduct.UpdatedOn = DateTime.Now;

									int zip = 0;
									if(!string.IsNullOrWhiteSpace(record.DealerZip) && int.TryParse(record.DealerZip.Split('-')[0], out zip))
										objProduct.zip = zip;
									else
										objProduct.zip = null;

									#region "addCustomer"

									var customer = _edriveEntity.Customer.Where(m => m.Email == record.DealerEmail).SingleOrDefault();

									if(customer == null)
									{
										customer = new Customer();
										customer.Password = customer.Email = record.DealerEmail;
										customer.Active = true;
										customer.CustomerGUID = Guid.NewGuid();
										customer.RegistrationDate = DateTime.Now;
										customer.ExpiryDate = DateTime.Now.AddMonths(1);
										customer.IsTrial = true;
										customer.Name = record.DealerName;
										customer.FirstName = customer.Name;
										customer.LastName = "";
										customer.Company = record.DealerCompany;
										customer.CustomerType = 1;
										customer.StreetAddress = record.DealerAddress;
										//-------add customer's State------------
										var csvState = record.DealerState;
										var stateName = _edriveEntity.StateProvince.FirstOrDefault(m => m.Abbreviation == csvState);
										if(stateName != null)
										{
											customer.Stateid = stateName.StateProvinceID;
										}
										//-------end of add customer State------------
										customer.City = record.DealerCity;
										if(String.IsNullOrEmpty(record.DealerZip.Replace("-", String.Empty)) == false)
											customer.ZipPostalCode = Int32.Parse(record.DealerZip);
										customer.Phone = record.DealerTel;
										_edriveEntity.Customer.AddObject(customer);
									}

									#endregion

									//-------------------------------use of third party services
									//var dealer = GetDealerByDealerID(Prd.customerId);
									var CityName = record.DealerCity;
									objProduct.Name = record.Make + " " + record.Model
													  + " " + record.Vin + " " + objProduct.Year + " " + CityName +
													  ((customer.ZipPostalCode ?? 0) == 0 ? "" : " " + (customer.ZipPostalCode ?? 0).ToString());
									UsedCars oUsedCarService = new UsedCars();
									UsedCarResultSet oUsedCarsResultSet = oUsedCarService.GetUsedCars("EdriveAutos", "ed12uc20", record.Vin);
									UsedCar uc = oUsedCarsResultSet.UsedCars.FirstOrDefault();
									if(uc != null)
									{
										objProduct.City_Fuel = uc.CityFuelHigh;
										objProduct.Highway_Fuel = uc.HighwayFuelHigh;
										objProduct.AverageRetailPrice = uc.AverageRetailPrice;
										objProduct.AverageTradeinPrice = uc.AverageTradeinPrice;

									}
									//------------
									objProduct.CustomerID = customer.CustomerID;


									if(Status == ProductStatus.NewRecordInserted)
									{
										_edriveEntity.AddToProduct(objProduct);
										_edriveEntity.SaveChanges();
										AddProductOptions(record.Options, objProduct.ProductId);
										AddPicsToProduct(objProduct.Pics.Split(';'), objProduct.ProductId);
										addCount++;
									}
									else
									{
										if(Status == ProductStatus.RecordUpdated)
										{
											//update offer property
											_edriveEntity.SaveChanges();
											//Update Product Option
											UpdateProudctOption(record.Options, objProduct.ProductId);
											//update ProcutPics
											//add new Pics
											UpdatePicsToProduct(objProduct.Pics.Split(';'), objProduct.ProductId);
											updateCount++;
										}

									}
								}
								else
								{
									log += "Cant cast current price. Price is";
								}
							}
							else
							{
								log += "vin, PriceCurrent, PriceCurrent and DealerEmail one of item is not exist. Vin is " + vin + ", PriceCurrent is " + record.PriceCurrent +
									   ", DealerEmail is " + record.DealerEmail;
							}
						}
						catch(Exception ex)
						{
							log += ex.Message;
						}
					}

					log += String.Format("Add Record Count is: {0}, update record count is {1}, delete after NADA validation count is {2}, need delete after NADA but don't find in bd count is : {3}", addCount, updateCount, deleteCount, deleteCount1);
					log += "\n All record count is " + recordsList.Count();

					return log;
				}
			}
			catch(Exception ex)
			{
				return ex.Message;
			}
		}


		///Commented by Rajeev on 17 May 2012 for backup
		//private void CopyDataToDestination(String File, String fileName)
		//{
		//    String VIN;
		//    DataTable dt = ImportCsvFile(File);
		//    try
		//    {

		//        List<Product> lstProduct = new List<Product>();
		//        //table.ReadLine();//to read header rows

		//        foreach (DataRow item in dt.Rows)
		//        {
		//            //  var item = table.ReadLine().Split(',');
		//            ProductStatus Status = ProductStatus.JunkRecord;
		//            //for (int i = 0; i < item.Length; i++)
		//            //{
		//            //    item[i] = item[i].Trim('"');//remove quotes from beg and end
		//            //}
		//            try
		//            {
		//                VIN = "";
		//                VIN = item[(Int32)csvColumns.VIN].ToString();
		//                String Email = item[(Int32)csvColumns.DealerEmail].ToString();
		//                if (VIN.Length > 0 && item[(Int32)csvColumns.Price_Current].ToString().Length > 0 && item[(Int32)csvColumns.Price_Current].ToString() != "0" && Email.Length > 0)
		//                {
		//                    decimal Price_Current = 0;
		//                    Boolean PriceCastingSuccess = true;
		//                    try
		//                    {
		//                        Price_Current = decimal.Parse(item[(Int32)csvColumns.Price_Current].ToString());
		//                    }
		//                    catch
		//                    {
		//                        Status = ProductStatus.Price_CastingError;
		//                        PriceCastingSuccess = false;
		//                    }

		//                    if (PriceCastingSuccess & ValidRecord(VIN, Price_Current, ref Status))//--for live
		//                    //if (PriceCastingSuccess)//-for testing
		//                    {

		//                        Product objProduct;
		//                        objProduct = _edriveEntity.Product.Where(m => m.VIN == VIN).SingleOrDefault();

		//                        if (objProduct == null)
		//                        {
		//                            objProduct = new Product();
		//                            Status = ProductStatus.NewRecordInserted;
		//                        }
		//                        else
		//                        {
		//                            Status = ProductStatus.RecordUpdated;
		//                        }


		//                        //objProduct.=item["Make"]
		//                        Product_Make _prdMake;
		//                        var Make = item[(Int32)csvColumns.Make].ToString();
		//                        _prdMake = _edriveEntity.Product_Make.Where(m => m.Make == Make).SingleOrDefault();
		//                        if (_prdMake == null)
		//                        {
		//                            _prdMake = new Product_Make();
		//                            _prdMake.Make = Make;
		//                            _edriveEntity.AddToProduct_Make(_prdMake);
		//                            _edriveEntity.SaveChanges();
		//                        }


		//                        var Model = item[(Int32)csvColumns.Model].ToString();

		//                        var prdModel = _edriveEntity.Product_Model.Where(m => m.ModeLName == Model && m.MakeID==_prdMake.id).SingleOrDefault();
		//                        if (prdModel == null)
		//                        {
		//                            prdModel = new Product_Model();
		//                            prdModel.ModeLName = Model;
		//                            prdModel.MakeID = _prdMake.id;
		//                            _edriveEntity.AddToProduct_Model(prdModel);
		//                            _edriveEntity.SaveChanges();
		//                        }


		//                        objProduct.Trim = item[(Int32)csvColumns.Trim].ToString();
		//                        objProduct.Free_Text = item[(Int32)csvColumns.Free_Text].ToString();

		//                        var Body = item[(Int32)csvColumns.Body].ToString();

		//                        var prdBody = _edriveEntity.Product_Body.Where(m => m.Body == Body).SingleOrDefault();

		//                        if (prdBody == null)
		//                        {
		//                            prdBody = new Product_Body();
		//                            prdBody.Body = Body;
		//                            _edriveEntity.AddToProduct_Body(prdBody);
		//                            //_edriveEntityEntity.SaveChanges();
		//                        }
		//                        var Type = item[(Int32)csvColumns.Type].ToString();
		//                        Product_Type prdType = _edriveEntity.Product_Type.Where(m => m.Type == Type).SingleOrDefault();
		//                        if (prdType == null)
		//                        {
		//                            prdType = new Product_Type();
		//                            prdType.Type = Type;
		//                            _edriveEntity.AddToProduct_Type(prdType);
		//                            //_edriveEntityEntity.SaveChanges();
		//                        }


		//                        double wholeSale = 0;
		//                        bool abc = double.TryParse(item[(Int32)csvColumns.Price_Wholesale].ToString(), out wholeSale);

		//                        double Price = 0;

		//                        bool _price = double.TryParse(item[(Int32)csvColumns.Price_Cost].ToString(), out Price);
		//                        objProduct.Type = prdType.id;
		//                        Decimal PriceCurrent = 0;
		//                        if (String.IsNullOrEmpty(item[(Int32)csvColumns.Price_Current].ToString()) == false)
		//                            PriceCurrent = Convert.ToDecimal(item[(Int32)csvColumns.Price_Current]);

		//                        objProduct.Price_Current = PriceCurrent;
		//                        objProduct.Price_WholeSale = wholeSale;
		//                        objProduct.Reserved = Convert.ToString(item[(Int32)csvColumns.Reserved]);
		//                        objProduct.Stock = item[(Int32)csvColumns.Stock].ToString();
		//                        int Mileage;

		//                        if (Int32.TryParse(item[(Int32)csvColumns.Mileage].ToString(), out   Mileage))
		//                        {
		//                            objProduct.Mileage = Mileage;
		//                        }


		//                        objProduct.Model = prdModel.id;//---------------
		//                        objProduct.Body = prdBody.id;//---------------
		//                        objProduct.VIN = item[(Int32)csvColumns.VIN].ToString();
		//                        objProduct.Year = Convert.ToInt32(item[(Int32)csvColumns.Year].ToString());
		//                        objProduct.Name = item[(Int32)csvColumns.Make].ToString() + " " + item[(Int32)csvColumns.Model].ToString() + " " + objProduct.VIN + " " + objProduct.Year + " " + item[(Int32)csvColumns.DealerCity].ToString();
		//                        objProduct.Price_Cost = Price;
		//                        objProduct.Title = "";

		//                        objProduct.Condition = item[(Int32)csvColumns.Condition].ToString();
		//                        objProduct.Exterior_Color = item[(Int32)csvColumns.Exterior].ToString();
		//                        objProduct.Interior_Color = item[(Int32)csvColumns.Interior].ToString();
		//                        Int32? Doors = String.IsNullOrEmpty(item[(Int32)csvColumns.Doors].ToString()) ? 0 : Convert.ToInt32(item[(Int32)csvColumns.Doors]);
		//                        objProduct.Doors = Doors;
		//                        objProduct.Engine = item[(Int32)csvColumns.Engine].ToString();
		//                        objProduct.Transmission = item[(Int32)csvColumns.Transmission].ToString();
		//                        objProduct.Fuel_Type = item[(Int32)csvColumns.Fuel_Type].ToString();
		//                        objProduct.Drive_Type = item[(Int32)csvColumns.Drive_Type].ToString();
		//                        //objProduct.Options = item[(Int32)csvColumns.Options].ToString();
		//                        objProduct.Warranty = (item[(Int32)csvColumns.Warranty].ToString().ToLower() == "yes");
		//                        objProduct.Description = item[(Int32)csvColumns.Description].ToString();
		//                        objProduct.Pics = item[(Int32)csvColumns.Pics].ToString();

		//                        if(String.IsNullOrEmpty(item[(Int32)csvColumns.Date_In_Stock].ToString().Trim())==false)
		//                        {
		//                            objProduct.Date_in_Stock = Convert.ToDateTime(item[(Int32)csvColumns.Date_In_Stock]);
		//                        }
		//                        else
		//                        {  
		//                            objProduct.Date_in_Stock =DateTime.Now;
		//                        }

		//                        objProduct.CreatedOn = DateTime.Now;
		//                        objProduct.UpdatedOn = DateTime.Now;

		//                        if (String.IsNullOrEmpty(item[(Int32)csvColumns.DealerZip].ToString()))
		//                            objProduct.zip = null;
		//                        else
		//                            objProduct.zip = Convert.ToInt32(item[(Int32)csvColumns.DealerZip]);
		//                        //--------------
		//                        //---------------
		//                        //-------------------------------

		//                        #region "addCustomer"

		//                        var customer = _edriveEntity.Customer.Where(m => m.Email == Email).SingleOrDefault();
		//                        if (customer == null)
		//                        {
		//                            customer = new Customer();
		//                            customer.Password = customer.Email = Email;
		//                            customer.Active = true;
		//                            customer.CustomerGUID = Guid.NewGuid();
		//                            customer.RegistrationDate = DateTime.Now;
		//                            customer.ExpiryDate = DateTime.Now.AddMonths(1);
		//                            customer.IsTrial = true;
		//                            customer.Name = item[(Int32)csvColumns.DealerName].ToString();
		//                            customer.FirstName = customer.Name;
		//                            customer.LastName = "";
		//                            customer.Company = item[(Int32)csvColumns.DealerCompany].ToString();
		//                            customer.CustomerType = 1;
		//                            customer.StreetAddress = item[(Int32)csvColumns.DealerAddress].ToString();
		//                            //-------add customer's State------------
		//                            var csvState=item[(Int32)csvColumns.DealerState].ToString();
		//                            var stateName = _edriveEntity.StateProvince.FirstOrDefault(m => m.Abbreviation == csvState);
		//                            if (stateName != null)
		//                            {
		//                                customer.Stateid = stateName.StateProvinceID;

		//                            }
		//                            //-------end of add customer State------------

		//                            customer.City = item[(Int32)csvColumns.DealerCity].ToString();
		//                            if (String.IsNullOrEmpty(item[(Int32)csvColumns.DealerZip].ToString()) == false)
		//                                customer.ZipPostalCode = Int32.Parse(item[(Int32)csvColumns.DealerZip].ToString());
		//                            customer.Phone = item[(Int32)csvColumns.DealerTelephone].ToString();

		//                            _edriveEntity.Customer.AddObject(customer);
		//                            //_edriveEntityEntity.SaveChanges();

		//                        }
		//                        #endregion
		//                        //-------------------------------use of third party services
		//                        //var dealer = GetDealerByDealerID(Prd.customerId);
		//                        var CityName = item[(Int32)csvColumns.DealerCity].ToString();
		//                        objProduct.Name = item[(Int32)csvColumns.Make].ToString() + " " + item[(Int32)csvColumns.Model].ToString()
		//                            + " " + VIN + " " + objProduct.Year + " " + CityName + ((customer.ZipPostalCode ?? 0) == 0 ? "" : " " + (customer.ZipPostalCode??0).ToString());
		//                        NADA_UsedCars.UsedCars oUsedCarService = new NADA_UsedCars.UsedCars();
		//                        NADA_UsedCars.UsedCarResultSet oUsedCarsResultSet = oUsedCarService.GetUsedCars("EdriveAutos", "ed12uc20", VIN);
		//                        Nada_UsedCarPrices.UsedCarPrices oUsedCarPrices = new Nada_UsedCarPrices.UsedCarPrices();
		//                        UsedCar uc = oUsedCarsResultSet.UsedCars.FirstOrDefault();
		//                        if (uc != null)
		//                        {
		//                            objProduct.City_Fuel = uc.CityFuelHigh;
		//                            objProduct.Highway_Fuel = uc.HighwayFuelHigh;
		//                            objProduct.AverageRetailPrice = uc.AverageRetailPrice;
		//                            objProduct.AverageTradeinPrice = uc.AverageTradeinPrice;

		//                        }
		//                        //------------
		//                        objProduct.CustomerID = customer.CustomerID;


		//                        if (Status == ProductStatus.NewRecordInserted)
		//                        {
		//                            _edriveEntity.AddToProduct(objProduct);
		//                            _edriveEntity.SaveChanges();
		//                            AddProductOptions(item[(Int32)csvColumns.Options].ToString(), objProduct.ProductId);

		//                            AddPicsToProduct(objProduct.Pics.Split(';'), objProduct.ProductId);
		//                        }
		//                        else
		//                        { //Update Product Option
		//                            UpdateProudctOption(item[(Int32)csvColumns.Options].ToString(), objProduct.ProductId);

		//                        }

		//                    }
		//                    else
		//                    {//--delete existing records for which nada validation has failed.
		//                        var delProduct = _edriveEntity.Product.Where(m => m.VIN == VIN).SingleOrDefault();
		//                        if (delProduct != null)
		//                        {
		//                            _edriveEntity.Product.DeleteObject(delProduct);
		//                            _edriveEntity.SaveChanges();

		//                        }

		//                    }
		//                }
		//                else
		//                {
		//                    //VIN = "";
		//                    Status = ProductStatus.JunkRecord; //Junk record
		//                }

		//                //-----------insert rec to  product Log table
		//                _edriveEntity.AddToED_DataImportLog(new EdriveService.ED_DataImportLog
		//                {
		//                    CreatedDate = DateTime.Now,
		//                    Description = Status.ToString(),
		//                    Status = (Int32)Status,
		//                    FileName = fileName,
		//                    VIN = VIN
		//                });
		//                //----------
		//                _edriveEntity.SaveChanges();


		//            }
		//            catch (Exception ex)
		//            {


		//            }
		//        }


		//        return;
		//    }
		//    catch (Exception ex)
		//    {


		//    }

		//}

		//---------------Stream reader code ------------
		//private void CopyDataToDestination(StreamReader table, String fileName)
		//{
		//    String VIN;

		//    try
		//    {

		//        List<Product> lstProduct = new List<Product>();
		//        table.ReadLine();//to read header rows

		//        while (!table.EndOfStream)
		//        {
		//            var item = table.ReadLine().Split(',');
		//            ProductStatus Status = ProductStatus.JunkRecord;
		//            for (int i = 0; i < item.Length; i++)
		//            {
		//                item[i] = item[i].Trim('"');//remove quotes from beg and end

		//            }
		//            try
		//            {
		//                VIN = item[(Int32)csvColumns.VIN].ToString();
		//                String Email = item[(Int32)csvColumns.DealerEmail].ToString();
		//                if (VIN.Length > 0 && item[(Int32)csvColumns.Price_Current].ToString().Length > 0 && item[(Int32)csvColumns.Price_Current].ToString() != "0" && Email.Length > 0)
		//                {
		//                    decimal Price_Current = 0;
		//                    Boolean PriceCastingSuccess = true;
		//                    try
		//                    {
		//                        Price_Current = decimal.Parse(item[(Int32)csvColumns.Price_Current].ToString());
		//                    }
		//                    catch
		//                    {
		//                        Status = ProductStatus.Price_CastingError;
		//                        PriceCastingSuccess = false;
		//                    }

		//                    if (PriceCastingSuccess & ValidRecord(VIN, Price_Current, ref Status))//--for live
		//                    //if (PriceCastingSuccess)//-for testing
		//                    {

		//                        Product objProduct;
		//                        objProduct = _edriveEntity.Product.Where(m => m.VIN == VIN).SingleOrDefault();

		//                        if (objProduct == null)
		//                        {
		//                            objProduct = new Product();
		//                            Status = ProductStatus.NewRecordInserted;
		//                        }
		//                        else
		//                        {
		//                            Status = ProductStatus.RecordUpdated;
		//                        }


		//                        //objProduct.=item["Make"]
		//                        Product_Make _prdMake;
		//                        var Make = item[(Int32)csvColumns.Make].ToString();
		//                        _prdMake = _edriveEntity.Product_Make.Where(m => m.Make == Make).SingleOrDefault();
		//                        if (_prdMake == null)
		//                        {
		//                            _prdMake = new Product_Make();
		//                            _prdMake.Make = Make;
		//                            _edriveEntity.AddToProduct_Make(_prdMake);
		//                            //_edriveEntityEntity.SaveChanges();
		//                        }


		//                        var Model = item[(Int32)csvColumns.Model].ToString();

		//                        var prdModel = _edriveEntity.Product_Model.Where(m => m.ModeLName == Model).SingleOrDefault();
		//                        if (prdModel == null)
		//                        {
		//                            prdModel = new Product_Model();
		//                            prdModel.ModeLName = Model;
		//                            prdModel.MakeID = _prdMake.id;
		//                            _edriveEntity.AddToProduct_Model(prdModel);
		//                            //_edriveEntityEntity.SaveChanges();
		//                        }


		//                        objProduct.Trim = item[(Int32)csvColumns.Trim].ToString();
		//                        objProduct.Free_Text = item[(Int32)csvColumns.Free_Text].ToString();

		//                        var Body = item[(Int32)csvColumns.Body].ToString();

		//                        var prdBody = _edriveEntity.Product_Body.Where(m => m.Body == Body).SingleOrDefault();

		//                        if (prdBody == null)
		//                        {
		//                            prdBody = new Product_Body();
		//                            prdBody.Body = Body;
		//                            _edriveEntity.AddToProduct_Body(prdBody);
		//                            //_edriveEntityEntity.SaveChanges();
		//                        }
		//                        var Type = item[(Int32)csvColumns.Type].ToString();
		//                        Product_Type prdType = _edriveEntity.Product_Type.Where(m => m.Type == Type).SingleOrDefault();
		//                        if (prdType == null)
		//                        {
		//                            prdType = new Product_Type();
		//                            prdType.Type = Type;
		//                            _edriveEntity.AddToProduct_Type(prdType);
		//                            //_edriveEntityEntity.SaveChanges();
		//                        }


		//                        double wholeSale = 0;
		//                        bool abc = double.TryParse(item[(Int32)csvColumns.Price_Wholesale].ToString(), out wholeSale);

		//                        double Price = 0;

		//                        bool _price = double.TryParse(item[(Int32)csvColumns.Price_Cost].ToString(), out Price);
		//                        objProduct.Type = prdType.id;
		//                        Decimal PriceCurrent = 0;
		//                        if (String.IsNullOrEmpty(item[(Int32)csvColumns.Price_Current].ToString()) == false)
		//                            PriceCurrent = Convert.ToDecimal(item[(Int32)csvColumns.Price_Current]);

		//                        objProduct.Price_Current = PriceCurrent;
		//                        objProduct.Price_WholeSale = wholeSale;
		//                        objProduct.Reserved = Convert.ToString(item[(Int32)csvColumns.Reserved]);
		//                        objProduct.Stock = item[(Int32)csvColumns.Stock].ToString();
		//                        int Mileage;

		//                        if (Int32.TryParse(item[(Int32)csvColumns.Mileage].ToString(), out   Mileage))
		//                        {
		//                            objProduct.Mileage = Mileage;
		//                        }


		//                        objProduct.Model = prdModel.id;//---------------
		//                        objProduct.Body = prdBody.id;//---------------
		//                        objProduct.VIN = item[(Int32)csvColumns.VIN].ToString();
		//                        objProduct.Year = Convert.ToInt32(item[(Int32)csvColumns.Year].ToString());
		//                        objProduct.Name = item[(Int32)csvColumns.Make].ToString() + " " + item[(Int32)csvColumns.Model].ToString();
		//                        objProduct.Price_Cost = Price;
		//                        objProduct.Title = "";

		//                        objProduct.Condition = item[(Int32)csvColumns.Condition].ToString();
		//                        objProduct.Exterior_Color = item[(Int32)csvColumns.Exterior].ToString();
		//                        objProduct.Interior_Color = item[(Int32)csvColumns.Interior].ToString();
		//                        Int32? Doors = String.IsNullOrEmpty(item[(Int32)csvColumns.Doors].ToString()) ? 0 : Convert.ToInt32(item[(Int32)csvColumns.Doors]);
		//                        objProduct.Doors = Doors;
		//                        objProduct.Engine = item[(Int32)csvColumns.Engine].ToString();
		//                        objProduct.Transmission = item[(Int32)csvColumns.Transmission].ToString();
		//                        objProduct.Fuel_Type = item[(Int32)csvColumns.Fuel_Type].ToString();
		//                        objProduct.Drive_Type = item[(Int32)csvColumns.Drive_Type].ToString();
		//                        //objProduct.Options = item[(Int32)csvColumns.Options].ToString();
		//                        objProduct.Warranty = (item[(Int32)csvColumns.Warranty].ToString().ToLower() == "yes");
		//                        objProduct.Description = item[(Int32)csvColumns.Description].ToString();
		//                        objProduct.Pics = item[(Int32)csvColumns.Pics].ToString();
		//                        objProduct.Date_in_Stock = Convert.ToDateTime(item[(Int32)csvColumns.Date_In_Stock]);
		//                        objProduct.CreatedOn = DateTime.Now;
		//                        objProduct.UpdatedOn = DateTime.Now;

		//                        if (String.IsNullOrEmpty(item[(Int32)csvColumns.DealerZip].ToString()))
		//                            objProduct.zip = null;
		//                        else
		//                            objProduct.zip = Convert.ToInt32(item[(Int32)csvColumns.DealerZip]);
		//                        //-------------------------------

		//                        #region "addCustomer"

		//                        var customer = _edriveEntity.Customer.Where(m => m.Email == Email).SingleOrDefault();
		//                        if (customer == null)
		//                        {
		//                            customer = new Customer();
		//                            customer.Password = customer.Email = Email;
		//                            customer.Active = true;
		//                            customer.CustomerGUID = Guid.NewGuid();
		//                            customer.RegistrationDate = DateTime.Now;
		//                            customer.ExpiryDate = DateTime.Now.AddMonths(1);
		//                            customer.IsTrial = true;
		//                            customer.Name = item[(Int32)csvColumns.DealerName].ToString();
		//                            customer.Company = item[(Int32)csvColumns.DealerCompany].ToString();
		//                            customer.CustomerType = 1;
		//                            customer.StreetAddress = item[(Int32)csvColumns.DealerAddress].ToString();
		//                            customer.City = item[(Int32)csvColumns.DealerCity].ToString();
		//                            if (String.IsNullOrEmpty(item[(Int32)csvColumns.DealerZip].ToString()) == false)
		//                                customer.ZipPostalCode = Int32.Parse(item[(Int32)csvColumns.DealerZip].ToString());
		//                            customer.Phone = item[(Int32)csvColumns.DealerTelephone].ToString();
		//                            _edriveEntity.Customer.AddObject(customer);
		//                            //_edriveEntityEntity.SaveChanges();

		//                        }
		//                        #endregion
		//                        //-------------------------------use of third party services

		//                        NADA_UsedCars.UsedCars oUsedCarService = new NADA_UsedCars.UsedCars();
		//                        NADA_UsedCars.UsedCarResultSet oUsedCarsResultSet = oUsedCarService.GetUsedCars("EdriveAutos", "ed12uc20", VIN);
		//                        Nada_UsedCarPrices.UsedCarPrices oUsedCarPrices = new Nada_UsedCarPrices.UsedCarPrices();
		//                        UsedCar uc = oUsedCarsResultSet.UsedCars.FirstOrDefault();
		//                        if (uc != null)
		//                        {
		//                            objProduct.City_Fuel = uc.CityFuelHigh;
		//                            objProduct.Highway_Fuel = uc.HighwayFuelHigh;
		//                            objProduct.AverageRetailPrice = uc.AverageRetailPrice;
		//                            objProduct.AverageTradeinPrice = uc.AverageTradeinPrice;

		//                        }
		//                        //------------
		//                        objProduct.CustomerID = customer.CustomerID;


		//                        if (Status == ProductStatus.NewRecordInserted)
		//                        {
		//                            _edriveEntity.AddToProduct(objProduct);
		//                            _edriveEntity.SaveChanges();
		//                        }
		//                        AddProductOptions(item[(Int32)csvColumns.Options].ToString(), objProduct.ProductId);


		//                    }
		//                    else
		//                    {//--delete existing records for which nada validation has failed.
		//                        var delProduct = _edriveEntity.Product.Where(m => m.VIN == VIN).SingleOrDefault();
		//                        if (delProduct != null)
		//                        {
		//                            _edriveEntity.Product.DeleteObject(delProduct);
		//                            _edriveEntity.SaveChanges();
		//                        }

		//                    }
		//                }
		//                else
		//                {
		//                    VIN = "";
		//                    Status = ProductStatus.JunkRecord; //Junk record
		//                }

		//                //-----------insert rec to  product Log table
		//                _edriveEntity.AddToED_DataImportLog(new EdriveService.ED_DataImportLog
		//                {
		//                    CreatedDate = DateTime.Now,
		//                    Description = Status.ToString(),
		//                    Status = (Int32)Status,
		//                    FileName = fileName,
		//                    VIN = VIN
		//                });
		//                //----------
		//                _edriveEntity.SaveChanges();

		//            }
		//            catch (Exception ex)
		//            {


		//            }
		//        }


		//        return;
		//    }
		//    catch (Exception ex)
		//    {


		//    }

		//}

		/// <summary>
		/// return all list of Product Options
		/// </summary>
		/// <returns></returns>
		public List<EdriveService.DataContract.ProductOptions> GetAllProductOptions()
		{
			return _edriveEntity.ProductOption.Where(m => m.OptionName != "").Select(m => new EdriveService.DataContract.
				ProductOptions { id = m.id, OptionName = m.OptionName }).ToList();

		}
		//public String[] GetProductOptions(Int32 ProductID)
		//{
		//    return  GetAllProductOptions_By_ProductID(ProductID);
		//   // return _edriveEntity.ProductOption.Where(m => m.ProductID == ProductID).Select(m => m.OptionName).ToArray();
		//}
		public List<EdriveService.DataContract.ProductOptions> GetAllProductOptions_By_ProductID(Int32 ProductID)
		{

			return _edriveEntity.ProductOptionMapping.Where(m => m.ProductID == ProductID).
   Select(m => new EdriveService.DataContract.ProductOptions { id = m.ProductOption.id, OptionName = m.ProductOption.OptionName }).ToList();


		}
		/// <summary>
		/// To update the Product's option of existing product
		/// </summary>
		/// <param name="OPTIONS"></param>
		/// <param name="ProductID"></param>
		public void UpdateProudctOption(string OPTIONS, Int32 ProductID)
		{
			try
			{
				if(!String.IsNullOrEmpty(OPTIONS))
				{

					var opt = OPTIONS.Split(';');
					//------------delete previouus option Mapping---
					var delProductOption = _edriveEntity.ProductOptionMapping.Where(m => m.ProductID == ProductID);
					foreach(var item in delProductOption)
					{
						_edriveEntity.ProductOptionMapping.DeleteObject(item);
					}
					_edriveEntity.SaveChanges();
					for(int i = 0; i < opt.Length; i++)
					{
						if(!String.IsNullOrEmpty(opt[i]))
						{
							var optname = opt[i];
							if(_edriveEntity.ProductOption.Any(m => m.OptionName == optname) == false)//add new Ooption
							{
								var Opt = new ProductOption { OptionName = opt[i] };
								_edriveEntity.ProductOption.AddObject(Opt);
								_edriveEntity.ProductOptionMapping.AddObject(new ProductOptionMapping { ProductID = ProductID, OptionID = Opt.id });
								_edriveEntity.SaveChanges();
							}
							else //option exist n check for option mapping
							{

								var OptionID = (_edriveEntity.ProductOption.First(m => m.OptionName == optname)).id;
								//--delete previous mapping-- if not exist


								if(_edriveEntity.ProductOptionMapping.Any(m => m.OptionID == OptionID && m.ProductID == ProductID) == false) // if object not exitst add new option mapping
								{
									_edriveEntity.ProductOptionMapping.AddObject(new ProductOptionMapping { ProductID = ProductID, OptionID = OptionID });
									_edriveEntity.SaveChanges();
								}
							}

						}
					}


				}

			}

			catch(Exception ex)
			{


			}
		}


		/// <summary>
		/// It adds the new product option to new product
		/// </summary>
		/// <param name="OPTIONS"></param>
		/// <param name="ProductID"></param>
		public void AddProductOptions(string OPTIONS, Int32 ProductID)
		{
			try
			{


				if(!String.IsNullOrEmpty(OPTIONS))
				{
					var opt = OPTIONS.Split(';');
					for(int i = 0; i < opt.Length; i++)
					{
						if(!String.IsNullOrEmpty(opt[i]))
						{
							var optname = opt[i];
							if(_edriveEntity.ProductOption.Any(m => m.OptionName == optname) == false)
							{
								var Opt = new ProductOption { OptionName = opt[i] };
								_edriveEntity.ProductOption.AddObject(Opt);
								_edriveEntity.ProductOptionMapping.AddObject(new ProductOptionMapping { ProductID = ProductID, OptionID = Opt.id });
								_edriveEntity.SaveChanges();
							}
							else //option exist n check for option mapping
							{
								var OptionID = (_edriveEntity.ProductOption.First(m => m.OptionName == optname)).id;
								if(_edriveEntity.ProductOptionMapping.Any(m => m.OptionID == OptionID && m.ProductID == ProductID) == false) // if object not exitst add new option mapping
								{
									_edriveEntity.ProductOptionMapping.AddObject(new ProductOptionMapping { ProductID = ProductID, OptionID = OptionID });
									_edriveEntity.SaveChanges();
								}
							}

						}
					}

				}

			}

			catch(Exception ex)
			{


			}
		}

		/// <summary>
		/// Unzip the zip file
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="zipFileName"></param>
		/// <returns></returns>
		private string UnzipFile(string filePath, string zipFileName)
		{
			string unzippedFileName = "";
			using(var s = new ZipInputStream(File.OpenRead(filePath + zipFileName)))
			{
				ZipEntry theEntry;
				while((theEntry = s.GetNextEntry()) != null)
				{
					string directoryName = Path.GetDirectoryName(theEntry.Name);
					string fileName = Path.GetFileName(theEntry.Name);

					if(directoryName.Length > 0)
					{
						Directory.CreateDirectory(filePath + directoryName);
					}

					if(fileName != String.Empty)
					{
						using(FileStream streamWriter = File.Create(filePath + theEntry.Name))
						{
							unzippedFileName = filePath + theEntry.Name;
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
					}
				}
			}
			return unzippedFileName;
		}

		#endregion
















		#region "Search Services"
		/// <summary>
		/// Method used for filter searching on search page
		/// </summary>
		/// <param name="Price"></param>
		/// <param name="Milage"></param>
		/// <param name="Make"></param>
		/// <param name="Year"></param>
		/// <param name="Type"></param>
		/// <param name="Zip"></param>
		/// <param name="sortByColumn"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <param name="CarsCount">if null it updates the Cars Count else not update the Cars Count</param>
		public List<Products> SearchCars(String Price, String Milage, String Make, String ModelID,
			String Year, String Body, String Type, Int32 Zip, String Warranty, String Vin, String Transmission, String Engine, String DriveType, String sortByColumn,
			Int32 pageSize, Int32 pageIndex, ref Int32? CarsCount)
		{
			pageIndex = pageIndex == 0 ? 1 : pageIndex + 1;//for first page condition
			pageIndex--;
			if(pageSize <= 0)
				pageSize = 25;
			var SkipRecords = pageIndex * pageSize;

			String expression = BuildExpressionFor_SearchCars(ref Price, ref Milage, ref Make, ref 
                ModelID, ref Year, ref Body, ref Type, Zip,
				Warranty, ref Vin, Transmission, Engine, ref DriveType, ref pageSize, ref pageIndex);

			List<Products> lstModel;

			if(sortByColumn.ToLower() == "year" || sortByColumn.ToLower() == "mileage" ||
				sortByColumn.ToLower() == "price")
			{//---sort the result---
				var Model = new List<Product>();
				switch(sortByColumn.ToLower())
				{
					case "year": Model = _edriveEntity.Product.Where
				(expression).OrderBy(m => m.Year).Skip(SkipRecords).Take(pageSize).ToList();
						break;
					case "mileage": Model = _edriveEntity.Product.Where
			(expression).OrderBy(m => m.Mileage).Skip(SkipRecords).Take(pageSize).ToList();
						break;

					case "price": Model = _edriveEntity.Product.Where
		(expression).OrderBy(m => m.Price_Current).Skip(SkipRecords).Take(pageSize).ToList();
						break;


				}


				lstModel = Model.Select(m => new EdriveService.DataContract.Products
				{

					pics = m.Pics,
					mileage = m.Mileage ?? 0,
					body = m.Product_Body.Body,
					transmission = m.Transmission,
					exterior = m.Exterior_Color,
					drive_Type = m.Drive_Type,
					zip = m.zip ?? 0,
					Year = m.Year ?? 0,
					vin = m.VIN,
					MakeName = m.Product_Model.Product_Make.Make,
					ModelName = m.Product_Model.ModeLName,
					price_Current = m.Price_Current ?? 0,
					OwnerDetail = m.OwnerDetail,
					model = m.Model,
					updatedOn = m.UpdatedOn,
				}).ToList();
			}
			else
			{//---nosort result---


				var Model = _edriveEntity.Product.Where
					  (expression).OrderBy(m => m.ProductId).Skip(SkipRecords).Take(pageSize).ToList();

				lstModel = Model.Select(m => new EdriveService.DataContract.Products
				{
					productId = m.ProductId,
					pics = m.Pics,
					mileage = m.Mileage ?? 0,
					body = m.Product_Body.Body,
					transmission = m.Transmission,
					exterior = m.Exterior_Color,
					drive_Type = m.Drive_Type,
					MakeName = m.Product_Model.Product_Make.Make,
					ModelName = m.Product_Model.ModeLName,
					OwnerDetail = m.OwnerDetail,
					zip = m.zip ?? 0,
					Year = m.Year ?? 0,
					model = m.Model,
					price_Current = m.Price_Current ?? 0,
					updatedOn = m.UpdatedOn,
				}).ToList();

			}

			if(CarsCount == null)
				CarsCount = _edriveEntity.Product.Where(expression).Count();
			return lstModel;
		}

		public Int32 SearchCars_Count(String Price, String Milage, String Make, String ModelID, String Year,
			String Body, String Type, Int32 Zip, String Warranty, String Vin, String Transmission, String Engine, String DriveType, String sortByColumn, Int32 pageSize, Int32 pageIndex)
		{
			String expression = BuildExpressionFor_SearchCars(ref Price, ref Milage, ref Make,
				ref ModelID, ref Year, ref Body, ref Type, Zip, Warranty, ref Vin, Transmission, Engine, ref DriveType
				, ref pageSize, ref pageIndex);


			return _edriveEntity.Product.Where(expression).Count();

		}

		private String BuildExpressionFor_SearchCars(ref String Price, ref String Milage, ref String Make,
			ref string ModelID, ref String Year, ref String Body, ref String Type, Int32 Zip, String Warranty,
			ref String Vin, String Transmission, String Engine, ref String DriveType, ref Int32 pageSize, ref Int32 pageIndex)
		{
			if(ModelID.Contains(','))
				ModelID = ModelID.Remove(ModelID.IndexOf(','));
			pageIndex = pageIndex == 0 ? 1 : pageIndex + 1;//for first page condition
			pageIndex--;
			if(pageSize <= 0)
				pageSize = 25;

			var SkipRecords = pageIndex * pageSize;
			if(String.IsNullOrEmpty(ModelID))
				ModelID = "-1";

			if(Make.IndexOf(',') > 0)
				Make = Make.Substring(0, Make.LastIndexOf(','));
			if(Year.IndexOf(',') > 0)
				Year = Year.Substring(0, Year.LastIndexOf(','));
			if(Body.IndexOf(',') > 0)
				Body = Body.Substring(0, Body.LastIndexOf(','));
			if(Type.IndexOf(',') > 0)
				Type = Type.Substring(0, Type.LastIndexOf(','));
			String expression = "it.Pics!=''   and it.Deleted=false and it.IsQualified = true and it.CustomerID is not null and true ";

			//var countDownDaysSettings = _edriveEntity.Settings.FirstOrDefault(c => c.Name == "CountDown.Days");
			//int countDownDays = countDownDaysSettings != null ? Convert.ToInt32(countDownDaysSettings.Value) : 28;

			//expression += String.Format(" and SqlServer.DATEDIFF('minute', SqlServer.getdate(), SqlServer.DateAdd('day', {0}, it.updatedon)) > 0 ", countDownDays);



			if(!String.IsNullOrEmpty(Price))
			{
				if(Price.IndexOf(',') > 0)
					Price = Price.Substring(0, Price.LastIndexOf(','));
				String[] arPrice = Price.Split(',');

				var PriceExp = "";
				if(arPrice[0] != "-1")
				{
					for(int i = 0; i < arPrice.Length; i++)
					{
						PriceExp = PriceExp + " ( it.Price_Current>=" + (Int32.Parse(arPrice[i].Split('-')[0])) + " and it.Price_Current<=" + (Int32.Parse(arPrice[i].Split('-')[1])) + " ) ";
						if(i != arPrice.Length - 1)// not at last add or
							PriceExp += " or ";
					}

				}
				if(String.IsNullOrEmpty(PriceExp) == false)
				{
					expression += String.Format(" and ( {0} ) ", PriceExp);
				}
			}
			if(!String.IsNullOrEmpty(Milage))
			{
				if(Milage.IndexOf(',') > 0)
					Milage = Milage.Substring(0, Milage.LastIndexOf(','));
				String[] arMilage = Milage.Split(',');
				var MileageExp = "";
				if(arMilage[0] != "-1")
				{

					for(int i = 0; i < arMilage.Length; i++)
					{


						MileageExp = MileageExp + " ( it.Mileage>=" + (Int32.Parse(arMilage[i].Split('-')[0])) + " and it.Mileage<=" + (Int32.Parse(arMilage[i].Split('-')[1])) + " ) ";
						if(i != arMilage.Length - 1)// not at last add or
							MileageExp += " or ";

					}

				}

				if(String.IsNullOrEmpty(MileageExp) == false)
				{
					expression += String.Format(" and ( {0} ) ", MileageExp);
				}
			}
			//---------------
			if(String.IsNullOrEmpty(Make) == false)
			{
				if(Convert.ToInt32(ModelID) <= 0 && String.IsNullOrEmpty(Make) == false)// Model is not set
				{
					var MakeExp = "";

					var arMake = Make.Split(',');
					{
						if(arMake[0] != "-1")
						{
							for(int i = 0; i < arMake.Length; i++)
							{
								var makeID = Convert.ToInt32(arMake[i]);
								Int32[] arModelID = _edriveEntity.Product_Model.Where(m => m.MakeID == makeID).Select(m => m.id).ToArray();
								for(int j = 0; j < arModelID.Length; j++)
								{

									MakeExp += "  it.Model=" + arModelID[j].ToString() + " or ";


								}
							}

						}
					}


					if(String.IsNullOrEmpty(MakeExp) == false)
					{
						MakeExp = MakeExp.Remove(MakeExp.LastIndexOf("or"));//this line is added only for model expression
						expression += String.Format(" and ( {0} ) ", MakeExp);
					}
				}
				else
				{
					ModelID = ModelID.IndexOf(",") > 0 ? ModelID.Substring(0, ModelID.IndexOf(",")) : ModelID;
					expression += String.Format(" and  it.Model={0}  ", ModelID);
				}
			}



			//-----------------
			if(String.IsNullOrEmpty(Year) == false)
			{
				var YearExp = "";
				String[] arYear = Year.Split(',');
				if(arYear[0] != "-1")
				{

					for(int i = 0; i < arYear.Length; i++)
					{
						YearExp += " it.Year=" + Int32.Parse(arYear[i]);
						if(i != arYear.Length - 1)// not at last add or
							YearExp += " or ";
					}

				}
				if(String.IsNullOrEmpty(YearExp) == false)
				{
					expression += String.Format(" and ( {0} ) ", YearExp);
				}
			}
			//------------------
			if(String.IsNullOrEmpty(Body) == false)
			{
				var BodyExp = "";
				String[] arBody = Body.Split(',');
				if(arBody[0] != "-1")
				{

					for(int i = 0; i < arBody.Length; i++)
					{
						BodyExp += " it.Body=" + Int32.Parse(arBody[i]);
						if(i != arBody.Length - 1)// not at last add or
							BodyExp += " or ";
					}

				}
				if(String.IsNullOrEmpty(BodyExp) == false)
				{
					expression += String.Format(" and ( {0} ) ", BodyExp);
				}
			}
			//-------------

			if(String.IsNullOrEmpty(Type) == false)
			{

				var TypeExp = "";
				String[] arType = Type.Split(',');
				if(arType[0] != "-1")
				{

					for(int i = 0; i < arType.Length; i++)
					{
						TypeExp += " it.Type=" + Int32.Parse(arType[i]);
						if(i != arType.Length - 1)// not at last add or
							TypeExp += " or ";
					}

				}
				if(String.IsNullOrEmpty(TypeExp) == false)
				{
					expression += String.Format(" and ( {0} ) ", TypeExp);
				}
			}

			if(Zip >= 0)// zip is specified
			{
				expression += " and " + "it.Customer.ZipPostalCode=" + Zip;
			}
			if(Warranty == "0" || Warranty == "1")
				expression += String.Format(" and it.Warranty={0} ", Warranty == "0" ? "false" : "true");

			if(Vin != "-1" && !String.IsNullOrEmpty(Vin))
			{
				expression += String.Format(" and ( it.VIN='{0}' ) ", Vin);
			}

			if(DriveType != "-1" && !String.IsNullOrEmpty(DriveType))
			{
				expression += String.Format(" and ( it.Drive_Type='{0}' ) ", DriveType);
			}

			if(!String.IsNullOrEmpty(Transmission) && Transmission != "-1")
				expression += String.Format(" and ( it.Transmission='{0}' ) ", Transmission);

			if(!String.IsNullOrEmpty(Engine) && Engine != "-1")
				expression += String.Format(" and ( it.Engine='{0}' ) ", Engine);
			return expression;
		}

		/// <summary>
		/// Method returns total cars list search result for AdvanceSearch operation
		/// </summary>
		/// <param name="_attributes"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <returns>List of Cars</returns>
		public List<EdriveService.DataContract.Products> GetAdvancedSearchProducts(AdvancedSearchAttributes _attributes, int pageSize, int pageIndex, String SortByColumn)
		{
			int descriptionLength = 150;

			if(pageIndex < 1)
				pageIndex = 1;
			else
				pageIndex += 1;

			int count;
			var result = SearchAdvanced(_attributes, pageSize, pageIndex, SortByColumn, out count);
			GetProductsPictures(result);
			TrimDescription(descriptionLength, result);

			return result;
		}
		/// <summary>
		/// Method returns cars search result for home page search operation
		/// </summary>
		/// <param name="makeID"></param>
		/// <param name="modelId"></param>
		/// <param name="zipCode"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <param name="CarsCount">if null it updates the Cars Count else not update the Cars Count</param>
		/// <returns></returns>
		public List<DataContract.Products> searchProduct(int makeID, int modelId, String zipCode, Int32 pageSize, Int32 pageIndex, ref Int32? CarsCount, String SortByColumn)
		{


			if(SortByColumn != "Year" && SortByColumn != "Mileage" && SortByColumn != "Price")
				SortByColumn = "ProductId";
			if(SortByColumn == "Price")
				SortByColumn = "Price_Current";
			pageIndex = pageIndex == 0 ? 1 : pageIndex;// for 1st page Index
			var SkipRecords = (pageIndex - 1) * pageSize;
			List<EdriveService.DataContract.Products> lstModel = null;
			String expression = " true ";
			if(makeID == -1)//search all model
			{
				Int32[] arModelID = _edriveEntity.Product_Model.Select(m => m.id).ToArray();
				String ModelIds = "";
				for(int i = 0; i < arModelID.Length; i++)
				{
					ModelIds += "it.Model=" + arModelID[i].ToString();
					if(i != arModelID.Length - 1)
					{
						ModelIds += " or ";
					}

				}
				if(ModelIds != "")
					expression = expression + "and  (" + ModelIds + ")";
			}
			else
			{
				if(modelId == -1)////search for specifie make and all its model
				{
					Int32[] arModelID = _edriveEntity.Product_Model.Where(m => m.MakeID == makeID).Select(m => m.id).ToArray();
					String ModelIds = "";
					for(int i = 0; i < arModelID.Length; i++)
					{
						ModelIds += "it.Model=" + arModelID[i].ToString();
						if(i != arModelID.Length - 1)
						{
							ModelIds += " or ";
						}

					}
					if(ModelIds != "")
						expression = expression + "and  (" + ModelIds + ")";
				}
				else//search for specifi model
				{

					expression = expression + "and  (it.Model=" + modelId + ")";
				}
			}

			if(zipCode != "-1" && String.IsNullOrEmpty(zipCode) == false)
			{
				expression = expression + "and  ( it.Zip=" + zipCode + ")";

			}



			var Model = _edriveEntity.Product.Where(expression).OrderBy("it." + SortByColumn).Skip(SkipRecords).Take(pageSize).ToList();
			lstModel = Model.Select(m => new EdriveService.DataContract.Products
			{
				productId = m.ProductId,
				pics = m.Pics,
				mileage = m.Mileage ?? 0,
				body = m.Product_Body.Body,
				price_Current = m.Price_Current ?? 0,
				transmission = m.Transmission,
				exterior = m.Exterior_Color,
				OwnerDetail = m.OwnerDetail,
				drive_Type = m.Drive_Type,
				vin = m.VIN,
				MakeName = m.Product_Model.Product_Make.Make,
				ModelName = m.Product_Model.ModeLName,
				zip = m.zip ?? 0,
				Year = m.Year ?? 0,
				model = m.Model,
				updatedOn = m.UpdatedOn,
			}).ToList();
			if(CarsCount == null) CarsCount = _edriveEntity.Product.Where(expression).Count();




			return lstModel;

		}
		/// <summary>
		/// Return Adavance Search Expression for Advance Search Result
		/// </summary>
		/// <param name="_attributes"></param>
		/// <returns></returns>
		private string GetAdvSearchExp(AdvancedSearchAttributes _attributes)
		{
			//declare local variables
			string expression = string.Empty;
			int _body = -1;
			string _engine = string.Empty;
			int _mileageFrom = -1;
			int _mileageTo = -1;
			string _transmission = string.Empty;
			//Int32 _zip = -1;
			string _vin = string.Empty;
			string _radius = string.Empty;
			int _model = -1;
			int _minYaer = -1;
			int _maxYear = -1;
			decimal _minPrice = -1;
			decimal _maxPrice = -1;
			String _driveType = "-1";

			Int32 _Make = _attributes._make;

			//Initialize the Advenced search attributes
			var _type = _attributes._Type;
			_body = _attributes._body;
			_model = _attributes._model;
			_radius = _attributes._radius;
			_mileageFrom = _attributes._mileageFrom;
			_mileageTo = _attributes._mileageTo;
			_transmission = _attributes._transmission;
			_driveType = _attributes._driveType;
			_engine = _attributes._engine;
			_maxPrice = _attributes._maxPrice;
			_minPrice = _attributes._minPrice;
			_minYaer = Convert.ToInt32(_attributes._minYaer.ToString());
			_maxYear = Convert.ToInt32(_attributes._maxYear.ToString());
			//_zip = _radius == "-1" ? _attributes._zip : -1;
			_vin = _attributes._vin.ToString();
			//Search Product based on attributes
			expression = "it.Deleted=false and it.Pics!='' and it.CustomerID is not null and true ";

			//			var countDownDaysSettings = _edriveEntity.Settings.FirstOrDefault(c => c.Name == "CountDown.Days");
			//			int countDownDays = countDownDaysSettings != null ? Convert.ToInt32(countDownDaysSettings.Value) : 28;
			//
			//			expression += String.Format(" and SqlServer.DATEDIFF('minute', SqlServer.getdate(), SqlServer.DateAdd('day', {0}, it.updatedon)) > 0 ", countDownDays);

			if(_model != -1)
			{
				expression += " and it.Model=" + _model;
			}
			else
			{

				var Modelexp = "";
				if(_Make != -1)
				{
					Int32[] ModelIDs = _edriveEntity.Product_Model.Where(m => m.MakeID == _Make).Select(m => m.id).ToArray();
					for(int i = 0; i < ModelIDs.Length; i++)
					{

						Modelexp += " it.Model=" + ModelIDs[i].ToString();
						if(i != ModelIDs.Length - 1)
						{
							Modelexp += " or ";
						}

					}

				}
				if(!String.IsNullOrEmpty(Modelexp))
					expression += " and ( " + Modelexp + " ) ";

			}

			if(_mileageFrom != -1 && _mileageTo != -1)
			{
				expression += " and " + "( it.Mileage>=" + _mileageFrom + " and it.Mileage<=" + _mileageTo + ")";

			}
			if(_minYaer != -1 && _maxYear != -1)
			{
				expression += " and " + "( it.Year>=" + _minYaer + " and it.Year<=" + _maxYear + ")";

			}
			if(_minPrice != -1 && _maxPrice != -1)
			{
				expression += " and " + "( it.Price_Current>=" + _minPrice + " and it.Price_Current<=" + _maxPrice + ")";

			}

			if(_body != -1)
			{
				expression = expression + " and it.Body=" + _body;
			}
			if(_transmission != "-1")
			{
				expression = expression + " and it.Transmission='" + _transmission + "'";
			}
			if(_engine != "-1")
			{
				expression = expression + " and it.Engine='" + _engine + "'";
			}
			if(_driveType != "-1")
			{
				expression = expression + " and it.Drive_Type='" + _driveType + "'";
			}

			if(_vin != "-1" && !String.IsNullOrEmpty(_vin))
			{
				expression = expression + " and it.vin='" + _vin + "'";
			}
			//            if (_zip != -1)
			//            {
			//                expression = expression + " and it.Customer.ZipPostalCode=" + _zip;
			//            }
			if(_type != -1)
			{
				expression = expression + " and it.Type=" + _type;
			}

			return expression;
		}
		/// <summary>
		/// Method returns total cars count search result for AdvanceSearch operation
		/// </summary>
		/// <param name="_attributes"></param>
		/// <returns>List of Cars</returns>
		public Int32 GetAdvancedSearchProducts_Count(AdvancedSearchAttributes _attributes)
		{
			int count;
			SearchAdvanced(_attributes, 1, 1, null, out count);

			return count;
		}

		private List<Product> CheckRadius(List<Product> list, AdvancedSearchAttributes attributes)
		{
			if(attributes._zip == -1 && attributes._radius == "-1")
			{
				return list;
			}

			if(attributes._zip != -1 && attributes._radius == "-1")
			{
				return list.Where(it => it.zip == attributes._zip).ToList();
			}

			var zipItem = _edriveEntity.zip_code.First(it => it.zip_code1 == attributes._zip);

			if(zipItem == null)
			{
				//zipcode not found
				return list;
			}
			var miles = int.Parse(attributes._radius);

			var maxLatitude = zipItem.lattitude + miles / 69.17;
			var minLatitude = zipItem.lattitude - (maxLatitude - zipItem.lattitude);

			var maxLongitude = zipItem.longitude + miles / (Math.Cos(minLatitude * Math.PI / 180) * 69.17);
			var minLongitude = zipItem.longitude - (maxLongitude - zipItem.longitude);

			//Get zip codes in rectangle
			var zipCodesInSearchRaduis = _edriveEntity.zip_code.Where(it => it.lattitude > minLatitude && it.lattitude < maxLatitude && it.longitude > minLongitude && it.longitude < maxLongitude).ToList();
			//filter zip codes to circle
			zipCodesInSearchRaduis = CheckDistance(zipCodesInSearchRaduis, zipItem, miles);

			var zipArray = zipCodesInSearchRaduis.Select(it => it.zip_code1).ToList();

			if(!zipArray.Contains(zipItem.zip_code1))
			{
				zipArray.Add(zipItem.zip_code1);
			}
			var temp = list.Where(it => it.zip != null && zipArray.Contains((int)it.zip)).ToList();
			return list.Where(it => it.zip != null && zipArray.Contains((int)it.zip)).ToList();
		}
		private const int GeoRadiusMiles = 3960;
		private static List<zip_code> CheckDistance(List<zip_code> zipCodes, zip_code currentItem, int miles)
		{
			var result = new List<zip_code>();

			foreach(var code in zipCodes)
			{
				var distance = GetDistance(currentItem.lattitude, currentItem.longitude, code.lattitude, code.longitude);

				if(distance <= miles)
				{
					result.Add(code);
				}
			}

			return result;
		}

		private static int GetDistance(double lat1, double lon1, double lat2, double lon2)
		{
			double theta = lon1 - lon2;
			double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));
			if(dist >= 1)
			{
				//distance for point is 0
				return 0;
			}
			dist = Math.Acos(dist);
			dist = Rad2Deg(dist);
			dist = dist * 60 * 1.1515;
			return Convert.ToInt32(dist);
		}

		private static double Deg2Rad(double deg)
		{
			//convert decimal degrees to radians
			return (deg * Math.PI / 180.0);
		}
		private static double Rad2Deg(double rad)
		{
			//convert radians to decimal degrees
			return (rad / Math.PI * 180.0);
		}
		#endregion

		/// <summary>
		/// Return Product's Make Record
		/// </summary>
		/// <param name="makeId">make id whose records to be returned</param>
		/// <returns></returns>

		public DataContract.Product_Make GetMakeById(Int32 makeId)
		{
			return _edriveEntity.Product_Make.Where(m => m.id == makeId).Select(m => new DataContract.Product_Make { id = m.id, make = m.Make }).
				 SingleOrDefault();

		}
		/// <summary>
		/// Return Product's Body Values
		/// </summary>
		/// <param name="bodyid">BodId whose records to be returned</param>
		/// <returns></returns>
		public DataContract.Product_Body GetBodybyId(Int32 bodyid)
		{
			return _edriveEntity.Product_Body.Where(m => m.id == bodyid).Select(m =>
				new DataContract.Product_Body { id = m.id, body = m.Body }).SingleOrDefault();
		}
		/// <summary>
		/// Return Product Model Values
		/// </summary>
		/// <param name="ModelID">Product's Model ID</param>
		/// <returns></returns>
		public DataContract.Product_Model GetModelbyId(Int32 ModelID)
		{
			return _edriveEntity.Product_Model.Where(m => m.id == ModelID).Select(m =>
				new DataContract.Product_Model { id = m.id, modelName = m.ModeLName, makeID = m.MakeID }).SingleOrDefault();
		}
		/// <summary>
		/// Return  Dealer Record Values specified by ProductID
		/// </summary>
		/// <param name="ProductID"></param>
		/// <returns></returns>
		public DataContract.Customer GetDealerbyProductID(Int32 ProductID)
		{
			var CustomerID = _edriveEntity.Product.Where(m => m.ProductId == ProductID).Select(m => m.CustomerID).SingleOrDefault();
			if(CustomerID == null)
				return null;
			var dealer = _edriveEntity.Customer.Where(m => m.CustomerID == CustomerID).Select(m =>
				new DataContract.Customer
				{
					customerID = m.CustomerID,
					email = m.Email,
					Name = m.Name,
					FirstName = m.FirstName,
					LastName = m.LastName,
					CompanyName = m.Company,
					Phone = m.Phone,
					City = m.City,
					StateID = m.Stateid ?? 0,
					StateName = m.StateProvince.Name,
					StreetAddress1 = m.StreetAddress,
					StreetAddress2 = m.StreetAddress2,
					Zip = m.ZipPostalCode ?? 0,
				}).SingleOrDefault();
			if(String.IsNullOrEmpty(dealer.FirstName) == false)
			{
				dealer.Name = dealer.FirstName + " " + dealer.LastName ?? "";

			}

			return dealer;
		}
		/// <summary>
		/// to delete the Dealer
		/// </summary>
		/// <param name="CustomerID"></param>
		/// <returns></returns>
		public Boolean Delete_Dealer(Int32 CustomerID)
		{
			try
			{


				var _cust = _edriveEntity.Customer.First(m => m.CustomerID == CustomerID && m.Customer_Type.Role == "Dealer");
				_cust.Deleted = true;
				_edriveEntity.SaveChanges();
				return true;

			}
			catch(Exception)
			{
				return false;

			}
		}

		/// <summary>
		/// To delte the logo from  Dealer Profile
		/// </summary>
		/// <param name="CustomerID"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		public Boolean Delete_Dealer_Profile_Logo(int CustomerID, out string Msg)
		{
			try
			{

				CustomerProfile prf = _edriveEntity.CustomerProfile.First(m => m.Customer.CustomerID == CustomerID);
				prf.Logo = "";
				_edriveEntity.SaveChanges();
				Msg = "";
				return true;
			}
			catch(Exception ex)
			{

				Msg = ex.Message;
				return false;
			}
		}
		/// <summary>
		/// To delte the logo from  Dealer Profile PageImage
		/// </summary>
		/// <param name="CustomerID"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		public Boolean Delete_Dealer_Profile_PageImage(int CustomerID, out string Msg)
		{
			try
			{

				CustomerProfile prf = _edriveEntity.CustomerProfile.First(m => m.Customer.CustomerID == CustomerID);
				prf.PageImage = "";
				_edriveEntity.SaveChanges();
				Msg = "";
				return true;
			}
			catch(Exception ex)
			{

				Msg = ex.Message;
				return false;
			}
		}



		/// <summary>
		/// to update dealer personal details on myaccount front end section
		/// </summary>
		/// <param name="Msg"></param>
		/// <param name="_customer"></param>
		/// <returns></returns>
		public Boolean Update_Dealer_Personal_Details(out String Msg, DataContract.Customer _customer)
		{
			try
			{

				//if email exist for other dealer then email already in use
				if(_edriveEntity.Customer.Any(m => m.Customer_Type.Role == "Dealer" && m.CustomerID != _customer.customerID && m.Email == _customer.email && m.Deleted == false))
				{
					Msg = "Thie email is already in user. Please change the email address.";
					return false;
				}
				var dealer = _edriveEntity.Customer.First(m => m.Deleted == false && m.CustomerID == _customer.customerID);
				dealer.FirstName = dealer.Name = _customer.Name;
				dealer.LastName = "";

				#region for search update product name with city
				{
					if(dealer.ZipPostalCode != _customer.Zip) //-- then update products for its zip name for search page.
					{
						var prdts = _edriveEntity.Product.Where(m => m.CustomerID == dealer.CustomerID);
						foreach(var item in prdts)
						{
							try
							{
								item.Name = item.Product_Model.Product_Make.Make + " " + item.Product_Model.ModeLName.ToString() + " " + item.VIN + " " + item.Year + " " + (_customer.City ?? "").Trim() + (_customer.Zip == 0 ? "" : " " + _customer.Zip.ToString());

							}
							catch(Exception ex)
							{

							}

						}
					}
				}
				#endregion

				dealer.ZipPostalCode = _customer.Zip;
				dealer.Email = _customer.email;
				dealer.Phone = _customer.Phone;

				_edriveEntity.SaveChanges();

				Msg = "";
				return true;
			}
			catch(Exception ex)
			{
				Msg = "error:" + ex.Message;
				return false;
			}

		}

		/// <summary>
		/// It makes the specified the Dealer as featured dealer.
		/// </summary>
		/// <param name="DealerId"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		public Boolean MakeDealer_as_FeaturedDealer(int DealerId, out String Msg)
		{
			try
			{
				var Prevcust = _edriveEntity.Customer.FirstOrDefault(m => m.IsFeatured == true);
				if(Prevcust != null)
					Prevcust.IsFeatured = false;

				var cust = _edriveEntity.Customer.First(m => m.CustomerID == DealerId);
				cust.IsFeatured = true;

				_edriveEntity.SaveChanges();
				Msg = "";
				return true;
			}
			catch(Exception ex)
			{
				Msg = ex.Message;
				return false;
			}
		}
		/// <summary>
		/// It return the List of vehicle of featured dealers
		/// </summary>
		/// <param name="rec_to_return"></param>
		/// <returns></returns>

		public List<Products> Get_Dealers_FeaturedVehicle(Int32 rec_to_return)
		{
			var cust = _edriveEntity.Customer.FirstOrDefault(m => m.IsFeatured == true);
			try
			{


				String cityName = "";
				if(cust.Stateid != null)
					cityName = cust.StateProvince.Abbreviation;
				var OwnerDet = cust.FirstName + ", " + cityName;
				if(cust != null)
				{
					var lstprd = _edriveEntity.Product.Where(m => m.CustomerID == cust.CustomerID).Take(rec_to_return).
						Select(m => new Products
						{
							productId = m.ProductId,
							Year = m.Year ?? 0,
							MakeName = m.Product_Model.Product_Make.Make,
							ModelName = m.Product_Model.ModeLName,
							body = m.Product_Body.Body,
							mileage = m.Mileage ?? 0,
							exterior = m.Exterior_Color,
							customerId = m.CustomerID ?? 0,
							OwnerDetail = OwnerDet,
							price_Current = m.Price_Current ?? 0
						}).ToList();

					foreach(var item in lstprd)
					{
						if(item.exterior.Length > 15)
						{
							item.exterior = item.exterior.Substring(0, 14) + "...";
						}
					}
					return lstprd;
				}
			}
			catch(Exception)
			{
				return null;

			}
			return null;
		}

		/// <summary>
		/// This return the Dealers Details by its Email(Username)
		/// </summary>
		/// <param name="Email"></param>
		/// <returns></returns>
		public DataContract.Customer GetDealerByDealerEmail(String Email)
		{

			var Dealer = _edriveEntity.Customer.Where(m => m.Email == Email).Select(m => m).SingleOrDefault();
			if(Dealer == null)
				return null;
			var dealr = new DataContract.Customer
			{
				active = Dealer.Active,
				customerGUID = Dealer.CustomerGUID.ToString(),
				customerType = Dealer.CustomerType,
				customerID = Dealer.CustomerID,
				DateofBirth = Dealer.DateOfBirth,
				ExpiryDate = Dealer.ExpiryDate,
				Gender = Dealer.Gender,
				iPAddress = Dealer.IPAddress,
				isTrial = Dealer.IsTrial ?? false,
				Newsletter = Dealer.newsletter ?? false,
				registrationDate = Dealer.RegistrationDate,
				email = Dealer.Email,
				Name = Dealer.Name,
				CompanyName = Dealer.Company,
				Zip = Dealer.ZipPostalCode ?? 0,
				FirstName = Dealer.FirstName,
				LastName = Dealer.LastName,
				StreetAddress1 = Dealer.StreetAddress,
				StreetAddress2 = Dealer.StreetAddress2,
				City = Dealer.City,
				StateID = Dealer.Stateid ?? 0,
				Phone = Dealer.Phone,
				password = Dealer.Password,
				Fax = Dealer.Fax,
				StateName = (Dealer.StateProvince == null ? "" : Dealer.StateProvince.Name),
			};
			if(String.IsNullOrEmpty(dealr.FirstName) == false)
			{
				dealr.Name = dealr.FirstName + " " + dealr.LastName ?? "";

			}
			return dealr;

		}
		/// <summary>
		/// This return the Dealers Details by its  CustomerID
		/// </summary>
		/// <param name="CustomerID"></param>
		/// <returns></returns>
		public DataContract.Customer GetDealerByDealerID(int? CustomerID)
		{
			try
			{


				var Dealer = _edriveEntity.Customer.Where(m => m.CustomerID == CustomerID).FirstOrDefault();
				if(Dealer == null)
					return null;
				///split and get the first and last names
				if(String.IsNullOrEmpty(Dealer.FirstName) && Dealer.Name != null)
				{
					if(Dealer.Name.IndexOf(' ') > 0)
					{
						Dealer.FirstName = Dealer.Name.Substring(0, Dealer.Name.IndexOf(' ')).Trim();
						Dealer.LastName = Dealer.Name.Substring(Dealer.Name.IndexOf(' ')).Trim();
					}
					else
					{
						Dealer.FirstName = Dealer.Name;
						Dealer.LastName = "";
					}

				}
				var dealer = new DataContract.Customer
				{
					customerID = Dealer.CustomerID,
					email = Dealer.Email ?? "",
					Name = Dealer.Name ?? "",
					CompanyName = Dealer.Company ?? "",
					Zip = Dealer.ZipPostalCode ?? 0,
					FirstName = Dealer.FirstName ?? "",
					LastName = Dealer.LastName ?? ""
				  ,
					StateName = Dealer.StateProvince == null ? "" : Dealer.StateProvince.Name,
					StreetAddress1 = Dealer.StreetAddress ?? "",
					StreetAddress2 = Dealer.StreetAddress2 ?? "",
					DateofBirth = Dealer.DateOfBirth,
					City = Dealer.City ?? "",
					StateID = Dealer.Stateid ?? -1,
					Phone = Dealer.Phone ?? "",
					password = Dealer.Password ?? "",
					Fax = Dealer.Fax ?? "",
					registrationDate = Dealer.RegistrationDate,

					Newsletter = Dealer.newsletter ?? false

				};
				if(String.IsNullOrEmpty(dealer.FirstName) == false)
				{
					dealer.Name = dealer.FirstName + " " + dealer.LastName ?? "";

				}
				return dealer;

			}
			catch(Exception ex)
			{

				return null;
			}

		}
		/// <summary>
		/// To get the Dealer Profile by its DealerID
		/// </summary>
		/// <param name="CustomerID"></param>
		/// <returns></returns>
		public DataContract._CustomerProfile GetDealer_Profile_ByDealerID(int? CustomerID)
		{
			var Profile = _edriveEntity.CustomerProfile.FirstOrDefault(m => m.CustomerID == CustomerID);
			if(Profile != null)
			{
				var _Profile = new DataContract._CustomerProfile();
				_Profile.CustomerID = Profile.CustomerID ?? 0;
				_Profile.ApplicationURL = Profile.ApplicationURL;
				_Profile.Description = Profile.Description;
				_Profile.Logo = Profile.Logo;
				_Profile.PageImage = Profile.PageImage;
				_Profile.ServiceURL = Profile.ServiceURL;
				_Profile.WarrantyURL = Profile.WarrantyURL;
				return _Profile;
			}
			else
			{
				return new DataContract._CustomerProfile();
			}

		}


		#region "Products Methods"
		/// <summary>
		///  Return  Product Record specified by ProductID
		/// </summary>
		/// <param name="ProductID"></param>
		/// <returns></returns>
		public DataContract.Products GetProductByID(int ProductID)
		{
			var Ad_Pics = GetProductPicture_By_ProductID(ProductID);


			var prd = _edriveEntity.Product.First(m => m.ProductId == ProductID);
			string pic_url = string.Empty;

			{
				foreach(var item in Ad_Pics)
				{
					if(String.IsNullOrEmpty(item.PictureURL) == false)
						pic_url += item.PictureURL + ";";
				}

				if(pic_url.IndexOf(';') > 0)
				{
					pic_url = pic_url.Substring(0, pic_url.LastIndexOf(';'));
				}

			}

			prd.Pics = pic_url;
			var lst = new Products
			{
				bodyID = prd.Body,
				productId = prd.ProductId,
				pics = prd.Pics,
				mileage = prd.Mileage ?? 0,
				body = prd.Product_Body.Body,
				price_Current = prd.Price_Current ?? 0,
				price_WholeSale = Convert.ToDecimal(prd.Price_WholeSale ?? 0),
				price_Cost = Convert.ToDecimal(prd.Price_Cost ?? 0),
				VehicleType = prd.Product_Type.Type,
				transmission = prd.Transmission,
				exterior = prd.Exterior_Color,
				OwnerDetail = prd.OwnerDetail,
				drive_Type = prd.Drive_Type,
				vin = prd.VIN,
				//zip = prd.zip??0,
				zip = prd.Customer.ZipPostalCode ?? 0,
				Year = prd.Year ?? 0,
				type = prd.Type,
				Make = prd.Product_Model.Product_Make.Make,
				MakeName = prd.Product_Model.Product_Make.Make,
				ModelName = prd.Product_Model.ModeLName,
				model = prd.Model,
				descriptiont = prd.Description ?? "",
				updatedOn = prd.UpdatedOn,
				stock = prd.Stock,
				doors = prd.Doors ?? 0,
				Reserved = prd.Reserved,
				fuel_Type = prd.Fuel_Type,
				Title = prd.Title,
				condition = prd.Condition,
				free_Text = prd.Free_Text,
				isfeature = prd.IsFeature ?? false,
				customerId = prd.CustomerID ?? 0,
				ShowOnDealerProfile = prd.ShowOnDealerProfile ?? false,
				engine = prd.Engine
				,
				date_in_Stock = prd.Date_in_Stock ?? DateTime.Now,
				interior = prd.Interior_Color,
				trim = prd.Trim,
				savingAmount = prd.SavingAmount ?? 0,
				averageRetailPrice = prd.AverageRetailPrice ?? 0,
				averageTradeinPrice = prd.AverageTradeinPrice ?? 0,
				qualifyPrice = prd.QualifyPrice ?? 0,
				city_Fuel = prd.City_Fuel ?? 0,
				highWay_Fuel = prd.Highway_Fuel ?? 0,
				name = prd.Name,
				fileName = prd.FileName,
				warranty = prd.Warranty ?? false
			};
			if(lst.customerId == 0)//then add admin id
			{
				var admin = _edriveEntity.Customer.FirstOrDefault(m => m.Deleted == false && m.Customer_Type.Role == "Admin");
				if(admin != null)
				{
					lst.customerId = admin.CustomerID;
				}
			}

			return lst;
		}

		/// <summary>
		/// To return the list of multiple productsds
		/// </summary>
		/// <param name="productIDs"></param>
		/// <returns></returns>
		public List<DataContract.Products> GetProductByIDs(List<int> productIDs)
		{
			List<Products> lstProducts;
			var lst = _edriveEntity.Product.Where(m => productIDs.Contains(m.ProductId));
			if(lst.Count() > 0)
			{
				lstProducts = new List<Products>();

				foreach(var prd in lst)
				{
					lstProducts.Add(new Products
					{
						productId = prd.ProductId,
						pics = prd.Pics,
						mileage = prd.Mileage ?? 0,
						body = prd.Product_Body.Body,
						price_Current = prd.Price_Current ?? 0,
						price_WholeSale = (decimal)(prd.Price_WholeSale ?? 0),
						price_Cost = (Decimal)(prd.Price_Cost ?? 0),
						VehicleType = prd.Product_Type.Type,
						transmission = prd.Transmission,
						exterior = prd.Exterior_Color,
						OwnerDetail = prd.OwnerDetail,
						drive_Type = prd.Drive_Type,
						vin = prd.VIN,
						//zip = prd.zip??0,
						zip = prd.Customer.ZipPostalCode ?? 0,
						Year = prd.Year ?? 0,
						type = prd.Type,
						Make = prd.Product_Model.Product_Make.Make,
						MakeName = prd.Product_Model.Product_Make.Make,
						ModelName = prd.Product_Model.ModeLName,
						model = prd.Model,
						descriptiont = prd.Description ?? "",
						updatedOn = prd.UpdatedOn,
						stock = prd.Stock,
						doors = prd.Doors ?? 0,
						Reserved = prd.Reserved,
						fuel_Type = prd.Fuel_Type,
						Title = prd.Title,
						condition = prd.Condition,
						free_Text = prd.Free_Text,
						isfeature = prd.IsFeature ?? false,
						customerId = prd.CustomerID ?? 0,
						ShowOnDealerProfile = prd.ShowOnDealerProfile ?? false,
						engine = prd.Engine
						,
						date_in_Stock = prd.Date_in_Stock ?? DateTime.Now,
						interior = prd.Interior_Color,
						trim = prd.Trim
					});




				}
				GetProductsPictures(lstProducts);
				return lstProducts;
			}
			else
				return null;


		}


		/// <summary>
		/// if product exist
		/// </summary>
		/// <param name="Vin"></param>
		/// <returns></returns>
		public Boolean IsProductExist_by_VIN(String Vin)
		{

			return _edriveEntity.Product.Any(m => m.VIN == Vin && m.Deleted == false);
		}

		/// <summary>
		/// if product exist for the same vin for other proeduct
		/// </summary>
		/// <param name="Vin"></param>
		/// <returns></returns>
		public Boolean IsProductExist_by_VIN_for_other_vehicle(String Vin, Int32 ProductID)
		{
			return _edriveEntity.Product.Any(m => m.VIN == Vin && m.ProductId != ProductID && m.Deleted == false);

		}



		/// <summary>
		///  Return  Product Record specified by DealerID for the product details page
		/// </summary>
		/// <param name="DealerID"></param>
		/// <returns></returns>
		public List<DataContract.Products> GetProductsByDealerID(Int32 DealerID)
		{//top 8 product will be returne
			var lstModel = _edriveEntity.Product.Where(m => m.CustomerID == DealerID && m.Deleted == false).OrderByDescending(m => m.CreatedOn).ThenByDescending(m => m.ProductId).Take(8).Select(m => new EdriveService.DataContract.
					Products
			{
				productId = m.ProductId,
				pics = m.Pics,
				mileage = m.Mileage ?? 0,
				ModelName = m.Product_Model.ModeLName,
				Year = m.Year ?? 0,
				model = m.Model,
				descriptiont = m.Description ?? "",
				updatedOn = m.UpdatedOn,
				stock = m.Stock,
				engine = m.Engine,
				Make = m.Product_Model.Product_Make.Make,
				price_Current = m.Price_Current ?? 0,
				interior = m.Interior_Color,
				trim = m.Trim,
				body = m.Product_Body.Body,
				bodyID = m.Body
			}
				).ToList();
			// to update the Picture 
			GetProductsPictures(lstModel);
			return lstModel;

		}
		/// <summary>
		/// It retunr the list of Feature vehicle specifed in the count
		/// </summary>
		/// <returns></returns>
		public List<Products> Get_FeaturedVehicles(Int32 VehCounts)
		{

			var lstCars = _edriveEntity.Product.Where(m => m.Deleted == false && m.IsFeature == true).OrderByDescending(m => m.UpdatedOn).Take(VehCounts).Select(m => new DataContract.Products
			{
				productId = m.ProductId,
				pics = m.Pics,
				mileage = m.Mileage ?? 0,
				body = m.Product_Body.Body,
				price_Current = m.Price_Current ?? 0,
				transmission = m.Transmission ?? "",
				exterior = m.Exterior_Color ?? "",
				OwnerDetail = m.OwnerDetail,
				drive_Type = m.Drive_Type,
				vin = m.VIN,
				MakeName = m.Product_Model.Product_Make.Make,
				ModelName = m.Product_Model.ModeLName,
				//zip = m.zip??0,
				zip = m.Customer.ZipPostalCode ?? 0,
				Year = m.Year ?? 0,
				model = m.Model,
				updatedOn = m.UpdatedOn,
				customerId = m.CustomerID ?? 0
			}).ToList();

			foreach(var item in lstCars)
			{
				if(item.customerId != null)
				{
					if(item.customerId != 0)
					{
						item.Customer = GetDealerByDealerID(item.customerId);
					}
				}

			}
			// to update the product picture by sort order
			GetProductsPictures(lstCars);
			foreach(var item in lstCars)
			{
				if(String.IsNullOrEmpty(item.pics) == false)
				{
					if(item.pics.Contains(';'))
					{
						item.pics = item.pics.Split(';')[0];
					}
				}

			}
			return lstCars;
		}

		/// <summary>
		/// to add the product rating for product Details page
		/// </summary>
		/// <param name="productID"></param>
		/// <param name="score"></param>
		/// <param name="p"></param>
		/// <param name="Msg"></param>
		public Boolean AddProductRating(int productID, int score, string p, out string Msg)
		{
			try
			{


				var prd_rat = _edriveEntity.Product_Rating.FirstOrDefault(m => m.ProductID == productID);
				Double AvgCount = 0;
				Int32 TotalVotes = 0;
				if(prd_rat != null)
				{
					AvgCount = prd_rat.AvgRating ?? 0;
					TotalVotes = prd_rat.TotaVotes ?? 0;
					prd_rat.AvgRating = ((AvgCount * TotalVotes) + score) / (TotalVotes + 1);
					prd_rat.TotaVotes = TotalVotes + 1;
				}
				else
				{
					prd_rat = new Product_Rating { AvgRating = score, TotaVotes = 1, ProductID = productID };
					_edriveEntity.Product_Rating.AddObject(prd_rat);
				}
				_edriveEntity.SaveChanges();
				Msg = "";
				return true;
			}
			catch(Exception ex)
			{
				Msg = ex.Message;
				return false;
			}

		}

		/// <summary>
		/// To return the proudct rating by its product id
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		public Int32 GetProductRating(Int32 productId)
		{
			try
			{


				return Convert.ToInt32(Math.Ceiling(_edriveEntity.Product_Rating.FirstOrDefault(m => m.ProductID == productId).AvgRating ?? 0));
			}
			catch(Exception)
			{
				return 0;
			}
		}
		#endregion

		#endregion
		#region "HarMethods"
		//Data Entity Model Object
		//  edriveautoEntities _edriveEntity = new edriveautoEntities();
		//bind Models to dropdown by make id
		public List<EdriveService.DataContract.Product_Model> BindModel(int makeid)
		{
			List<EdriveService.DataContract.Product_Model> lstModel = new List<EdriveService.DataContract.Product_Model>();
			lstModel = _edriveEntity.Product_Model.Where(m => m.MakeID == makeid).Select(m => new EdriveService.DataContract.Product_Model { id = m.id, modelName = m.ModeLName }).ToList();
			return lstModel;
		}


		//bind make  dropdown
		public List<EdriveService.DataContract.Product_Make> BindMake()
		{
			var lstMake = _edriveEntity.Product_Make.OrderBy(m => m.Make).Select(m => new EdriveService.DataContract.Product_Make { id = m.id, make = m.Make }).ToList();
			return lstMake;
		}

		public Product_Model AddProductModel(String ModelName, Int32 MakeID)
		{
			var Model = _edriveEntity.Product_Model.Where(m => m.ModeLName == ModelName && m.MakeID == MakeID).SingleOrDefault();
			if(Model == null)
			{
				Model = new Product_Model { MakeID = MakeID, ModeLName = ModelName };
				_edriveEntity.Product_Model.AddObject(Model);
				_edriveEntity.SaveChanges();
			}
			return Model;


		}
		/// <summary>
		/// This add the new make to database and return the new make object . if make already exisat it will return that make
		/// </summary>
		/// <param name="Make"></param>
		/// <returns></returns>
		public Product_Make AddProductMake(String Make)
		{
			if(String.IsNullOrEmpty(Make) == false)
			{
				var make = _edriveEntity.Product_Make.FirstOrDefault(m => m.Make == Make);
				if(make == null)
				{
					make = new Product_Make
					{
						Make = Make
					};
					_edriveEntity.SaveChanges();
				}
				return make;
			}

			else
			{
				throw new Exception("make can not be null");
			}

		}
		/// <summary>
		/// This method is used by private users to sell their product.
		/// </summary>
		/// <param name="VIN"></param>
		/// <param name="Condition"></param>
		/// <param name="mileage"></param>
		/// <param name="SellerZip"></param>
		/// <param name="SellerEmail"></param>
		/// <param name="SellerName"></param>
		/// <param name="SellerNotes"></param>
		/// <param name="Phone"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		public Boolean AddProductUsingNadaService(String VIN, String Condition, Int32 mileage, Int32 SellerZip, String SellerEmail, String SellerName, String SellerNotes, String Phone, Boolean Offer, ref String Msg)
		{
			//using (edriveautoEntities _edriveEntity = new edriveautoEntities())
			// using(TransactionScope tc=new TransactionScope ())
			{
				try
				{

					NADA_UsedCars.UsedCars oUsedCarService = new NADA_UsedCars.UsedCars();
					NADA_UsedCars.UsedCarResultSet oUsedCarsResultSet = oUsedCarService.GetUsedCars("EdriveAutos", "ed12uc20", VIN);
					Nada_UsedCarPrices.UsedCarPrices oUsedCarPrices = new UsedCarPrices();


					foreach(NADA_UsedCars.UsedCar uc in oUsedCarsResultSet.UsedCars)
					{

						//for getting options of the vehicles.

						string strOptions = string.Empty;


						string Engine = string.Empty;


						string condition = string.Empty;
						string name = string.Empty;
						//string mileage = string.Empty;
						string options = string.Empty;
						string zip = string.Empty;
						int city_fuel = 0;
						int highway_fuel = 0;
						decimal retail_price = 0;
						decimal tradein_price = 0;
						//strOptions = uc.UsedCarOptions[0].DisplayName.Trim();



						city_fuel = uc.CityFuelHigh;
						highway_fuel = uc.HighwayFuelHigh;
						retail_price = uc.AverageRetailPrice;
						tradein_price = uc.AverageTradeinPrice;


						DataTable dtOptions = new DataTable();


						String Make = uc.MakeDisplay;
						Int32 Makeid = AddProductMake(Make).id;
						String ModelName = uc.ModelDisplay.Trim();
						Int32 ModelID = AddProductModel(ModelName, Makeid).id;

						name = uc.MakeDisplay + " " + uc.ModelDisplay;

						DataObject obj = oUsedCarPrices.GetStylesForModel("EdriveAutos", "ed12uc20", uc.C_ModelID);
						DataSet ds = obj.GetData;
						DataTable dt1 = ds.Tables[0];
						String BodyName = "";
						Int32 year = uc.Year;
						foreach(DataRow dr in dt1.Rows)
						{
							if(dr["VehicleID"].ToString() == uc.StyleID.ToString())
							{
								BodyName = dr["StyleDisplay"].ToString().Trim();
								Engine = dr["Engine"].ToString();

								break;
							}
						}
						Product_Body prdBody;
						prdBody = _edriveEntity.Product_Body.FirstOrDefault(m => m.Body == BodyName);
						if(prdBody == null)
						{
							prdBody = new Product_Body { Body = BodyName };
							_edriveEntity.Product_Body.AddObject(prdBody);
							_edriveEntity.SaveChanges();
						}
						Int32 TypeID = _edriveEntity.Product_Type.First(m => m.Type == "Car").id;


						Product newPRd = new Product
						{
							AverageRetailPrice = retail_price,
							AverageTradeinPrice = tradein_price,
							Body = prdBody.id,
							City_Fuel = city_fuel,
							Condition = Condition,
							CreatedOn = DateTime.Now,
							CustomerID = null,
							Stock = "",
							Trim = "",
							Transmission = "",
							Warranty = false,
							Reserved = "",
							Title = "",
							Date_in_Stock = DateTime.Now,
							Deleted = false,
							Model = ModelID,
							Offer = Offer,
							Description = SellerNotes,
							Drive_Type = "",
							Engine = Engine,
							Exterior_Color = "",
							FileName = "",
							Free_Text = "",
							Fuel_Type = "",
							Highway_Fuel = highway_fuel,
							Interior_Color = "",
							IsFeature = false,
							IsNew = false,
							Mileage = mileage,
							Name = Make + " " + ModelName + " " + VIN + " " + year + " " + "" + SellerZip.ToString(),
							OwnerDetail = "",
							Pics = "",
							//Price_Cost=Convert.ToDouble(retail_price),
							UpdatedOn = DateTime.Now,
							Price_Current = retail_price,
							VIN = VIN,
							Year = year,
							Type = TypeID,
							SellerEmail = SellerEmail,
							SellerName = SellerName,
							zip = SellerZip


						};

						_edriveEntity.Product.AddObject(newPRd);
						_edriveEntity.SaveChanges();



						ProductOption prdOption;

						for(int i = 1; i < uc.UsedCarOptions.Length; i++)
						{
							if(String.IsNullOrEmpty(uc.UsedCarOptions[i].DisplayName))
								continue;
							string optionName = uc.UsedCarOptions[i].DisplayName = uc.UsedCarOptions[i].DisplayName.Trim();

							prdOption = _edriveEntity.ProductOption.FirstOrDefault(m => m.OptionName == optionName);

							if(prdOption == null)
							{
								prdOption = new ProductOption { OptionName = optionName };
								_edriveEntity.ProductOption.AddObject(prdOption);
								_edriveEntity.SaveChanges();
							}

							if(_edriveEntity.ProductOptionMapping.Any(m => m.ProductID == newPRd.ProductId && m.OptionID == prdOption.id) == false)
							{
								_edriveEntity.ProductOptionMapping.AddObject(new ProductOptionMapping { OptionID = prdOption.id, ProductID = newPRd.ProductId });
								_edriveEntity.SaveChanges();
							}
						}
						_edriveEntity.SaveChanges();
						break;


					}
					_edriveEntity.SaveChanges();
					// tc.Complete();
					return true;
				}
				catch(Exception ex)
				{
					Msg = "Error:" + ex.Message;

					return false;
				}
			}
		}


		public String GetDealerGUID(Int32 customerid)
		{
			string guid;
			using(edriveautoEntities _entity = new edriveautoEntities())
			{
				var cust = _entity.Customer.FirstOrDefault(m => m.CustomerID == customerid);
				guid = cust.GUID;
				if(String.IsNullOrEmpty(guid))
					guid = cust.GUID = Guid.NewGuid().ToString();
				_entity.SaveChanges();
				return guid;
			}
		}

		/// <summary>
		/// This method adds the product to inventory in manage inventory section
		/// </summary>
		/// <param name="Prd"></param>
		/// <param name="status"></param>
		/// <param name="Msg"></param>
		/// <param name="OptionsID"></param>
		/// <returns></returns>
		public Products AddProduct(Products Prd, out Boolean status, ref String Msg, String[] OptionsID)
		{

			try
			{
				if(_edriveEntity.Product.Any(m => m.VIN == Prd.vin))
				{
					throw new Exception("Product already exists");
				}

				var MakeID = Convert.ToInt32(Prd.Make);
				var Model = AddProductModel(Prd.ModelName, MakeID);
				var ModelID = Model.id;
				var MakeName = Model.Product_Make.Make;

				//Int32 BodyID = _edriveEntityEntity.Product_Body.Where(m => m.Body == Prd.bod && m.MakeID == MakeID).SingleOrDefault().id;

				Prd.model = ModelID;
				var dealer = GetDealerByDealerID(Prd.customerId);
				var CityName = dealer.City;
				Prd.name = Prd.name = MakeName + " " + Prd.ModelName.ToString() + " " + Prd.vin + " " + Prd.Year + " " + CityName + (dealer.Zip == 0 ? "" : " " + dealer.Zip.ToString());
				;
				Prd.createdOn = Prd.updatedOn = DateTime.UtcNow;

				var newProduct = new Product
				{
					AverageRetailPrice = Prd.averageRetailPrice,
					AverageTradeinPrice = Prd.averageTradeinPrice,
					Body = Convert.ToInt32(Prd.body),
					City_Fuel = Prd.city_Fuel,
					Condition = Prd.condition,
					CreatedOn = Prd.createdOn,
					CustomerID = Prd.customerId,
					Date_in_Stock = Prd.date_in_Stock,
					Deleted = false,
					Description = Prd.descriptiont,
					Doors = Prd.doors,
					Drive_Type = Prd.drive_Type,
					Engine = Prd.engine,
					Exterior_Color = Prd.exterior,
					Interior_Color = Prd.interior,
					IsFeature = Prd.isfeature,
					//  IsNew = Prd.isfeature,
					Mileage = Prd.mileage,
					Model = ModelID,
					Name = MakeName + " " + Prd.ModelName.ToString() + " " + Prd.vin + " " + Prd.Year + " " + CityName,
					//Options = Prd.options,
					OwnerDetail = Prd.OwnerDetail,
					Pics = Prd.pics,
					Price_Cost = Convert.ToDouble(Prd.price_Cost),
					Price_Current = Prd.price_Current,

					QualifyPrice = Prd.qualifyPrice,
					Reserved = Prd.Reserved,
					SavingAmount = Prd.savingAmount,
					Stock = Prd.stock,
					Title = Prd.Title,
					Transmission = Prd.transmission,
					Trim = Prd.trim,
					Type = Prd.type,
					UpdatedOn = Prd.updatedOn,
					VIN = Prd.vin,
					Warranty = Prd.warranty,
					Year = Prd.Year,
					zip = Prd.zip
				};

				_edriveEntity.Product.AddObject(newProduct);
				if(OptionsID != null)
				{
					for(int i = 0; i < OptionsID.Length; i++)
					{
						var optID = Convert.ToInt32(OptionsID[i]);
						_edriveEntity.ProductOptionMapping.AddObject(new ProductOptionMapping { OptionID = optID, ProductID = newProduct.ProductId });
					}
				}

				_edriveEntity.SaveChanges();
				Prd.productId = newProduct.ProductId;
				status = true;
				return Prd;

			}
			catch(Exception ex)
			{

				status = false;
				Msg = ex.Message;
				return null;
			}
		}


		public Products UpdateProduct(Products Prd, out bool status, ref string Msg, string[] OptionsID)
		{
			try
			{
				if(_edriveEntity.Product.Any(m => m.VIN == Prd.vin && m.ProductId != Prd.productId))//--check if another product with same vin exists.
				{
					throw new Exception("Vin already exists");
				}
				Product prevProduct = _edriveEntity.Product.First(m => m.ProductId == Prd.productId);

				var MakeID = Convert.ToInt32(Prd.Make);
				var Model = AddProductModel(Prd.ModelName, MakeID);
				var ModelID = Model.id;
				var MakeName = Model.Product_Make.Make;
				Prd.model = ModelID;
				var dealer = GetDealerByDealerID(Prd.customerId);
				var CityName = dealer.City;

				Prd.name = MakeName + " " + Prd.ModelName.ToString() + " " + Prd.vin + " " + Prd.Year + " " + CityName + (dealer.Zip == 0 ? "" : " " + dealer.Zip.ToString());
				{
					prevProduct.UpdatedOn = DateTime.UtcNow;
					prevProduct.Date_in_Stock = Prd.date_in_Stock;
					prevProduct.Description = Prd.descriptiont;
					prevProduct.Fuel_Type = Prd.fuel_Type;
					prevProduct.Transmission = Prd.transmission;
					prevProduct.Doors = Prd.doors;
					prevProduct.Drive_Type = Prd.drive_Type;
					prevProduct.Engine = Prd.engine;
					prevProduct.IsFeature = Prd.isfeature;
					prevProduct.Exterior_Color = Prd.exterior;
					prevProduct.Interior_Color = Prd.interior;
					prevProduct.Condition = Prd.condition;
					prevProduct.Title = Prd.Title;
					prevProduct.Price_Cost = Convert.ToDouble(Prd.price_Cost);
					prevProduct.Price_WholeSale = Convert.ToDouble(Prd.price_WholeSale);
					prevProduct.Reserved = Prd.Reserved;
					prevProduct.Price_Current = Prd.price_Current;
					prevProduct.Mileage = Prd.mileage;
					prevProduct.Body = Convert.ToInt32(Prd.body);
					prevProduct.Free_Text = Prd.free_Text;
					prevProduct.Trim = Prd.trim;
					prevProduct.Model = ModelID;
					prevProduct.Year = Prd.Year;
					prevProduct.VIN = Prd.vin;
					prevProduct.Type = Prd.type;
					prevProduct.Stock = Prd.stock;
					prevProduct.Warranty = Prd.warranty;
					prevProduct.ShowOnDealerProfile = Prd.ShowOnDealerProfile;
					prevProduct.IsFeature = Prd.isfeature;
				}



				_edriveEntity.SaveChanges();
				//--deleter prev option Mapping
				var prevmapp = _edriveEntity.ProductOptionMapping.Where(m => m.ProductID == prevProduct.ProductId);
				foreach(var item in prevmapp)
				{
					_edriveEntity.ProductOptionMapping.DeleteObject(item);
				}
				//--end of del prev option Mapping

				//--add new mapping--

				if(OptionsID != null)
				{
					for(int i = 0; i < OptionsID.Length; i++)
					{
						var optID = Convert.ToInt32(OptionsID[i]);

						_edriveEntity.ProductOptionMapping.AddObject(new ProductOptionMapping { OptionID = optID, ProductID = prevProduct.ProductId });
					}
				}
				//--end of new mapping
				_edriveEntity.SaveChanges();//---update the product--
				Prd.productId = prevProduct.ProductId;
				status = true;
				return Prd;

			}
			catch(Exception ex)
			{

				status = false;
				Msg = ex.Message;
				return null;
			}
		}

		public Boolean DeleteProduct(out String Msg, Int32 ProductId)
		{
			try
			{
				_edriveEntity.Product.DeleteObject(_edriveEntity.Product.First(m => m.ProductId == ProductId));
				_edriveEntity.SaveChanges();
				Msg = "";
				return false;

			}
			catch(Exception ex)
			{
				Msg = ex.Message;
				return false;

			}

		}
		public void AddProductPicture(int ProductId, string fileName)
		{
			var prd = _edriveEntity.Product.Where(m => m.ProductId == ProductId).Single();
			prd.Pics += ";" + fileName;
			_edriveEntity.SaveChanges();
		}
		//search products count
		//public int searchProduct_Count(int modelId, string zipCode)
		//{
		//    List<EdriveService.DataContract.Products> lstModel = new List<EdriveService.DataContract.Products>();
		//    Int32 _zipCode = Convert.ToInt32(zipCode);
		//    if (zipCode == "" && modelId != null)
		//    {
		//       return _edriveEntity.Product.Where(m => m.Model == modelId && m.zip == _zipCode).Count();
		//    }
		//    else if (zipCode != "" && modelId != null)
		//    {

		//        return  _edriveEntity.Product.Where(m => m.Model == modelId && m.zip == _zipCode).Count();

		//    }
		//    else
		//        return 0;


		//}

		//search products
		//public List<DataContract.Products> searchProduct(int modelId, string zipCode)
		//{
		//    List<EdriveService.DataContract.Products> lstModel = new List<EdriveService.DataContract.Products>();
		//    Int32 _zipCode = Convert.ToInt32(zipCode);
		//    if (zipCode == "" && modelId != null)
		//    {
		//        lstModel = _edriveEntity.Product.Where(m => m.Model == modelId && m.zip == _zipCode).Select(m => new EdriveService.DataContract.Products
		//        {
		//            pics = m.Pics,
		//            mileage = m.Mileage??0,
		//            body = m.Body,
		//            transmission = m.Transmission,
		//            exterior = m.Exterior_Color,
		//            drive_Type = m.Drive_Type,
		//            zip = m.zip??0,
		//            Year = m.Year??0,
		//            model = m.Model,
		//            updatedOn = m.UpdatedOn,
		//        }).ToList();
		//    }
		//    else if (zipCode != "" && modelId != null)
		//    {

		//        lstModel = _edriveEntity.Product.Where(m => m.Model == modelId && m.zip == _zipCode).Select(m => new EdriveService.DataContract.Products
		//        {
		//            productId = m.ProductId,
		//            pics = m.Pics,
		//            mileage = m.Mileage??0,
		//            body = m.Body,
		//            transmission = m.Transmission,
		//            exterior = m.Exterior_Color,
		//            drive_Type = m.Drive_Type,
		//            zip = m.zip??0,
		//            Year = m.Year??0,
		//            model = m.Model,
		//            updatedOn = m.UpdatedOn,
		//        }).ToList();
		//    }
		//    return lstModel;

		//}
		//Bind the search make attributes
		/// <summary>
		/// return  List of All Product's Make
		/// </summary>
		/// <returns></returns>
		public List<DataContract.Product_Make> bindMakeAttributes()
		{
			var lstMake = _edriveEntity.Product_Make.Select(m => new EdriveService.DataContract.Product_Make { id = m.id, make = m.Make }).ToList();
			return lstMake;
		}
		//Bind the search Type attributes
		/// <summary>
		/// return  List of All Product's Product Type
		/// </summary>
		/// <returns></returns>

		public List<EdriveService.DataContract.Product_Type> bindtypeAttributes()
		{
			var lstMake = _edriveEntity.Product_Type.Select(m => new EdriveService.DataContract.Product_Type { id = m.id, type = m.Type }).ToList();
			return lstMake;
		}
		//Bind the search BOdy attributes
		/// <summary>
		/// return  List of All Product's Body Type
		/// </summary>
		/// <returns></returns>
		public List<DataContract.Product_Body> BindBodyType()
		{
			var lstMake = _edriveEntity.Product_Body.Where(m => m.Body != null && m.Body != "").Select(m => new EdriveService.DataContract.Product_Body { id = m.id, body = m.Body.Trim() }).OrderBy(m => m.body).ToList();

			return lstMake;
		}
		/// <summary>
		///  return  List of All Product's Drive Type
		/// </summary>
		/// <returns></returns>
		public List<String> BindDriveType()
		{
			return _edriveEntity.Product.Where(m => m.Drive_Type != null && m.Drive_Type != "").Select(m => m.Drive_Type).OrderBy(m => m).Distinct().ToList();
		}
		//Bind the search Transmission attributes
		/// <summary>
		/// return  List of All Product's Transmission Type
		/// </summary>
		/// <returns></returns>
		public List<EdriveService.DataContract.Products> GetTransmission()
		{
			var lstMake = _edriveEntity.Product.Where(m => m.Transmission != "").Select(m => new EdriveService.DataContract.Products { transmission = m.Transmission }).Distinct().ToList();

			List<Products> lstMakesNew = new List<Products>();
			foreach(var item in lstMake)
			{
				if(!String.IsNullOrEmpty(item.transmission) && !String.IsNullOrWhiteSpace(item.transmission))
					lstMakesNew.Add(item);
			}

			return lstMakesNew;
		}
		//Bind the search Engine attributes
		/// <summary>
		///  return  List of All Product's Engine Type
		/// </summary>
		/// <returns></returns>
		public List<EdriveService.DataContract.Products> GetEngine()
		{
			var lstMake = _edriveEntity.Product.Where(m => m.Engine != "").Select(m => new EdriveService.DataContract.Products { engine = m.Engine }).Distinct().ToList();

			return lstMake;
		}
		#endregion

		/// <summary>
		/// Updte the Picsture of New Proudct by removing previousimages
		/// </summary>
		/// <param name="Pics"></param>
		/// <param name="ProductID"></param>
		public void UpdatePicsToProduct(String[] Pics, Int32 ProductID)
		{
			var picsPrdt = _edriveEntity.ProductPicture.Where(m => m.ProductID == ProductID);
			foreach(var item in picsPrdt)
			{
				_edriveEntity.ProductPicture.DeleteObject(item);
			}


			for(int i = 0; i < Pics.Length; i++)
			{
				if(String.IsNullOrEmpty(Pics[i]) == false)
					_edriveEntity.ProductPicture.AddObject(new ProductPicture { DisplayOrder = i + 1, PictureURL = Pics[i], ProductID = ProductID });
			}
			_edriveEntity.SaveChanges();
		}

		/// <

		/// <summary>
		/// It insert the Image to Newly Created Product
		/// </summary>
		/// <param name="Pics"></param>
		/// <param name="ProductID"></param>
		public void AddPicsToProduct(String[] Pics, Int32 ProductID)
		{
			for(int i = 0; i < Pics.Length; i++)
			{
				if(String.IsNullOrEmpty(Pics[i]) == false)
					_edriveEntity.ProductPicture.AddObject(new ProductPicture { DisplayOrder = i + 1, PictureURL = Pics[i], ProductID = ProductID });
			}
			_edriveEntity.SaveChanges();
		}

		/// <summary>
		/// Update the Product's picture listing in table and also update Pictures with updated ProductpictureID,return true on success and update Msg withe exception
		/// </summary>
		/// <param name="Pictures"></param>
		/// <param name="ProductID"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		public Boolean Update_Product_Picture(ref List<Product_Picture> Pictures, Int32 ProductID, out String Msg)
		{
			//using (var tran = new TransactionScope())
			{
				try
				{

					Msg = "";

					var allImage = _edriveEntity.ProductPicture.Where(M => M.ProductID == ProductID);//--GET ALL IMAGE FOR PRODUCT
					foreach(var item in allImage)
					{
						if(Pictures.Any(m => m.ProductPictureID == item.ProductPictureID) == false)//if new coll not have product picture id
						{
							_edriveEntity.ProductPicture.DeleteObject(item);
						}
					}

					foreach(var item in Pictures)
					{
						if(item.ProductPictureID > 0)//--update the picture--
						{
							var pic = _edriveEntity.ProductPicture.First(m => m.ProductPictureID == item.ProductPictureID);
							pic.PictureURL = item.PictureURL;
							pic.DisplayOrder = item.DisplayOrder;
						}
						else
							if(item.ProductPictureID == 0)//--new item
							{
								_edriveEntity.ProductPicture.AddObject(new ProductPicture { DisplayOrder = item.DisplayOrder, PictureURL = item.PictureURL, ProductID = ProductID });
							}


					}

					_edriveEntity.SaveChanges();//--update done-

					Pictures = GetProductPicture_By_ProductID(ProductID); //- Get update pictures info

					//var prdt = _edriveEntity.Product.First(m => m.ProductId == ProductID);
					//foreach (var item in prdt)
					//{

					//}

					// tran.Complete();
					return true;
				}
				catch(Exception ex)
				{
					Msg = ex.Message;
					return false;
				}
			}


		}


		#region "Dealer"
		public Boolean Authenticate_Dealer_or_Admin(String UserName, String Password, String RoleName, ref DataContract.Customer customer)
		{
			customer = _edriveEntity.Customer.Where(m => m.Email == UserName && m.Password == Password && m.Customer_Type.Role == RoleName).Select(m => new DataContract.Customer { customerID = m.CustomerID, customerType = m.CustomerType, Zip = m.ZipPostalCode ?? 0 }
			 ).SingleOrDefault();
			if(customer != null)
			{
				return true;
			}
			else
			{
				return false;
			}



		}
		#endregion

		#region "ManageProduct"

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Vin"></param>
		/// <param name="Stock"></param>
		/// <param name="_MakeID"></param>
		/// <param name="_DealerID"></param>
		/// <param name="Featured"></param>
		public Int32 Get_Count_SearchProduct_for_ManageProduct
			(string VIN, string Stock, Int32 makeId, String _ComapnyName, String _DealerEmail, String RoleName, bool? isOnlyFeatured)
		{

			String expression = BuldExpression_for_SearchPRoduct_for_ManageProduct(VIN, Stock, makeId, _ComapnyName, _DealerEmail, RoleName, isOnlyFeatured);

			var lstProducts = _edriveEntity.Product.Where(expression).Count();

			return lstProducts;

		}
		//---search product for manage--
		/// <summary>
		/// SearchProduct for Manage Controller
		/// </summary>
		/// <param name="VIN"></param>
		/// <param name="Stock"></param>
		/// <param name="make"></param>
		/// <param name="_ComapnyName"></param>
		/// <param name="isOnlyFeatured"></param>
		/// <param name="pageIndex">0 for 1st page and 1 for 2nd page</param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public List<Products> SearchProduct_for_ManageProduct
			(string VIN, string Stock, Int32 makeId, String _ComapnyName, String _DealerEmail, String RoleName, bool? isOnlyFeatured, Int32 pageIndex, Int32 pageSize, ref Int32 CarsCount)
		{
			var Skip = (pageIndex) * pageSize;
			String expression = BuldExpression_for_SearchPRoduct_for_ManageProduct(VIN, Stock, makeId, _ComapnyName, _DealerEmail, RoleName, isOnlyFeatured);
			var lstProducts = _edriveEntity.Product.Where(expression).OrderByDescending(m => m.ProductId).Skip(Skip).Take(pageSize).Select(m => m).ToList();
			var returnList = new List<Products>();
			foreach(Product item in lstProducts)
			{
				var Prod = new Products
				{
					productId = item.ProductId,
					pics = item.Pics,
					mileage = item.Mileage ?? 0,
					body = item.Product_Body.Body,
					transmission = item.Transmission,
					exterior = item.Exterior_Color,
					drive_Type = item.Drive_Type,
					zip = item.zip ?? 0,
					vin = item.VIN,
					Year = item.Year ?? 0,
					model = item.Model,

					ModelName = item.Product_Model.ModeLName,
					MakeName = item.Product_Model.Product_Make.Make,
					isfeature = item.IsFeature ?? false,
					name = item.Product_Model.Product_Make.Make + " " + item.Product_Model.ModeLName,
					OwnerDetail = item.OwnerDetail ?? "",
					updatedOn = item.UpdatedOn
					,
					price_Current = item.Price_Current ?? 0
				};
				returnList.Add(Prod);

			}


			CarsCount = _edriveEntity.Product.Where(expression).Count();
			return returnList;

		}

		private String BuldExpression_for_SearchPRoduct_for_ManageProduct(string VIN, string Stock, Int32 makeId, String _ComapnyName, String _DealerEmail, String RoleName, bool? isOnlyFeatured)
		{
			String expression = "it.Customerid is not null and it.Deleted=false";
			if(!String.IsNullOrEmpty(VIN))
			{
				expression += String.Format(" and it.VIN='{0}'", VIN);
			}
			if(!String.IsNullOrEmpty(Stock))
			{
				expression += String.Format(" and it.Stock='{0}'", Stock);
			}
			if(String.IsNullOrEmpty(_ComapnyName) == false)
			{
				expression += String.Format(" and it.Customer.Company ='{0}'", _ComapnyName);
			}
			if(isOnlyFeatured.HasValue && isOnlyFeatured == true)
			{
				expression += String.Format(" and it.IsFeature=true");
			}
			if(makeId != -1)
			{
				Int32[] modelids = _edriveEntity.Product_Model.Where(m => m.MakeID == makeId).Select(m => m.id).ToArray();
				string modelExp = "";
				for(int i = 0; i < modelids.Length; i++)
				{
					if(i != 0)
					{
						modelExp += " or it.Model=" + modelids[i].ToString();
					}
					else// very first iteration
					{

						modelExp += "  it.Model=" + modelids[i].ToString();
					}

				}
				if(!String.IsNullOrEmpty(modelExp))
					expression += String.Format(" and ({0})", modelExp);
			}
			///--is user id deler then only show product for that dealer only

			if(RoleName.ToLower() == "Dealer".ToLower())
			{
				Int32 DealerId = _edriveEntity.Customer.First(m => m.Email == _DealerEmail).CustomerID;
				expression += String.Format(" and it.CustomerID={0}", DealerId);
			}
			return expression;
		}
		//--- end of search product--


		//---Binde Dealers---
		/// <summary>
		/// return the list of All Dealers
		/// </summary>
		/// <returns></returns>

		public List<DataContract.Customer> GetDealers()
		{
			return _edriveEntity.Customer.
				Where(m => m.Active == true && m.Deleted == false && m.Customer_Type.Role == "Dealer").
				OrderBy(m => m.Name).Select(m => new DataContract.
				Customer
				{
					registrationDate = m.RegistrationDate,
					active = m.Active,
					customerID = m.CustomerID,
					email = m.Email,
					Name = m.Name,
					CompanyName = m.Company,
					Zip = m.ZipPostalCode ?? 0,
					FirstName = m.FirstName,
					LastName = m.LastName,
					StreetAddress1 = m.StreetAddress,
					StreetAddress2 = m.StreetAddress2,
					DateofBirth = m.DateOfBirth,
					City = m.City,
					StateID = m.Stateid ?? -1,
					Phone = m.Phone,
					password = m.Password,
					Fax = m.Fax
				}).ToList();
		}
		/// <summary>
		/// This method return the list of dealer whose producta have updated in last 45 days
		/// </summary>
		/// <returns></returns>
		public List<DataContract.Customer> GetDealers_for_ManageProoduct(Int32 CountDownDays)
		{
			//            //-----------------live site store proc is [ED_GetAllDealerName]   -------
			//            var lastUpdated = DateTime.Now.AddDays(-CountDownDays);
			//            ///get only product's customer that have updated in last 45 days
			//            var lstPrdtsCustomer = _edriveEntity.Product.Where(m => (m.Customer != null && m.UpdatedOn >= lastUpdated)).Select(m => m.CustomerID).Distinct().ToList();
			//            var lstCustomer = lstPrdtsCustomer.Join(_edriveEntity.Customer.Where(m => m.Customer_Type.Role == "Dealer"),
			//               m => m.Value, n => n.CustomerID, (m, n) => n).OrderBy(n => n.Name).Select(n => new DataContract.
			//                   Customer
			//                   {
			//
			//                       CompanyName = n.Company,
			//
			//                   }).Distinct().ToList();

			var customers = _edriveEntity.DealersForManagerProduct().Select(n => new DataContract.Customer
			{
				CompanyName = n.Company,

			}).Distinct().ToList();

			return customers;
		}

		///// <summary>
		///// To get The list Company Name
		///// </summary>
		///// <returns></returns>
		//public List<String> GetDealerCompanyName()
		//{
		//    return _edriveEntity.Customer.Where(m => m.Active == true && m.Deleted == false).Select(m => m.Company).Distinct().ToList();

		//}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="CompanyName"></param>
		/// <param name="CompanyName2"></param>
		/// <param name="Email"></param>
		/// <param name="LastName"></param>
		/// <param name="Name"></param>
		/// <param name="RegFrom"></param>
		/// <param name="RegTo"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="CarsCount"></param>
		/// <returns></returns>
		public List<DataContract.Customer> SearchDealer_For_ManagDealerSection(string CompanyName, string CompanyName2,
			string Email, string LastName, string Name, DateTime? RegFrom, DateTime? RegTo, int pageIndex,
			int pageSize, out int CarsCount)
		{
			String expression = "it.Customer_Type.Role=='Dealer' and it.Deleted=false and true  ";
			if(!String.IsNullOrEmpty(CompanyName))
			{
				expression += String.Format(" and it.Company='{0}'", CompanyName);

			}
			if(!String.IsNullOrEmpty(CompanyName2))
			{
				expression += String.Format(" and it.Company='{0}'", CompanyName2);

			}

			if(!String.IsNullOrEmpty(Email))
			{
				expression += String.Format(" and it.Email='{0}'", Email);

			}


			if(!String.IsNullOrEmpty(LastName))
			{
				expression += String.Format(" and it.LastName='{0}'", LastName);

			}


			//if (!String.IsNullOrEmpty(Name))
			//{
			//    expression += String.Format(" and it.Name='{0}'", Name);

			//}


			if((RegFrom != null))
			{

				expression += String.Format(" and it.RegistrationDate>=DATETIME'{0}'", RegFrom.Value.ToString("yyyy-MM-dd hh:mm")); //'2010-13-8 00:00

			}
			if((RegTo != null))
			{
				expression += String.Format(" and it.RegistrationDate<=DATETIME'{0}'", RegTo.Value.ToString("yyyy-MM-dd hh:mm"));
			}

			var SkipRecords = pageSize * pageIndex;
			CarsCount = _edriveEntity.Customer.Where(expression).Count();

			return _edriveEntity.Customer.Where(expression).OrderByDescending(m => m.RegistrationDate).Skip(SkipRecords).Take(pageSize)
				.Select(m => new DataContract.Customer
				{
					registrationDate = m.RegistrationDate,
					active = m.Active,
					customerID = m.CustomerID,
					email = m.Email,
					Name = m.Name,
					CompanyName = m.Company,
					Zip = m.ZipPostalCode ?? 0,
					FirstName = m.FirstName,
					LastName = m.LastName,
					StreetAddress1 = m.StreetAddress,
					StreetAddress2 = m.StreetAddress2,
					DateofBirth = m.DateOfBirth,
					City = m.City,
					StateID = m.Stateid ?? -1,
					Phone = m.Phone,
					password = m.Password,
					IsFeatured = m.IsFeatured ?? false,
					Fax = m.Fax
				}
		  ).ToList();
		}
		/// <summary>
		/// Delete Products from DB
		/// </summary>
		/// <param name="ProductIDs">Products to be Deleted</param>
		public Boolean DeleteProduct(string[] ProductIDs)
		{
			try
			{

				for(int i = 0; i < ProductIDs.Length; i++)
				{
					var prdID = Convert.ToInt32(ProductIDs[i]);
					Product prd = _edriveEntity.Product.First(m => m.ProductId == prdID);

					if(prd != null)
					{
						prd.Deleted = true;
					}
					_edriveEntity.SaveChanges();

				}


				return true;

			}
			catch(Exception)
			{
				return false;

			}

		}
		/// <summary>
		/// It resets the qualify price to 0 for all products`
		/// </summary>
		public Boolean ApproveProducts()
		{
			try
			{


				var products = _edriveEntity.Product.Where(m => m.Deleted == false);
				foreach(var item in products)
				{
					item.QualifyPrice = 0;
				}
				_edriveEntity.SaveChanges();
				return true;

			}
			catch(Exception)
			{
				return false;

			}

		}

		//prev sign of mehod is-- private decimal ValidRecord(String VIN, Decimal price)
		private decimal ValidQualifyPriceRecord(String VIN, Decimal price)
		{
			NADA_UsedCars.UsedCars oUsedCarService = new NADA_UsedCars.UsedCars();
			NADA_UsedCars.UsedCarResultSet oUsedCarsResultSet = oUsedCarService.GetUsedCars("EdriveAutos", "ed12uc20", VIN.ToString());
			decimal qualifyPrice = 0;
			if(oUsedCarsResultSet.UsedCars != null)
			{
				foreach(NADA_UsedCars.UsedCar uc in oUsedCarsResultSet.UsedCars)
				{
					//Decimal Five_PPrice = Convert.ToDecimal((uc.AverageTradeinPrice) * 0.15);
					//Decimal newAverageTradeInPrice = Convert.ToDecimal(uc.AverageTradeinPrice) + Five_PPrice;
					Decimal newAverageTradeInPrice = Convert.ToDecimal(uc.AverageTradeinPrice);
					if(newAverageTradeInPrice != 0) //Nirav - 28/04/11 - to import all data for demo purpose.
					{
						qualifyPrice = price - newAverageTradeInPrice;
						return qualifyPrice;
						break;
					}
				}

			}
			return qualifyPrice;
		}
		/// <summary>
		/// Updates the Qualify price of Product using third party service
		/// </summary>
		/// <param name="ProductIDs">Product Ids to be qualified</param>
		/// <returns></returns>
		public bool QualifyPriceofProduct(List<string> ProductIDs)
		{
			try
			{
				foreach(var item in ProductIDs)
				{
					var prdID = Convert.ToInt32(item);
					var product = _edriveEntity.Product.First(m => m.ProductId == prdID);
					//updates the qualify price
					product.QualifyPrice = ValidQualifyPriceRecord(product.VIN, product.Price_Current ?? 0);

				}
				_edriveEntity.SaveChanges();

				return true;
			}
			catch(Exception ex)
			{
				throw ex;
				return false;
			}

		}





		#endregion

		#region "ManageDealer"
		/// <summary>
		/// It return the List of Country
		/// </summary>
		/// <returns></returns>
		public List<DataContract.Country> GetCountry()
		{
			return _edriveEntity.Country.OrderBy(m => m.DisplayOrder).Select(m => new DataContract.Country { CountryID = m.CountryID, DisplayOrder = m.DisplayOrder, Name = m.Name }).ToList();
		}
		/// <summary>
		/// It return the List of State by its CountryID
		/// </summary>
		/// <param name="CountryID"></param>
		/// <returns></returns>
		public List<DataContract.State> GetStateByCountry(Int32 CountryID)
		{
			return _edriveEntity.StateProvince.Where(m => m.CountryID == CountryID).OrderBy(m => m.DisplayOrder).Select(m => new DataContract.
			State { Abbreviation = m.Abbreviation, CountryID = m.CountryID, DisplayOrder = m.DisplayOrder, Name = m.Name, StateProvinceID = m.StateProvinceID }).ToList();
		}
		/// <summary>
		/// It State Details by its Stateid
		/// </summary>
		/// <param name="CountryID"></param>
		/// <returns></returns>

		public DataContract.State GetState_Detail_By_StateID(Int32 StateID)
		{
			return _edriveEntity.StateProvince.Where(m => m.StateProvinceID == StateID)
				.Select(m => new DataContract.
			State { Abbreviation = m.Abbreviation, CountryID = m.CountryID, DisplayOrder = m.DisplayOrder, Name = m.Name }).SingleOrDefault();



		}

		/// <summary>
		/// If Dealer exists
		/// </summary>
		/// <param name="Email"></param>
		/// <returns></returns>
		public Boolean IsDealerExists(String Email)
		{
			return _edriveEntity.Customer.Any(m => m.Email == Email && m.Deleted == false);

		}
		/// <summary>
		/// If other Dealer exists for the same email
		/// </summary>
		/// <param name="Email"></param>
		/// <returns></returns>
		public Boolean Is_other_DealerExist_for_same_email(String Email, Int32 currrent_CustomerId)
		{
			return _edriveEntity.Customer.Any(m => m.CustomerID != currrent_CustomerId && m.Email == Email && m.Deleted == false && (m.Customer_Type.Role == "Dealer" || m.Customer_Type.Role == "Admin"));

		}

		/// <summary>
		/// It Creates new Dealer and retunr true on Succes and update Msg if it falis
		/// </summary>
		/// <param name="cust"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		public Boolean AddDealer(DataContract.Customer cust, ref String Msg, DataContract._CustomerProfile Profile)
		{

			try
			{
				if(IsDealerExists(cust.email))//--if email alredy exists
				{
					throw new Exception("This Email already exists.");
				}

				Customer objCustomer = new Customer();
				objCustomer.Active = true;
				objCustomer.Email = cust.email;
				objCustomer.Password = cust.password;
				objCustomer.Gender = cust.Gender;
				objCustomer.CustomerGUID = Guid.NewGuid();
				objCustomer.FirstName = cust.FirstName;
				objCustomer.LastName = cust.LastName;
				objCustomer.Name = cust.FirstName + " " + cust.LastName;

				objCustomer.DateOfBirth = cust.DateofBirth;
				objCustomer.Company = cust.CompanyName;
				objCustomer.StreetAddress = cust.StreetAddress1;
				objCustomer.StreetAddress2 = cust.StreetAddress2;
				objCustomer.ZipPostalCode = cust.Zip;
				objCustomer.City = cust.City;
				if(cust.StateID > 0)
				{
					objCustomer.Stateid = cust.StateID;
				}

				//objCustomer.Stateid = cust.StateID==-1?null:cust.StateID;
				objCustomer.Phone = cust.Phone;
				objCustomer.Fax = cust.Fax;
				objCustomer.newsletter = cust.Newsletter;
				objCustomer.RegistrationDate = DateTime.Now;
				objCustomer.CustomerType = _edriveEntity.Customer_Type.First(m => m.Role == "Dealer").ID;
				_edriveEntity.Customer.AddObject(objCustomer);

				var _Profile = new CustomerProfile();
				_Profile.CustomerID = objCustomer.CustomerID;
				_Profile.ApplicationURL = Profile.ApplicationURL;
				_Profile.Description = Profile.Description;
				_Profile.Logo = Profile.Logo;
				_Profile.PageImage = Profile.PageImage;
				_Profile.ServiceURL = Profile.ServiceURL;
				_Profile.WarrantyURL = Profile.WarrantyURL;
				_edriveEntity.CustomerProfile.AddObject(_Profile);
				_edriveEntity.SaveChanges();
				return true;
			}
			catch(Exception ex)
			{
				Msg = ex.Message;
				return false;
			}

		}
		/// <summary>
		/// Update password for Dealer
		/// </summary>
		/// <param name="_DealerId"></param>
		/// <param name="Pwd"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		public Boolean UpdatePassword_for_Dealer(Int32 _DealerId, String Pwd, out String Msg)
		{
			try
			{
				var _dealer = _edriveEntity.Customer.First(m => m.CustomerID == _DealerId);
				_dealer.Password = Pwd;
				_edriveEntity.SaveChanges();
				Msg = "";
				return true;

			}
			catch(Exception ex)
			{
				Msg = ex.Message;
				return false;

			}
		}

		public Boolean UpdateAdmin_InfoDetails(DataContract.Customer cust, ref String Msg)
		{
			try
			{
				using(edriveautoEntities _edriveEntityNew = new edriveautoEntities())
				{
					Customer objCustomer = _edriveEntityNew.Customer.First(m => m.CustomerID == cust.customerID);
					if(_edriveEntityNew.Customer.Any(m => m.Email == objCustomer.Email && m.CustomerID != cust.customerID))
					{
						throw new Exception("This Email already exists for Dealer.");
					}

					objCustomer.Active = true;
					objCustomer.Email = cust.email;
					// objCustomer.Password = cust.password;
					objCustomer.Gender = cust.Gender;
					objCustomer.CustomerGUID = Guid.NewGuid();
					objCustomer.FirstName = cust.FirstName;
					objCustomer.LastName = cust.LastName;
					objCustomer.Name = cust.FirstName + " " + cust.LastName;
					objCustomer.DateOfBirth = cust.DateofBirth;

					objCustomer.Company = cust.CompanyName;
					objCustomer.StreetAddress = cust.StreetAddress1;
					objCustomer.StreetAddress2 = cust.StreetAddress2;
					objCustomer.ZipPostalCode = cust.Zip;
					objCustomer.City = cust.City;
					if(cust.StateID > 0)
					{
						objCustomer.Stateid = cust.StateID;
					}
					objCustomer.Phone = cust.Phone;
					objCustomer.Fax = cust.Fax;
					objCustomer.newsletter = cust.Newsletter;
					_edriveEntityNew.SaveChanges();
				}
				return true;
			}
			catch(Exception)
			{
				return false;
			}

		}

		public Boolean Update_Dealer(DataContract.Customer cust, ref String Msg, DataContract._CustomerProfile Profile)
		{
			try
			{

				Customer objCustomer = _edriveEntity.Customer.First(m => m.CustomerID == cust.customerID);
				objCustomer.Active = true;
				objCustomer.Email = cust.email;
				// objCustomer.Password = cust.password;
				objCustomer.Gender = cust.Gender;
				objCustomer.CustomerGUID = Guid.NewGuid();
				objCustomer.FirstName = cust.FirstName;
				objCustomer.LastName = cust.LastName;
				objCustomer.Name = cust.FirstName + " " + cust.LastName;
				objCustomer.DateOfBirth = cust.DateofBirth;

				objCustomer.Company = cust.CompanyName;
				objCustomer.StreetAddress = cust.StreetAddress1;
				objCustomer.StreetAddress2 = cust.StreetAddress2;
				objCustomer.ZipPostalCode = cust.Zip;
				objCustomer.City = cust.City;
				if(cust.StateID > 0)
				{
					objCustomer.Stateid = cust.StateID;
				}
				objCustomer.Phone = cust.Phone;
				objCustomer.Fax = cust.Fax;
				objCustomer.newsletter = cust.Newsletter;

				objCustomer.CustomerType = _edriveEntity.Customer_Type.First(m => m.Role == "Dealer").ID;
				//_edriveEntity.Customer.AddObject(objCustomer);

				var _Profile = _edriveEntity.CustomerProfile.FirstOrDefault(m => m.CustomerID == cust.customerID);
				if(_Profile == null)//if  profiel not exist then create
				{
					_Profile = new CustomerProfile();
					_edriveEntity.CustomerProfile.AddObject(_Profile);
				}
				_Profile.CustomerID = objCustomer.CustomerID;
				_Profile.ApplicationURL = Profile.ApplicationURL;
				_Profile.Description = Profile.Description;
				_Profile.Logo = Profile.Logo;
				_Profile.PageImage = Profile.PageImage;
				_Profile.ServiceURL = Profile.ServiceURL;
				_Profile.WarrantyURL = Profile.WarrantyURL;

				if(objCustomer.ZipPostalCode != cust.Zip || objCustomer.City.Trim() != cust.City.Trim()) //-- then update products for its zip name for search page.
				{
					var prdts = _edriveEntity.Product.Where(m => m.CustomerID == objCustomer.CustomerID);
					foreach(var item in prdts)
					{
						try
						{
							item.Name = item.Product_Model.Product_Make.Make + " " + item.Product_Model.ModeLName.ToString() + " " + item.VIN + " " + item.Year + " " + cust.City.Trim() + (cust.Zip == 0 ? "" : " " + cust.Zip.ToString());

						}
						catch(Exception ex)
						{

						}

					}
				}
				_edriveEntity.SaveChanges();
				return true;
			}
			catch(Exception ex)
			{
				Msg = ex.Message;
				return false;
			}

		}

		public List<DataContract.Customer> GetDealersByZip(string zipcode)
		{
			List<DataContract.Customer> result = new List<DataContract.Customer>();
			int zip;

			if(int.TryParse(zipcode, out zip))
			{
				result = _edriveEntity.Customer
					.Where(c => c.ZipPostalCode == zip && c.Deleted == false && c.Active == true)
					.Select(m =>
							new DataContract.Customer
							{
								customerID = m.CustomerID,
								email = m.Email,
								Name = m.Name,
								FirstName = m.FirstName,
								LastName = m.LastName,
								CompanyName = m.Company,
								Phone = m.Phone,
								City = m.City,
								StateID = m.Stateid ?? 0,
								StateName = m.StateProvince.Name,
								StreetAddress1 = m.StreetAddress,
								StreetAddress2 = m.StreetAddress2,
								Zip = m.ZipPostalCode ?? 0,
							}).ToList();
			}

			return result;
		}

		public DataContract.ED_Zipcodes GetED_ZipCodes(String ZipCode)
		{

			var ZipObj = _edriveEntity.ED_Zipcodes.FirstOrDefault(m => m.zip_code == ZipCode);
			if(ZipObj != null)
				return new DataContract.ED_Zipcodes { longitude = ZipObj.longitude, latitude = ZipObj.latitude, zip_code = ZipObj.zip_code };
			else
				return null;

		}

		/// <summary>
		/// It return the HotSheeet for the Admin Section
		/// </summary>
		/// <param name="latitude"></param>
		/// <param name="longitude"></param>
		/// <param name="NewLatitude"></param>
		/// <param name="NewLongitude"></param>
		/// <param name="makeID"></param>
		/// <param name="Zipcode"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="CarsCount"></param>
		/// <returns></returns>
		public List<DataContract.Products> GetHotSheet(double? latitude, double? longitude,
			double NewLatitude, double NewLongitude, int makeID, string Zipcode, Int32 pageIndex, Int32 pageSize, out Int32 CarsCount)
		{
			//------------live site stored proc Nop_ProductLoadAllForHotsheetNew------------------
			var skipRecords = pageIndex * pageSize;
			String[] zipcodes = null;
			if(String.IsNullOrEmpty(Zipcode) == false)///--get the zipcodes under the range of lattitude and longitude.
			{
				zipcodes = _edriveEntity.ED_Zipcodes.Where(m => m.latitude >= latitude && m.latitude <= NewLatitude
				 && m.longitude >= longitude && m.longitude <= NewLongitude).Select(m => m.zip_code).ToArray();
			}
			String MakeidExp = "it.Deleted=false and true and ";

			if(makeID != -1)//i.e. All makes is selected
				MakeidExp = "it.Deleted=false and it.Product_Model.Product_Make.id=" + makeID + " and ";
			List<Product> products = new List<Product>();

			if(zipcodes != null && zipcodes.Length > 0)//--if any row for zips under the lattitued and longitude is found---
			{
				//--get all customers-

				for(int i = 0; i < zipcodes.Length; i++)
				{
					Int32 ZipPostalCode = Convert.ToInt32(zipcodes[i]);
					//Get all Product  for this customer zippostalcode and Make

					var st1 = _edriveEntity.Product.Where(MakeidExp + "it.Customer.ZipPostalCode=" + ZipPostalCode).ToList();

					//Get all Product  for this Product zippostalcode and Make
					var st2 = _edriveEntity.Product.Where(MakeidExp + "it.zip=" + ZipPostalCode).ToList();
					products = products.Union(st1.Union(st2)).ToList();
				}
				CarsCount = products.Count();
				return products.Skip(skipRecords).Take(pageSize).Select(m => new DataContract.Products
				{
					productId = m.ProductId,
					name = m.Name,
					MakeName = m.Product_Model.Product_Make.Make,
					ModelName = m.Product_Model.ModeLName,
					Year = m.Year ??
						0,
					averageTradeinPrice = m.AverageTradeinPrice ?? 0,
					averageRetailPrice = m.AverageRetailPrice ?? 0,
					price_Current = m.Price_Current ?? 0,
					customerId = m.CustomerID ?? 0
				}).ToList();
			}
			else////--if no row for zips exists
			{
				if(String.IsNullOrEmpty(Zipcode) == false)
				{
					////--if no row for zips under the lattitued and longitude is found search against the zip---
					Int32 ZipPostalCode = Convert.ToInt32(Zipcode);
					var st = _edriveEntity.Product.Where(MakeidExp + "it.Customer.ZipPostalCode=" + ZipPostalCode).ToList();

					products = products.Union(st).ToList();
					//Get all Product  for this Product zippostalcode and Make

					st = _edriveEntity.Product.Where(MakeidExp + "it.zip=" + ZipPostalCode).ToList();


					products = products.Union(st).ToList();
					CarsCount = products.Count();
					return products.Skip(skipRecords).Take(pageSize).Select(m => new DataContract.Products
					{
						productId = m.ProductId,
						name = m.Name,
						MakeName = m.Product_Model.Product_Make.Make,
						ModelName = m.Product_Model.ModeLName,
						Year = m.Year ?? 0,
						averageTradeinPrice
							= m.AverageTradeinPrice ?? 0,
						averageRetailPrice = m.AverageRetailPrice ?? 0,
						price_Current = m.Price_Current ?? 0,
						customerId = m.CustomerID ?? 0
					}).ToList();
				}
				else//--if zip code is empty search will be done only on Make( if Make is specified i.e. Makeid!=-1) 
				{
					if(makeID != -1)
					{
						//make can be used with only product not on customer
						products = products.Union(_edriveEntity.Product.Where(MakeidExp + " true ")).ToList();
						CarsCount = products.Count();
						return products.Skip(skipRecords).Take(pageSize).Select(m => new DataContract.Products
						{
							productId = m.ProductId,
							name = m.Name,
							MakeName = m.Product_Model.Product_Make.Make,
							ModelName = m.Product_Model.ModeLName,
							Year = m.Year ?? 0,
							averageTradeinPrice = m.AverageTradeinPrice ?? 0,
							averageRetailPrice = m.AverageRetailPrice ?? 0,
							price_Current = m.Price_Current ?? 0,
							customerId = m.CustomerID ?? 0
						}).ToList();
					}
					else// other wise return all records-- without search----
					{

						products = products.Union(_edriveEntity.Product.Where(MakeidExp + " true ")).ToList();
						CarsCount = products.Count();
						return products.Skip(skipRecords).Take(pageSize).
							Select(m => new DataContract.Products
							{
								productId = m.ProductId,
								name = m.Name,
								MakeName = m.Product_Model.Product_Make.Make,
								ModelName = m.Product_Model.ModeLName,
								Year = m.Year ?? 0,
								averageTradeinPrice = m.AverageTradeinPrice ?? 0,
								averageRetailPrice = m.AverageRetailPrice ?? 0,
								price_Current = m.Price_Current ?? 0,
								customerId = m.CustomerID ?? 0
							}).
							ToList();


					}
				}
			}

		}
		/// <summary>
		/// Gets the List of States by CountryID
		/// </summary>
		/// <param name="CountryID"></param>
		/// <returns></returns>


		public List<_StateProvince> GetStateProvince_By_CountryID(Int32 CountryID)
		{
			return _edriveEntity.StateProvince.Where(m => m.CountryID == CountryID).Select(
				 m => new DataContract._StateProvince { CountryID = m.CountryID, Name = m.Name, StateProvinceID = m.StateProvinceID }).ToList();
		}

		/// <summary>
		/// This returns the couns of Seller and Dealers for Hot Sheet page.
		/// </summary>
		/// <param name="zip"></param>
		/// <param name="Make"></param>
		/// <param name="lat1"></param>
		/// <param name="lat2"></param>
		/// <param name="long1"></param>
		/// <param name="long2"></param>
		/// <param name="SellerCount"></param>
		/// <param name="DealerCount"></param>
		public DataContract._SellerCount GetSellerCount_for_HotSheet(String zip, Int32? Make, double? lat1, double? lat2, double? long1, double? long2)
		{
			var obj = _edriveEntity.Nop_SellerCnt(zip, Make, lat1, lat2, long1, long2).First();
			if(obj != null)
			{
				return new DataContract._SellerCount
				{
					Dealer = obj.Dealer,
					Seller = obj.Seller
				};
			}
			else
			{
				return null;
			}
		}
		#endregion
		#region Intrested Customer
		/// <summary>
		/// Add Intrested customer details
		/// Created by Harpreet Singh on 16-02-2012
		/// </summary>
		/// <param name="_customer"></param>
		/// <param name="_productid"></param>
		/// <returns></returns>
		public Boolean IntrestedCustomer(IntrestedCustomer _customer, int _productid)
		{
			//create table object
			ED_InterestedCustomer objCustomer = new ED_InterestedCustomer();
			try
			{
				objCustomer.ProductId = _customer.productId;
				objCustomer.FirstName = _customer.firstname;
				objCustomer.LastName = _customer.lastname;
				objCustomer.CustomerId = _customer.customerId;

				objCustomer.Email = _customer.email;


				objCustomer.PhoneNumber = _customer.phoneno;
				objCustomer.ContactType = _edriveEntity.ContactType.First(m => m.ContactType1 == _customer.contactType).id;
				objCustomer.AdditionalComment = _customer.additionalComments;
				objCustomer.IsActive = true;
				objCustomer.Financing = _customer.finacing;
				objCustomer.Trade_in = _customer.Trade_in;
				objCustomer.Price_Lock = _customer.price_Lock;
				objCustomer.CreatedOn = _customer.createdOn;
				objCustomer.UpdatedOn = _customer.updateOn;
				objCustomer.InterestType = _customer.intrestType;


				_edriveEntity.ED_InterestedCustomer.AddObject(objCustomer);
				_edriveEntity.SaveChanges();
				return true;
			}
			catch(Exception ex)
			{
				return false;
				throw new Exception(ex.Message);
			}
		}
		#endregion



		/// <summary>
		/// Search on home page
		/// </summary>
		/// <param name="searchKey"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <param name="CarsCount"></param>
		/// <param name="SortByColumn"></param>
		/// <returns></returns>
		/// 

		#region SearchMethod

		public List<Products> SearchProductBy_Make_Model_City_Zip(string searchKey, int pageSize, int pageIndex, ref int? CarsCount, string SortByColumn,
			 ref String Price, ref String Milage, ref String Make,
			 ref string ModelID, ref String Year, ref String Body, ref String Type, Int32 Zip, int? radius, String Warranty,
			 ref String Vin, String Transmission, String Engine, ref String DriveType, Boolean isSearchByDealer, Int32 SearchByDealerID)
		{
			int descriptionLength = 150;// number of characters to substring from the description

			if(pageIndex < 1)
				pageIndex = 1;
			else
				pageIndex += 1;

			var products = SearchStandard(searchKey, pageSize, pageIndex, ref CarsCount, SortByColumn, ref Price, ref Milage,
										  ref Make, ref ModelID, ref Year,
										  ref Body, Zip, radius, Warranty);

			GetProductsPictures(products);
			TrimDescription(descriptionLength, products);

			return products;
		}

		private List<DataContract.Products> SearchStandard(string searchText, int pageSize, int pageIndex, ref int? carsCount, string sortByColumn,
			ref string price, ref string mileage, ref string make,
			ref string model, ref String year, ref string body, int zip, int? radius, string warrantyStr)
		{
			if(String.IsNullOrWhiteSpace(sortByColumn) ||
				(!sortByColumn.StartsWith("Year") && !sortByColumn.StartsWith("Mileage") && !sortByColumn.StartsWith("Price") && !sortByColumn.StartsWith("savingAmount")))
				sortByColumn = "ProductId";
			if(sortByColumn.StartsWith("Price"))
			{
				if(sortByColumn.ToLower().EndsWith("desc"))
					sortByColumn = "Price_Current desc";
				else
					sortByColumn = "Price_Current";
			}

			if(String.IsNullOrWhiteSpace(searchText))
				searchText = null;
			else
				searchText = searchText.Trim().Replace(" and ", " ");

			model = !String.IsNullOrWhiteSpace(model) && !model.StartsWith("-1") ? model.Trim(',').Replace(" ", "") : null;
			make = !String.IsNullOrWhiteSpace(make) && !make.StartsWith("-1") ? make.Trim(',').Replace(" ", "") : null;
			year = !String.IsNullOrWhiteSpace(year) && !year.StartsWith("-1") ? year.Trim(',').Replace(" ", "") : null;
			body = !String.IsNullOrWhiteSpace(body) && !body.StartsWith("-1") ? body.Trim(',').Replace(" ", "") : null;
			price = !String.IsNullOrWhiteSpace(price) && !price.StartsWith("-1") ? price.Trim(',').Replace(" ", "") : null;
			mileage = !String.IsNullOrWhiteSpace(mileage) && !mileage.StartsWith("-1") ? mileage.Trim(',').Replace(" ", "") : null;
			string zipCode = zip > 0 ? zip.ToString().PadLeft(5, '0') : null;
			string sortColumn = sortByColumn.Split(' ')[0].Trim();
			string sortOrder = sortByColumn.EndsWith(" desc") ? "desc" : null;
			bool? warranty = null;
			radius = radius.HasValue && radius > 0 ? radius : null;

			if(!String.IsNullOrWhiteSpace(zipCode) && radius.GetValueOrDefault(0) <= 0)
				radius = 100;//default radius in miles.
			if(String.IsNullOrWhiteSpace(zipCode))
				radius = null;

			if(!String.IsNullOrWhiteSpace(warrantyStr) && !warrantyStr.StartsWith("-1"))
				warranty = warrantyStr == "1";

			ObjectParameter totalCount = new ObjectParameter("tOTALRESULTS", 0);
			ObjectParameter totalPages = new ObjectParameter("tOTALPAGES", 0);
			ObjectParameter exactMatch = new ObjectParameter("eXACTMATCH", 0);

			var result = _edriveEntity.SearchStandard(searchText, zipCode, radius, price, year, mileage, make, model,
				warranty, body, true, sortColumn, sortOrder, pageSize, pageIndex, totalCount, totalPages, exactMatch).Select(m => new Products
				{
					productId = m.ProductId,
					pics = m.Pics,
					mileage = m.Mileage ?? 0,
					body = m.Product_Body.Body,
					price_Current = m.Price_Current ?? 0,
					transmission = m.Transmission ?? "",
					exterior = m.Exterior_Color ?? "",
					OwnerDetail = m.OwnerDetail,
					drive_Type = m.Drive_Type,
					vin = m.VIN,
					MakeName = m.Product_Model.Product_Make.Make,
					ModelName = m.Product_Model.ModeLName,
					//zip = m.zip??0,
					zip = m.Customer.ZipPostalCode ?? 0,
					Year = m.Year ?? 0,
					model = m.Model,
					interior = m.Interior_Color,
					updatedOn = m.UpdatedOn,
					descriptiont = m.Description ?? ""
				}).ToList();

			carsCount = (int)totalCount.Value;

			return result;
		}

		private List<DataContract.Products> SearchAdvanced(AdvancedSearchAttributes attributes, int pageSize, int pageIndex, string sortByColumn, out int count)
		{
			if(String.IsNullOrWhiteSpace(sortByColumn) ||
				(!sortByColumn.StartsWith("Year") && !sortByColumn.StartsWith("Mileage") && !sortByColumn.StartsWith("Price") && !sortByColumn.StartsWith("savingAmount")))
				sortByColumn = "ProductId";
			if(sortByColumn.StartsWith("Price"))
			{
				if(sortByColumn.ToLower().EndsWith("desc"))
					sortByColumn = "Price_Current desc";
				else
					sortByColumn = "Price_Current";
			}

			string zipCode = attributes._zip > 0 ? attributes._zip.ToString().PadLeft(5, '0') : null;
			string sortColumn = sortByColumn.Split(' ')[0].Trim();
			string sortOrder = sortByColumn.EndsWith(" desc") ? "desc" : null;
			string vin = !String.IsNullOrWhiteSpace(attributes._vin) ? attributes._vin.Trim() : null;
			int? radius = Convert.ToInt32(attributes._radius);
			radius = radius > 0 ? radius : null;

			if(!String.IsNullOrWhiteSpace(zipCode) && radius.GetValueOrDefault(0) <= 0)
				radius = 100;//default radius in miles.
			if(String.IsNullOrWhiteSpace(zipCode))
				radius = null;

			ObjectParameter totalCount = new ObjectParameter("tOTALRESULTS", 0);
			ObjectParameter totalPages = new ObjectParameter("tOTALPAGES", 0);

			var result = _edriveEntity.SearchAdvanced(attributes._make, attributes._model, attributes._mileageTo, attributes._minYaer, attributes._maxYear,
				attributes._minPrice, attributes._maxPrice, attributes._body, attributes._transmission, attributes._engine, attributes._driveType,
				vin, zipCode, true, radius, sortColumn, sortOrder, pageSize, pageIndex,
				totalCount, totalPages).Select(m => new Products
				{
					productId = m.ProductId,
					pics = m.Pics,
					mileage = m.Mileage ?? 0,
					body = m.Product_Body.Body,
					price_Current = m.Price_Current ?? 0,
					transmission = m.Transmission ?? "",
					exterior = m.Exterior_Color ?? "",
					OwnerDetail = m.OwnerDetail,
					drive_Type = m.Drive_Type,
					vin = m.VIN,
					MakeName = m.Product_Model.Product_Make.Make,
					ModelName = m.Product_Model.ModeLName,
					//zip = m.zip??0,
					zip = m.Customer.ZipPostalCode ?? 0,
					Year = m.Year ?? 0,
					model = m.Model,
					interior = m.Interior_Color,
					updatedOn = m.UpdatedOn,
					descriptiont = m.Description ?? ""
				}).ToList();

			count = (int)totalCount.Value;

			return result;
		}

		/// <summary>
		/// to get the total CarsCounts fo search expression
		/// </summary>
		/// <param name="searchKey"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <param name="CarsCount"></param>
		/// <param name="SortByColumn"></param>
		/// <param name="Price"></param>
		/// <param name="Milage"></param>
		/// <param name="Make"></param>
		/// <param name="ModelID"></param>
		/// <param name="Year"></param>
		/// <param name="Body"></param>
		/// <param name="Type"></param>
		/// <param name="Zip"></param>
		/// <param name="Warranty"></param>
		/// <param name="Vin"></param>
		/// <param name="Transmission"></param>
		/// <param name="Engine"></param>
		/// <param name="DriveType"></param>
		/// <param name="isSearchByDealer"></param>
		/// <param name="SearchByDealerID"></param>
		public void SearchProductBy_Make_Model_City_Zip_Count(string searchKey, int pageSize, int pageIndex, ref int? CarsCount, string SortByColumn,
			  ref String Price, ref String Milage, ref String Make,
			  ref string ModelID, ref String Year, ref String Body, ref String Type, Int32 Zip, int? radius, String Warranty,
			  ref String Vin, String Transmission, String Engine, ref String DriveType, Boolean isSearchByDealer, Int32 SearchByDealerID)
		{
			SearchStandard(searchKey, 1, 1, ref CarsCount, SortByColumn, ref Price, ref Milage,
										  ref Make, ref ModelID, ref Year,
										  ref Body, Zip, radius, Warranty);
		}

		/// <summary>
		/// To trim the description of PRoduct Item
		/// </summary>
		/// <param name="descriptionLength"></param>
		/// <param name="lstproducts"></param>
		private static void TrimDescription(Int32 descriptionLength, List<Products> lstproducts)
		{
			foreach(var item in lstproducts)
			{
				if(String.IsNullOrEmpty(item.descriptiont) == false)
				{
					if(item.descriptiont.Length > descriptionLength)
					{
						item.descriptiont = item.descriptiont.Substring(0, descriptionLength) + "...";
					}
				}
			}
		}


		#endregion

		/// <summary>
		/// To show the autocomplete box for home page search. it pulls the car name
		/// </summary>
		/// <param name="searchKey"></param>
		/// <param name="Counts_to_ret"></param>
		/// <returns></returns>
		public List<Products> SearchCars_for_home_Page(String searchKey, Int32 Counts_to_ret)
		{
			if(String.IsNullOrWhiteSpace(searchKey))
				return new List<Products>();

			var searcharray = searchKey.Trim().Split(' ');

			var query =
				_edriveEntity.Product_Model.Select(
					m =>
					new
					{
						name = m.Product_Make.Make + " " + m.ModeLName,
						model = m.id,
						modelName = m.ModeLName,
						makeName = m.Product_Make.Make
					});

			query = searcharray.Select(q => q.Trim()).Aggregate(query, (current, key) => current.Where(m => m.name.Contains(key.Trim())));
			var result = query.OrderBy(m => m.makeName).ThenBy(m => m.modelName).Take(Counts_to_ret).ToList();

			return result.Select(m => new Products { name = m.name }).ToList();
		}

		private void GetProductsPictures(List<EdriveService.DataContract.Products> lstModel)
		{
			///---------------get product picture-----------
			///var Ad_Pics = _edriveEntity.ProductPicture.Where(m => m.ProductID == ProductID).OrderBy(m => m.DisplayOrder);

			foreach(var item in lstModel)
			{
				List<Product_Picture> picture = GetProductPicture_By_ProductID(item.productId);

				if(picture != null)
				{
					var picUrls = "";
					foreach(var picitem in picture)
					{
						if(String.IsNullOrEmpty(picitem.PictureURL) == false)
						{

							picUrls += picitem.PictureURL + ";";
						}

					}
					if(picUrls != "")
					{
						item.pics = picUrls;
					}


				}

			}

			///
		}

		/// <summary>
		/// This method return the product picture from product picture table
		/// </summary>
		/// <param name="ProductID"></param>
		/// <returns></returns>
		public List<Product_Picture> GetProductPicture_By_ProductID(Int32 ProductID)
		{
			if(_edriveEntity.ProductPicture.Any(m => m.ProductID == ProductID) == false)
			{
				var pics = _edriveEntity.Product.FirstOrDefault(m => m.ProductId == ProductID);

				if(pics != null && pics.Pics != null)
				{
					var picsarray = pics.Pics.Split(';');
					AddPicsToProduct(picsarray, ProductID);
				}
			}
			return _edriveEntity.ProductPicture.Where(m => m.ProductID == ProductID)
				.OrderBy(m => m.DisplayOrder)
				.Select(m => new DataContract.Product_Picture
				{
					ProductPictureID = m.ProductPictureID,
					ProductID = m.ProductID,
					PictureURL = m.PictureURL,
					DisplayOrder = m.DisplayOrder
				}).ToList();
		}
		/// <summary>
		/// To return the total Cars Count on header of page.
		/// </summary>
		/// <returns></returns>
		public Int32 Get_TotalVehicles_Count()
		{
			int totalCount = 0;
			try
			{
				//				string expression = String.Empty;
				//				var countDownDaysSettings = _edriveEntity.Settings.FirstOrDefault(c => c.Name == "CountDown.Days");
				//				int countDownDays = countDownDaysSettings != null ? Convert.ToInt32(countDownDaysSettings.Value) : 28;
				//
				//				expression +=
				//					String.Format(
				//						" SqlServer.DATEDIFF('minute', SqlServer.getdate(), SqlServer.DateAdd('day', {0}, it.updatedon)) > 0 ",
				//						countDownDays);

				totalCount = _edriveEntity.Product.Count(m => m.Deleted == false);
			}
			catch
			{
				totalCount = 0;
			}

			return totalCount;
		}
		/// <summary>
		/// To return the total Dealers Count on header of page.
		/// </summary>
		/// <returns></returns>
		public Int32 Get_TotalDealers_Count()
		{
			return _edriveEntity.Customer.Where(m => m.Deleted == false && m.Customer_Type.Role == "Dealer").Count();

		}

		/// <summary>
		/// To get the list of all interested customer
		/// </summary>
		/// <param name="InterestType"></param>
		/// <returns></returns>
		public List<DataContract.IntrestedCustomer> Get_InterestedCustomer(Int32 InterestType)
		{

			var lstInterested = _edriveEntity.ED_InterestedCustomer.Where(m => m.IsActive == true && m.InterestType == InterestType).ToList();
			var lstInterestedCustomer = new List<DataContract.IntrestedCustomer>();

			foreach(var item in lstInterested)
			{
				var it =
				new DataContract.IntrestedCustomer
				{
					additionalComments = item.AdditionalComment,
					contactType = item.ContactType == null ? "" : item.ContactType.Value.ToString(),
					createdOn = item.CreatedOn ?? DateTime.Now,
					customerId = item.CustomerId ?? 0,
					email = item.Email,
					finacing = item.Financing ?? false,
					firstname = item.FirstName,
					intrestType = item.InterestType ?? 0,
					IsActive = true,
					lastname = item.LastName,
					phoneno = item.PhoneNumber,
					price_Lock = item.Price_Lock ?? false,
					productId = item.ProductId ?? 0,
					Trade_in = item.Trade_in ?? false,
					updateOn = item.UpdatedOn ?? DateTime.Now
				 ,
					InterestedCustomerID = item.InterestedCustomerID

				};
				var zipcode = 0;
				if(Int32.TryParse(item.ZipCode, out zipcode))
				{
					it.zipcode = zipcode;
				}
				lstInterestedCustomer.Add(it);
			}
			return lstInterestedCustomer;


		}
		/// <summary>
		/// To Delete the Interested Customer Records
		/// </summary>
		/// <param name="id"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		public Boolean Delete_InterestedCustomer(int id, out  String Msg)
		{
			Msg = "";
			try
			{
				var obj = _edriveEntity.ED_InterestedCustomer.First(m => m.InterestedCustomerID == id);
				obj.IsActive = false;
				_edriveEntity.SaveChanges();
				return true;
			}
			catch(Exception ex)
			{
				Msg = ex.Message;
				return false;
			}
		}


	}

}

