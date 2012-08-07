using System;
using System.IO;
using System.Linq;

//<Summary>
	// This class used to created log files
	// Created by ali ahmad h - 2002
	//</Summary>

	public class CreateLogFiles
	{
		private string sLogFormat;
		private string sErrorTime;

		public CreateLogFiles()
		{
			//sLogFormat used to create log files format :
			// dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
			sLogFormat = DateTime.Now.ToShortDateString().ToString()+" "+DateTime.Now.ToLongTimeString().ToString()+" ==> ";
			
			//this variable used to create log filename format "
			//for example filename : ErrorLogYYYYMMDD
			string sYear	= DateTime.Now.Year.ToString();
			string sMonth	= DateTime.Now.Month.ToString();
			string sDay	= DateTime.Now.Day.ToString();
            sErrorTime = sYear + "_" + sMonth + "_" + sDay + "_" + DateTime.Now.Ticks.ToString() + ".txt";
		}

		public void ErrorLog(Exception sErrMsg)
        {
            TextWriter sw = new StreamWriter(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath,"ErrorLog","Error_"+sErrorTime));
			 
			sw.WriteLine("-----------Error-------------\r\n");
            sw.WriteLine("-----------Exception-------------\r\n");
            sw.WriteLine(sErrMsg.Message+"\r\n");
            sw.WriteLine("-----------Inner Exception-------------\r\n");
            sw.WriteLine(sErrMsg.InnerException==null?"":sErrMsg.InnerException.Message);


			sw.Flush();
			sw.Close();
            throw sErrMsg;
		}
	}

