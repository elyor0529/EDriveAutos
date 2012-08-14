using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Edrive.Areas.Admin.Controllers;
using Edrive.CommonHelpers;
using Edrive.Logic.Interfaces;
using Edrive.Models;
using Edrive.Edrivie_Service_Ref;

namespace Edrive.Controllers
{
	public class HomeController : Controller
	{
		private readonly IProductService _productService;
		private readonly IDealerService _dealerService;
		private readonly IProductModelService _productModelService;

		public HomeController(IProductService productService, IDealerService dealerService, IProductModelService productModelService)
		{
			_productService = productService;
			_dealerService = dealerService;
			_productModelService = productModelService;
		}

		#region HomePage

		/// <summary>
		/// For the Home page
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{

			#region "CheckFor Users First visit popup."
			HttpCookie cookieIsUsersFirstVisit = Request.Cookies["IsUsersFirstVisit"];
			if(cookieIsUsersFirstVisit == null)
			{
				// create cookie to ensuer that users have now visited site and have shown the download popup.
				cookieIsUsersFirstVisit = new HttpCookie("IsUsersFirstVisit");
				cookieIsUsersFirstVisit["isvisited"] = "yes";
				cookieIsUsersFirstVisit.Expires = DateTime.Now.AddYears(100);
				Response.Cookies.Add(cookieIsUsersFirstVisit);
				ViewData["IsUsersFirstVisit"] = true;
			}
			#endregion

			ViewData["metatitle"] = "Pre-Auction Vehicles for sale | Wholesale Prices on Dealer Used Cars Online | Save Thousands on Cars and Trucks | E-Drive AUTOS";
			ViewData["description"] = SettingManager.GetSettingValue("SEO.Homepage.description");
			ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Homepage.keywords");
			ViewBag.TotalVehicleCount = _productService.GetTotalVehiclesCount();
			ViewBag.TotalDealersCount = _dealerService.GetTotalDealersCount();

			return View();
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
				var vehicles = _productService.GetFeaturedVehicles(20);

				return PartialView(vehicles);
			}
			catch
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
			using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
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
			using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
			{
				return View(entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == "PrivacyInfo"));
			}
		}

		public ActionResult ConditionsOfUse()
		{
			using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
			{
				var content = entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == "ConditionsOfUse");
				
				return View(content);
			}
		}

		public ActionResult GetCarsName(String searchText, Int32 maxResults)
		{
			var results = _productService.HomePageSearchHint(searchText, 20);

			return Json(results, JsonRequestBehavior.AllowGet);
		}

		#region TestService for third party

		public ActionResult TestService()
		{
			return View();


		}

		#endregion

		public ContentResult TotalVehicles()
		{
			int count = _productService.GetTotalVehiclesCount();

			return Content(String.Format("{0:n0}", count));
		}

		public ContentResult TotalDealers()
		{
			int count = _dealerService.GetTotalDealersCount();
			
			return Content(String.Format("{0:n0}", count));
		}

		#region CommanMethods

		public ActionResult GetProudctDefaultImage(Int32 productId)
		{
			return Content(Common_Methods.GetProductDefaultPicture(productId));
		}

		public JsonResult GetModel(Int32 makeId)
		{
			if(makeId == 0)
				return null;

			var model = _productModelService.GetModelsByMake(makeId).OrderBy(m => m.modelName).ToList();
			model.Insert(0, new Core.Model.Product_Model { modelName = "All", id = -1 });

			return Json(model);
		}

		public ContentResult TopicDetail(String id)
		{
			try
			{
				var topicname = id;
				using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
				{
					var topicDetals = entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == topicname);
					return Content(topicDetals.Body);
				}
			}
			catch(Exception ex)
			{
				return Content("Error: " + ex.Message);
			}

		}

		public ContentResult TopicTitle(String id)
		{
			try
			{
				var topicname = id;
				using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
				{
					var topicDetals = entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == topicname);
					return Content(topicDetals.Title);
				}
			}
			catch(Exception ex)
			{
				return Content("Error: " + ex.Message);
			}

		}
		#endregion

		#region DynamicPages for CMS

		public ActionResult Topic(String id)
		{
			var topicname = id;
			using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
			{
				var topicDetals = entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == topicname);
				
				if(ViewExists(topicname) == false)//then default view
					return View(topicDetals);
				else
				{
					return View(topicname, topicDetals);
				}
			}

		}

		private bool ViewExists(string name)
		{
			ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, name, null);
	
			return (result.View != null);
		}

		#endregion
		
		#region "Get Settings"

		public ContentResult GetSetting(String id)
		{
			String settingName = id;
			try
			{
				var val = SettingManager.GetSettingValue(settingName);
				return Content(val);
			}
			catch(Exception ex)
			{
				return Content("Error: " + ex.Message);
			}
		}

		#endregion
		
		public ActionResult SiteMap()
		{
			ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Sitemap.metatitle");
			ViewData["description"] = SettingManager.GetSettingValue("SEO.Sitemap.description");
			ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Sitemap.keywords");

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
			if(User.Identity.IsAuthenticated == false)
			{
				return RedirectToAction("Logon", "Account");

			}
			if(User.IsInRole("Guest"))
			{
				return RedirectToAction("Logon", "Account");
			}

			var dealerID = id;
			using(Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
			{
				var dealerProfileModel = ManageDealerController.GetDealerModel(dealerID);

				if(dealerProfileModel.StateID > 0)
				{
					ViewData["StateNameAbr"] = service.GetState_Detail_By_StateID(dealerProfileModel.StateID).Abbreviation;
				}
				
				ViewData["DealerDetails"] = service.GetProductsByDealerID(dealerID);
				
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
			using(Edrive_ServiceClient _service = new Edrive_ServiceClient())
			{

				var customer = _service.GetDealerByDealerID(DealerID); //CustomerManager.GetCustomerById(this.CustomerId);

				State spm = null;
				var CountryName = "United States";

				if(customer.StateID != -1)
				{
					spm = _service.GetState_Detail_By_StateID(customer.StateID);// StateProvinceManager.GetStateProvinceById(customer.StateProvinceId);
					CountryName = _service.GetCountry().FirstOrDefault(m => m.CountryID == spm.CountryID).Name;
				}
				string address = customer.City + "," + (spm == null ? "" : spm.Name) + "," + CountryName;
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
