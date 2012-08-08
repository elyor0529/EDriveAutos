using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;


namespace Edrive.Controllers
{
    [Authorize]
    public class MyAccountController : Controller
    {
        //
        // GET: /MyAccount/

        public ActionResult Index()
        {
            ViewData["Msg"]=TempData["Msg"];
            Bind_Filter();
            return View();
        }

        private void Bind_Filter()
        {
            var _PasswordDetails = new _PasswordDetails();
            _PersonalDetails _prd = new _PersonalDetails();
            if (User.IsInRole("Admin") || User.IsInRole("Dealer"))
            {
                using (Edrivie_Service_Ref.Edrive_ServiceClient _servicae = new Edrive_ServiceClient())
                {
                    var dealer = _servicae.GetDealerByDealerEmail(User.Identity.Name);
                    _prd.Name = dealer.FirstName + " " + dealer.LastName;
                    _prd.Email = dealer.email;
                    _prd.PostalCode = dealer.Zip;
                    _prd.Telephone = dealer.Phone;
                    _prd.CustomerID = dealer.customerID;
                    ViewData["PersonalDetails"] = _prd;
                    _PasswordDetails.CustomerID = dealer.customerID;
                    ViewData["_PasswordDetails"] = _PasswordDetails;

                }
            }
            else
            {
                if (User.IsInRole("Customer"))
                {
                    using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
                    {
                     

                        var Customer = _entity.Customer.First(m => m.Email == User.Identity.Name);
                        _prd.Name = Customer.FirstName + " " + Customer.LastName;
                        _prd.Email = Customer.Email;
                        _prd.PostalCode = Customer.zip ?? 0;
                        _prd.Telephone = Customer.Phone;
                        _prd.CustomerID = Customer.CustomerID;
                        ViewData["PersonalDetails"] = _prd;
                        _PasswordDetails.CustomerID = Customer.CustomerID;
                        ViewData["_PasswordDetails"] = _PasswordDetails;
                    }

                }
            }
        }
        [HttpPost]
        public ActionResult SavePersonalDetails(_PersonalDetails model)
        {
            String Msg="";
            try
            {

          
            if (ModelState.IsValid == false)
            {
                ViewData["Msg"] = "Please fill the required details.";
                Bind_Filter();
                return RedirectToAction("Index");
            }
            if (User.IsInRole("Customer"))
            {
                
                using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
                {
                   

                    var Customer = _entity.Customer.First(m => m.CustomerID == model.CustomerID);
                    //if this eamail already exist for other users
                    if (_entity.Customer.Any(m => m.Deleted == false && m.Email == model.Email && m.CustomerID != model.CustomerID))
                    {
                        ViewData["Msg"] = "Thie email is already in use by other user. Please change the email address.";
                        Bind_Filter();
                        return View("Index");

                    }

                    Customer.Name = Customer.FirstName = model.Name;
                    Customer.LastName = "";
                    Customer.zip = model.PostalCode;
                    Customer.Email = model.Email;
                    Customer.Phone = model.Telephone;
                    _entity.SaveChanges();
                    TempData["Msg"] = "Personal Details Updated successfully.";
                    FormsAuthentication.SignOut();
                    Common_Methods.CreateUser(model.Email, "Customer");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (User.IsInRole("Admin") || User.IsInRole("Dealer"))
                {
                    using (Edrivie_Service_Ref.Edrive_ServiceClient _servicae = new Edrive_ServiceClient())
                    {
                        if (_servicae.Is_other_DealerExist_for_same_email(model.Email,model.CustomerID))
                        {
                            ViewData["Msg"] = "Thie email is already in use by other user. Please change the email address.";
                            Bind_Filter();
                            return View("Index");
 
                        }
                        var dealer = new Edrivie_Service_Ref.Customer(); //_servicae.GetDealerByDealerEmail(User.Identity.Name);
                        dealer.Name = dealer.FirstName = model.Name;
                        dealer.LastName = "";
                        dealer.Zip = model.PostalCode;
                        dealer.email = model.Email;
                        dealer.Phone = model.Telephone;
                        dealer.customerID = model.CustomerID;

                        /// if dealer is updated successfully
                        if (_servicae.Update_Dealer_Personal_Details(out Msg, dealer) == false)
                        {
                            ViewData["Msg"] = "error" + Msg;
                           
                            Bind_Filter();
                            return View("Index");
                        }
                        else
                        {
                            String RoleName = User.IsInRole("Admin") ? "Admin" : "Dealer";
                            FormsAuthentication.SignOut();
                               Common_Methods.CreateUser(model.Email, RoleName);
                            TempData["Msg"] = "Personal Details Updated successfully.";
 
                        }
                        return RedirectToAction("Index");
                    }

                }

            }
           
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "error" + ex.Message;
                    return RedirectToAction("Index");
                  
            }
 
        }


        public ActionResult SavePersonalDetails()
        {
           return RedirectToAction("Index");
        }

   //     private void    Common_Methods.CreateUser(String Email, String RoleName)
   //     {
   //         FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, Email, DateTime.Now,
   //DateTime.Now.AddMinutes(20), false, RoleName, FormsAuthentication.FormsCookiePath);
   //         String hashCookies = FormsAuthentication.Encrypt(authTicket);
   //         HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
   //         Response.Cookies.Add(cookie);
   //     }
        [HttpPost]
        public ActionResult SavePassword(_PasswordDetails model)
        {
            try
            {
              
            if (ModelState.IsValid == false)
            {
                TempData["Msg"] = "Please fill the required details.";
                return RedirectToAction("Index");
            }

            if (User.IsInRole("Customer"))
            {
                using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
                {

                    //if this old password matches
                    if (_entity.Customer.Any(m => m.Password != model.OldPassword && m.CustomerID == model.CustomerID))
                    {
                        TempData["Msg"] = "Error: Incorrrect Old password";
                        return RedirectToAction("Index");
                    }
                    var Customer = _entity.Customer.First(m => m.CustomerID == model.CustomerID);

                    Customer.Password = model.NewPassword;
                    _entity.SaveChanges();
                    TempData["Msg"] = "Password Updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (User.IsInRole("Admin") || User.IsInRole("Dealer"))
                {
                    using (Edrivie_Service_Ref.Edrive_ServiceClient _servicae = new Edrive_ServiceClient())
                    {
                        var dealer= _servicae.GetDealerByDealerID(model.CustomerID);
                        //if this old password matches
                        if (dealer.password != model.OldPassword)
                        {
                            TempData["Msg"] = "Error: Incorrrect Old password";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            String Msg = "";
                            if (_servicae.UpdatePassword_for_Dealer(out Msg, model.CustomerID, model.NewPassword))
                            {
                                TempData["Msg"] = "Password Updated successfully.";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["Msg"] = "error: " + Msg;
                                return RedirectToAction("Index");
                            }
                        }
                    }

                }
            }
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
