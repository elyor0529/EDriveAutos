using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace EdriveService.BAL
{
    public class GetDealerInfo
    {
        private byte[] downloadedData;
        public string downloadFile(string FTPAddress, string fileName, string username, string password, int customerId)
        {
            try
            {
                string msg = string.Empty;
                downloadedData = new byte[0];
                string name = Path.GetFileName(fileName);
                string filePath = string.Empty;


                //Create FTP request
                //Note: format is ftp://server.com/file.ext
                FtpWebRequest request = FtpWebRequest.Create(FTPAddress + "/" + name) as FtpWebRequest;

                //Get the file size first (for progress bar)
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = true; //don't close the connection

                int dataLength = (int)request.GetResponse().ContentLength;

                //Now get the actual data
                request = FtpWebRequest.Create(FTPAddress + "/" + name) as FtpWebRequest;
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

                while (true)
                {

                    //Try to read the data
                    int bytesRead = reader.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
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



                if (downloadedData != null && downloadedData.Length != 0)
                {
                    //String ss = System.Configuration.ConfigurationManager.AppSettings["CarfaxPath"].ToString();
                    filePath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "EdriveDataCsv\\Carfax\\"  +name;
                    //Write the bytes to a file
                    FileStream newFile = new FileStream(filePath, FileMode.Create);
                    newFile.Write(downloadedData, 0, downloadedData.Length);
                    newFile.Close();

                }

                msg = "Data downloaded successfully.";
                username = string.Empty;
                password = string.Empty;

                return msg;
            }
            catch (Exception ex)
            {
                insertCarfaxLog(ex.Message.ToString(), DateTime.Now, 1, 1, customerId);
                return string.Empty;
            }


        }
        public string GenerateCSVFiles(string parameterName, string spName, int customerId)
        {
            try
            {
                string fileName = string.Empty;
                string path = string.Empty;
                string ans = string.Empty;
                string filePath = string.Empty;
                DateTime dtd, date = new DateTime();

                date = DateTime.Now;
                dtd = Convert.ToDateTime(date.ToShortDateString());

                string fileDate = (dtd).ToString("MMddyyyy");
               
                fileName = parameterName;
                fileName = fileName + fileDate + ".txt";

                String CarfaxPath = ConfigurationManager.AppSettings["CarfaxPath"].ToString();

                filePath = CarfaxPath;

                DataTable dtData = new DataTable();

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
                using (SqlCommand cmd= new SqlCommand(spName,con))
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        dtData.Load(cmd.ExecuteReader());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                        }

                    }


                }
                 
                if (dtData.Rows.Count > 0)
                {
                    CreateCSVFile(dtData, fileName);
                    path = filePath + fileName;
                    ans = UploadData(path);
                    return ans;
                }
                else
                {
                    return ans;
                    throw new Exception("Delaer's information not found.");
                }

            }
            catch (Exception ex)
            {
                insertCarfaxLog(ex.Message.ToString(), DateTime.Now, 1, 1, customerId);
                return string.Empty;
            }
        }
        private void CreateCSVFile(DataTable dt, string fileName)
        {
            try
            {

                string filePath = string.Empty;

                // filePath = @"D:\Edrive\Carfax\" + fileName;

                StringBuilder sb = new StringBuilder();

                string ss = ConfigurationManager.AppSettings["CarfaxPath"];
                filePath = ss + fileName;

                StreamWriter sw = new StreamWriter(filePath, false);

                //for header of the table 
                //foreach (DataColumn col in dt.Columns
                //{
                //    sb.Append('"' + col.ColumnName + '"' + "|");
                //}

                //sb.Remove(sb.Length - 1, 1);
                //sb.Append(Environment.NewLine);

                foreach (DataRow row in dt.Rows)
                {
                    if (dt.Columns.Count == 2)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sb.Append(row[i].ToString() + "|");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sb.Append('"' + row[i].ToString() + '"' + "|");
                        }
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(Environment.NewLine);
                }
                sw.Write(sb.ToString());
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

   

        private string UploadData(string filePath)
        {
            string FTPAddress;
            string username;
            string password;
            string msg = string.Empty;

            //FTPAddress = "ftp://173.248.133.242/files/CARFAX_Testing";
            //username = "edrive_henisha";
            //password = "edrive2324";

            FTPAddress = "ftp://ftp.carfax.com";
            username = "EDRIVEAUTO";
            password = "8883163374";

            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(FTPAddress + "/" + Path.GetFileName(filePath));
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;


            FileStream stream = File.OpenRead(filePath);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();

            Stream reqStream = request.GetRequestStream();
            reqStream.Write(buffer, 0, buffer.Length);
         
            reqStream.Close();
            msg = "File Uploaded Successfully.";
            return msg;

        }

        public void insertCarfaxLog(string logmsg, DateTime logdate, int status, int success, int CustomerId)
        {
            using (edriveautoEntities _enity = new edriveautoEntities())
            {
                ED_CarfaxLogDetail newCarFaxObj = new ED_CarfaxLogDetail
                {
                    LogMsg = logmsg,
                    CreateBy = CustomerId,
                    CreateOn = DateTime.Now,
                    Status = status,
                    Success = success
                };
                _enity.ED_CarfaxLogDetail.AddObject(newCarFaxObj);
                _enity.SaveChanges();

            }
        }
        public string updateAllProduct(string fileName, int customerId)
        {DataTable dtData = new DataTable();
            try
            {
                string msg = string.Empty;
                using (edriveautoEntities _entiye = new edriveautoEntities())
                {
                    IDbConnection conn = (_entiye.Connection as EntityConnection).StoreConnection;
                    conn.Open();
                    
                    using (DbCommand cmd = (DbCommand)conn.CreateCommand())
                    {

                       
                            try
                            {
                                //if (cmd.Connection.State == ConnectionState.Closed)
                                //{
                                //    cmd.Connection.Open();
                                //}
//cmd.CommandText = "ED_ProductCarfaxReport";
                                //cmd.CommandType = CommandType.StoredProcedure;

                                //var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                                var s = System.Configuration.ConfigurationManager.AppSettings["CarfaxPath"].ToString(); ;
                                string path = Convert.ToString(s) + fileName;
                                _entiye.ExecuteStoreCommand("exec ED_ProductCarfaxReport @File={0}", path );
                                
                                //var p= cmd.CreateParameter();
                                //p.DbType = DbType.String;
                                //p.ParameterName = "@File";
                                //p.Value = path;
                                //cmd.Parameters.Add(p);
                                //dtData.Load(dr);

                            }
                            catch (Exception ex)
                            {


                            }
                            finally
                            {
                                if (cmd.Connection.State == ConnectionState.Open)
                                    cmd.Connection.Close();
                            }
                        
                    }
                }

              

                msg = "Products updated successfully";
                return msg;
            }
            catch (Exception ex)
            {
                insertCarfaxLog(ex.Message.ToString(), DateTime.Now, 1, 1, customerId);
                return string.Empty;
            }
        }


    }
}