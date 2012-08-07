using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Logic.Interfaces;
using Edrive.Models;


namespace Edrive.Controllers
{
    public class DealersController : Controller
    {
		private readonly IStateProvinceService _stateProvinceService;

		public DealersController(IStateProvinceService stateProvinceService)
		{
			_stateProvinceService = stateProvinceService;
		}

        public ActionResult Index()
        {
            ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Dealer.metatitle");
            ViewData["description"] = SettingManager.GetSettingValue("SEO.Dealer.description");
            ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Dealer.keywords");
			ViewData["Msg"] = TempData["Msg"];
            
			using (eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
            {
                var becomeADealer = entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == "BecomeADealer");
                ViewData["BecomeADealer"] = becomeADealer.Body;
                var dealerTextThree = entity.Nop_TopicLocalized.FirstOrDefault(m => m.Nop_Topic.Name == "DealerTextThree");
                ViewData["DealerTextThree"] = dealerTextThree.Body;
                
                var lst= _stateProvinceService.GetStatesByCountryCode("US").OrderBy(m=>m.DisplayOrder);// 1 for united States;

                ViewData["States"] = new SelectList(lst, "Name", "Name");
                
                return View(becomeADealer);
            }
        }

        [HttpPost]
        public ActionResult Register(_DealerRegisteration User)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    #region Send Email to Admin
                    string[] strEnquiry = new string[31];
                    strEnquiry[0] = User.Email;
                    strEnquiry[1] = User.Firstname;
                    strEnquiry[2] = User.Lastname;
                    strEnquiry[3] = User.Title;
                    strEnquiry[4] = User.Dealership;
                    strEnquiry[5] = User.StateID;
                    if (String.IsNullOrEmpty(User.TelephoneExtenstion)==false)
                    {
                        strEnquiry[6] = User.TelephoneExtenstion + "-" + User.Telephone;
                    }
                    else
                    {
                        strEnquiry[6] = User.Telephone;
                    }

                    strEnquiry[7] = User.Zip.ToString();
                    strEnquiry[8] = User.DMSProvider;

                     

                    MessageManager.SendDealersToAdmin(strEnquiry, 0);
                    MessageManager.SendAdminToDealers(strEnquiry, 0);
                    #endregion

                    TempData["Msg"] = "Thank you for writing to us.";
                //    lMessage.Text = GetLocaleResourceString("Dealers.ThanksForContact");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "error:" + ex.Message;
                //pnlMessage.Visible = true;
                //lMessage.Text = ex.Message.ToString();
            }
            return RedirectToAction("Index");
        }



        public ActionResult Dealer_Pricing()
        {
            try
            {
                ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Dealer-Pricing.metatitle");
                ViewData["description"] = SettingManager.GetSettingValue("SEO.Dealer-Pricing.description");
                ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Dealer-Pricing.keywords");


            }
            catch (Exception)
            {

            }

 
            return View();
 
        }


    }
}
