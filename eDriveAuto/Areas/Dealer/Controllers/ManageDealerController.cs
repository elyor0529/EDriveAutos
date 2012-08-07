using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Areas.Dealer.Controllers
{
    [Authorize(Roles="Dealer")]
    public class ManageDealerController : Controller
    {
        //
        // GET: /Dealer/DealerDetail/

        #region EditDealerInfo
        [HttpPost]
        public ActionResult ChangePassword(DealerInfo DealerModel, String txtPassword)
        {
           
            {
                try
                {
                    Admin.Controllers.ManageDealerController mg = new Admin.Controllers.ManageDealerController();
                    mg.ChangePassword(DealerModel, DealerModel.Password);
                    TempData["Msg"] = "Password Successfully Changed";
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "error: "+ex.Message;
                }

                return RedirectToAction("DealerInfo");
            }
        }

        public ActionResult DealerInfo()
        {
            ViewData["Msg"]=TempData["Msg"];
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var CustomerID = service.GetDealerByDealerEmail(User.Identity.Name).customerID;
                BindFilter(service);
               DealerInfo DealerModel =Admin.Controllers.ManageDealerController.GetDealerModel(CustomerID, service);
                return View(DealerModel);
            }
        }

            private void BindFilter(Edrive_ServiceClient service)
        {
            var lstCountries = service.GetCountry();
            var country = lstCountries.Select(m => new SelectListItem { Selected = (m.DisplayOrder == 1), Text = m.Name, Value = m.CountryID.ToString() }).ToList();
            ViewData["Country"] = country;
            var SelectedCountryID = lstCountries.First(m => m.DisplayOrder == 1).CountryID;
            var StateID = service.GetStateByCountry(SelectedCountryID).Select(m => new SelectListItem { Selected = (m.DisplayOrder == 1), Text = m.Name, Value = m.CountryID.ToString() }).ToList();
            ViewData["Country"] = country;
            ViewData["StateID"] = StateID;
        }

            [HttpPost]
            public ActionResult DealerInfo(DealerInfo DealerModel, String Delete, String changepasssword, String txtPassword)
            {
                using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
                {
                    if (Delete != null)
                    {
                        if (service.Delete_Dealer(DealerModel.CustomerID))
                        {
                            TempData["Msg"] = "Record deleted successfully.";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewData["Msg"] = "Record Delete Failulre";
                            return View(DealerModel);
                        }

                    }

                    else
                    {//udpate dealer
                        String Msg = "";
                        Edrivie_Service_Ref.Customer objDealer;
                      
                   

                        _CustomerProfile _Profile;
                        GetDealer_Info(DealerModel, out objDealer, out _Profile, true);
                        _Profile = service.GetDealer_Profile_ByDealerID(DealerModel.CustomerID);
                       
                     


                        if (service.Update_Dealer(objDealer, ref Msg, _Profile))
                        {
                            String DealerEmail = User.Identity.Name;
                            if (DealerModel.Email != DealerEmail)//if User updated the Email then change the relogin the User
                            {
                                FormsAuthentication.SignOut();
                                Common_Methods.CreateUser(DealerModel.Email, "Dealer");
                                
 
                            }
                            TempData["Msg"] = "Dealer Updated Successfully";
                            return RedirectToAction("DealerInfo");
                        }
                        else
                        {
                            TempData["Msg"] = "Dealer Updating Failure,error=" + Msg;

                        }
                        //------------to get update image of Dealer---
                        //_Profile = service.GetDealer_Profile_ByDealerID(DealerModel.CustomerID);
                        //DealerModel.Logo = _Profile.Logo;
                        //DealerModel.PageImage = _Profile.PageImage;

                        //------------end of ---to get updated image of Dealer---
                        //BindFilter(service);
                        return DealerInfo();
                    }
                }
            }
            [HttpPost]
            public JsonResult GetState(Int32 CountryID)
            {
                Admin.Controllers.ManageDealerController ctrl = new Admin.Controllers.ManageDealerController();
                return ctrl.GetState(CountryID);
            }
            public void GetDealer_Info(DealerInfo DealerModel, out Edrivie_Service_Ref.Customer objDealer, out _CustomerProfile _Profile, Boolean IsEdit = false)
            {
                objDealer = new Edrivie_Service_Ref.Customer();
                objDealer.active = true;
                objDealer.email = DealerModel.Email;
                objDealer.customerID = DealerModel.CustomerID;
                objDealer.password = DealerModel.Password;
                objDealer.Gender = DealerModel.Gender;
                objDealer.FirstName = DealerModel.FirstName;
                objDealer.LastName = DealerModel.LastName;
                objDealer.DateofBirth = DealerModel.DateofBirth;
                objDealer.CompanyName = DealerModel.Company;
                objDealer.StreetAddress1 = DealerModel.StreetAddress1;
                objDealer.StreetAddress2 = DealerModel.StreetAddress2;
                objDealer.Zip = DealerModel.Zip;
                objDealer.City = DealerModel.City;
                objDealer.StateID = DealerModel.StateID;
                objDealer.Phone = DealerModel.Phone;
                objDealer.Fax = DealerModel.Fax;
                objDealer.Newsletter = DealerModel.Newsletter;
                objDealer.registrationDate = DateTime.UtcNow;
                _Profile = new _CustomerProfile { };
                _Profile.ApplicationURL = DealerModel.ApplicationURL;
                _Profile.Description = DealerModel.Description;
                // _Profile.Logo = DealerModel.Logo;
                _Profile.PageImage = DealerModel.PageImage;
                _Profile.ServiceURL = DealerModel.ServiceURL;
                _Profile.WarrantyURL = DealerModel.WarrantyURL;
                //---upload photo--


                if (Request.Files["LogoFup"] != null)
                {
                    if (String.IsNullOrEmpty(Request.Files["LogoFup"].FileName) == false)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(Request.Files["LogoFup"].FileName) + DateTime.Now.Ticks + Path.GetExtension(Request.Files["LogoFup"].FileName);
                        var filePath = Server.MapPath("~/Content/Images/Dealer/" + fileName);
                        Request.Files["LogoFup"].SaveAs(filePath);
                        _Profile.Logo = Common_Methods.GetDomainUrl() + "Content/Images/Dealer/" + fileName;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(DealerModel.Logo))
                        {
                            _Profile.Logo = null;
                        }
                        else
                        {
                            _Profile.Logo = DealerModel.Logo;
                        }
                    }
                }

                if (Request.Files["PageImageFup"] != null)
                {
                    if (String.IsNullOrEmpty(Request.Files["PageImageFup"].FileName) == false)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(Request.Files["PageImageFup"].FileName)
                            + DateTime.Now.Ticks + Path.GetExtension(Request.Files["PageImageFup"].FileName);
                        var filePath = Server.MapPath("~/Content/Images/Dealer/" + fileName);
                        Request.Files["PageImageFup"].SaveAs(filePath);
                        _Profile.PageImage = Common_Methods.GetDomainUrl() + "Content/Images/Dealer/" + fileName;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(DealerModel.PageImage))
                        {
                            _Profile.PageImage = null;// Common_Methods.GetDomainUrl() + "Content/Images/Dealer/photo-comming-soon.jpg";
                        }
                        else
                        {
                            _Profile.PageImage = DealerModel.PageImage;
                        }
                    }
                }
            }

            public JsonResult IsDealerExist(String Email, String CustomerID)
            {
                Admin.Controllers.ManageDealerController mg = new Admin.Controllers.ManageDealerController();
                return mg.IsDealerExist(Email, CustomerID);
            }
        #endregion
        #region DealerProfile

            public ActionResult DealerProfile()
            {

                    return DealerInfo();
                
            }

            [HttpPost]
            public ActionResult DealerProfile(DealerInfo DealerModel)
            {
                using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
                {
                    var CustomerID = service.GetDealerByDealerEmail(User.Identity.Name).customerID;
                    DealerInfo DealerModel_to_Update = Admin.Controllers.ManageDealerController.GetDealerModel(CustomerID, service);
                   //----------Update only profile Info
                    DealerModel_to_Update.ApplicationURL = DealerModel.ApplicationURL;
                    DealerModel_to_Update.WarrantyURL = DealerModel.WarrantyURL;
                    DealerModel_to_Update.ServiceURL = DealerModel.ServiceURL;
                    DealerModel_to_Update.Description = DealerModel.Description;
                    DealerModel_to_Update.Logo = DealerModel.Logo;
                    DealerModel_to_Update.PageImage = DealerModel.PageImage;

                    String Msg = "";
                    Edrivie_Service_Ref.Customer objDealer;
                    _CustomerProfile _Profile;
                    GetDealer_Info(DealerModel_to_Update, out objDealer, out _Profile, true);


                    if (service.Update_Dealer(objDealer, ref Msg, _Profile))
                    {
                        TempData["Msg"] = "Dealer  Updated Successfully";
                    }
                    else
                    {
                        TempData["Msg"] = "Dealer Updating Failure,error=" + Msg;

                    }
                    return RedirectToAction("DealerProfile");
                }

            }

            [HttpPost]
            public ActionResult DeleteLogo(DealerInfo DealerModel)
            {
                using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
                {
                    var Msg = "";
                    if (service.Delete_Dealer_Profile_Logo(out Msg, DealerModel.CustomerID))
                    {
                        TempData["Msg"] = "Logo Deleted successfully.";
                        return RedirectToAction("DealerProfile");
                    }
                    else
                    {
                        TempData["Msg"] = Msg;
                        return RedirectToAction("DealerProfile");
                    }
                }

            }
         [HttpPost]
        public ActionResult DeletePageImage(DealerInfo DealerModel)
        {
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var Msg = "";
               if( service.Delete_Dealer_Profile_PageImage(out Msg,DealerModel.CustomerID))
                {
                    TempData["Msg"] = "Page Image Deleted successfully.";
                    return RedirectToAction("DealerProfile");
                }
                else
                {
                    TempData["Msg"] = Msg;
                    return RedirectToAction("DealerProfile");
                }
            }
            
        }
        #endregion
    }

    }

