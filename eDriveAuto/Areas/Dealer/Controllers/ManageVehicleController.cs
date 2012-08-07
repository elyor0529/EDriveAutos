using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Logic.Interfaces;
using Edrive.Models;
using Customer = Edrive.Edrivie_Service_Ref.Customer;
using Products = Edrive.Edrivie_Service_Ref.Products;

namespace Edrive.Areas.Dealer.Controllers
{
	[Authorize(Roles = "Dealer")]
    public class ManageVehicleController : Controller
    {
		private readonly IStateProvinceService _stateProvinceService;
		private readonly IDealerService _dealerService;
		private readonly IProductService _productService;

		public ManageVehicleController(IStateProvinceService stateProvinceService,
			IDealerService dealerService, IProductService productService)
		{
			_stateProvinceService = stateProvinceService;
			_dealerService = dealerService;
			_productService = productService;
		}

		public ActionResult Index()
        {
            return View();
        }

        #region UploadVehicle
        public ActionResult UploadVehicle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadVehicle(String csvFile)
        {
            if (Request.Files["csvFile"].FileName == "")
            {
                ViewData["Msg"] = "Please select a csv file to upload.";
                return View();
            }

            if (Request.Files["csvFile"] != null)
            {

                if (Path.GetExtension(Request.Files["csvFile"].FileName).ToLower() != ".csv")
                {//failure
                    ViewData["Msg"] = "Error:Only csv file can be uploaded.";
                    return View();
                }
                //success

                var fileName = Path.GetFileNameWithoutExtension(Request.Files["csvFile"].FileName) + "_" + DateTime.Now.
                    ToString("MM_dd_yyyy_HH_mm_ss") + ".csv";

                string physicalpath = Path.Combine(Server.MapPath("~/Content/Data"), fileName);
                Request.Files["csvFile"].SaveAs(physicalpath);
                using (var service = new Edrive_ServiceClient())
                {
                    service.Data_Import_Update(physicalpath);
                }
                ViewData["Msg"] = "Csv file has Uploaded and updated the DataBase also.";
                return View();

            }
            return View();

        }
        #endregion
        #region ManageVehicle

        /// <summary>
        /// This method is for manage vehicle section
        /// </summary>
        /// <returns></returns>
       
