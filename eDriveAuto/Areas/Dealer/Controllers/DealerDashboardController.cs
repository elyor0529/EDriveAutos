using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Areas.Dealer.Controllers
{
  
   
    public class DealerDashboardController : Controller
    {
        //
        // GET: /Dealer/DealerDashboard/

         [Authorize(Roles = "Dealer")]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
            
        public ActionResult Login()
         {
             try
             {
                 ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Dealer.Login.metatitle");
                 ViewData["description"] = SettingManager.GetSettingValue("SEO.Dealer.Login.description");
                 ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Dealer.Login.keywords");

             }
             catch (Exception)
             {

             }

 

            return View();
        }


        [HttpPost]
        public ActionResult Login(UserLogin UserModel, string ReturnUrl)
        {
            UserModel.Email = UserModel.Email.Trim();

            using (var _entity = new eDriveAutoWebEntities())
            {
                Models.Customer cust = _entity.Customer.FirstOrDefault(m => m.Email == UserModel.Email && m.Deleted == false && m.Password ==
                    UserModel.Password && m.Customer_Type.RoleName == "Customer");
                if (cust != null)
                {
                    if (cust.ExpirationDate.Value < DateTime.Now)
                    {
                        if (cust.IsTrial == true)
                        {
                            return RedirectToAction("Customer/" + cust.CustomerID.ToString() + "/true", "MemberShip/");
                        }
                        else
                        {
                            return RedirectToAction("Customer/" + cust.CustomerID.ToString() + "/false", "MemberShip/");


                        }
                    }

                    var RememberMe = false;
                    if (Request.Form["RememberMe"] != null)
                        RememberMe = true;
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, UserModel.Email, DateTime.Now,
                   DateTime.Now.AddMinutes(20), RememberMe, "Customer", FormsAuthentication.FormsCookiePath);
                    String hashCookies = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
                    Response.Cookies.Add(cookie);



                    return RedirectToAction("Index", "Home", new { area=""});
                     
                }
                else
                    using (Edrive_ServiceClient _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
                    {
                        Edrivie_Service_Ref.Customer Dealer = new Edrivie_Service_Ref.Customer();

                        if (_service.Authenticate_Dealer_or_Admin(UserModel.Email, UserModel.Password, "Admin", ref Dealer))
                        {
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, UserModel.Email, DateTime.Now,
                            DateTime.Now.AddMinutes(20), false, "Admin", FormsAuthentication.FormsCookiePath);
                            String hashCookies = FormsAuthentication.Encrypt(authTicket);
                            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
                            Response.Cookies.Add(cookie);
                            if (ReturnUrl != null)
                            {
                                if (String.IsNullOrEmpty(ReturnUrl) == false)
                                {
                                    Response.Redirect(ReturnUrl);
                                }
                            }
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        }
                        else
                        {
                            //---if it is dealer


                            if (_service.Authenticate_Dealer_or_Admin(UserModel.Email, UserModel.Password, "Dealer", ref Dealer))
                            {
                                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, UserModel.Email, DateTime.Now,
                       DateTime.Now.AddMinutes(20), false, "Dealer", FormsAuthentication.FormsCookiePath);
                                String hashCookies = FormsAuthentication.Encrypt(authTicket);
                                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
                                Response.Cookies.Add(cookie);
                                return RedirectToAction("Index", "DealerDashboard", new { area = "Dealer" });
                            }
                            else
                            {
                                ViewData["Msg"] = "Wrong Username or Password";
                                return View();
                            }


                        }
                    }



            }

            // If we got this far, something failed, redisplay form
           
        }

            

            // If we got this far, something failed, redisplay form
           
       

      

      
    }
}
