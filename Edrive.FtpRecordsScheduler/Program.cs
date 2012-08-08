using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edrive.FtpRecordsScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
// ReSharper disable SimplifyConditionalTernaryExpression
            const bool isLoggerOn = true;
            string logFileName = DateTime.UtcNow.ToString("yyyyMMdd") + "log.txt";
// ReSharper restore SimplifyConditionalTernaryExpression

            var logger = new LoggerHelper(logFileName);

            if (isLoggerOn)
            {
                logger.AddLog(String.Format("A FTP utility started the work at {0}.", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm")));
                logger.AddLog(String.Format("Sending the request for getting the data from Aultech folder at {0}.", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm")));
            }

            //Send Request for Aultech folder
            var responseAultech = HttpPostHelper.SendRequest("Home/TestService", "GetData=GetDataFromAULtec");

            if (isLoggerOn)
            {
                logger.AddLog("");
                logger.AddLog(String.Format("Got response for GetDataFromAULtec at {0}, response is {1}.", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm"), responseAultech));
                logger.AddLog("");
                logger.AddLog(String.Format("Sending the request for getting data from GetAuto folder at {0}.", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm")));
            }

            //Send Request for GetAuto folder
            var responseGetAuto = HttpPostHelper.SendRequest("Home/TestService", "GetData=GetDataFromGetAuto");

            if (isLoggerOn)
            {
                logger.AddLog("");
                logger.AddLog(String.Format("Got response for GetDataFromGetAuto at {0}, response is {1}.", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm"), responseGetAuto));
                logger.AddLog("");
                logger.AddLog(String.Format("Sending the request for getting data from Schumacher folder at {0}.", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm")));
            }

            //Send Request for Schumacher  folder
            var responseSchumacher = HttpPostHelper.SendRequest("Home/TestService", "GetData=GetDataFromSchumacher");

            if (isLoggerOn)
            {
                logger.AddLog("");
                logger.AddLog(String.Format("Got response for GetDataFromSchumacher at {0}, response is {1}.", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm"), responseSchumacher));
                logger.AddLog("");
                logger.AddLog(String.Format("Sending the request for getting data from Autobase folder at {0}.", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm")));
            }

            //Send Request for Autobase  folder
            var responseAutobase = HttpPostHelper.SendRequest("Home/TestService", "GetData=GetDataFromAutoBase");

            if (isLoggerOn)
            {
                logger.AddLog("");
                logger.AddLog(String.Format("Got response for GetDataFromAutoBase at {0}, response is {1}.", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm"), responseAutobase));
                logger.AddLog("");
                logger.AddLog(String.Format("The FTP utility finished the work at {0}.", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm")));
                logger.AddLog("");
            }
        }
    }
}
