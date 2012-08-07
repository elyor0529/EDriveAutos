using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Edrive.Helpers
{
    public class Log
    {
        public static void Event(string title, string message, string code)
        {
            var fileName = HttpContext.Current.Server.MapPath(String.Format("~/Content/ErrorLog/{0}_{1}.txt",
            title,
            DateTime.Now.ToString("dd_MM_yyy_hh_mm_ss")));
            StreamWriter fs = new StreamWriter(fileName);
            fs.WriteLine("--------------------Message--------------------");
            fs.WriteLine(title);
            fs.WriteLine("--------------------InnerException--------------------");
            fs.WriteLine(message);
            fs.WriteLine("--------------------error code--------------------");
            fs.WriteLine(code);
            fs.Close();
        }
    }
}