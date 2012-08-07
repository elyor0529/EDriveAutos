using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using EdriveService.NADA_UsedCars;

namespace EdriveService.BAL
{
    /// <summary>
    /// This class is added to speed up the delete process of records while importing in services Qualify_All_Products() methods
    /// </summary>
    public class ThreadProcess
    {

        public enum ProductStatus { JunkRecord = 1, ExcetionInValidateMethod = 2, NewRecordInserted = 3, RecordUpdated = 4, PriceValidationFailed = 5, NadaWebServiceError = 6, Price_CastingError = 7, RecordDeleted }
        
       Int32 maxLimit;
        public DataTable tbl;
        public ThreadProcess(Int32  Lim,DataTable t)
        {
            tbl = t;
            maxLimit = Lim;
        }
        public void Process()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
             
            try
            {
                for (int i = maxLimit - 1000; i <= maxLimit && i <= tbl.Rows.Count; i++)
                {
                    if (i < 0)
                        continue;
                    DataRow dr = tbl.Rows[i];
                    string pics = dr["pics"].ToString();
                    string vin = dr["vin"].ToString();
                    Int32 Productid = Convert.ToInt32(dr["Productid"]);
                    decimal Price_Current = Convert.ToDecimal(dr["Price_Current"]);
                    ProductStatus Status = ProductStatus.JunkRecord;
                    Boolean IsPriceValid = ValidRecord(vin, Convert.ToDecimal(Price_Current), ref Status);
                    Boolean IsImageUrlExist = UrlExists(pics);
                    Boolean delProduct = false;
                    if (IsPriceValid == false)
                    {
                        if (Status == ProductStatus.PriceValidationFailed || String.IsNullOrEmpty(pics) || (UrlExists(pics) == false))
                        {
                            delProduct = true;
                        }
                    }
                    else
                        if (IsImageUrlExist == false)
                        {
                            delProduct = true;
                        }

                    if (delProduct)
                    {
                        SqlCommand cmdDelete = new SqlCommand("Update product set deleted=1 where productid=" + Productid.ToString(), con);
                        if (con.State == ConnectionState.Closed)
                            con.Open(); 
                        cmdDelete.ExecuteNonQuery();

                    }

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();


            }
 
        }
        public void RecoverDeleted()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

            try
            {
                for (int i = maxLimit - 1000; i <= maxLimit && i <= tbl.Rows.Count; i++)
                {
                    if (i < 0)
                        continue;
                    DataRow dr = tbl.Rows[i];
                    string pics = dr["pics"].ToString();
                    string vin = dr["vin"].ToString();
                    Int32 Productid = Convert.ToInt32(dr["Productid"]);
                    decimal Price_Current = Convert.ToDecimal(dr["Price_Current"]);
                    ProductStatus Status = ProductStatus.JunkRecord;
                    Boolean IsPriceValid = ValidRecord(vin, Convert.ToDecimal(Price_Current), ref Status);
                    Boolean IsImageUrlExist = UrlExists(pics);
                    // price is valid amd image exist then recover products
                    if (IsPriceValid == true && IsImageUrlExist == true)
                    {
                            SqlCommand cmdDelete = new SqlCommand("Update product set deleted=0 where productid=" + Productid.ToString(), con);
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmdDelete.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();


            }

        }
     
        private static bool UrlExists(string Pics)
        {

            if (String.IsNullOrEmpty(Pics))
                return false;
            try
            {
                String[] imgUrl = Pics.Split(';');
                Boolean flag = false;
                foreach (var url in imgUrl)
                {
                    try
                    {
                        new System.Net.WebClient().DownloadData(url);
                        flag = true;

                    }
                    catch (Exception ex)
                    {
                        break;
                    }

                }
                return flag;
            }
            catch (System.Net.WebException e)
            {
                if (((System.Net.HttpWebResponse)e.Response).StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;
                else
                    throw;
            }
        }
       
        private bool ValidRecord(String VIN, Decimal price, ref ProductStatus Status)
        {
            try
            {
                UsedCars oUsedCarService = new UsedCars();
                UsedCarResultSet oUsedCarsResultSet = oUsedCarService.GetUsedCars("EdriveAutos", "ed12uc20", VIN.ToString());
                NADA_UsedCars.UsedCar uc = oUsedCarsResultSet.UsedCars.FirstOrDefault();
                if (uc != null)
                {
                    //Nirav - 16-Aug-11
                    //Client wants Clean AverageRetailPrice now
                    //Decimal Five_PPrice = Convert.ToDecimal((uc.AverageRetailPrice) * 0.15);
                    //Decimal newAverageTradeInPrice = Convert.ToDecimal(uc.AverageRetailPrice) + Five_PPrice;
                    Decimal Five_PPrice = Convert.ToDecimal((uc.AverageRetailPrice));
                    Decimal newAverageTradeInPrice = Five_PPrice;

                    //if (price + 3000 <= newAverageTradeInPrice) //Nirav - 28/04/11 - to import all data for demo purpose. --Commented by henisha rathod 8-9-2011 4:16pm
                    //if (true) //Nirav - 28/04/11 - to import all data for demo purpose.
                    if (price <= newAverageTradeInPrice)//Added by henisha rathod 8-9-2011 4:16pm
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
            catch (Exception ex)
            {
                CreateLogFiles err = new CreateLogFiles();
                err.ErrorLog(ex);
                Status = ProductStatus.ExcetionInValidateMethod;
                return false;
            }
        }
    }
}