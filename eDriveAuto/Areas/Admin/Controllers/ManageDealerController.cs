using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;
using Customer = Edrive.Edrivie_Service_Ref.Customer;

namespace Edrive.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageDealerController : Controller
    {
        //
        // GET: /Admin/ManageDealer/


        public ActionResult Index()
        {
            ViewData["Msg"] = TempData["Msg"];
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var lst = service.GetDealers().Select(m => new { CompanyName = m.CompanyName }).Distinct().ToList();
                ViewData["Dealers"] = new SelectList(lst, "CompanyName", "CompanyName");
                ViewData["CarsCount"] = 0;
                ViewData["PageIndex"] = 0;

            }
            ViewData["IsPostBack"] = true;
            return View();
        }

        [HttpPost]
        public ActionResult Index(String Featured, String chkFeatured, String AddNew, _DealerManageFilter Model_Search, Int32 PageIndex, String CustomerID, String SendMail)
        {
            if (Featured != null)
            {
                if(String.IsNullOrEmpty(chkFeatured)==false)
                using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
                {
                    String Msg;
                    if (service.MakeDealer_as_FeaturedDealer(out Msg, Convert.ToInt32(chkFeatured)))
                    {
                        ViewData["Msg"] = "Featured Added for Dealer successfully.";
                    }
                    else
                    {
                        ViewData["Msg"] = "error:"+Msg;
 
                    }
                    Int32 CarsCount, pageSize = 50;
                    ViewData["Customers"] = service.SearchDealer_For_ManagDealerSection
                         (out CarsCount, Model_Search.CompanyName, Model_Search.CompanyName2, Model_Search.Email,
                         Model_Search.LastName, Model_Search.Name, Model_Search.RegFrom, Model_Search.RegTo
                         , PageIndex, pageSize);
                    var lst = service.GetDealers().Select(m => new { CompanyName = m.CompanyName }).Distinct().ToList();
                    ViewData["Dealers"] = new SelectList(lst, "CompanyName", "CompanyName");
                    ViewData["CarsCount"] = CarsCount;
                    ViewData["PageIndex"] = PageIndex;
                    return View();
                }

 
            }
            if (AddNew != null)
            {
                return RedirectToAction("Add");
            }
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                if (SendMail != null)//then Send Mail
                {
                    Int32 customerID;
                    if (Int32.TryParse(CustomerID, out customerID))
                    {
                        var customer = service.GetDealerByDealerID(customerID);
                        MessageManager.SendCustomerWelcomeMessage(customer, 0);
                        ViewData["Msg"] = "Message sent successfully!";
                    }
                    else
                    {
                        ViewData["Msg"] = "Invalid CustomerID=" + customerID;
                    }
                }

                {
                    Int32 CarsCount, pageSize = 50;
                    ViewData["Customers"] = service.SearchDealer_For_ManagDealerSection
                         (out CarsCount, Model_Search.CompanyName, Model_Search.CompanyName2, Model_Search.Email,
                         Model_Search.LastName, Model_Search.Name, Model_Search.RegFrom, Model_Search.RegTo
                         , PageIndex, pageSize);
                    var lst = service.GetDealers().Select(m => new { CompanyName = m.CompanyName }).Distinct().ToList();
                    ViewData["Dealers"] = new SelectList(lst, "CompanyName", "CompanyName");
                    ViewData["CarsCount"] = CarsCount;
                    ViewData["PageIndex"] = PageIndex;

                }
            }
            return View();
        }

        public ActionResult Edit(Int32 id)
        {
            ViewData["Msg"]=TempData["Msg"];
            var CustomerID = id;
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
    
                DealerInfo DealerModel = GetDealerModel(CustomerID, service);
                BindFilter(service,DealerModel.StateID);
                return View(DealerModel);
            }
        }


         public static DealerInfo GetDealerModel(int CustomerID, Edrive_ServiceClient service)
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
            dealer = service.GetDealerByDealerID(CustomerID);
            //}
            DealerInfo DealerModel = new DealerInfo();
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
            var _Profile = service.GetDealer_Profile_ByDealerID(dealer.customerID);
            DealerModel.ApplicationURL = _Profile.ApplicationURL;
            DealerModel.Description = _Profile.Description;
            DealerModel.Logo = _Profile.Logo;
            DealerModel.PageImage = _Profile.PageImage;
            DealerModel.ServiceURL = _Profile.ServiceURL;
            DealerModel.WarrantyURL = _Profile.WarrantyURL;

            return DealerModel;
        }

        [HttpPost]
         public ActionResult  ChangePassword(DealerInfo DealerModel, String txtPassword)
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
                        result = service.UpdatePassword_for_Dealer(out  Msg, DealerModel.CustomerID, txtPassword);
                        if (result == true)
                        {
                            TempData["Msg"] = "Password Successfully Changed";
                        }
                        else
                        {
                            TempData["Msg"] = "Password  Changed faliure";
                        }
                    }
                    return RedirectToAction("Edit", new { id = DealerModel.CustomerID });
                }
        }

        [HttpPost]
        public ActionResult Delete(DealerInfo DealerModel, String Delete, String changepasssword, String txtPassword)
        {
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
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
 
        }

        [HttpPost]
        public ActionResult Edit(DealerInfo DealerModel, String Delete, String changepasssword, String txtPassword)
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
                        ViewData["Msg"] = "Record deletion Failure";
                        return View(DealerModel);
                    }

                }
               
                else
                {//update dealer
                    String Msg = "";
                    Edrivie_Service_Ref.Customer objDealer;
                    _CustomerProfile _Profile;
                    GetDealer_Info(DealerModel, out objDealer, out _Profile, true);
                    
                  
                    if (service.Update_Dealer(objDealer, ref Msg, _Profile))
                    {
                        TempData["Msg"] = "Dealer Updated Successfully";
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
                    return Edit(DealerModel.CustomerID );
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteLogo(DealerInfo DealerModel)
        {
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var Msg = "";
               if( service.Delete_Dealer_Profile_Logo(out Msg,DealerModel.CustomerID))
                {
                    return RedirectToAction("Edit", new { id = (DealerModel.CustomerID) });
                }
               else
               {
                   TempData["Msg"] = Msg;
                   return RedirectToAction("Edit", new { id = (DealerModel.CustomerID) });
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
                    return RedirectToAction("Edit",new{id= (DealerModel.CustomerID)});
                }
                else
                {
                    TempData["Msg"]=Msg;
                    return RedirectToAction("Edit", new { id = (DealerModel.CustomerID) });
                }
            }
            
        }
        

        public ActionResult Add()
        {
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                BindFilter(service);
                return View();
            }
        }

        public JsonResult IsDealerExist(String Email, String CustomerID)
        {
            using (var _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                //----new user adding--
                if (CustomerID == "0" || CustomerID == "")
                {
                    if (_service.IsDealerExists(Email))
                    {
                        //validation  is false
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    //validation  is true
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                //---------
                //----edit user--
                     
                else
                {
                    var _custID = Convert.ToInt32(CustomerID);
                    if (_service.Is_other_DealerExist_for_same_email(Email, _custID)
                        )
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
        private void BindFilter(Edrive_ServiceClient service,Int32 StatedID=-1)
        {
            var lstCountries = service.GetCountry();
            var country = lstCountries.Select(m => new SelectListItem { Selected = (m.DisplayOrder  == 1), Text = m.Name, Value = m.CountryID.ToString() }).ToList();
            ViewData["Country"] = country;
            var SelectedCountryID = lstCountries.First(m => m.DisplayOrder == 1).CountryID;
            var StateID = service.GetStateByCountry(SelectedCountryID).Select(m => new SelectListItem { Selected = (m.StateProvinceID == StatedID), Text = m.Name, Value = m.StateProvinceID.ToString() }).ToList();
            ViewData["Country"] = country;
            ViewData["StateID"] = StateID;
        }

        [HttpPost]
        public ActionResult Add(DealerInfo DealerModel)
        {
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                Edrivie_Service_Ref.Customer objDealer;
                _CustomerProfile _Profile;
                GetDealer_Info(DealerModel, out objDealer, out _Profile);

                //--- end ofupload photo--
                String Msg = "";
                if (service.AddDealer(objDealer, ref Msg, _Profile))
                {
                    TempData["Msg"] = "Dealer Added successfully.";
                    var customer = service.GetDealerByDealerEmail(DealerModel.Email);
                    DealerModel.CustomerID = customer.customerID;
                    SendDealerInfoToCarfax(customer);
                    //TempData["EmailID"] = DealerModel.Email;
                    return RedirectToAction("Edit", new { id = DealerModel.CustomerID });
                }
                else
                {
                    ViewData["Msg"] = "Dealer Adding Failure,error=" + Msg;
                }
                BindFilter(service);
                // var  lstCountries=service.GetCountry();
                // var country = lstCountries.Select(m => new SelectListItem {Selected=(m.DisplayOrder==1) ,Text=m.Name,Value=m.CountryID.ToString()}).ToList();

                // var SelectedCountryID=lstCountries.First(m=>m.DisplayOrder==1).CountryID;

                // var StateID = service.GetStateByCountry(SelectedCountryID).Select(m => new SelectListItem {Selected=(m.DisplayOrder==1) ,Text=m.Name,Value=m.CountryID.ToString()}).ToList();
                //ViewData["Country"]= country;
                //ViewData["StateID"] = StateID;
                 
                return View();
            }
        }

        private void SendDealerInfoToCarfax(Customer customer)
        {
            if (DateTime.UtcNow.Hour < 6 && DateTime.UtcNow.Hour > 12)
            {
                return;
            }
            // data for carfax must contains the following:
            // The dealer list must contain the following info:  
            //Dealer ID/Unique Identifier (how you identify your dealer) 
            //Dealer name (100 character limit)
            //Physical Address –(not mailing) Example: 123 Elm St – (60 character limit)
            //City (60 character limit) 
            //State (abbreviated and in CAPS i.e. VA, NY, etc) 
            //Zip Code
            //Local phone number (10 characters with no punctuations, Example:7039342664)  
            //Additional information such as Fax#, Toll Free#, Primary Contact Info, assists us in the matching process.
            string dataToSend = "";

            dataToSend += customer.customerID;
            dataToSend += "|" + (customer.Name.Length > 100 ? customer.Name.Substring(0, 100) : customer.Name);
            dataToSend += "|" + (customer.StreetAddress1.Length > 60 ? customer.StreetAddress1.Substring(0, 60) : customer.StreetAddress1);
            dataToSend += "|" + (customer.StreetAddress2.Length > 60 ? customer.StreetAddress2.Substring(0, 60) : customer.StreetAddress2);
            dataToSend += "|" + customer.StateName;
            dataToSend += "|" + customer.Zip;
            dataToSend += "|" + customer.Phone;

            FileInfo file = new FileInfo(Server.MapPath(String.Format("~/EDA_dealerlist_{0}.txt", DateTime.UtcNow.ToString("MMddyyyy"))));

            using (StreamWriter write = file.CreateText())
            {
                write.WriteLine(dataToSend);
            }

            var carfaxUrl = ConfigurationManager.AppSettings["CarfaxFtpUrl"];
            var inboundCredential = new NetworkCredential(ConfigurationManager.AppSettings["CarfaxUserName"], ConfigurationManager.AppSettings["CarfaxPassword"]);

            FtpWebRequest request = WebRequest.Create(carfaxUrl + "/" + file.Name) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = inboundCredential;

            StreamReader sourceStream = new StreamReader(file.FullName);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
        }
        public   void GetDealer_Info(DealerInfo DealerModel, out Edrivie_Service_Ref.Customer objDealer, out _CustomerProfile _Profile, Boolean IsEdit = false)
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
           
           
            if ( Request.Files["LogoFup"] != null)
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
        
        [HttpPost]
        public JsonResult GetState(Int32 CountryID)
        {
            using (Edrive_ServiceClient service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var States = service.GetStateByCountry(CountryID).Select(m => m);// new SelectListItem { Selected = (m.DisplayOrder == 1), Text = m.Name, Value = m.CountryID.ToString() }).ToList();
                return Json(States);
            }

        }
    }
}
