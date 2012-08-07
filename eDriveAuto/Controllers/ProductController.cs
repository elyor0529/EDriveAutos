using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Logic;
using Edrive.Logic.Interfaces;
using Edrive.Models;

using Edrive.NADA_UsedCars;
using Products = Edrive.Edrivie_Service_Ref.Products;

namespace Edrive.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

    	private IProductService _productService;
		private IProductPictureService _productPictureService;
    	private IDealerService _dealerService;
		private IInterestedCustomerService _interestedCustomerService;

		public ProductController(IProductService productService, 
			IProductPictureService productPictureService, 
			IDealerService dealerService,
			IInterestedCustomerService interestedCustomerService)
		{
			_productService = productService;
			_productPictureService = productPictureService;
			_dealerService = dealerService;
			_interestedCustomerService = interestedCustomerService;
		}

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Show the Product Details of the select Product
        /// created by harpreet singh on 16-02-2012
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult ProductDetail(Int32 Id)
        {
            //for SEO add these
            ViewData["description"] = "";
            ViewData["keywords"] = "";
			
			ViewData["Msg"] = TempData["Msg"];
            Core.Model.Products product = _productService.GetProductByID(Id);
            //#region NADA_WebService

            var dealer = _dealerService.GetDealerByProductID(product.productId);
            string address = String.Empty;
			if(dealer != null)
			{
				address = dealer.City + ", " + dealer.StateName + " ";
				ViewData["DealerDetails"] = _productService.GetProductsByDealerID(dealer.customerID);
			}
            ViewData["GetDealer"] = dealer;
            ViewData["ProductId"] = product.productId;
			ViewData["metatitle"] = string.Format("{0} {1} {2} {3} {4}", product.Year, product.MakeName, product.ModelName, address, Common_Methods.GetZip(product.zip));

            ViewData["vin"] = product.vin;
            ViewData["year"] = product.Year;
            ViewData["make"] = product.Make;
            ViewData["model"] = product.ModelName;
            ViewData["body"] = product.body;
            Int32 CountDownDays;
            CountDownDays = GetCountDownDays();
            ViewData["CountDownDays"] = CountDownDays;
            string logo = _dealerService.GetProfile(dealer.customerID).Logo;
            if (String.IsNullOrEmpty(logo) == false)
            {
                ViewData["ProfileLogo"] = logo;
            }

			return View(product);
        }




        [HttpPost]
        public ActionResult AddRating(Int32 id, Int32 score)
        {
        	_productService.AddProductRating(id, score, User.Identity.Name);
            
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Redirect to Thanku Page afetr submitting Intrested customer details
        /// created by harpreet singh on 16-02-2012
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ThankYou(int id)
        {
            int productId = id;
            var product = _productService.GetProductByID(productId);
			var defaultImage = _productPictureService.GetProductPicture_By_ProductID(productId).OrderBy(m => m.DisplayOrder);
			var defaultImageUrl = String.Empty;
				
			if(defaultImage.Any())
				defaultImageUrl = defaultImage.First().PictureURL;
				
            ViewData["DefImage"] = defaultImageUrl;
			ViewData["ThankDealDetails"] = _dealerService.GetDealerByProductID(productId);
                
			return View(product);
        }


        //<summary>
        /// Add intrested Customer to DB and Email to Dealer
        /// created by harpreet singh on 16-02-2012
        /// </summary>
        /// <param name="_intCustomer"></param>
        /// <param name="hidProductId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IntrestedCustomer(Core.Model.IntrestedCustomer _intCustomer, String hidProductId, String[] InterestedType)
        {
			Core.Model.IntrestedCustomer intrestedCustomer = new Core.Model.IntrestedCustomer();
            try
            {
                int productid = Convert.ToInt32(hidProductId);
                var productdetail = _productService.GetProductByID(productid);
                var customeDetails = _dealerService.GetDealerByProductID(productid);
                int customerid = customeDetails.customerID;
                intrestedCustomer.firstname = _intCustomer.firstname;
                intrestedCustomer.lastname = _intCustomer.lastname;
                _intCustomer.email = intrestedCustomer.email = _intCustomer.email ?? "";
                _intCustomer.phoneno = intrestedCustomer.phoneno = _intCustomer.phoneno ?? "";
                _intCustomer.additionalComments = intrestedCustomer.additionalComments = _intCustomer.additionalComments ?? "";
                intrestedCustomer.customerId = customerid;
                intrestedCustomer.contactType = _intCustomer.contactType;
                intrestedCustomer.intrestType = 1;
                intrestedCustomer.createdOn = DateTime.Now;
                intrestedCustomer.updateOn = DateTime.Now;

				if(InterestedType != null)
					foreach (var item in InterestedType)
					{
						switch (item)
						{
							case "financing": intrestedCustomer.finacing = true;
								break;
							case "trade-in": intrestedCustomer.Trade_in = true;
								break;
						}
					}

                bool result = _interestedCustomerService.IntrestedCustomer(intrestedCustomer, productid);
                
				if (result)
                {
                    string[] strEnquiry = new string[15];
                    strEnquiry[0] = _intCustomer.email;
                    if (customeDetails != null)
                    {
                        strEnquiry[1] = customeDetails.email;
                        strEnquiry[2] = productdetail.Year + " " + productdetail.Make + " " + productdetail.model + " " + productdetail.body;
                        strEnquiry[3] = _intCustomer.phoneno;
                        if (_intCustomer.contactType == "Email")
                        {
                            strEnquiry[5] = "email";
                        }
                        if (_intCustomer.contactType == "Phone")
                        {
                            strEnquiry[5] = "phone";
                        }
                        if (!string.IsNullOrEmpty(_intCustomer.additionalComments))
                        {
                            strEnquiry[6] = _intCustomer.additionalComments;
                        }
                        else
                        {
                            strEnquiry[6] = "None";
                        }
                        strEnquiry[7] = "No";
                        strEnquiry[10] = "No";
                        strEnquiry[13] = "No";
                        strEnquiry[8] = _intCustomer.firstname;
                        strEnquiry[9] = _intCustomer.lastname;
                        if (customeDetails != null)
                        {
                            strEnquiry[11] = customeDetails.FirstName + "" + customeDetails.LastName;
                        }
                        if (productdetail != null)
                        {
                            strEnquiry[12] = productdetail.vin;
                        }
                        MessageManager.SendEnquiryToDealer(strEnquiry, 1);
                    }
                }

				Session.Remove("ClientInfo");
				Session["ClientInfo"] = intrestedCustomer;

                return RedirectToAction("ThankYou", new { id = productid });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// Add Wish List
        /// created by Harpreet singh on 20-02-2012
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SaveWistList(Int32 id)
        {
            try
            {
                AddUsersWishList(id);
				
                TempData["Msg"] = "Wishlist added successfully";
                RedirectToAction("ProductDetail", new { id = id });
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "error:" + ex.Message;

            }
            return RedirectToAction("ProductDetail", new { id = id });
        }

        private void AddUsersWishList(Int32 ProductID)
        {
            using (eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
            {
                var CustomerType = "";
                Int32 CustomerID = 0;

                if (User.IsInRole("Customer"))
                {
                    CustomerType = "Customer";
                    CustomerID = entity.Customer.First(m => m.Email == User.Identity.Name).CustomerID;
                }
                if (User.IsInRole("Admin"))
                {
                    CustomerType = "Admin";
                    CustomerID = _dealerService.GetDealerByDealerEmail(User.Identity.Name).customerID;

                }
                if (User.IsInRole("Dealer"))
                {
                    CustomerType = "Dealer";
                    CustomerID = _dealerService.GetDealerByDealerEmail(User.Identity.Name).customerID;

                }

                // if product is not already added in wishlist
                if (entity.WishList.Any(m => m.CustomerID == CustomerID && m.CustomerType == CustomerType && m.ProductID == ProductID) == false)
                {
                    entity.WishList.AddObject(new WishList { CustomerID = CustomerID, CustomerType = CustomerType, ProductID = ProductID });
                    entity.SaveChanges();
                }
            }
        }
		
        public ActionResult SavedWishList(Int32 CustomerId, String CustomerType)
        {
            using (eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
            {
            	List<Int32> productIDs = entity.WishList
					.Where(m => m.CustomerID == CustomerId && m.CustomerType == CustomerType)
					.Where(m => m.ProductID != null).Select(m => m.ProductID.Value).ToList();
                
                if (productIDs.Count > 0)
                {
                    var products = _productService.GetProductsByIDs(productIDs);
                        
					return View(products);
                }
                else
                {
                    return View();
                }
            }
        }
		
        public ActionResult SaveWistListProduct(Int32 id, string comp)
        {
            try
            {
                AddUsersWishList(id);
				
                TempData["Msg"] = "Wishlist added successfully";
                return RedirectToAction("CompareVehicle", new { id = comp });
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "error:" + ex.Message;

            }
            return RedirectToAction("CompareVehicle", new { id = comp });
        }
		
        /// <summary>
        /// Show Wish list
        /// created by Harpreet singh on 20-02-2012
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize]
        public ActionResult Wishlist()
        {
            String CustomerGuid = "";
            Int32 CountDownDays;
            CountDownDays = GetCountDownDays();
            ViewData["CountDownDays"] = CountDownDays;

            try
            {
                using (eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
                {
                    var customerType = "";
                    int customerID = 0;

                    if (User.IsInRole("Customer"))
                    {
                        customerType = "Customer";
                    }
                    if (User.IsInRole("Admin"))
                    {
                        customerType = "Admin";
                    }
                    if (User.IsInRole("Dealer"))
                    {
                        customerType = "Dealer";
                    }

                    if (User.IsInRole("Dealer") || User.IsInRole("Admin"))
                    {
                        var cust = _dealerService.GetDealerByDealerEmail(User.Identity.Name);
                        customerID = cust.customerID;
                    }
                    else
                    {
                        if (User.IsInRole("Customer"))
                        {
                            var cust = entity.Customer.FirstOrDefault(m => m.Email == User.Identity.Name);
                            customerID = cust.CustomerID;
                        }
                    }
                    ViewData["WishListUrl"] = customerType + "/" + customerID.ToString();
                    List<Int32> productIDs = entity.WishList.Where(m => m.CustomerID == customerID && m.CustomerType == customerType)
						.Where(m => m.ProductID != null).Select(m => m.ProductID.Value).ToList();
                    
                    if (productIDs.Any())
                    {
                    	var products = _productService.GetProductsByIDs(productIDs);
                        
						return View(products);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return View();
        }

        private static Int32 GetCountDownDays()
        {
            Int32 CountDownDays = 0;
            using (eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
            {
                var Set = entity.Settings.FirstOrDefault(m => m.Name == "CountDownDays");
                if (Set != null)
                {
                    CountDownDays = Convert.ToInt32(Set.Value);
                }
            }
            return CountDownDays;
        }
        /// <summary>
        /// Remove from Wish list
        /// created by Harpreet singh on 20-02-2012
        /// </summary>
        /// <param name="chkproductId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Wishlist(string[] chkproductId, String Delete)
        {
            if (chkproductId == null)
            {
                return Wishlist();
            }
            try
            {
                if (Delete != null)
                {
                    using (eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
                    {
                        var customerType = "";
                        Int32 customerID = 0;

                        if (User.IsInRole("Customer"))
                        {
                            customerType = "Customer";
                        }
                        if (User.IsInRole("Admin"))
                        {
                            customerType = "Admin";
                        }
                        if (User.IsInRole("Dealer"))
                        {
                            customerType = "Dealer";
                        }

                        if (User.IsInRole("Dealer") || User.IsInRole("Admin"))
                        {
                            customerID = _dealerService.GetDealerByDealerEmail(User.Identity.Name).customerID;
                        }
                        else
                        {
                            if (User.IsInRole("Customer"))
                            {
                                customerID = entity.Customer.FirstOrDefault(m => m.Email == User.Identity.Name).CustomerID;
                            }
                        }
                        int[] prdIds = new int[chkproductId.Length];
                        for (int i = 0; i < chkproductId.Length; i++)
                        {
                            prdIds[i] = Convert.ToInt32(chkproductId[i]);
                        }

                        var delWishlist = entity.WishList.Where(m => m.CustomerID == customerID && m.CustomerType == customerType && prdIds.Contains(m.ProductID ?? 0)).Select(m => m);
                        foreach (var item in delWishlist)
                        {
                            entity.WishList.DeleteObject(item);
                        }
                        entity.SaveChanges();
                    }
					
                    return Wishlist();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Wishlist();
        }


        /// <summary>  
        /// To save the WislhList
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult AddWistList(Int32 id)
        {
            try
            {

                AddUsersWishList(id);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }

        }

        /// <summary>
        /// This methd lock the price for vehicle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PriceLock(int id)
        {
            int productID = id;
            
            var product = _productService.GetProductByID(productID);
            var customer = _dealerService.GetByID(product.customerId);
        	_LockPrice model = new _LockPrice
        	                   	{
        	                   		DealerName = customer.Name,
        	                   		MakeName = product.MakeName,
        	                   		ModelName = product.ModelName,
        	                   		price_Current = product.price_Current,
        	                   		ProductID = productID,
        	                   		vin = product.vin,
        	                   		zip = product.zip
        	                   	};
            
			return View(model);
        }

        /// <summary>
        /// This methd lock the price for vehicle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PriceLock(_LockPrice model)
        {
            var product = _productService.GetProductByID(model.ProductID);
            var customer = _dealerService.GetByID(product.customerId);
                
			model.companyName = customer.CompanyName;
            model.dealerCity = customer.City;
            model.DealerName = customer.Name;
            model.DealerPhone = customer.Phone;
            model.Exterior = product.exterior;
            model.Engine = product.engine;
            model.Interior = product.interior;
            model.MakeName = product.MakeName;
            model.Mileage = product.mileage;
            model.ModelName = product.ModelName;
            model.price_Current = product.price_Current;
            model.ProductID = product.productId;
            model.Stock = product.stock;
            model.streetAddress = customer.StreetAddress1;
            model.Transmission = product.transmission;
            model.Type = product.VehicleType;
            model.vin = product.vin;
            model.Warranty = product.warranty;
            model.Year = product.Year;
            model.zip = customer.Zip;

            return View("PriceGuarantee", model);
        }

        /// <summary>
        /// This method show the page for selling users cars.
        /// </summary>
        /// <returns></returns>
        public ActionResult SellYourCar()
        {
            return View();
        }

        /// <summary>
        /// This method accept the Car's details to be sold and then show car's details using NADA Service on the next page where seller add his own personal credential as the owner of vehicle.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SellYourCar(_SellCar model)
        {
            try
            {
                if (_productService.IsProductExists(model.VIN))
                {
                    ViewData["Msg"] = "This VIN already exist.";
                    return View();
                }

                UsedCars oUsedCarService = new UsedCars();
                UsedCarResultSet oUsedCarsResultSet = oUsedCarService.GetUsedCars("EdriveAutos", "ed12uc20", model.VIN);
				
                if (oUsedCarsResultSet.UsedCars == null)
                {
                    ViewData["Msg"] = "Invalid VIN. Please enter correct VIN.";
                    return View();
                }
                else
                {
                    TempData["_SellCar"] = model;
                    try
                    {
                        bindData(model);
                    }
                    catch (Exception ex)
                    {
                        ViewData["Msg"] = "error:" + ex.Message;
                    }
                    ViewData["vin"] = model.VIN;
                    return View("SellYourCarDetail");
                }
            }
            catch (Exception ex)
            {

                ViewData["Msg"] = ex.Message;
                return View();
            }

        }
        /// <summary>
        /// This method add the neccessary filter to bind the page.
        /// </summary>
        /// <param name="model"></param>

        protected void bindData(_SellCar model)
        {
            try
            {
                string VIN = model.VIN;
                NADA_UsedCars.UsedCars oUsedCarService = new NADA_UsedCars.UsedCars();
                NADA_UsedCars.UsedCarResultSet oUsedCarsResultSet = oUsedCarService.GetUsedCars("EdriveAutos", "ed12uc20", VIN);
                UsedCarPrices.UsedCarPrices oUsedCarPrices = new UsedCarPrices.UsedCarPrices();
                UsedCarPrices.MileageAdjustment oUsedCarMileage = new UsedCarPrices.MileageAdjustment();

                foreach (NADA_UsedCars.UsedCar uc in oUsedCarsResultSet.UsedCars)
                {
                    ViewData["ProductName"] = Convert.ToString(uc.Year) + " " + uc.MakeDisplay + " " + uc.ModelDisplay;
                    ViewData["lblStyleName"] = uc.TrimDisplay;

                    Decimal price = Convert.ToDecimal(uc.MSRP);
                    string nPrice = String.Format("{0:0,0}", price);
                    ViewData["lblMSRP"] = "$" + nPrice;

                    Decimal priceOne = Convert.ToDecimal(uc.AverageRetailPrice);
                    string nPriceOne = String.Format("{0:0,0}", priceOne);
                    ViewData["lblAvgRetail"] = "$" + nPriceOne;

                    Decimal priceTwo = Convert.ToDecimal(uc.AverageTradeinPrice);
                    string nPriceTwo = String.Format("{0:0,0}", priceTwo);
                    ViewData["lblAvgTrade"] = "$" + nPriceTwo;

                    ViewData["zip"] = model.Zip;
                    ViewData["Mileage"] = model.Mileage;

                    ViewData["UsedCarOptions"] = uc.UsedCarOptions;
                    ViewData["Condition"] = model.Condition;

                    break;
                }
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }


        public ActionResult SellYourCarDetails()
        {
            return RedirectToAction("SellYourCar");
        }

        /// <summary>
        /// This method add the Users product to be sold in the product inventory.
        /// </summary>
        /// <param name="MODEL"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SellYourCarDetails(_SellYourCarDetails MODEL)
        {
            try
            {
                using (Edrive_ServiceClient service = new Edrive_ServiceClient())
                {
                    String msg = "";
                    Boolean Offer = false;
                    Boolean.TryParse(MODEL.Offer, out Offer);
                    if (service.AddProductUsingNadaService(MODEL.VIN, MODEL.Condition, MODEL.Mileage, MODEL.zip, MODEL.Email, MODEL.Name, MODEL.Notes, MODEL.Phone, Offer, ref msg))
                    {
                        ViewData["Msg"] = "Your vehicle is registered successfully with E-Drive Autos, LLC.";
                    }
                    else// show  the error Message
                    {
                        ViewData["Msg"] = msg;
                    }
                }

            }
            catch (Exception ex)
            {
                ViewData["Msg"] = ex.Message;
            }
            bindData(new _SellCar { Condition = MODEL.Condition, Mileage = MODEL.Mileage, VIN = MODEL.VIN, Zip = MODEL.zip });
            return View("SellYourCarDetail");

        }

        [Authorize]
        public ActionResult CompareVehicle(String id)
        {
            ViewData["CompareID"] = id;
            ViewData["Msg"] = TempData["Msg"];
            if (String.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                string[] ids = id.Split('_');
				List<Core.Model.Products> lstPrd = new List<Core.Model.Products>();
                for (int i = 0; i < ids.Length; i++)
                {
                    if (string.IsNullOrEmpty(ids[i]) == false)
                    {
                        lstPrd.Add(_productService.GetProductByID(Convert.ToInt32(ids[i])));
                    }
                }
                return View(lstPrd);

            }
        }


        public ActionResult BackButton()
        {


            var srchSession = Session["SearchType"] as SearchSession;
            if (srchSession == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (srchSession.prpSearchType == SearchType.HomePageSearch)
            {
                return RedirectToAction("Index", "Search", new
                                                           	{
                                                           		SearchKey = srchSession.prpHomePageSearch.SearchKey,
																SearchByDealer = srchSession.prpHomePageSearch.SearchByDealer, 
																ZipCode = srchSession.prpHomePageSearch.ZipCode
                                                           	});
            }

            if (srchSession.prpSearchType == SearchType.AdvanceSearch)
            {
                AdvanceSearch advSearch = srchSession.prpAdvSearchParameter;
                TempData["BackButton"] = advSearch;
                return RedirectToAction("SearchAdvance", "Search");
            }
            if (srchSession.prpSearchType == SearchType.SearchonSearchPage)
            {
                SearchOnSearchPage SearchOnSearch = srchSession.prpSearchonSearchPageParameter;
                TempData["BackButton"] = SearchOnSearch;
                return RedirectToAction("Index", "Search");
 
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
