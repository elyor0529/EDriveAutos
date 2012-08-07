using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Edrive.Edrivie_Service_Ref;
using Edrive.Logic.Interfaces;
using Edrive.Models;

namespace Edrive.Areas.Admin.Controllers
{
    public class DashBoardController : Controller
    {
    	private readonly IStateProvinceService _stateProvinceService;
    	private readonly ICountryService _countryService;

		public DashBoardController(IStateProvinceService stateProvinceService, ICountryService countryService)
		{
			_stateProvinceService = stateProvinceService;
			_countryService = countryService;
		}
        
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return View("Login");
            }
            if (User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return View("Login");
            }
        }
        [HttpPost]
        public ActionResult Index(string btnName)
        {
            if (btnName == null)
                return View();
            switch (btnName)
            {
                case "Add New Vehicle": return RedirectToAction("Add", "ManageVehicle", new { area = "Admin" });
                case "Upload Vehicle": return RedirectToAction("UploadVehicle", "ManageVehicle", new { area = "Admin" });
                case "Manage/Edit/Delete Vehicle": return RedirectToAction("Manage", "ManageVehicle", new { area = "Admin" });
                default: return View();
            }
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");

            }
            return View();
        }
        [HttpPost]

        public ActionResult Login(UserLogin UserModel)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");

            }
            using (var _service = new Edrive_ServiceClient())
            {
                Edrivie_Service_Ref.Customer admin = null;
                if (_service.Authenticate_Dealer_or_Admin(UserModel.Email, UserModel.Password, "Admin", ref admin))
                {
                    Boolean RememberMe = false;
                    if (Request.Form["RememberMe"] != null)
                        RememberMe = true;
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, UserModel.Email, DateTime.Now,

                    DateTime.Now.AddMinutes(20), RememberMe, "Admin", FormsAuthentication.FormsCookiePath);
                    String hashCookies = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
                    Response.Cookies.Add(cookie);
                    //Response.Redirect("~/Administration");
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Msg"] = "Wrong Username or Password";
                    return View();
                }
            }
        }
        /// <summary>
        /// for manage email controler
        /// </summary>
        /// <param name="TemplateName"></param>
        /// <param name="MessageTemplateID"></param>
        /// <returns></returns>
        public JsonResult isTemplateExists(String TemplateName, Int32 MessageTemplateID)
        {
            using (eDriveAutoWebEntities _entity = new eDriveAutoWebEntities())
            {
                if (MessageTemplateID == 0)
                {
                    if (_entity.MessageTemplate.Any(m => (m.Deleted == false || m.Deleted == null) && m.Name == TemplateName))
                        return Json(false, JsonRequestBehavior.AllowGet);
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (_entity.MessageTemplate.Any(m => (m.Deleted == false || m.Deleted == null) && m.MessageTemplateID != MessageTemplateID && m.Name == TemplateName))
                        return Json(false, JsonRequestBehavior.AllowGet);
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }

                }

            }
        }

        public ActionResult MyDetails()
        {


            ViewData["Msg"] = TempData["Msg"];

            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {

                MyInfo DealerModel = GetDealerModel(service);
                BindFilter(DealerModel.StateID);
                return View(DealerModel);
            }
            // return View();
        }
        public MyInfo GetDealerModel(Edrive_ServiceClient service)
        {
            var dealer = new Edrivie_Service_Ref.Customer();
            //if (CustomerID == null)
            //{
            //    ViewData["Msg"] = TempData["Msg"];
            //    // TempData["EmailID"] = DealerModel.Email;
            //      dealer = service.GetDealerByDealerEmail(TempData["EmailID"].ToString());
            //}
            //else
            //{
            dealer = service.GetDealerByDealerEmail(User.Identity.Name);
            //}
            MyInfo DealerModel = new MyInfo();
            DealerModel.Email = dealer.email;

            DealerModel.Gender = dealer.Gender;
            DealerModel.FirstName = dealer.FirstName;
            DealerModel.LastName = dealer.LastName;
            DealerModel.DateofBirth = dealer.DateofBirth;
            DealerModel.Company = dealer.CompanyName;
            DealerModel.StreetAddress1 = dealer.StreetAddress1;
            DealerModel.StreetAddress2 = dealer.StreetAddress2;
            DealerModel.Zip = dealer.Zip;
            DealerModel.City = dealer.City;
            DealerModel.StateID = dealer.StateID;
            DealerModel.Phone = dealer.Phone;
            DealerModel.Newsletter = dealer.Newsletter;
            DealerModel.Password = dealer.password;
            DealerModel.Fax = dealer.Fax;




            DealerModel.CustomerID = dealer.customerID;
            DealerModel.RegisterationDate = dealer.registrationDate;


            return DealerModel;
        }

        private void BindFilter(int statedID = -1)
        {
            var lstCountries = _countryService.GetAll();
            var country = lstCountries.Select(m => new SelectListItem { Selected = (m.DisplayOrder == 1), Text = m.Name, Value = m.CountryID.ToString() }).ToList();
            var selectedCountryID = lstCountries.First(m => m.DisplayOrder == 1).CountryID;
            var stateID = _stateProvinceService.GetStateByCountry(selectedCountryID).Select(m => new SelectListItem { Selected = (m.StateProvinceID == statedID), Text = m.Name, Value = m.StateProvinceID.ToString() }).ToList();
            
			ViewData["Country"] = country;
            ViewData["StateID"] = stateID;
        }
        [HttpPost]
        public ActionResult ChangePassword(MyInfo MyInfo, String txtPassword)
        {
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                if (String.IsNullOrEmpty(txtPassword))
                {
                    TempData["Msg"] = "Password  is required.";

                }
                else
                {

                    string Msg = string.Empty;
                    bool result = false;
                    result = service.UpdatePassword_for_Dealer(out  Msg, MyInfo.CustomerID, txtPassword);
                    if (result == true)
                    {
                        TempData["Msg"] = "Password Successfully Changed";
                    }
                    else
                    {
                        TempData["Msg"] = "Password  Changed faliure";
                    }
                }
                return RedirectToAction("MyDetails", new { id = MyInfo.CustomerID });
            }
        }

        [HttpPost]
        public ActionResult MyDetails(MyInfo AdminInfo, String changepasssword)
        {
            using (Edrive_ServiceClient _service = new Edrive_ServiceClient())
            {
                try
                {
                  

                    string Msg = "";
                    Edrivie_Service_Ref.Customer objadmin = new Edrivie_Service_Ref.Customer();
                    objadmin.active = true;
                    objadmin.email = AdminInfo.Email;
                    objadmin.customerID = AdminInfo.CustomerID;
                    objadmin.password = AdminInfo.Password;
                    objadmin.Gender = AdminInfo.Gender;
                    objadmin.FirstName = AdminInfo.FirstName;
                    objadmin.LastName = AdminInfo.LastName;
                    objadmin.DateofBirth = AdminInfo.DateofBirth;
                    objadmin.CompanyName = AdminInfo.Company;
                    objadmin.StreetAddress1 = AdminInfo.StreetAddress1;
                    objadmin.StreetAddress2 = AdminInfo.StreetAddress2;
                    objadmin.Zip = AdminInfo.Zip;
                    objadmin.City = AdminInfo.City;
                    objadmin.StateID = AdminInfo.StateID;
                    objadmin.Phone = AdminInfo.Phone;
                    objadmin.Fax = AdminInfo.Fax;
                    objadmin.Newsletter = AdminInfo.Newsletter;
                    objadmin.registrationDate = AdminInfo.RegisterationDate;
                    if (_service.UpdateAdmin_InfoDetails(objadmin, ref Msg))
                    {
                        TempData["Msg"] = "Records Updated successfully.";
                        return RedirectToAction("MyDetails");
                    }
                    else
                    {
                        ViewData["Msg"] = "Error:" + Msg;
                        BindFilter(AdminInfo.StateID);
                        return View(AdminInfo);
                    }

                }
                catch (Exception ex)
                {
                    ViewData["Msg"] = "Error:" +ex.Message;
                    BindFilter(AdminInfo.StateID);
                    return View(AdminInfo);
                }

            }
              

        }

    }
}
