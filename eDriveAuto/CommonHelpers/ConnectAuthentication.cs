using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.CommonHelpers
{
    /// <summary>
    /// Summary description for ConnectAuthentication
    /// </summary>
    public class ConnectAuthentication
    {
        public ConnectAuthentication()
        {

        }

        public static bool isConnected()
        {
            return (SessionKey != null && UserID != -1);
        }

        public static string ApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["APIKey"];
            }
        }

        public static string SecretKey
        {
            get
            {
                return ConfigurationManager.AppSettings["Secret"];
            }
        }

        public static string SessionKey
        {
            get
            {
                return GetFacebookCookie("session_key");
            }
        }

        public static int UserID
        {
            get
            {
                int userID = -1;
                int.TryParse(GetFacebookCookie("user"), out userID);
                return userID;
            }
        }

        private static string GetFacebookCookie(string cookieName)
        {
            string retString = null;
            string fullCookie = ApiKey + "_" + cookieName;

            if (HttpContext.Current.Request.Cookies[fullCookie] != null)
                retString = HttpContext.Current.Request.Cookies[fullCookie].Value;

            return retString;
        }
    }
}