        public  ActionResult Manage()
        {
           ViewData["Msg"] = TempData["Msg"];

            using (var servicae = new Edrive_ServiceClient())
            {
                Bind_ManageProduct(servicae);
                //var CarsCount = 0;

                var lstProducts = SearchVehicle("", "", -1, "", false, 0, 50);// servicae.SearchProduct_for_ManageProduct("", "", -1, -1, false, 0, 50, ref CarsCount);
                // ViewData["CarsCount"] = CarsCount;
                return View(lstProducts);
            }


        }
        /// <summary>
        /// This method is for manage vehicle section and when any actin is done on the page
        /// </summary>
        /// <param name="Vin"></param>
        /// <param name="Stock"></param>
        /// <param name="Make"></param>
        /// <param name="DealerName"></param>
        /// <param name="Featured"></param>
        /// <param name="btnSubmit"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Manage(String Vin, String Stock, String Make, string DealerName, bool Featured, String btnSubmit, String PageIndex)
        {
            var _MakeID = -1;
            if (!string.IsNullOrEmpty(Make))
            {
                _MakeID = Convert.ToInt32(Make);

            }
            var _DealerID = DealerName;

            var DealerEmail = User.Identity.Name;
            var RoleName = "Admin";
            if (User.IsInRole("Dealer"))
                RoleName = "Dealer";
            //if (!string.IsNullOrEmpty(DealerName))
            //{
            //    _DealerID = Convert.ToInt32(DealerName);

            //}
            using (var service = new Edrive_ServiceClient())
            {

                if (btnSubmit != null)
                {

                    switch (btnSubmit)
                    {
                        case "go":
                            var lstProducts = SearchVehicle(Vin, Stock, _MakeID, _DealerID, Featured, 0, 50);
                            //ViewData["CarsCount"]                               = service.Get_Count_SearchProduct_for_ManageProduct(Vin, Stock, _MakeID, _DealerID, Featured);

                            return View(lstProducts);

                        case "Search": lstProducts = SearchVehicle(Vin, Stock, _MakeID, _DealerID, Featured, 0, 50);
                            //ViewData["CarsCount"] = service.Get_Count_SearchProduct_for_ManageProduct(Vin, Stock, _MakeID, _DealerID, Featured);

                            return View(lstProducts);
                        case "Add New": return RedirectToAction("Add");
                        case "Approve": service.ApproveProducts();
                            lstProducts = SearchVehicle(Vin, Stock, _MakeID, _DealerID, Featured, Convert.ToInt32(PageIndex), 50);
                            ViewData["CarsCount"] = service.Get_Count_SearchProduct_for_ManageProduct(Vin, Stock, _MakeID, _DealerID, DealerEmail, RoleName, Featured);
                            return View(lstProducts);

                        case "Qualify":
                            {
                                if (Request.Form["chkProduct"] != null)
                                {
                                    List<string> ProductIDs = new List<string>();
                                    if (Request.Form["chkProduct"].Contains(','))
                                        ProductIDs = Request.Form["chkProduct"].Split(',').ToList();
                                    else
                                        ProductIDs.Add(Request.Form["chkProduct"]);
                                    // qualifyPrice = ValidRecord(product.VIN, product.Price_Current);
                                    if (!service.QualifyPriceofProduct(ProductIDs))
                                    {
                                        throw new Exception();
                                    }
                                }
                                lstProducts = SearchVehicle(Vin, Stock, _MakeID, _DealerID, Featured, 0, 50);
                                return View(lstProducts);
                            }



                        case "Delete Selected":
                            try
                            {
                                if (Request.Form["chkProduct"] != null)
                                {
                                    List<string> ProductIDs = new List<string>();
                                    if (Request.Form["chkProduct"].Contains(','))
                                        ProductIDs = Request.Form["chkProduct"].Split(',').ToList();
                                    else
                                        ProductIDs.Add(Request.Form["chkProduct"]);


                                    if (service.DeleteProduct(ProductIDs))
                                    {
                                        lstProducts = SearchVehicle(Vin, Stock, _MakeID, _DealerID, Featured, 0, 50);
                                        ViewData["Msg"] = "Records Deleted successfully.";
                                        return View(lstProducts);
                                    }
                                    else
                                    {
                                        ViewData["Msg"] = "Error:Unable to delete the records.";
                                    }
                                }
                                lstProducts = SearchVehicle(Vin, Stock, _MakeID, _DealerID, Featured, Convert.ToInt32(PageIndex), 50);
                                return View(lstProducts);
                            }
                            catch
                            {
                            }
                            return View();

                    }
                }

                else
                {
                    //new page Index
                    var lstProducts = SearchVehicle(Vin, Stock, _MakeID, _DealerID, Featured, Convert.ToInt32(PageIndex), 50);
                    ViewData["CarsCount"] = service.Get_Count_SearchProduct_for_ManageProduct(Vin, Stock, _MakeID, _DealerID, DealerEmail, RoleName, Featured);
                    return View(lstProducts);
                }
            }
            return View();

        }

