using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Edrive.Areas.Admin.Controllers;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class HomeController : Controller
    {

        #region HomePage

        /// <summary>
        /// For the Home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            #region "CheckFor Users First visit popup."
            HttpCookie cookie_IsUsersFirstVisit = Request.Cookies["IsUsersFirstVisit"];
            if (cookie_IsUsersFirstVisit == null)
            {
                // create cookie to ensuer that users have now visited site and have shown the download popup.
                cookie_IsUsersFirstVisit = new HttpCookie("IsUsersFirstVisit");
                cookie_IsUsersFirstVisit["isvisited"] = "yes";
                cookie_IsUsersFirstVisit.Expires = DateTime.Now.AddYears(100);
                Response.Cookies.Add(cookie_IsUsersFirstVisit);
                ViewData["IsUsersFirstVisit"] = true;
            }
            #endregion

            using (Edrive_ServiceClient _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                try
                {
                    ViewData["metatitle"] = "Pre-Auction Vehicles for sale | Wholesale Prices on Dealer Used Cars Online | Save Thousands on Cars and Trucks | E-Drive AUTOS";
                    ViewData["description"] = SettingManager.GetSettingValue("SEO.Homepage.description");
                    ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Homepage.keywords");
					ViewBag.TotalVehicleCount = _service.Get_TotalVehicles_Count();
					ViewBag.TotalDealersCount = _service.Get_TotalDealers_Count();
                }
                catch (Exception)
                {
                	ViewBag.TotalVehicleCount = 0;
					ViewBag.TotalDealersCount = 0;
                }

                //List<Product_Make> lstmake = _service.BindMake();
                //lstmake.Insert(0, new Product_Make { id = -1, make = "All" });
                //ViewData["Make"] = lstmake;
                return View();
            }
        }
        /// <summary>
        /// to show the featured vehicle on home page
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult FeaturedVehicle()
        {
            try
            {
                using (Edrive.Edrivie_Service_Ref.Edrive_ServiceClient _service = new Edrive_ServiceClient())
                {
                    return PartialView(_service.Get_FeaturedVehicles(20));
                }
            }
            catch (Exception ex)
            {

                return PartialView(null);
            }

        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AboutUs()
        {
            using (eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
            {
                return View(entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == "AboutUs"));
            }
        }

		public ActionResult Advertiser()
		{
			using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
			{
				return View(entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == "Advertiser"));
			}
		}

		public ActionResult PrivacyInfo()
        {
            using (eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
            {
                return View(entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == "PrivacyInfo"));
            }
        }
		
		public ActionResult ConditionsOfUse()
        {
            using (eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
            {
                return View(entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == "ConditionsOfUse"));
            }
        }


        /// <summary>
        /// Home Page autocompte help textbox
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public ActionResult GetCarsName(String searchText, Int32 maxResults)
        {
            using (Edrivie_Service_Ref.Edrive_ServiceClient Service = new Edrive_ServiceClient())
            {
                var lst = Service.SearchCars_for_home_Page(searchText, 20);

                return Json(lst, JsonRequestBehavior.AllowGet);
            }

        }

        #region TestService for third party

        public ActionResult TestService()
        {
            return View();


        }

        [HttpPost]
        public object TestService(String GetData)
        {
            try
            {

            using (Edrive_ServiceClient service = new Edrive_ServiceClient())
            {
                switch (GetData)
                {
                    case "GetDataFromAULtec":
                        return service.GetDataFromAULtec();
                    case "GetDataFromAutoBase":
                        return service.GetDataFromAutoBase();
                    case "GetDataFromGetAuto":
                        return service.GetDataFromGetAuto();
                    case "GetDataFromSchumacher":
                        return service.GetDataFromSchumacher();
                    case "DeleteUsingPriceValidation":
                         service.Qualify_All_Products();
                         return View();
                    case "RecoverDeletedProducts": 
                        service.Qualify_All_Products_to_RecoverDeletedProducts();
                        return View();

                }
            }
            return "ActionType not find";

            }
            catch (Exception ex)
            {
                return ex.Message + " _|_ " + ex.StackTrace;
            }
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // <summary>
        /// to return the Vehicles Count
        /// </summary>
        /// <returns></returns>



        public ContentResult TotalVehicles()
        {
            using (Edrive_ServiceClient _service = new Edrive_ServiceClient())
            {
                Int32 count ;
                try
                {

                
                  count = _service.Get_TotalVehicles_Count();
                }
                catch (Exception)
                {

                    count = 0;
                }
                return Content(String.Format("{0:n0}", count));
            }
        }
        /// <summary>
        /// to return the Dealers Count
        /// </summary>
        /// <returns></returns>
        public ContentResult TotalDealers()
        {
            using (Edrive_ServiceClient _service = new Edrive_ServiceClient())
            {
                Int32 count;
                try
                {
                      count = _service.Get_TotalDealers_Count();
                }
                catch (Exception)
                {

                    count = 0;
                }
              
                return Content(String.Format("{0:n0}", count));
            }
        }

        #region CommanMethods

        /// <summary>
        /// To get the product defalutImage
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult GetProudctDefaultImage(Int32 productId)
        {
            return Content(Common_Methods.GetProductDefaultPicture(productId));
        }


        /// <summary>
        /// To return the list of Model Name by its Make ID
        /// </summary>
        /// <param name="MakeId"></param>
        /// <returns></returns>
        public JsonResult GetModel(Int32 MakeId)
        {
            using (Edrive_ServiceClient _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {

                if (MakeId == 0)
                {
                    return null;
                }
                List<Product_Model> Model = _service.BindModel(MakeId).OrderBy(m => m.modelName).ToList();
                Model.Insert(0, new Product_Model { modelName = "All", id = -1 });
                
				return Json(Model);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// To get the body details of topic
        /// </summary>
        /// <param name="id">id is the name of topic</param>
        /// <returns></returns>
        public ContentResult TopicDetail(String id)
        {
            try
            {


                var topicname = id;
                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var topicDetals = _entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == topicname);
                    return Content(topicDetals.Body);
                }
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }

        }
        /// <summary>
        /// To get the title of topic
        /// </summary>
        /// <param name="id">id is the topic name</param>
        /// <returns></returns>
        public ContentResult TopicTitle(String id)
        {
            try
            {
                var topicname = id;
                using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
                {
                    var topicDetals = _entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == topicname);
                    return Content(topicDetals.Title);
                }
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }

        }
        #endregion

        #region DynamicPages for CMS
        /// <summary>
        /// To show the content of dynamic page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Topic(String id)
        {
            var topicname = id;
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                var topicDetals = _entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == topicname);
                if (ViewExists(topicname) == false)//then default view
                    return View(topicDetals);
                else
                {
                    return View(topicname, topicDetals);
                }
            }

        }

        /// <summary>
        /// To check the view exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        private bool ViewExists(string name)
        {
            ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, name, null);
            return (result.View != null);

        }


        #endregion



        //public ActionResult AffiliateProgram()
        //{
        //    return View("Management");
        //}
        //public ActionResult FAQ()
        //{
        //    return View("Management");
        //}


        //public ActionResult Press()
        //{
        //    return View();

        //}




        #region "Get Settings"

        /// <summary>
        /// This method  returns the Common Setting for the website e.g. storeurl,Admin Emal address
        /// </summary>
        /// <param name="SettingName"></param>
        /// <returns></returns>
        public ContentResult GetSetting(String id)
        {
            String SettingName = id;
            try
            {

                var val = SettingManager.GetSettingValue(SettingName);
                return Content(val);
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }

        #endregion


        public ActionResult SiteMap()
        {
            try
            {
                ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Sitemap.metatitle");
                ViewData["description"] = SettingManager.GetSettingValue("SEO.Sitemap.description");
                ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Sitemap.keywords");


            }
            catch (Exception)
            {

            }

 
            return View();
        }



        #region DealerProfile
       
        /// <summary>
        /// To show the Dealer profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DealerProfile(Int32 id)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Logon", "Account");

            }
            if (User.IsInRole("Guest"))
            {
                return RedirectToAction("Logon", "Account");
            }
            var DealerID = id;
            using (Edrive_ServiceClient _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {

                var dealerProfileModel = ManageDealerController.GetDealerModel(DealerID, _service);

                if (dealerProfileModel.StateID > 0)
                {
                    ViewData["StateNameAbr"] = _service.GetState_Detail_By_StateID(dealerProfileModel.StateID).Abbreviation;
                }


                ViewData["DealerDetails"] = _service.GetProductsByDealerID(DealerID);
                return View(dealerProfileModel);
            }
        }

       /// <summary>
       /// To view the Dealer Address on Google Map
       /// </summary>
       /// <param name="DealerID"></param>
       /// <returns></returns>

        public ActionResult GoogleMap(Int32 DealerID)
        {
            using (Edrive_ServiceClient _service = new Edrive_ServiceClient())
            {

                var customer = _service.GetDealerByDealerID(DealerID); //CustomerManager.GetCustomerById(this.CustomerId);

                State spm=null;
                var CountryName = "United States";

                if (customer.StateID != -1)
                {
                    spm = _service.GetState_Detail_By_StateID(customer.StateID);// StateProvinceManager.GetStateProvinceById(customer.StateProvinceId);
                    CountryName = _service.GetCountry().FirstOrDefault(m => m.CountryID == spm.CountryID).Name;
                }
                string address = customer.City + "," + (spm==null?"":spm.Name) + "," + CountryName;
                DisplayAddress(address);
                return View();
            }
 
        }
        protected void DisplayAddress(string address)
        {
            //this.BindJQuery();
            StringBuilder alertJsStart = new StringBuilder();
            alertJsStart.AppendLine("<script type=\"text/javascript\">");
            alertJsStart.AppendLine("$(document).ready(function() {");
            alertJsStart.AppendLine(string.Format("initialize();"));
            alertJsStart.AppendLine(string.Format("showAddress('{0}');", address.Trim()));

            alertJsStart.AppendLine("});");
            alertJsStart.AppendLine("</script>");
            string js = alertJsStart.ToString();
            // Page.ClientScript.RegisterClientScriptBlock(GetType(), "alertScriptKey", js);
           ViewData["alertScriptKey"] = js;

        }
         #endregion
    }

}
