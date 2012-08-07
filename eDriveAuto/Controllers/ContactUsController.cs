using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Logic.Interfaces;
using Edrive.Models;


namespace Edrive.Controllers
{
	public class ContactUsController : Controller
	{
		private readonly IStateProvinceService _stateProvinceService;
		private readonly ICountryService _countryService;

		public ContactUsController(IStateProvinceService stateProvinceService, ICountryService countryService)
		{
			_stateProvinceService = stateProvinceService;
			_countryService = countryService;
		}

		public ActionResult Index()
		{
			var topicname = "ContactUs";
			using(eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
			{
				var topicDetals = _entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == topicname);

				ViewData["ContactUsNew"] = _entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == "ContactUsNew").Body;

				var lstCountry = _countryService.GetAll().OrderBy(m => m.DisplayOrder);
				ViewData["Countries"] = new SelectList(lstCountry, "CountryID", "Name");
				var states = _stateProvinceService.GetStateByCountry(lstCountry.First().CountryID);
				if(states != null)
				{
					ViewData["States"] = new SelectList(states, "StateProvinceID", "Name");
				}
					
				return View(topicDetals);
			}
		}


		[HttpPost]
		public JsonResult GetState(Int32 CountryID)
		{
			var states = _stateProvinceService.GetStateByCountry(CountryID).Select(m => m);
			
			return Json(states);
		}

		[HttpPost]
		public ActionResult ContactUsSubmit(_ContactUsForm Model)
		{
			try
			{

				#region Send Email to Admin
				string[] strEnquiry = new string[11];
				strEnquiry[0] = Model.Email;
				strEnquiry[1] = Model.Name;
				Model.TelephoneExt = Model.TelephoneExt ?? "";
				Model.Telephone = Model.Telephone ?? "";
				Model.Fax = Model.Fax ?? "";
				Model.Address = Model.Address ?? "";
				Model.City = Model.City ?? "";
				Model.Zip = Model.Zip ?? "";



				if(Model.Address.Length > 0)
				{
					strEnquiry[2] = Model.Address ?? "";
				}
				else
				{
					strEnquiry[2] = "---";
				}
				if(Model.City.Length > 0)
				{
					strEnquiry[3] = Model.City ?? "";
				}
				else
				{
					strEnquiry[3] = "---";
				}

				try
				{
					int countryID = Convert.ToInt32(Model.Country);
					int stateID = Convert.ToInt32(Model.State);
					strEnquiry[4] = _countryService.GetAll().FirstOrDefault(c => c.CountryID == countryID).Name;
					strEnquiry[5] = _stateProvinceService.GetStateByCountry(countryID).FirstOrDefault(c => c.StateProvinceID == stateID).Name;
				}
				catch
				{
					strEnquiry[4] = Model.Country;
					strEnquiry[5] = Model.State;
				}

				if(Model.Zip.Length > 0)
				{
					strEnquiry[6] = Model.Zip ?? "";
				}
				else
				{
					strEnquiry[6] = "---";
				}


				if(Model.Telephone.Length > 0)
				{
					if(Model.TelephoneExt.Length > 0)
					{
						strEnquiry[7] = Model.TelephoneExt + "-" + Model.Telephone;
					}
					else if(Model.Telephone.Length > 0)
					{
						strEnquiry[7] = Model.Telephone;
					}
				}
				else
				{
					strEnquiry[7] = "---";
				}
				if(Model.Fax.Length > 0)
				{
					strEnquiry[8] = Model.Fax ?? "";
				}
				else
				{
					strEnquiry[8] = "---";
				}

				if(Model.HearAboutUsBy == "0")
				{
					strEnquiry[9] = "---";
				}
				else
				{
					strEnquiry[9] = Model.HearAboutUsBy;
				}

				strEnquiry[10] = string.Empty;

				if(String.IsNullOrEmpty(Model.CustomerType) == false && String.IsNullOrWhiteSpace(Model.CustomerType) == false)
				{
					if(Model.CustomerType.EndsWith(","))
						Model.CustomerType = Model.CustomerType.Substring(0, Model.CustomerType.LastIndexOf(','));
					strEnquiry[10] = strEnquiry[10] + " , " + Model.CustomerType;

				}
				else
				{
					strEnquiry[10] = " I am a Buyer";
				}


				MessageManager.SendAdminToContact(strEnquiry, 0);
				#endregion

			}
			catch(Exception ex)
			{

				return Json(new { Msg = ex.Message });// },  JsonRequestBehavior.AllowGet);
			}
			return Json(new { Msg = "Thank you for writing to us." });// }, JsonRequestBehavior.AllowGet);


		}

	}
}
