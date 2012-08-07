using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Logic.Interfaces;
using Edrive.Models;

namespace Edrive.Controllers
{
    public class ApplyForFinancingController : Controller
    {
    	private readonly IStateProvinceService _stateProvinceService;

		public ApplyForFinancingController(IStateProvinceService stateProvinceService)
		{
			_stateProvinceService = stateProvinceService;
		}
		
        public ActionResult Index()
        {
            try
            {
                ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Apply-for-financing.metatitle");
                ViewData["description"] = SettingManager.GetSettingValue("SEO.Apply-for-financing.description");
                ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Apply-for-financing.keywords");

            }
            catch (Exception)
            {

            }

            return View();
        }


        public ActionResult Register()
        {
            var lstSTates = _stateProvinceService.GetStatesByCountryCode("US");
        	var selectListStates = new SelectList(lstSTates, "Name", "Name");

			ViewData["Msg"] = TempData["Msg"];
			ViewData["States"]=selectListStates;
            
            return View();
        }

        [HttpPost]
        public ActionResult Register(FinancingInfo UserRequestModel)
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                try
                {
                    var lstSTates = _stateProvinceService.GetStatesByCountryCode("US");
                    var selectListStates = new SelectList(lstSTates, "Name", "Name");
                    ViewData["States"] = selectListStates;

                    var finRequest = new FinanceRequest();
                    #region AddFinanceRequestObject
                    finRequest.City = UserRequestModel.City;
                    finRequest.CreatedOn = UserRequestModel.CreatedOn= DateTime.Now;
                    finRequest.CreditScore = UserRequestModel.CreditScore;
                    finRequest.DownPayment = Convert.ToString(UserRequestModel.DownPayment);
                    finRequest.Email = UserRequestModel.Email;
                    finRequest.EmployerName = UserRequestModel.EmpolyerName;
                    finRequest.FirstName = UserRequestModel.FirstName;
                    finRequest.HomePhone = UserRequestModel.HomePhone;
                    finRequest.HowLongAtAddress = UserRequestModel.HowLongAtAddressYear + " Years " + UserRequestModel.HowLongAtAddressMonth + " Months";
                    finRequest.HowLongWorking = UserRequestModel.WorkedForYear + " Years " + UserRequestModel.WorkedForMonth + " Months";
                    finRequest.IsBankrupt = UserRequestModel.IsBankrupt;
                    finRequest.IsCosignerAvailable = UserRequestModel.IsCoSigner;
                    finRequest.JobTitle = UserRequestModel.JobTitle;
                    finRequest.LastName = UserRequestModel.LastName;
                    finRequest.MobilePhone = UserRequestModel.MobilePhone;
                    finRequest.MonthlyIncomeBeforeTaxes = UserRequestModel.MonthlyIncome.ToString();
                    finRequest.MonthRent_Mortage = UserRequestModel.MonthlyRent;
                    finRequest.ResidenceType = UserRequestModel.ResidenceType;
                    finRequest.State = UserRequestModel.State;
                    finRequest.StreetAddress = UserRequestModel.StreedAddress;
                    finRequest.WorkPhone = UserRequestModel.WorkPhone;
                    finRequest.ZipCode = UserRequestModel.ZipCode;

                    _entity.FinanceRequest.AddObject(finRequest);
                    _entity.SaveChanges();
                    #endregion
                    #region SendMail
                    try
                    {

                    
                    MessageManager.SendFinanceRequest(UserRequestModel);
                    }
                    catch (Exception ex)
                    {
                        TempData["Msg"] = "Your Request Submitted successfully, but Email not sent, Error="+ex.Message;
                        return RedirectToAction("Register");
                         
                    }
                    #endregion

                    TempData["Msg"] = "Your Request Submitted successfully";
                    return RedirectToAction("Register");

                }
                catch (Exception ex)
                {
                    
                     ViewData["Msg"]="Error:"+ex.Message;
                }

            }
            return View();
        }


    }
}
