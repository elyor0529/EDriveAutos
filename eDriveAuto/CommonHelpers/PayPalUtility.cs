using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Edrive.PayPalAPI;

namespace Edrive.CommonHelpers
{
    public class PayPalUtility
    {
        public static string Version
        {
            get { return ConfigurationManager.AppSettings["PPVersion"]; }
        }

        public static string URL
        {
            get { return ConfigurationManager.AppSettings["PPSubmitUrl"]; }
        }

		public static CustomSecurityHeaderType BuildPayPalWebservice()
        {
            // more details on https://www.paypal.com/en_US/ebook/PP_APIReference/architecture.html
			var paypal = new CustomSecurityHeaderType()
            {
                Credentials = BuildCredentials()
            };

            return paypal;
        }

        public static UserIdPasswordType BuildCredentials()
        {
            UserIdPasswordType credentials = new UserIdPasswordType()
            {
                Username = ConfigurationManager.AppSettings["PPUsername"],
                Password = ConfigurationManager.AppSettings["PPPassword"],
                Signature = ConfigurationManager.AppSettings["PPSignature"],
            };

            return credentials;
        }

        internal static void HandleError(AbstractResponseType resp)
        {
            if (resp.Errors != null && resp.Errors.Length > 0)
            {
                // errors occured
                throw new Exception("Exception(s) occured when calling PayPal. First exception: " +
                    resp.Errors[0].LongMessage);
            }
        }
    }
}