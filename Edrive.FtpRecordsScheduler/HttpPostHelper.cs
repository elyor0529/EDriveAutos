using System;
using System.Threading;

namespace Edrive.FtpRecordsScheduler
{
    public class HttpPostHelper
    {
        private const string BaseAppUrl = "http://www.edriveautos.com/";
       // private const string BaseAppUrl = "http://localhost/Edrive/";
        public static string SendRequest(string subUrl, string dataToSend)
        {
            try
            {
                return HttpPost(BaseAppUrl + subUrl, dataToSend);
            }
            catch (Exception ex)
            {
                return ex.Message + "||||" + ex.StackTrace;
            }
        }

        private static string HttpPost(string url, string parameters)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            req.Timeout = Timeout.Infinite;
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }
    }
}