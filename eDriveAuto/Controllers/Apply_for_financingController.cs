using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Controllers
{
    
    public class Apply_for_financingController : Controller
    {
        //
        // GET: /Apply_for_financing/
        
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
           
            ViewData["Msg"] = TempData["Msg"];
            using(Edrivie_Service_Ref.Edrive_ServiceClient _service=new Edrive_ServiceClient() )
            {
                var lstSTates=_service.GetStateByCountry(1);
                var SelectListStates= new SelectList (  lstSTates,"Name","Name");
                //ViewData["CarsCount"] = lstSTates.Count;
                //ViewData["PageIndex"] = 0;
                ViewData["States"]=SelectListStates;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(FinancingInfo UserRequestModel)
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                try
                {
                    using (Edrive_ServiceClient _service = new Edrive_ServiceClient())
                    {
                        var lstSTates = _service.GetStateByCountry(1);
                        var SelectListStates = new SelectList(lstSTates, "Name", "Name");
                        ViewData["States"] = SelectListStates;
                        
                    }

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