        /// <summary>
        /// To  Get the list of search vehicle for manage search page
        /// </summary>
        /// <param name="Vin"></param>
        /// <param name="Stock"></param>
        /// <param name="_MakeID"></param>
        /// <param name="_ComapanyName"></param>
        /// <param name="Featured"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private List<Products> SearchVehicle(String Vin, string Stock, Int32 _MakeID, String _ComapanyName, bool Featured, Int32 pageIndex, Int32 pageSize)
        {
            using (var service = new Edrive_ServiceClient())
            {
                var DealerEmail = HttpContext.User.Identity.Name;
                var RoleName = "Admin";
                if (HttpContext.User.IsInRole("Dealer"))
                    RoleName = "Dealer";
                Int32 carsCount = 0;
                var lstProducts = service.SearchProduct_for_ManageProduct(Vin, Stock, _MakeID, _ComapanyName, DealerEmail, RoleName, Featured, pageIndex, pageSize, ref carsCount);
                ViewData["CarsCount"] = carsCount;
                ViewData["PageIndex"] = pageIndex;
                Bind_ManageProduct(service, _MakeID, _ComapanyName);
                return lstProducts;
            }
        }
        /// For manage product it binds the filters
        /// </summary>
        /// <param name="service"></param>
        /// <param name="MakeID"></param>
        /// <param name="DealerID"></param>
        public void Bind_ManageProduct(Edrive_ServiceClient service, Int32 MakeID = -1, String DealerID = "")
        {
            var makes = service.BindMake();
            SelectList lst = new SelectList(makes, "id", "make");
            //if (MakeID != -1)
            //{
            //    var str = MakeID.ToString();
            //    var mk = lst.Where(m => m.Value == str).SingleOrDefault();
            //    if (mk != null)
            //        mk.Selected = true;
            //}

            ViewData["Make"] = lst;
            var dealers = new List<Edrivie_Service_Ref.Customer>();
			var CountDownDays = 28; //Convert.ToInt32(SettingManager.GetSettingValue("CountDown.Days"));
            var lstDealers = service.GetDealers_for_ManageProoduct(CountDownDays);
            foreach (var item in lstDealers)
            {
                if (String.IsNullOrEmpty(item.CompanyName) == false)
                    dealers.Add(item);
            }
            dealers = dealers.OrderBy(m => m.CompanyName).Distinct().ToList();
            var lstDealerName = dealers.Select(m => new { CompanyName = m.CompanyName }).ToList();
            ViewData["DealerName"] = new SelectList(lstDealerName, "CompanyName", "CompanyName");
        }
        //public void Bind_ManageProduct(Edrive_ServiceClient service, Int32 MakeID = -1, String DealerID = "")
        //{
        //    var makes = service.BindMake();
        //    SelectList lst = new SelectList(makes, "id", "make");
         

        //    ViewData["Make"] = lst;
        //    var dealers = new List<Edrivie_Service_Ref.Customer>();
        //    var lstDealers = service.GetDealers();
        //    foreach (var item in lstDealers)
        //    {
        //        if (String.IsNullOrEmpty(item.CompanyName) == false)
        //            dealers.Add(item);
        //    }
        //    dealers = dealers.OrderBy(m => m.CompanyName).Distinct().ToList();
        //    var lstDealerName = dealers.Select(m => new { CompanyName = m.CompanyName }).ToList();
        //    ViewData["DealerName"] = new SelectList(lstDealerName, "CompanyName", "CompanyName");
        //}
        
        /// <summary>
        /// To add the new Cars
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {

            using (var _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var options = _service.GetAllProductOptions().Select(m => new SelectListItem { Text = m.OptionName, Value = m.id.ToString() }).ToList();
                ViewData["Options"] = options;
                ViewData["ProductID"] = 0;
                BindDropDowns(_service);
                return View();
            }
        }

        /// <summary>
        /// To Save the new Cars Details to DB
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Add(Products Model, string[] ProductsOptions)
        {


            var Msg = "";
            using (var _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var options = _service.GetAllProductOptions().Select(m => new SelectListItem { Text = m.OptionName, Value = m.id.ToString() }).ToList();
                if (ProductsOptions != null)
                    foreach (var item in options)
                    {
                        if (ProductsOptions.Contains(item.Value))
                            item.Selected = true;

                    }
                ViewData["Options"] = options;
                if (String.IsNullOrEmpty(Model.body))
                {

                    BindDropDowns(_service);
                    ViewData["Msg"] = "Please fill the required values.";
                    return View();
                }
                Boolean success;
                Model.customerId = _service.GetDealerByDealerEmail(User.Identity.Name).customerID;

                if (String.IsNullOrEmpty(Model.pics))
                {
                    Model.pics = "http://edriveautos.com/Content/Images/Dealer/photo-comming-soon.jpg";
                }
                
                var newProduct = _service.AddProduct(out success, Model, ref Msg, ProductsOptions != null ? ProductsOptions.ToList() : null);
                if (success)
                {

                    ViewData["Msg"] = "Product Added successfully";
                    SendCarfaxDealerVin(newProduct.vin, Model.customerId + "");
                }
                else
                {
                    BindDropDowns(_service);
                    ViewData["Msg"] = Msg;
                    return View(Model);
                }

                if (newProduct != null)
                    ViewData["ProductID"] = newProduct.productId;
                Model.productId = newProduct.productId;
                BindDropDowns(_service);

                return RedirectToActionPermanent("edit", new { Id = newProduct.productId, NewProduct = "Yes" });
                //View("edit", Model);
            }
        }

