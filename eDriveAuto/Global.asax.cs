using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // look if any security information exists for this request

            if (HttpContext.Current.User != null)
            {

                // see if this user is authenticated, any authenticated cookie (ticket) exists for this user

                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {

                    // see if the authentication is done using FormsAuthentication

                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {

                        // Get the roles stored for this request from the ticket

                        // get the identity of the user

                        FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;

                        //Get the form authentication ticket of the user

                        FormsAuthenticationTicket ticket = identity.Ticket;

                        //Get the roles stored as UserData into ticket

                        string[] roles = ticket.UserData.Split(',');

                        //Create general prrincipal and assign it to current request

                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(identity, roles);

                        if (Request != null)
                        {
                            if (!String.IsNullOrEmpty(Request.Url.LocalPath))
                            {
                                String pth = Request.Url.LocalPath;

                                String RoleName = "Admin";
                                if(User.IsInRole("Dealer"))
                                {
                                    RoleName="Dealer";
                                }
                                if (pth.Contains("ViewInventory.aspx"))
                                {
                                    Response.Redirect("~/"+RoleName+"/ManageVehicle/Manage");
                                }
                                if (RoleName == "Admin")
                                {
                                    if (pth.Contains("ManageLeads.aspx"))
                                        Response.Redirect("~/" + RoleName + "/ManageLeads");
                                }
                                else
                                {
                                    if (RoleName == "Dealer")
                                    {
                                        if (pth.Contains("ManageLeads.aspx"))
                                            Response.Redirect("~/Dealer");
                                    }
 
                                }
                                if (pth.Contains("Hotsheet.aspx"))
                                    Response.Redirect("~/" + RoleName + "/ManageVehicle/Hotsheet");
                                if (pth.Contains("Contact-us.aspx"))
                                    Response.Redirect("~/" + RoleName + "/Contact_us");
                            }
                        }
                    }
                }
            }
        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            //return RedirectToAction("MemberShip/Customer/" + cust.CustomerID, new { Trial = "true" });
            routes.MapRoute(
             "SavedWishList", // Route name
             "Product/SavedWishList/{CustomerType}/{Customerid}", // URL with parameters
             new { controller = "Product", action = "SavedWishList" } // Parameter defaults
         );

             routes.MapRoute(
             "ChangePassword", // Route name
             "Account/ChangePassword/{CustomerType}/{Email}/{guid}", // URL with parameters
             new { controller = "Account", action = "ChangePassword" } // Parameter defaults
         );
            
            routes.MapRoute(
             "MemberShip", // Route name
             "MemberShip/Customer/{id}/{trial}", // URL with parameters
             new { controller = "Account", action = "Customer" } // Parameter defaults
         );
              routes.MapRoute(
             "MembershipRenewed", // Route name
             "MembershipRenewed", // URL with parameters
             new { controller = "Account", action = "MembershipRenewed" } // Parameter defaults
         );
            
            routes.MapRoute(
              "newsDetails", // Route name
              "News/{id}", // URL with parameters
              new { controller = "Press_Release", action = "NewsDetails", id = UrlParameter.Optional } // Parameter defaults
          );
            routes.MapRoute(
              "SiteMap", // Route name
              "SiteMap", // URL with parameters
              new { controller = "Home", action = "SiteMap"} // Parameter defaults
          );
            routes.MapRoute(
            "dealerPricing", // Route name
            "dealer_Pricing", // URL with parameters
            new { controller = "Dealers", action = "Dealer_Pricing" } // Parameter defaults
        );
           // routes.MapRoute(
           //    "ManageLeads.aspx", // Route name
           //    "ManageLeads.aspx", // URL with parameters
           //    new { area = "Admin", controller = "ManageVehicle", action = "ManageLeads", id = UrlParameter.Optional } // Parameter defaults
           //);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
				, new[] { "Edrive.Controllers" }
            );

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

               

        }

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            //GlobalFilters.Filters.Add(new PrevUrlAttribute());
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(typeof(CustomControllerFactory));

        }
        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {

                var error = Server.GetLastError();
                var code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;
                var fileName = Server.MapPath(String.Format("~/Content/ErrorLog/Error_{0}.txt",
                DateTime.Now.ToString("dd_MM_yyy_hh_mm_ss")));
                StreamWriter fs = new StreamWriter(fileName);
                fs.WriteLine("--------------------Sender--------------------");
                fs.WriteLine(sender.ToString());
                fs.WriteLine("--------------------Message--------------------");
                fs.WriteLine(error.Message);
                fs.WriteLine("--------------------InnerException--------------------");
                fs.WriteLine(error.InnerException);
                fs.WriteLine("--------------------error code--------------------");
                fs.WriteLine(code);
                fs.WriteLine("--------------------Stack Trace--------------------");
                fs.WriteLine(error.StackTrace);
                fs.Close();
            }
            catch (Exception ex)
            {


            }
        }

    }
}