using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Logic.Interfaces;
using Edrive.Models;
using Products = Edrive.Edrivie_Service_Ref.Products;

namespace Edrive.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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
		
		#region "Actions"
        public ActionResult Index()
        {
            return View();
        }
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
                var newProduct = _service.AddProduct(out success, Model, ref Msg, ProductsOptions != null ? ProductsOptions.ToList() : null);
                if (success)
                {

                    ViewData["Msg"] = "Product Added successfully";
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


        /// <summary>
        /// Save the Images of newly added Cars
        /// </summary>
        /// <param name="prdId"></param>
        /// <param name="Model"></param>
        /// <param name="ProductsOptions"></param>
        /// <param name="PictureURL"></param>
        /// <param name="ProductPictureID"></param>
        /// <param name="PictureDisplayOrder"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult AddPics(String prdId, Products Model, string[] ProductsOptions
          , String[] PictureURL, String[] ProductPictureID, String[] PictureDisplayOrder)
        {


            if (prdId != null)
            {
                var ProductId = Convert.ToInt32(prdId);
                if (Request.Files["ProductPic"] != null && ProductId != 0)
                {
                    var file = Guid.NewGuid().ToString().Replace('-', '_') + Request.Files["ProductPic"].FileName;
                    var fileName = Server.MapPath("~/Content/Images/Product") + "\\" + file;

                    Request.Files["ProductPic"].SaveAs(fileName);
                    using (var service = new Edrive_ServiceClient())
                    {
                        //--add product's pics--- 
                        // service.AddProductPicture(ProductId, GetDomainUrl() + "/Content/Images/Product/" + fileName);
                        var options = service.GetAllProductOptions().Select(m => new SelectListItem { Text = m.OptionName, Value = m.id.ToString() }).ToList();
                        if (ProductsOptions != null)
                            foreach (var item in options)
                            {
                                if (ProductsOptions.Contains(item.Value))
                                    item.Selected = true;
                            }

                        ViewData["Options"] = options;
                        //-----------
                        var Pictures = new List<Product_Picture>();
                        var Product_Pic_Url = Common_Methods.GetDomainUrl() + "Content/Images/Product/" + fileName;
                        Pictures.Add(new Product_Picture { DisplayOrder = 1, PictureURL = Product_Pic_Url, ProductID = 0, ProductPictureID = 0 });
                        ViewData["Pictures"] = Pictures;
                        //-------------
                        //if (ViewData["Pictures"] != null)
                        //    ViewData["Pictures"] = ViewData["Pictures"] + ";" + Product_Pic_Url;
                        //else
                        //{
                        //    ViewData["Pictures"] = GetDomainUrl() + "/Content/Images/Product/" + file;
                        //}
                        BindDropDowns(service);
                        return View("Edit", Model);
                    }

                }

            }
            return View("Add");
        }


        /// <summary>
        /// To delete the existing Cars
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult Delete(Int32 id)
        {
            using (var _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
            {
                List<String> lst = new List<string>();
                lst.Add(id.ToString());
                if (_service.DeleteProduct(lst))
                    return RedirectToAction("Manage");
                else
                {
                    ViewData["Msg"] = "Record Delete Faiulre";
                    return RedirectToAction("edit", new { id = id });

                }

            }

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
                if (Command.ToLower() == "upload" || Command.ToLower()== "AddUrl".ToLower())
                {
                    var Pictures = CollectPictureData(PictureURL, ProductPictureID, PictureDisplayOrder);
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
                            var Product_Pic_Url =Common_Methods. GetDomainUrl() + "Content/Images/Product/" + file;
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
                        var Pictures = CollectPictureData(PictureURL, ProductPictureID, PictureDisplayOrder);
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
                        if (Command.ToLower() == "delete")
                        {
                            //using (var _service = new Edrivie_Service_Ref.Edrive_ServiceClient())
                            {
                                List<String> lst = new List<string>();
                                lst.Add(Model.productId.ToString());
                                if (_service.DeleteProduct(lst))
                                    return RedirectToAction("Manage");
                                else
                                {
                                    ViewData["Msg"] = "Record Delete Failure";
                                    //   return RedirectToAction("edit", new { id = Model.productId });

                                }

                            }

                        }
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

        /// <summary>
        /// This upload the csv file of vehicles on server and insert records from csv file to Database on success.
        /// </summary>
        /// <param name="csvFile"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult UploadVehicle(String csvFile)
        {
            if (Request.Files["csvFile"].FileName == "" )
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

       /// <summary>
        /// it return the new page for custom paging
        /// </summary>
        /// <param name="Vin"></param>
        /// <param name="Stock"></param>
        /// <param name="Make"></param>
        /// <param name="DealerName"></param>
        /// <param name="Featured"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult NewPage(String Vin,
            String Stock, String Make, string DealerName, bool Featured, Int32 id)
        {
            var _MakeID = -1;
            if (!string.IsNullOrEmpty(Make))
            {
                _MakeID = Convert.ToInt32(Make);

            }
            var ComapnyName = DealerName;
            //if (!string.IsNullOrEmpty(DealerName))
            //{
            //    _DealerID = Convert.ToInt32(DealerName);

            //}
            using (var servicae = new Edrive_ServiceClient())
            {
                Int32 pageIndex = id;
                var lstProducts = SearchVehicle(Vin, Stock, _MakeID, ComapnyName, Featured, pageIndex, 50);
                var DealerEmail = User.Identity.Name;
                var RoleName = "Admin";
                if (User.IsInRole("Dealer"))
                    RoleName = "Dealer";
                ViewData["CarsCount"] = servicae.Get_Count_SearchProduct_for_ManageProduct(Vin, Stock, _MakeID, ComapnyName, DealerEmail, RoleName, Featured);

                return View(lstProducts);
            }


        }

        /// <summary>
        /// for uplading vehicle it retunr the view just
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadVehicle()
        {

            return View();

        }


        /// <summary>
        /// This method is for manage vehicle section
        /// </summary>
        /// <returns></returns>
     
       virtual public ActionResult Manage()
        {
            //if (User.IsInRole("Dealer"))
            //{
            //    return RedirectToAction("Manage", "ManageVehicle", new { area = "Dealer" });
            //}
            ViewData["Msg"] = TempData["Msg"];
            using (var servicae = new Edrive_ServiceClient())
            {
                Bind_ManageProduct(servicae);
                //var CarsCount = 0;

                var lstProducts = SearchVehicle("", "", -1,"", false, 0, 50);// servicae.SearchProduct_for_ManageProduct("", "", -1, -1, false, 0, 50, ref CarsCount);
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
                            ViewData["CarsCount"] = service.Get_Count_SearchProduct_for_ManageProduct(Vin, Stock, _MakeID, _DealerID,DealerEmail,RoleName, Featured);
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
        /// check the Validation price
        /// </summary>
        /// <param name="VIN"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        private decimal ValidRecord(String VIN, Decimal price)
        {
            NADA_UsedCars.UsedCars oUsedCarService = new NADA_UsedCars.UsedCars();
            NADA_UsedCars.UsedCarResultSet oUsedCarsResultSet = oUsedCarService.GetUsedCars("EdriveAutos", "ed12uc20", VIN.ToString());
            UsedCarPrices.pricing oUsedCarPrices = new UsedCarPrices.pricing();
            decimal qualifyPrice = 0;
            if (oUsedCarsResultSet.UsedCars != null)
            {
                foreach (NADA_UsedCars.UsedCar uc in oUsedCarsResultSet.UsedCars)
                {
                    //Decimal Five_PPrice = Convert.ToDecimal((uc.AverageTradeinPrice) * 0.15);
                    //Decimal newAverageTradeInPrice = Convert.ToDecimal(uc.AverageTradeinPrice) + Five_PPrice;
                    Decimal newAverageTradeInPrice = Convert.ToDecimal(uc.AverageTradeinPrice);
                    if (newAverageTradeInPrice != 0) //Nirav - 28/04/11 - to import all data for demo purpose.
                    {
                        qualifyPrice = price - newAverageTradeInPrice;
                        return qualifyPrice;
                        break;
                    }
                }

            }
            return qualifyPrice;
        }

        public JsonResult IsProductExist(String vin,String productid)
        {
            using (Edrivie_Service_Ref.Edrive_ServiceClient service = new Edrive_ServiceClient())
            {
                if (productid == "0")//----adding new Product
                {
                   var result= service.IsProductExist_by_VIN(vin);
                   return Json(!result, JsonRequestBehavior.AllowGet);
 
                }
                else//-------adding new Product
                {
                    var prdid = Convert.ToInt32(productid);
                    var result = service.IsProductExist_by_VIN_for_other_vehicle(vin,prdid);
 
                    return Json(!result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
 
        }


        /// <summary>
        /// This is for showing the Hot sheet report in Admins Section.
        /// </summary>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public ActionResult HotSheet(String Msg)
        {
            //if (User.IsInRole("Dealer"))
            //{
            //    return RedirectToAction("HotSheet", "ManageVehicle", new { area = "Dealer" });
            //}
            if (!String.IsNullOrEmpty(Msg))
            {
                ViewData["Msg"] = Msg;
            }
            

                 var Zipcode =""; Int32 CarsCount;
            using (Edrive_ServiceClient service = new Edrive_ServiceClient())
            {
                try
            {
                //-----------------
                  Zipcode = service.GetDealerByDealerEmail(User.Identity.Name).Zip.ToString();//customer zip code
                double NewLongitude, NewLatitude; double? latitude = null, longitude = null;

                GetPoints(Convert.ToString(Zipcode), "100", out   NewLongitude, out   NewLatitude, ref   latitude, ref  longitude);
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
                CarsCount=0;
                BindFilter(0,service,Zipcode,CarsCount  , null);
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
                GetPoints(Convert.ToString(Zipcode), _SearchModel.Miles, out   NewLongitude, out   NewLatitude, ref   latitude, ref  longitude);


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

            using (Edrive_ServiceClient service = new Edrive_ServiceClient())
            {
                string[] strEnquiry = new string[8];
                var product = service.GetProductByID(MailModel._prdID);
                if (product.customerId != 0)
                {
                    var cust = service.GetDealerbyProductID(product.productId);

                    if (cust != null)
                    {
                        strEnquiry[0] = cust.email;
                    }
                }
                //else
                //{
                //    strEnquiry[0] = product.SellerEmail;
                //}
                if (MailModel.Name != string.Empty)
                {
                    strEnquiry[1] = MailModel.Name;
                }
                if (MailModel.Email != string.Empty)
                {
                    strEnquiry[2] = MailModel.Email;
                }
                if (MailModel.Phone != string.Empty)
                {
                    strEnquiry[3] = MailModel.Phone;
                }
                if (MailModel.City != string.Empty)
                {
                    strEnquiry[4] = MailModel.City;
                }
                strEnquiry[5] = MailModel.StateID;

                if (MailModel.Subject != string.Empty)
                {
                    strEnquiry[6] = MailModel.Subject;
                }
                if (MailModel.Comments != string.Empty)
                {
                    strEnquiry[7] = MailModel.Comments;
                }

                SendMessageFromAdminToDealer(strEnquiry, MailModel);
                var Msg = "Thanks for Sending Message";

                return RedirectToAction("HotSheet", new { Msg = Msg });



            }
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



        #region "Methods"
       
        
        /// <summary>
        /// It collects the temporary pictures added by the users details and the return these list of image to controller
        /// </summary>
        /// <param name="PictureURL"></param>
        /// <param name="ProductPictureID"></param>
        /// <param name="PictureDisplayOrder"></param>
        /// <returns></returns>
        public static List<Product_Picture> CollectPictureData(String[] PictureURL, String[] ProductPictureID, String[] PictureDisplayOrder)
        {
            var Pictures = new List<Product_Picture>();

            if (PictureDisplayOrder != null && PictureURL != null && ProductPictureID != null)
            {
                for (int i = 0; i < PictureURL.Length; i++)
                {
                    Pictures.Add(new Product_Picture
                    {
                        DisplayOrder = Convert.ToInt32(PictureDisplayOrder[i]),
                        PictureURL = PictureURL[i],
                        ProductPictureID = Convert.ToInt32(ProductPictureID[i])
                    });
                }
            }
            return Pictures;
        }
        /// <summary>
        /// For manage product it binds the filters
        /// </summary>
        /// <param name="service"></param>
        /// <param name="MakeID"></param>
        /// <param name="DealerID"></param>

          public  void Bind_ManageProduct(Edrive_ServiceClient service, Int32 MakeID = -1, String DealerID="")
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
            var dealers =new  List<Edrivie_Service_Ref.Customer>();
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
        /// <summary>
        /// Email code for Sending Mail to Dealer
        /// </summary>
        /// <param name="strEnquiry"></param>
        /// <param name="malModel"></param>
        /// <returns></returns>
        public static bool SendMessageFromAdminToDealer(String[] strEnquiry, _SendDealerEmail malModel)
        {
            using (var _edriveautoweb_Entity = new eDriveAutoWebEntities())
            {

                var localizedMessageTemplate = _edriveautoweb_Entity.MessageTemplateLocalized.First(m => m.MessageTemplate.Name == "EDrive.MessageFromAdminTodealer");
                if (localizedMessageTemplate == null)
                    return false;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

                string subject = ReplaceMessageTemplateTokensForDealer(malModel, localizedMessageTemplate.Subject);
                string body = ReplaceMessageTemplateTokensForDealer(malModel, localizedMessageTemplate.Body);
                var AdminEmailAddress = _edriveautoweb_Entity.Settings.First(m => m.Name == "Email.AdminEmailAddress").Value;
                //string cc = AdminEmailAddress; //uncomment this line for live site
                string bcc = localizedMessageTemplate.BCCEmailAddresses;
                var from = new MailAddress(malModel.Email, malModel.Email);

                //var to = new MailAddress(strEnquiry[0], strEnquiry[0]);//uncomment this line for live site


                var to = new MailAddress("support@edriveautos.com");//this line for development
                String cc = System.Configuration.ConfigurationManager.AppSettings["CC"].ToString();

                System.Net.Mail.SmtpClient obj = new System.Net.Mail.SmtpClient();
                System.Net.Mail.MailMessage Mailmsg = new System.Net.Mail.MailMessage();
                Mailmsg.To.Clear();


                Mailmsg.To.Add(to);
                Mailmsg.From = (from);
                 Mailmsg.CC.Add(cc);
                Mailmsg.Subject = subject;


                Mailmsg.Body = body;
                Mailmsg.IsBodyHtml = true;
                obj.Send(Mailmsg);
                // var queuedEmail = InsertQueuedEmail(5, from, to, cc, bcc, subject, body, DateTime.Now, 0, null);

                return true;

            }
        }
        /// <summary>
        /// For Getting new lattitude and longitude info against the Zip n selected values
        /// </summary>
        /// <param name="strZip"></param>
        /// <param name="SelectedMile"></param>
        /// <param name="NewLongitude"></param>
        /// <param name="NewLatitude"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        static public void GetPoints(string strZip, String SelectedMile, out double NewLongitude, out double NewLatitude, ref  double? latitude, ref  double? longitude)
        {
            using (Edrive_ServiceClient service = new Edrive_ServiceClient())
            {
                var ed_zipDetails = service.GetED_ZipCodes(strZip);

                if (ed_zipDetails != null)
                {
                    latitude = ed_zipDetails.latitude;
                    longitude = ed_zipDetails.longitude;
                }

                if (latitude != null && longitude != null)
                {
                    if (SelectedMile == "1000")// All MiLies is selected //Nirav - 19th July, 2011 - If user selects "All Miles", then it is as good as he didnt enter any Zip code.
                    {
                        latitude = 0;
                        longitude = 0;
                        NewLatitude = 0;
                        NewLongitude = 0;
                    }
                    else if (SelectedMile == "select")//Nirav - 19th July, 2011 - If user selects "[Select]", then it is as good as he wants to search only for the entered zip code.
                    {
                        NewLatitude = latitude.Value;
                        NewLongitude = longitude.Value;
                    }
                    else
                    {
                        CalculateNewPoints(latitude.Value, Convert.ToDouble(longitude), Convert.ToDouble(SelectedMile), out   NewLongitude, out   NewLatitude);

                    }
                }
                else
                    NewLatitude = NewLongitude = 0;
            }
        }

        /// <summary>
        /// Calculates the New  lattitude and longitude info against the Zip
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="miles"></param>
        /// <param name="NewLongitude"></param>
        /// <param name="NewLatitude"></param>
        static public   void CalculateNewPoints(double latitude, double longitude, double miles, out double NewLongitude, out double NewLatitude)
        {
            //For Calculating New Latitude
            double newlat;
            newlat = latitude + ((miles / 3960) * (180 / 3.14));
            NewLatitude = newlat;
            //For Calculating New Longitude
            double newlong;
            int longit = (int)longitude;
            newlong = longitude - (miles / (3960 * Math.Cos(longit * (Math.PI / 180)))) * (180 / Math.PI);
            NewLongitude = newlong;
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
        private static string ReplaceMessageTemplateTokensForDealer(_SendDealerEmail strEnquiry, string p)
        {
            if (p.Contains("%Message.Name%"))
                p = p.Replace("%Message.Name%", strEnquiry.Name);
            if (p.Contains("%Message.Email%"))
                p = p.Replace("%Message.Email%", strEnquiry.Email);
            if (p.Contains("%Message.Phone%"))
                p = p.Replace("%Message.Phone%", strEnquiry.Phone);
            if (p.Contains("%Message.City%"))
                p = p.Replace("%Message.City%", strEnquiry.City);
            if (p.Contains("%Message.State%"))
                p = p.Replace("%Message.State%", strEnquiry.StateID);
            if (p.Contains("%Message.Comments%"))
                p = p.Replace("%Message.Comments%", strEnquiry.Comments);
            if (p.Contains("%Message.Subject%"))
                p = p.Replace("%Message.Subject%", strEnquiry.Subject);



            return p;

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
        //protected void BindData(string zipcode, string make, double lat1, double lat2, double long1, double long2)
        //{
        //    using (Edrive_ServiceClient service = new Edrive_ServiceClient())
        //    {
        //        //DataTable dtData = new DataTable();
        //        //SqlParameter[] p = new SqlParameter[6];
        //        //p[0] = new SqlParameter("@ZipCode", zipcode);
        //        //p[1] = new SqlParameter("@Make", make);
        //        //p[2] = new SqlParameter("@Lat1", lat1);
        //        //p[3] = new SqlParameter("@Lat2", lat2);
        //        //p[4] = new SqlParameter("@Long1", long1);
        //        //p[5] = new SqlParameter("@Long2", long2);
        //        //dtData = this.GetDataWithParameter("Nop_ProductLoadAllForHotsheetNew", p, ref dtData);
        //        //return dtData;

        //    }
        //}
  
        #endregion

    
      
        
     
     
        public ActionResult ManageLeads()
        {
            return View();
        }
      
        


       

        //[HttpPost]
        //public ActionResult SellerInfo(Int32 id, _SendDealerEmail mailModel)
        //{
        //    using (Edrive_ServiceClient service = new Edrive_ServiceClient())
        //    {
        //        return View(service.GetProductByID(id));
        //    }
        //}

       
        }

}