         private void SendCarfaxDealerVin(string vin, string customerId)
         {
             if (DateTime.UtcNow.Hour < 6 && DateTime.UtcNow.Hour > 12)
             {
                 return;
             }

             string dataToSend = "";

             dataToSend += vin + "|" + customerId;

             FileInfo file = new FileInfo(Server.MapPath(String.Format("~/edriveauto _cfxiicr_{0}.txt", DateTime.UtcNow.ToString("MMddyyyy"))));

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


           public void BindDropDowns(Edrive_ServiceClient _service, String Body = "", String MakeName = "")
        {

            var Product_types = _service.bindtypeAttributes().Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.type }).ToList(); ;
            //  Product_types.Insert(0, new SelectListItem { Text = "All", Value = "-1" });
            ViewData["Types"] = new SelectList(Product_types, "Value", "Text");

            //----------years
            List<SelectListItem> lstYear = new List<SelectListItem>();
            //lstYear.Add(new SelectListItem { Text = "All", Value = "-1" });
            for (int i = 1998; i <= DateTime.Now.Year; i++)
            {
                lstYear.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }
            ViewData["Years"] = new SelectList(lstYear, "Value", "Text"); ;
            //-------makes
            var makes = _service.BindMake().Select(m => new SelectListItem { Text = m.make, Value = m.id.ToString(), Selected = (m.make == MakeName) }).ToList();


            // makes.Insert(0, new SelectListItem { Value = "-1", Text = "All" });
            ViewData["Make"] = makes;// new SelectList(makes, "Value", "Text");

            //----------Body----------
            //var _bodyid = Body!=""? Convert.ToInt32(Body):0;
            var _bodyList = _service.BindBodyType();

            //Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.body }).ToList();
            //foreach (var item in _bodyList)
            //{
            //    if (item.Value == Body)
            //    {
            //        item.Selected = true;
            //    }
            //}

            //_bodyList.Insert(0, new SelectListItem { Text = "All", Value = "-1" });
            if (String.IsNullOrEmpty(Body) == false)
                ViewData["_body"] = Body;
            ViewData["Body"] = _bodyList;
            ViewData["CustomerID"] = _service.GetDealerByDealerEmail(User.Identity.Name).customerID;
        }


        //public void BindDropDowns(Edrive_ServiceClient _service, String Body = "", String MakeName = "")
        //{

        //    var Product_types = _service.bindtypeAttributes().Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.type }).ToList(); ;
        //    //  Product_types.Insert(0, new SelectListItem { Text = "All", Value = "-1" });
        //    ViewData["Types"] = new SelectList(Product_types, "Value", "Text");

        //    //----------years
        //    List<SelectListItem> lstYear = new List<SelectListItem>();
        //    //lstYear.Add(new SelectListItem { Text = "All", Value = "-1" });
        //    for (int i = 1998; i <= DateTime.Now.Year; i++)
        //    {
        //        lstYear.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
        //    }
        //    ViewData["Years"] = new SelectList(lstYear, "Value", "Text"); ;
        //    //-------makes
        //    var makes = _service.BindMake().Select(m => new SelectListItem { Text = m.make, Value = m.id.ToString(), Selected = (m.make == MakeName) }).ToList();


         
        //    ViewData["Make"] = makes;// new SelectList(makes, "Value", "Text");

        //    //----------Body----------
         
        //    var _bodyList = _service.BindBodyType();

          
        //    if (String.IsNullOrEmpty(Body) == false)
        //        ViewData["_body"] = Body;
        //    ViewData["Body"] = _bodyList;
        //    ViewData["CustomerID"] = _service.GetDealerByDealerEmail(User.Identity.Name).customerID;
        //}



