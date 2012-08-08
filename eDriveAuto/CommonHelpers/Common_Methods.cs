using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;
using Facebook;

namespace Edrive.CommonHelpers
{
    public class PrevUrlAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var session = filterContext.HttpContext.Session;

            if (httpContext.Request.RequestType == "GET"
                && !httpContext.Request.IsAjaxRequest())
            {
                session["PrevUrl"] = session["CurUrl"] ?? httpContext.Request.Url;
                session["CurUrl"] = httpContext.Request.Url;
            }
        }
    }
    public static class Common_Methods
    { /// <summary>
        /// return the Domain Name Of Hosted  Site with http://
        /// </summary>
        /// <returns></returns>
        public static string GetDomainUrl()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            return url.Substring(0, url.IndexOf("/", url.IndexOf(":") + 3)) + "/";
        }
        public static string GetProductDefaultPicture(int productId)
        {
            using (Edrivie_Service_Ref.Edrive_ServiceClient _service = new Edrive_ServiceClient())
            {
                var defaultImage = _service.GetProductPicture_By_ProductID(productId).OrderBy(m => m.DisplayOrder);
                var defaultImageUrl = "";
                if (defaultImage != null)
                {
                    if (defaultImage.Count() > 0)
                    {
                        defaultImageUrl = defaultImage.First().PictureURL;
                    }
                }
                return defaultImageUrl;
            }
        }

        public static void CreateUser(String Email, String RoleName)
        {
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, Email, DateTime.Now,
   DateTime.Now.AddMinutes(20), false, RoleName, FormsAuthentication.FormsCookiePath);
            String hashCookies = FormsAuthentication.Encrypt(authTicket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
           HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// This method create the userLogin for application when User hv logged in through the facebook
        /// </summary>
        /// <param name="PageName"></param>
        public static void CreateUserFromFaceBook(String PageName)
        {
            try
            {
                //Create instance of REST api using current authanticated session
                dynamic fbCodeGiven =HttpContext.Current.Request.QueryString["code"];
                string getAccessToken = "";

                if ((fbCodeGiven != null))
                {
                    //string urlstr = "https://graph.facebook.com/oauth/access_token?client_id=394447697245581&redirect_uri=http://localhost:2165/Search&client_secret=96492af495fc84efa0fd7781cd6e404b&code=" + fbCodeGiven;
                    string urlstr = "https://graph.facebook.com/oauth/access_token?client_id=" + Common_Methods.GetFacebookApplicationId() + "&redirect_uri=" + Common_Methods.GetDomainUrl() + PageName+"&client_secret=" + Common_Methods.GetFacebookSecretKey() + "&code=" + fbCodeGiven;


                    WebRequest AccessTokenWebRequest = WebRequest.Create(urlstr);
                    StreamReader AccessTokenWebRequestStream = new StreamReader(AccessTokenWebRequest.GetResponse().GetResponseStream());
                    dynamic WebRequestResponse = AccessTokenWebRequestStream.ReadToEnd();
                    getAccessToken = WebRequestResponse.Substring(13, WebRequestResponse.Length - 13);
                    if (getAccessToken.LastIndexOf("&expires") >= 0)
                        getAccessToken = getAccessToken.Substring(0, getAccessToken.LastIndexOf("&expires"));
                    FacebookClient FBApp = new FacebookClient(getAccessToken);

                    dynamic user = FBApp.Get("me");

                    dynamic FBUID = Convert.ToInt64(user.id);

                    string username = "";
                    string email = "";
                    string firstName = "";
                    string lastName = "";

                    firstName = user.first_name;
                    lastName = user.last_name;




                    //User does have an option on Facebook to change their email address to an anonymous address, xxxx@proxymail.facebook.com 12/28/2010
                    email = user.email;
                    username = user.username;

                    Common_Methods.CreateUser(user.email, "Guest");

                    #region add Guest to DB
                    using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                    {
                        if (_entity.Customer.Any(m => m.Email == email) == false)
                        {
                            ///Guest Role if for only free users
                            var custType = _entity.Customer_Type.FirstOrDefault(m => m.RoleName == "Guest");

                            if (custType == null)
                            {
                                custType = new Customer_Type { RoleName = "Guest" };
                                _entity.Customer_Type.AddObject(custType);
                                _entity.SaveChanges();
                            }

                            _entity.AddToCustomer(new Models.Customer { Email = email, FirstName = firstName, LastName = lastName, Password = Guid.NewGuid().ToString(), Name = firstName + " " + lastName, Active = false, CustomerType = custType.id, Deleted = false, ExpirationDate = DateTime.Now, GUID = Guid.NewGuid().ToString(), RegisterationDate = DateTime.Now });
                            _entity.SaveChanges();

                        }
                    }

					SendFacebookRegistrationNotification(firstName, lastName, email, DateTime.Now);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
                 
            }
           
        }

		public static void SendFacebookRegistrationNotification(string firstName, string lastName, string email, DateTime registrationDate)
		{
			const string subject = "Edrive Auto new Facebook user registration";
			const string bodyFormat = "New Facebook user registered to Edrive Auto:\n\nFirst Name: {0}\n\nLast Name: {1}\n\nEmail: {2}\n\nRegistration Date: {3}";
			string body = String.Format(bodyFormat, firstName, lastName, email, registrationDate.ToLongDateString());

			try
			{
				MessageManager.SendEmail(subject, body, new MailAddress(GetEmailFrom()), new MailAddress(GetRegistrationNotificationEmail()));
			}
			catch
			{
				//Log exception
			}
		}

		public static Image ResizeByWidth(Image imgPhoto, int newWidth)
		{
			if(imgPhoto.Width <= newWidth)
				return imgPhoto;
			int targetH, targetW;
			targetW = newWidth;
			targetH = (int)(imgPhoto.Height * ((float)newWidth / (float)imgPhoto.Width));

			Bitmap bmPhoto = new Bitmap(targetW, targetH, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.FillRectangle(Brushes.White, 0, 0, targetW, targetH);
			grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, targetW, targetH), 0, 0, imgPhoto.Width, imgPhoto.Height,
							  GraphicsUnit.Pixel);

			grPhoto.Flush();
			grPhoto.Dispose();
			imgPhoto.Dispose();

			return bmPhoto;
		}

        public static String GetFacebookApplicationId()
        {
            return System.Configuration.ConfigurationManager.AppSettings["APIKey"];
        }
        public static String GetFacebookSecretKey()
        {
            return System.Configuration.ConfigurationManager.AppSettings["Secret"];
        }
		public static String GetRegistrationNotificationEmail()
		{
			return System.Configuration.ConfigurationManager.AppSettings["RegistrationNotificationEmail"];
		}
		public static String GetEmailFrom()
		{
			return System.Configuration.ConfigurationManager.AppSettings["EmailFrom"];
		}
        
        public static String getFaceBookScope()
        {
            return "email";//e.g.e email,userstatus
        }
		
		public static string TrimText(string text, int maxLength)
		{
			if(!String.IsNullOrWhiteSpace(text) && text.Length > maxLength)
			{
				text = text.Substring(0, maxLength - 1);
			}

			return text;
		}

		public static string GetZip(int? zip)
		{
			string result = string.Empty;

			if(zip.HasValue && zip > 0)
			{
				result = zip.ToString().PadLeft(5, '0');
			}

			return result;
		}
    }
}