        public JsonResult IsProductExist(String vin, String productid)
        {
            using (Edrivie_Service_Ref.Edrive_ServiceClient service = new Edrive_ServiceClient())
            {
                if (productid == "0")//----adding new Product
                {
                    var result = service.IsProductExist_by_VIN(vin);
                    return Json(!result, JsonRequestBehavior.AllowGet);

                }
                else//-------adding new Product
                {
                    var prdid = Convert.ToInt32(productid);
                    var result = service.IsProductExist_by_VIN_for_other_vehicle(vin, prdid);

                    return Json(!result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// To edit the changes of existing Cars
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="NewProduct"></param>
        /// <returns></returns>
        public ActionResult edit(Int32 Id, String NewProduct)
        {
            ViewData["Msg"] = TempData["Msg"];
            if(NewProduct!=null)
            if (NewProduct == "Yes")
            {
                ViewData["Msg"] = "Product Added successfully";

            }
            if (ModelState.IsValid == false)
                return View();

            var productId = Id;
            using (var _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var ProductsOptions = _service.GetAllProductOptions_By_ProductID(productId).Select(m => m.id.ToString()).ToArray();
                var options = _service.GetAllProductOptions().Select(m => new SelectListItem { Text = m.OptionName, Value = m.id.ToString() }).ToList();

                if (ProductsOptions != null)
                    foreach (var item in options)
                    {
                        if (ProductsOptions.Contains(item.Value))
                            item.Selected = true;

                    }

                ViewData["Options"] = options;
                ViewData["ProductID"] = productId;
                var Product = _service.GetProductByID(Id);
                ViewData["Pictures"] = _service.GetProductPicture_By_ProductID(productId);

                BindDropDowns(_service, Product.bodyID.ToString(), Product.Make);

                return View(Product);
            }

        }

        /// <summary>
        /// To save the changes to after editing the Cars details
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="ProductsOptions"></param>
        /// <param name="PictureURL"></param>
        /// <param name="ProductPictureID"></param>
        /// <param name="PictureDisplayOrder"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult edit(Products Model, string[] ProductsOptions, String[] PictureURL, String[] ProductPictureID, String[] PictureDisplayOrder, String Command, String Save)
        {
            ViewData["Msg"] = TempData["Msg"];
            var Msg = "";
            using (var _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                var options = _service.GetAllProductOptions().Select(m => new SelectListItem { Text = m.OptionName, Value = m.id.ToString() }).ToList();
                if (ProductsOptions != null)
                    foreach (var item in options)
                    {
                        if (ProductsOptions.Contains(item.Value))
                            item.Selected = true;

                    }
                ViewData["Options"] = options;
                //---upload pics
                if (Command.ToLower() == "upload" || Command.ToLower() == "AddUrl".ToLower())
                {
                    var Pictures = Admin.Controllers.ManageVehicleController.CollectPictureData(PictureURL, ProductPictureID, PictureDisplayOrder);
                    var Display_Order = 1;

                    if (Command.ToLower() == "AddUrl".ToLower())
                    {
                        Int32.TryParse(Request.Form["DisplayOrder2"], out Display_Order);
                        Pictures.Add(new Product_Picture { DisplayOrder = Display_Order, PictureURL = Request.Form["GetURL"], ProductPictureID = 0 });

                    }
                    else
                    {
                        if (String.IsNullOrEmpty(Request.Files["ProductPic"].FileName) == false)//upload image button click
                        {
                            Int32.TryParse(Request.Form["DisplayOrder1"], out Display_Order);

                            var file = Guid.NewGuid().ToString().Replace('-', '_') + Request.Files["ProductPic"].FileName;
                            var fileName = Server.MapPath("~/Content/Images/Product") + "\\" + file;
                            Request.Files["ProductPic"].SaveAs(fileName);
                            var Product_Pic_Url = Common_Methods.GetDomainUrl() + "Content/Images/Product/" + file;
                            Pictures.Add(new Product_Picture { DisplayOrder = 1, PictureURL = Product_Pic_Url, ProductPictureID = 0 });
                        }
                    }
                    ViewData["ImageUploaded"] = "yes";
                    ViewData["Msg"] = "Image Added successfully";
                    ViewData["Pictures"] = Pictures;
                    //---ends upload pics

                }
                else
                {
                    if (Command.ToLower() == "save")
                    {
                        //---update product
                        Boolean success;
                        var Pictures = Admin.Controllers.ManageVehicleController.CollectPictureData(PictureURL, ProductPictureID, PictureDisplayOrder);
                        ViewData["Pictures"] = Pictures;
                        Msg = "";
                        if (String.IsNullOrEmpty(Model.body))//--Mode- validation failse
                        {
                            ViewData["Msg"] = "Please fill the required values.";

                        }
                        else
                        {

                            var newProduct = _service.UpdateProduct(out success, Model, ref Msg, ProductsOptions != null ? ProductsOptions.ToList() : null);

                            if (success)
                            {
                                //--update imagees--
                                {
                                    if (_service.Update_Product_Picture(ref Pictures, out Msg, Model.productId) == false)//--failure
                                    {


                                        ViewData["Msg"] = "Updadating picture errors:" + Msg;
                                    }
                                    else
                                    {
                                        ViewData["Pictures"] = Pictures;//--udpate picture dictionary
                                        TempData["Msg"] = "Product Updated success fully";
                                        return RedirectToAction("edit", new { id = Model.productId });

                                    }
                                }

                               
                               
                            }
                            else
                            {
                                BindDropDowns(_service, Model.body, Model.Make);
                                ViewData["Msg"] = Msg;
                                return View(Model);
                            }

                        }

                    }
                    else
                    {
                        //if (Command.ToLower() == "delete")
                        //{
                          
                        //    {
                        //        List<String> lst = new List<string>();
                        //        lst.Add(Model.productId.ToString());
                        //        if (_service.DeleteProduct(lst))
                        //            return RedirectToAction("Manage");
                        //        else
                        //        {
                        //            ViewData["Msg"] = "Record Delete Failure";
                                  
                        //        }

                        //    }

                        //}
                    }

                }
                ViewData["ProductID"] = Model.productId;
                Model.productId = Model.productId;
                BindDropDowns(_service, Model.body, Model.Make);
                return View(Model);
            }
        }

        [HttpPost]

        public ActionResult DeleteVehicle(Products Model)
        {
            {
                using (var _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
                {
                    List<String> lst = new List<string>();
                    lst.Add(Model.productId.ToString());
                    if (_service.DeleteProduct(lst))
                    {
                        TempData["Msg"] = "Record Deleted Successfully";
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        TempData["Msg"] = "Record Delete Failure";
                        return RedirectToAction("edit", new { id = Model.productId });
                    }

                }

            }

        } 

        #endregion
        #region HotSheet
        /// <summary>
        /// This is for showing the Hot sheet report in Admins Section.
        /// </summary>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public ActionResult HotSheet(String Msg)
        {
            if (!String.IsNullOrEmpty(Msg))
            {
                ViewData["Msg"] = Msg;
            }


            var Zipcode = ""; Int32 CarsCount;
            using (Edrive_ServiceClient service = new Edrive_ServiceClient())
            {
                try
                {
                    //-----------------
                    Zipcode = service.GetDealerByDealerEmail(User.Identity.Name).Zip.ToString();//customer zip code
                    double NewLongitude, NewLatitude; double? latitude = null, longitude = null;

                   Admin.Controllers.ManageVehicleController.GetPoints(Convert.ToString(Zipcode), "100", out   NewLongitude, out   NewLatitude, ref   latitude, ref  longitude);
                    var makeID = -1;

                    List<Products> lstProduct = service.GetHotSheet(out CarsCount, latitude, longitude, NewLatitude, NewLongitude, makeID, Zipcode, 0, 50);

                    var obj = service.GetSellerCount_for_HotSheet(Zipcode, null, latitude, NewLatitude, longitude, NewLongitude);
                    ViewData["DealerCount"] = obj.Dealer;
                    ViewData["SellerCount"] = obj.Seller;

                    //-------------------
                    BindFilter(0, service, Zipcode, CarsCount, lstProduct);


                }


                catch (Exception ex)
                {
                    var lstMakes = service.BindMake().ToList();
                    lstMakes.Insert(0, new Product_Make { id = -1, make = "All" });
                    ViewData["Makes"] = new SelectList(lstMakes, dataTextField: "make", dataValueField: "id");
                    CarsCount = 0;
                    BindFilter(0, service, Zipcode, CarsCount, null);
                    ViewData["Msg"] = "Error: " + ex.Message;
                }
            }
            return View();

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="_SearchModel"></param>
        /// <param name="chkProduct">Products Selected</param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult HotSheet(Int32 PageIndex, _HotSheetFilter _SearchModel, String[] chkProduct, String Qualify)
        {
            using (Edrive_ServiceClient service = new Edrive_ServiceClient())
            {
                if (Qualify != null && chkProduct != null)//btn qualify is clicked
                {

                    if (!service.QualifyPriceofProduct(chkProduct.ToList()))
                    {
                        ViewData["Msg"] = "Error in Qualifying the Product";
                    }
                }

                //-----------------
                //var Zipcode = _SearchModel.Zip == null ? "" : _SearchModel.Zip.Value.ToString();//customer zip code
                String Zipcode;
                if (_SearchModel.Zip == null)
                {
                    Zipcode = ((service.GetDealerByDealerEmail(User.Identity.Name).Zip.ToString()));//customer zip code
                }
                else
                {
                    Zipcode = _SearchModel.Zip.Value.ToString();
                }


                double NewLongitude, NewLatitude; double? latitude = null, longitude = null;
                Admin.Controllers.ManageVehicleController.GetPoints(Convert.ToString(Zipcode), _SearchModel.Miles, out   NewLongitude, out   NewLatitude, ref   latitude, ref  longitude);


                Int32 CarsCount;

                List<Products> lstProduct = service.GetHotSheet(out CarsCount, latitude, longitude, NewLatitude, NewLongitude, _SearchModel.MakeID, Zipcode, PageIndex, 50);
                var obj = service.GetSellerCount_for_HotSheet(Zipcode, null, latitude, NewLatitude, longitude, NewLongitude);
                ViewData["DealerCount"] = obj.Dealer;
                ViewData["SellerCount"] = obj.Seller;
                //-------------------
                BindFilter(PageIndex, service, Zipcode, CarsCount, lstProduct);

                return View();
            }

        }

        private void BindFilter(Int32 PageIndex, Edrive_ServiceClient service, String Zipcode, Int32 CarsCount, List<Products> lstProduct)
        {
            var lstMakes = service.BindMake().ToList();
            lstMakes.Insert(0, new Product_Make { id = -1, make = "All" });
            ViewData["Makes"] = new SelectList(lstMakes, dataTextField: "make", dataValueField: "id");
            ViewData["Zipcode"] = Zipcode;
            ViewData["Products"] = lstProduct;
            ViewData["PageIndex"] = PageIndex;
            ViewData["CarsCount"] = CarsCount;
        }

        /// <summary>
        /// This method return the Dealer Info of specified Product Id for Hotsheet Section
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DealerInfo(Int32 id)
        {
            var productId = id;
            string stateName = string.Empty;
            var cust = _dealerService.GetDealerByProductID(productId);
			var state = _stateProvinceService.GetStatesByCountryCode("US");//  ---1-- for U.S.get state Name

			ViewData["ProductID"] = productId;
            ViewData["DealerName"] = cust.Name;
            ViewData["StreetAddress1"] = cust.StreetAddress1 ?? "";
            ViewData["Phone"] = cust.Phone ?? "";
			ViewData["StateID"] = new SelectList(state, "Name", "Name");
			ViewData["City"] = cust.City + (stateName) + cust.Zip.ToString();
            var product = _productService.GetProductByID(productId);
            ViewData["Subject"] = string.Format("{0} {1} {2} (VIN# {3} )", product.Year, product.Make, product.ModelName, product.vin);
            
			return View();
        }

        /// <summary>
        /// This Metod Send the mails to Dealer by admin about interested customer
        /// </summary>
        /// <param name="MailModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DealerInfo(_SendDealerEmail MailModel)
        {
            string Msg = "Thanks for Sending Message";
             
			return RedirectToAction("HotSheet", new { Msg = Msg });
        }
        
        /// <summary>
        /// This Method returns the Seller Info of specified productID for Hotsheet Section
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SellerInfo(Int32 id)
        {
            using (Edrive_ServiceClient service = new Edrive_ServiceClient())
            {
                return View(service.GetProductByID(id));
            }
        }
        #endregion
    }
}
