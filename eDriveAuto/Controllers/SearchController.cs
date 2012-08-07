using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Edrivie_Service_Ref;
using Edrive.Logic;
using Edrive.Logic.Interfaces;
using Edrive.Models;
using Edrive.NADA_UsedCars;
using Customer = Edrive.Edrivie_Service_Ref.Customer;
using Products = Edrive.Edrivie_Service_Ref.Products;

namespace Edrive.Controllers
{
	public class SearchController : Controller
	{
		private readonly IProductService _productService; 
		private readonly IProductTypeService _productTypeService; 
		private readonly IProductBodyService _productBodyService;
		private readonly IProductModelService _productModelService;
		private readonly IProductMakeService _productMakeService;
		private readonly IDealerService _dealerService;
		
		public SearchController(IProductService productService, 
			IProductTypeService productTypeService, 
			IProductBodyService productBodyService,
			IProductModelService productModelService,
			IProductMakeService productMakeService,
			IDealerService dealerService)
		{
			_productService = productService;
			_productTypeService = productTypeService;
			_productBodyService = productBodyService;
			_productModelService = productModelService;
			_productMakeService = productMakeService;
			_dealerService = dealerService;
		}

		#region property

		#endregion

		#region Search

		/// <summary>
		/// This action is for the Home Page Search
		/// </summary>
		/// <param name="SearchKey"></param>
		/// <returns></returns>
		public ActionResult Index(String SearchKey, String SearchByDealer, string ZipCode = "")
		{
			int? CarsCount = 0;
			if(TempData["BackButton"] != null)
			{
				if((Session["SearchType"] as SearchSession).prpSearchType == SearchType.SearchonSearchPage)
				{
					SearchOnSearchPage objSearch = TempData["BackButton"] as SearchOnSearchPage;
					using(Edrive_ServiceClient _service = new Edrive_ServiceClient())
					{
						objSearch.Make = objSearch.Make.Trim(',');
						objSearch.Model = objSearch.Model.Trim(',');
						objSearch.Body = objSearch.Body.Trim(',');
						objSearch.DriveType = objSearch.DriveType.Trim(',');
						objSearch.Engine = objSearch.Engine.Trim(',');
						objSearch.hiddenSearchKey = objSearch.hiddenSearchKey.Trim(',');
						objSearch.Mileage = objSearch.Mileage.Trim(',');
						objSearch.PageIndex = objSearch.PageIndex.Trim(',');
						objSearch.pageSize = objSearch.pageSize.Trim(',');

						objSearch.Price = objSearch.Price.Trim(',');
						objSearch.SearchByDealerID = objSearch.SearchByDealerID;
						objSearch.sortByColumn = objSearch.sortByColumn.Trim(',');
						objSearch.Transmission = objSearch.Transmission.Trim(',');
						objSearch.Type = objSearch.Type.Trim(',');

						objSearch.Vin = objSearch.Vin.Trim(',');
						objSearch.Warranty = objSearch.Warranty.Trim(',');
						objSearch.Year = objSearch.Year.Trim(',');
						objSearch.Zip = ZipCode.PadLeft(5, '0').Trim(',');

						BindSearchPageFilter(objSearch.Price,
											objSearch.Mileage,
											objSearch.Make,
											objSearch.Model,
											objSearch.Year,
											objSearch.Vin,
											objSearch.DriveType,
											objSearch.Transmission,
											objSearch.Engine,
												objSearch.Body,
											objSearch.Type,
											objSearch.Zip,
											objSearch.Warranty,
											objSearch.sortByColumn,
											objSearch.pageSize,
											objSearch.PageIndex,
											objSearch.hiddenSearchKey,
											objSearch.SearchByDealerID
											);
						// String Makeid=Convert.ToInt32(objSearch.Make.Contains(",")?objSearch.Make.Remove())
					}


					int pageIndex = 0;

					if(String.IsNullOrWhiteSpace(objSearch.PageIndex) || Convert.ToInt32(objSearch.PageIndex) == 0)
						pageIndex = -1;

					var lstPRoducts = GetSearchOnSearchPage(
		   objSearch.Price, objSearch.Mileage, objSearch.Make, objSearch.Model, objSearch.Year,
		   objSearch.Vin, objSearch.DriveType, objSearch.Transmission, objSearch.Engine,
			objSearch.Body, objSearch.Type, objSearch.Zip, objSearch.Warranty, objSearch.sortByColumn,
		   objSearch.pageSize, pageIndex.ToString(), objSearch.hiddenSearchKey, objSearch.SearchByDealerID, CarsCount);


					//ViewData["PageIndex"] = objSearch.PageIndex;
					//ViewData["CarsCount"] = CarsCount;

					return View(lstPRoducts);
				}
			}
			///--save the Search Type Session so that user can come back to search from product detail page
			var homePageSearchSession = new SearchSession(SearchType.HomePageSearch, SearchKey, SearchByDealer, ZipCode);
			Session["SearchType"] = homePageSearchSession;
			///-end of  Search Type Session

			#region for facebook authentication
			if(User.Identity.IsAuthenticated == false)
			{

				String retUrl = Common_Methods.GetDomainUrl() + "Search&scope=" + Common_Methods.getFaceBookScope();
				String facebookLoginUrl = "https://www.facebook.com/dialog/oauth?client_id=" + Common_Methods.GetFacebookApplicationId() + "&redirect_uri=" + retUrl;
				ViewData["facebookLoginUrl"] = facebookLoginUrl;

			}
			if(Request.QueryString["code"] != null && User.Identity.IsAuthenticated == false)
			{
				try
				{
					Common_Methods.CreateUserFromFaceBook("Search");
					//  return RedirectToAction("Index", "Home");

					if(String.IsNullOrWhiteSpace(SearchKey) == false)
					{
						return RedirectToAction("Index", "Search", new { SearchKey = SearchKey });
					}
					else
					{
						return RedirectToAction("Index", "Search");
					}
				}
				catch(Exception ex)
				{
					ViewData["Msg"] = "Error" + ex.Message;
				}

			}
			#endregion
			try
			{
				ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Search-metatitle");
				ViewData["description"] = SettingManager.GetSettingValue("SEO.Search-description");
				ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Search-keywords");

			}

			catch(Exception)
			{


			}
			int CountDownDays;
			CountDownDays = GetCountDownDays();
			ViewData["CountDownDays"] = CountDownDays;
			var isSearchByDealer = false;
			int DealerID = -1;
			if(String.IsNullOrEmpty(SearchByDealer) == false)
			{

				int.TryParse(Request.QueryString["DealerID"].ToString(), out DealerID);
				ViewData["SearchByDealerID"] = DealerID;
				isSearchByDealer = true;
			}

			// if SearchByDealerID is +ve  then search is on for Dealer product

			if(String.IsNullOrWhiteSpace(SearchKey))
				SearchKey = "";
			else
			{
				/// this is for  the home page search to split the work and add 'and' between the words e.g. 'acura 2009' will become 'acura and 2009'
				SearchKey = GetFullTextSearchKey(SearchKey);

			}

			using(Edrive_ServiceClient _service = new Edrive_ServiceClient())
			{
				int searchString;
				bool search_Key = int.TryParse(SearchKey, out searchString);

				String Price = string.Empty;
				String Mileage = string.Empty;
				String Make = string.Empty;
				String Model = string.Empty;
				String Year = string.Empty;
				String Vin = string.Empty;
				String DriveType = string.Empty;
				String Transmission = string.Empty;
				String Engine = string.Empty;
				String Body = string.Empty;
				String Type = string.Empty;
				String Warranty = string.Empty;
				int Zip;

				if(!int.TryParse(ZipCode, out Zip))
					Zip = -1;
				else
					ViewData["Zip"] = Zip;

				var vehicles = _productService.SearchStandard(SearchKey, 25, 1,
				                                              ref CarsCount, null, ref Price, ref Mileage, ref Make, ref Model,
				                                              ref Year, ref Body, Zip, null, Warranty);

//				List<Products> collections = _service.SearchProductBy_Make_Model_City_Zip(SearchKey, 25, 0,
//					ref CarsCount, "", ref Price, ref Mileage, ref Make, ref Model, ref Year, ref Body, ref Type,
//					Convert.ToInt32(Zip), null, Warranty, ref Vin, Transmission, Engine, ref DriveType, isSearchByDealer, DealerID);

				//collections = CheckNADAPrice(collections);

				//CheckCarfax(collections);

				ViewData["CarsCount"] = CarsCount;

				if((CarsCount ?? 0) > 0)
					ViewData["pageIndex"] = 0;
				//else
				//{
				//    ViewData["pageIndex"] = -1;
				//}

				#region Set Filters for Search summary page using Home page SearchKey
				int Makeid = -1;
				int ModelId = -1;
				String MinYear = "-1", MaxYear = "-1";
				// int Year = -1;



				if(Request.QueryString["SearchKey"] != null)
				{
					var SearchString = Request.QueryString["SearchKey"].ToString().ToLower();
					var searchValue = SearchString.Replace(" ", "");
					var lstMakes = _service.BindMake().OrderBy(m => m.make);
					foreach(Product_Make item in lstMakes)
					{
						var ismakeSpecified = searchValue.Contains(item.make.Replace(" ", "").ToLower());
						if(ismakeSpecified)
						{
							Makeid = item.id;
							// remove the make name from search string
							searchValue = SearchString.Remove(SearchString.IndexOf(item.make.ToLower()), item.make.Length);

							#region checkforModel
							if(String.IsNullOrEmpty(searchValue.Trim()) == false)
							{
								List<Product_Model> lstModel = _service.BindModel(item.id);
								foreach(var itemModel in lstModel)
								{
									// remove blan spaces
									var itemModelName = itemModel.modelName.Replace(" ", "").ToLower();
									var isModelSpecified = searchValue.Contains(itemModelName);
									if(isModelSpecified)
									{
										ModelId = itemModel.id;
										searchValue = searchValue.Remove(searchValue.IndexOf(itemModelName), itemModelName.Length);
										break;
									}
								}
							}
							#endregion
							break;
						}
					}

					// now check for the year.
					for(int i = 1998; i <= DateTime.Now.Year; i++)
					{
						var strYear = i.ToString();
						if(searchValue.Contains(strYear))
						{
							searchValue = searchValue.Remove(searchValue.IndexOf(strYear), strYear.Length);
							MinYear = MaxYear = strYear;
							break;
						}

					}


					// modified the searh key
					SearchKey = GetFullTextSearchKey(searchValue);


				}

				#endregion

				BindFilters(Makeid, ModelId, minYear: MinYear, maxYear: MaxYear);
				// if cars count is zero then remove search key
				if((CarsCount ?? 0) > 0)
				{
					ViewData["searchKey"] = SearchKey;
				}
				if((CarsCount ?? 0) > 0)
				{
					ViewData["PageCounts"] = CarsCount.Value / 25 + 1;
				}
				else
				{
					ViewData["PageCounts"] = 0;
				}

				if(search_Key == true)
					ViewData["Zip"] = searchString;
				if(vehicles != null)
				{
					return View("Index", vehicles.ToList());
				}
				else
				{
					return View("Index", vehicles);
				}
			}
		}

		[HttpPost]
		public ActionResult Index(
			String Body, String DriveType, String Engine,
			String PriceMax, String YearMax, String Mileage, String PriceMin, String YearMin, int Make,
			int? Model, int Radius, String Transmission, int Type, String Vin, String Zip
			)
		{
			///--save the Search Type Session so that user can come back to search from product detail page

			var AdvancePageSearchSession = new SearchSession();
			AdvancePageSearchSession.prpSearchType = SearchType.AdvanceSearch;
			AdvancePageSearchSession.prpAdvSearchParameter = new AdvanceSearch
			{
				Body = Body,
				DriveType = DriveType
				,
				Engine = Engine,
				PriceMax = PriceMax,
				YearMax = YearMax,
				Mileage = Mileage,
				PriceMin = PriceMin,
				YearMin = YearMin,
				Make = Make,
				Model = Model,
				Radius = Radius,
				Transmission = Transmission,
				Type = Type,
				Vin = Vin,
				Zip = Zip
			};
			Session["SearchType"] = AdvancePageSearchSession;
			// end of  Search Type Session 
			return SearchAdvance(Body, DriveType, Engine,
						   PriceMax, YearMax, Mileage, PriceMin, YearMin, Make,
						   Model, Radius, Transmission, Type, Vin,
						   Zip);

		}

		private List<Products> CheckCarfax(List<Products> products)
		{
			List<Products> result = new List<Products>();

			var carfaxUrl = "ftp://ftp.carfax.com";
			var inboundCredential = new NetworkCredential("EDRIVEAUTO", "8883163374");
			var outboundCredential = new NetworkCredential("EDRIVEAUTO_get", "8883163374");

			FileInfo file = new FileInfo(Server.MapPath(String.Format("~/edriveauto _cfxiicr_{0}.txt", DateTime.UtcNow.ToString("MMddyyyy"))));

			if(DateTime.UtcNow.Hour < 6 && DateTime.UtcNow.Hour > 12)
			{
				return products;
			}
			using(var service = new Edrive_ServiceClient())
			{
				using(StreamWriter write = file.CreateText())
				{

					foreach(var product in products)
					{
						write.WriteLine(product.vin + "|" + service.GetDealerbyProductID(product.productId).customerID);
					}
				}
			}
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

			var temp = result;


			return result;
		}

		public string DownloadFromFtp(string url, string username, string password)
		{
			// Get the object used to communicate with the server.
			FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
			request.Method = WebRequestMethods.Ftp.DownloadFile;

			// This example assumes the FTP site uses anonymous logon.
			request.Credentials = new NetworkCredential(username, password);

			FtpWebResponse response = (FtpWebResponse)request.GetResponse();

			Stream responseStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(responseStream);
			var content = reader.ReadToEnd();

			reader.Close();
			response.Close();

			return content;
		}

		public FtpWebResponse UploadToFtp(string url, string username, string password, string filePath)
		{
			// Get the object used to communicate with the server.
			FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
			request.Method = WebRequestMethods.Ftp.UploadFile;

			// This example assumes the FTP site uses anonymous logon.
			request.Credentials = new NetworkCredential(username, password);

			// Copy the contents of the file to the request stream.
			StreamReader sourceStream = new StreamReader(filePath);
			byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
			sourceStream.Close();
			request.ContentLength = fileContents.Length;

			Stream requestStream = request.GetRequestStream();
			requestStream.Write(fileContents, 0, fileContents.Length);
			requestStream.Close();

			FtpWebResponse response = (FtpWebResponse)request.GetResponse();

			response.Close();

			return response;
		}

		private List<Products> CheckNADAPrice(List<Products> collections)
		{
			List<Products> autos = new List<Products>();

			using(UsedCars nadaService = new UsedCars())
			{
				foreach(var car in collections)
				{
					var nadaAuto = nadaService.GetUsedCars("EdriveAutos", "ed12uc20", car.vin);

					if(nadaAuto == null) //auto don't find in nada service
					{
						continue;
					}

					var nadaPriceMin = nadaAuto.PriceMinMax.Values.Where(it => it.Min > 0).Average(it => it.Min);
					var nadaPriceMax = nadaAuto.PriceMinMax.Values.Where(it => it.Max > 0).Average(it => it.Max);


					if(car.price_Current < (decimal)nadaPriceMin - 500 || car.price_Current > (decimal)nadaPriceMax + 500)
					{
						continue;
					}

					autos.Add(car);
				}

			}

			return collections;
		}

		private void BindSearchPageFilter(String Prices,
			String Mileage,
			String Make,
			String Model,
			String Year,
			String Vin,
			String DriveType,
			String Transmission,
			String Engine,
			 String Body,
			String Type,
			String Zip,
			String Warranties,
			String sortByColumn,
			String pageSize,
			String PageIndex,
			String hiddenSearchKey,
			int SearchByDealerID
)
		{
			using(Edrive_ServiceClient _service = new Edrive_ServiceClient())
			{

				var lstPrices = new List<SelectListItem>();
				lstPrices.Add(new SelectListItem { Value = "-1", Text = "All" });

				lstPrices.Add(new SelectListItem { Value = "0 - 5000", Text = "0 - 5000" });
				for(int i = 5001; i <= 50001; i += 5000)
				{
					var str = i.ToString() + " - " + (i + 5000 - 1).ToString();
					lstPrices.Add(new SelectListItem { Value = str, Text = str });
				}

				//if (String.IsNullOrEmpty(MinPrice) && String.IsNullOrEmpty(MaxPrice))//--no price range is selected

				String[] arPrices = Prices.Split(',');
				if(arPrices.Count() > 0)
				{
					foreach(var item in lstPrices)
					{
						if(arPrices.Contains(item.Value))
						{
							item.Selected = true;
						}
					}
				}
				else
				{
					lstPrices[0].Selected = true;
				}
				ViewData["Price"] = lstPrices;
				//}
				//else
				//{
				//    var Price = new SelectList(lstPrices, "Value", "Text");
				//    ViewData["Price"] = Price;
				//}

				//-- bind Mileage
				var lstMileages = new List<SelectListItem>();
				lstMileages.Add(new SelectListItem { Value = "-1", Text = "All" });
				lstMileages.Add(new SelectListItem { Value = "0 - 10000", Text = "0 - 10000" });
				for(int i = 10001; i <= 90001; i += 10000)
				{
					var str = i.ToString() + " - " + (i + 10000 - 1).ToString();
					lstMileages.Add(new SelectListItem { Value = str, Text = str });
				}
				var _Mileages = new SelectList(lstMileages, "Value", "Text", Mileage);
				ViewData["Mileage"] = _Mileages;

				//-- bind make
				var makes = _service.BindMake().Select(m => new SelectListItem { Text = m.make, Value = m.id.ToString() }).ToList();
				makes.Insert(0, new SelectListItem { Value = "-1", Text = "All" });

				var MakeID = Convert.ToInt32(Make);
				ViewData["Make"] = new SelectList(makes, "Value", "Text", MakeID);

				if(MakeID > 0)
				{
					var lstmodel = _service.BindModel(MakeID).Select(m => new SelectListItem { Text = m.modelName, Value = m.id.ToString() }).ToList();


					lstmodel.Insert(0, new SelectListItem { Value = "-1", Text = "All" });
					var arModel = Model.Split(',');
					foreach(var item in lstmodel)
					{
						if(arModel.Contains(item.Value))
							item.Selected = true;

					}
					ViewData["Model"] = lstmodel;
					ViewData["MakeSelected"] = "yes";
				}
				else
				{
					// Change_to_titleCase(makes);
					ViewData["Model"] = null;
				}
				//-- bind Year
				List<SelectListItem> lstYear = new List<SelectListItem>();
				lstYear.Add(new SelectListItem { Text = "All", Value = "-1" });
				{
					for(int i = 1998; i <= DateTime.Now.Year; i++)
					{
						lstYear.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
					}

					String[] arYears = Year.Split(',');
					if(arYears.Count() > 0)
					{
						foreach(var item in lstYear)
						{
							if(arYears.Contains(item.Value))
								item.Selected = true;
						}
					}
					else
					{
						if(lstYear.Any(m => m.Selected == true) == false)
							lstYear[0].Selected = true;

					}
					ViewData["Year"] = lstYear;
				}
				//new SelectList(lstYear, "Value", "Text");

				var _bodyList = _service.BindBodyType().Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.body }).ToList();
				_bodyList.Insert(0, new SelectListItem { Text = "All", Value = "-1" });
				String[] arBody = Body.Split(',');
				foreach(var item in _bodyList)
				{
					if(arBody.Contains(item.Value))
						item.Selected = true;

				}
				//if none is selected then all should be checked
				if(_bodyList.Any(m => m.Selected) == false)
					_bodyList[0].Selected = true;


				ViewData["Body"] = _bodyList;

				var Product_types = _service.bindtypeAttributes().Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.type }).ToList(); ;
				Product_types.Insert(0, new SelectListItem { Text = "All", Value = "-1" });
				ViewData["Type"] = new SelectList(Product_types, "Value", "Text", Type);


				List<SelectListItem> Warranty = new List<SelectListItem>();
				Warranty.Add(new SelectListItem { Text = "All", Value = "-1" });
				Warranty.Add(new SelectListItem { Text = "No", Value = "0" });
				Warranty.Add(new SelectListItem { Text = "Yes", Value = "1" });
				if(Warranty.Any(m => m.Value == Warranties))
				{
					Warranty.First(m => m.Value == Warranties).Selected = true;
				}
				ViewData["Warranty"] = new SelectList(Warranty, "Value", "Text");

			}
		}

		public List<Core.Model.Products> GetSearchOnSearchPage(String Price, String Mileage, String Make, String Model, String Year,
		   String Vin, string DriveType, string Transmission, string Engine,
			String Body, String Type, String Zip, string Warranty, String sortByColumn,
		   String pageSize, String PageIndex, string hiddenSearchKey, int SearchByDealerID, int? CarsCount)
		{
			using(Edrive_ServiceClient _service = new Edrive_ServiceClient())
			{

				//int? CarsCount = null;
				string searchKey = string.Empty;
				if(hiddenSearchKey != string.Empty || hiddenSearchKey != null)
				{
					searchKey = hiddenSearchKey;
				}
				if(String.IsNullOrEmpty(Model))
					Model = "-1";
				if(String.IsNullOrEmpty(Zip))
					Zip
						= "-1";

				var isSearchByDealer = false;
				// if SearchByDealerID is +ve  then search is on for Dealer product
				if(SearchByDealerID > 0)
				{
					isSearchByDealer = true;
				}

				int pageIndex = Convert.ToInt32(PageIndex);

				var vehicles = _productService.SearchStandard(searchKey, Convert.ToInt32(pageSize), pageIndex,
				                                              ref CarsCount, sortByColumn, ref Price, ref Mileage, ref Make,
				                                              ref Model, ref Year, ref Body, Convert.ToInt32(Zip), null, Warranty);

//				var Cars_collection = _service.SearchProductBy_Make_Model_City_Zip(searchKey, Convert.ToInt32(pageSize), pageIndex,
//					ref CarsCount, sortByColumn, ref Price, ref Mileage, ref Make, ref Model, ref Year, ref Body, ref Type, Convert.ToInt32(Zip), null, Warranty, ref Vin, Transmission, Engine, ref DriveType, isSearchByDealer, SearchByDealerID);
				ViewData["CarsCount"] = CarsCount;
				int CountDownDays;
				CountDownDays = GetCountDownDays();
				ViewData["CountDownDays"] = CountDownDays;
				Session["sortByColumn"] = sortByColumn;
				ViewData["pageIndex"] = pageIndex < 0 ? 0 : pageIndex;
				ViewData["PageCounts"] = CarsCount.Value / Convert.ToInt32(pageSize) + 1;
				ViewData["PageSize"] = pageSize;

				return vehicles;
			}
		}

		private static String GetFullTextSearchKey(String SearchKey)
		{
			SearchKey = SearchKey.Trim();
			var tempSearchKey = SearchKey.Split(' ');
			var tempSrch = "";
			for(int i = 0; i < tempSearchKey.Length; i++)
			{
				if(String.IsNullOrEmpty(tempSearchKey[i].Trim()) == false)
				{
					tempSrch += tempSearchKey[i].Trim() + " and ";
				}

			}
			if(tempSrch.LastIndexOf(" and") > 0)
			{
				tempSrch = tempSrch.Substring(0, tempSrch.LastIndexOf(" and"));
			}
			SearchKey = tempSrch;
			return SearchKey;
		}

		public ActionResult SearchCars()
		{
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult SearchCars(SearchType searchType, bool isPreAuction = false)
		{
			String Price,
				   Mileage = "-1",
				   Make = "-1",
				   Model = "-1",
				   Year = "-1",
				   Vin = "",
				   DriveType = "-1",
				   Transmission = "-1",
				   Engine = "-1",
				   Body = "-1",
				   Type = "-1",
				   Zip = "",
				   Warranty = "-1",
				   sortByColumn = "",
				   pageSize = "25",
				   PageIndex = "0",
				   hiddenSearchKey = "";

			Price = "0 - 10000";

			var selectedPrices = new List<SelectListItem>();

			selectedPrices.Add(new SelectListItem { Value = "0 - 5000", Text = "0 - 5000" });
			selectedPrices.Add(new SelectListItem { Value = "5001 - 10000", Text = "5001 - 10000" });

			if(searchType == SearchType.Luxury)
			{
				Price = "50001 - 2000000";
				selectedPrices.Clear();
				selectedPrices.Add(new SelectListItem { Value = "50001 - 2000000", Text = "50001 and above" });
			}

			var SearchPageSearchSession = new SearchSession();
			SearchPageSearchSession.prpSearchType = SearchType.SearchonSearchPage;
			var objSearch = new SearchOnSearchPage
			{
				Price = Price,
				Mileage = Mileage,
				Make = Make,
				Model = Model,
				Year = Year,
				Vin = Vin,
				DriveType = DriveType,
				Transmission = Transmission,
				Engine = Engine,
				Body = Body,
				Type = Type,
				Zip = Zip,
				Warranty = Warranty,
				sortByColumn = sortByColumn,
				pageSize = pageSize,
				PageIndex = PageIndex,
				hiddenSearchKey = hiddenSearchKey,
				SearchByDealerID = 0
			};

			SearchPageSearchSession.prpSearchonSearchPageParameter = objSearch;
			Session["SearchType"] = SearchPageSearchSession;

			BindSearchPageFilter(objSearch.Price,
											objSearch.Mileage,
											objSearch.Make,
											objSearch.Model,
											objSearch.Year,
											objSearch.Vin,
											objSearch.DriveType,
											objSearch.Transmission,
											objSearch.Engine,
												objSearch.Body,
											objSearch.Type,
											objSearch.Zip,
											objSearch.Warranty,
											objSearch.sortByColumn,
											objSearch.pageSize,
											objSearch.PageIndex,
											objSearch.hiddenSearchKey,
											objSearch.SearchByDealerID
											);

			using(Edrive_ServiceClient _service = new Edrive_ServiceClient())
			{
				int? CarsCount = null;
				string searchKey = string.Empty;
				ViewData["pageIndex"] = PageIndex;
				if(hiddenSearchKey != string.Empty || hiddenSearchKey != null)
				{
					searchKey = hiddenSearchKey;
				}
				if(String.IsNullOrEmpty(Model))
					Model = "-1";
				if(String.IsNullOrEmpty(Zip))
					Zip
						= "-1";

				var isSearchByDealer = false;

				var vehicles = _productService.SearchStandard(searchKey, Convert.ToInt32(pageSize), Convert.ToInt32(PageIndex),
				                                              ref CarsCount, sortByColumn, ref Price, ref Mileage, ref Make,
				                                              ref Model, ref Year, ref Body, Convert.ToInt32(Zip), null, Warranty);

//				var Cars_collection = _service.SearchProductBy_Make_Model_City_Zip(searchKey, Convert.ToInt32(pageSize), Convert.ToInt32(PageIndex),
//					ref CarsCount, sortByColumn, ref Price, ref Mileage, ref Make, ref Model, ref Year, ref Body, ref Type, Convert.ToInt32(Zip), null, Warranty, ref Vin, Transmission, Engine, ref DriveType, isSearchByDealer, 0);

				ViewData["CarsCount"] = CarsCount;

				if((CarsCount ?? 0) > 0)
					ViewData["pageIndex"] = 0;
				//else
				//{
				//    ViewData["pageIndex"] = -1;
				//}

				#region Set Filters for Search summary page using Home page SearchKey
				
				int makeid = -1;
				int modelId = -1;
				string minYear = "-1";
				string maxYear = "-1";
				
				if(Request.QueryString["SearchKey"] != null)
				{
					var searchString = Request.QueryString["SearchKey"].ToLower();
					var searchValue = searchString.Trim();
					var lstMakes = _productMakeService.GetAll().OrderBy(m => m.make);
					
					foreach(var item in lstMakes)
					{
						var ismakeSpecified = searchValue.Contains(item.make.Replace(" ", "").ToLower());
						if(ismakeSpecified)
						{
							makeid = item.id;
							// remove the make name from search string
							searchValue = searchString.Remove(searchString.IndexOf(item.make.ToLower()), item.make.Length);

							#region checkforModel
							if(String.IsNullOrEmpty(searchValue.Trim()) == false)
							{
								var lstModel = _productModelService.GetModelsByMake(item.id);
								foreach(var itemModel in lstModel)
								{
									// remove blan spaces
									var itemModelName = itemModel.modelName.Replace(" ", "").ToLower();
									var isModelSpecified = searchValue.Contains(itemModelName);
									if(isModelSpecified)
									{
										modelId = itemModel.id;
										searchValue = searchValue.Remove(searchValue.IndexOf(itemModelName), itemModelName.Length);
										break;
									}
								}
							}
							#endregion
							break;
						}
					}

					// now check for the year.
					for(int i = 1998; i <= DateTime.Now.Year; i++)
					{
						var strYear = i.ToString();
						if(searchValue.Contains(strYear))
						{
							searchValue = searchValue.Remove(searchValue.IndexOf(strYear), strYear.Length);
							minYear = maxYear = strYear;
							break;
						}

					}
				}

				#endregion

				BindFilters(makeid, modelId, minYear: minYear, maxYear: maxYear, selectedPrices: selectedPrices);
				// if cars count is zero then remove search key
				if((CarsCount ?? 0) > 0)
				{
					ViewData["PageCounts"] = CarsCount.GetValueOrDefault(0) / 25 + 1;
				}
				else
				{
					ViewData["PageCounts"] = 0;
				}

				ViewData["CarsCount"] = CarsCount;
				int countDownDays = GetCountDownDays();
				ViewData["CountDownDays"] = countDownDays;
				return PartialView("Index", vehicles);
			}
		}

		public ActionResult SearchAdvance(String Body, String DriveType, String Engine,
			String PriceMax, String YearMax, String Mileage, String PriceMin, String YearMin, int? Make,
			int? Model, int? Radius, String Transmission, int? Type, String Vin, String Zip
		   )
		{
			// if it is back button clicked on  prodcut detail page
			if(TempData["BackButton"] != null)
			{
				AdvanceSearch advSearch = TempData["BackButton"] as AdvanceSearch;
				Body = advSearch.Body;
				DriveType = advSearch.DriveType;
				Engine = advSearch.Engine;
				PriceMax = advSearch.PriceMax;
				YearMax = advSearch.YearMax;
				Mileage = advSearch.Mileage;
				PriceMin = advSearch.PriceMin;
				YearMin = advSearch.YearMin;
				Make = advSearch.Make;
				Model = advSearch.Model;
				Radius = advSearch.Radius;
				Transmission = advSearch.Transmission;
				Type = advSearch.Type;
				Vin = advSearch.Vin;
				Zip = advSearch.Zip;
			}

			int countDownDays = GetCountDownDays();
			ViewData["CountDownDays"] = countDownDays;
			
			Core.Model.AdvancedSearchAttributes attr = new Core.Model.AdvancedSearchAttributes
				                                        {
				                                           	_body = Convert.ToInt32(Body),
				                                           	_driveType = DriveType,
				                                           	_engine = Engine,
				                                           	_make = Make ?? -1,
				                                           	_maxYear = YearMax.ConvertTo(-1)
				                                        };

			if(Mileage != null)
			{
				if(Mileage == "-1" || String.IsNullOrEmpty(Mileage))
				{
					attr._mileageFrom = -1; attr._mileageTo = -1;
				}
				else
				{
					attr._mileageFrom = 0;
					attr._mileageTo = Convert.ToInt32(Mileage);
					ViewData["AdvSearchMileageRange"] = attr._mileageFrom + " - " + attr._mileageTo;
				}
			}
			else
			{
				attr._mileageFrom = -1;
				attr._mileageTo = -1;
			}
			if(!String.IsNullOrEmpty(PriceMin) && !String.IsNullOrEmpty(PriceMax))
			{
				int minPrice, maxPrice;
				if(int.TryParse(PriceMin, out minPrice) && int.TryParse(PriceMax, out maxPrice))
					ViewData["AdvSearchPriceRange"] = string.Format("{0} - {1}", minPrice, maxPrice);
				else
				{
					ViewData["AdvSearchPriceRange"] = "";
				}
			}
			else
			{
				ViewData["AdvSearchPriceRange"] = "";
			}
			attr._maxPrice = String.IsNullOrEmpty(PriceMax) ? -1 : Convert.ToInt32(PriceMax);
			attr._minPrice = String.IsNullOrEmpty(PriceMin) ? -1 : Convert.ToInt32(PriceMin);
			attr._minYaer = String.IsNullOrEmpty(YearMin) ? -1 : Convert.ToInt32(YearMin);
			attr._maxYear = String.IsNullOrEmpty(YearMax) ? -1 : Convert.ToInt32(YearMax);
			var modelID = Model == null ? -1 : Model.Value;
			attr._model = modelID;
			attr._radius = Radius.ToString();
			attr._transmission = Transmission;
			attr._Type = Type ?? -1;
			attr._vin = Vin ?? "";
			attr._zip = String.IsNullOrEmpty(Zip) ? -1 : Convert.ToInt32(Zip);
			int count;
			List<Core.Model.Products> collection = _productService.SearchAdvanced(attr, 25, 1, null, out count);
			ViewData["pageIndex"] = 0;
			Mileage = Mileage == "" ? "-1" : Mileage;
			BindFilters(Make ?? -1, modelID, Type.ToString(), Mileage, Body,
			Transmission, Engine, attr._minYaer.ToString(), attr._maxYear.ToString(), PriceMin, PriceMax);
			ViewData["Vin"] = Vin;
			ViewData["Zip"] = Zip;
			ViewData["Transmission"] = Transmission;
			ViewData["Engine"] = Engine;
			ViewData["Radius"] = Radius;
			var carsCount = _productService.SearchAdvancedCount(attr, 1, 1, null, out count);
			ViewData["CarsCount"] = carsCount;
			ViewData["PageCounts"] = carsCount / 25 + 1;
			return View("Index", collection);
		}

		[HttpPost]
		public ActionResult SearchOnSearchPage(
			String Price, String Mileage, String Make, String Model, String Year,
			String Vin, string DriveType, string Transmission, string Engine,
			String Body, String Type, String Zip, string Warranty, String sortByColumn,
			String pageSize, String PageIndex, string hiddenSearchKey, int SearchByDealerID, bool isSort = false, int? radius = null)
		{
			var searchPageSearchSession = new SearchSession();
			searchPageSearchSession.prpSearchType = SearchType.SearchonSearchPage;
			searchPageSearchSession.prpSearchonSearchPageParameter = new SearchOnSearchPage
			{
				Price = Price,
				Mileage = Mileage,
				Make = Make,
				Model = Model,
				Year = Year,
				Vin = Vin,
				DriveType = DriveType,
				Transmission = Transmission,
				Engine = Engine,
				Body = Body,
				Type = Type,
				Zip = Zip,
				Warranty = Warranty,
				sortByColumn = sortByColumn,
				pageSize = pageSize,
				PageIndex = PageIndex,
				hiddenSearchKey = hiddenSearchKey,
				SearchByDealerID = SearchByDealerID
			};
			Session["SearchType"] = searchPageSearchSession;
		
			int? carsCount = null;
			ViewData["pageIndex"] = PageIndex;

			if(String.IsNullOrEmpty(Model))
				Model = "-1";
			if(String.IsNullOrEmpty(Zip))
				Zip
					= "-1";
				
			if(isSort)
			{
				if(Session["SearchSortOrder"] == null)
					Session["SearchSortOrder"] = "asc";

				if(Session["sortByColumn"] != null && Session["sortByColumn"].ToString() == sortByColumn)
				{
					if(Session["SearchSortOrder"].ToString() == "asc")
						Session["SearchSortOrder"] = "desc";
					else
						Session["SearchSortOrder"] = "asc";
				}
				else
					Session["SearchSortOrder"] = null;
			}

			Session["sortByColumn"] = sortByColumn;
			sortByColumn = string.Format("{0} {1}", sortByColumn, Session["SearchSortOrder"] ?? String.Empty).Trim();

			var vehicles = _productService.SearchStandard(hiddenSearchKey, Convert.ToInt32(pageSize), Convert.ToInt32(PageIndex),
				                            ref carsCount, sortByColumn, ref Price, ref Mileage, ref Make, ref Model, ref Year,
				                            ref Body, Convert.ToInt32(Zip), radius, Warranty);

			ViewData["CarsCount"] = carsCount;
			int countDownDays = GetCountDownDays();
			ViewData["CountDownDays"] = countDownDays;
			return PartialView("_SearchResult", vehicles);
		}

		private static int GetCountDownDays()
		{
			int CountDownDays = 0;
			using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
			{
				var Set = entity.Settings.FirstOrDefault(m => m.Name == "CountDownDays");
				if(Set != null)
				{
					CountDownDays = Convert.ToInt32(Set.Value);
				}
			}
			return CountDownDays;
		}

		[HttpPost]
		public JsonResult SearchOnSearchPage_CarsCount(String Price, String Mileage, String Make, String Model, String Year,
			String Vin, string DriveType, string Transmission, string Engine,
			 String Body, String Type, String Zip, string Warranty, String sortByColumn,
			String pageSize, String pageIndex, string hiddenSearchKey, int SearchByDealerID, int? radius = null)
		{
			int? carsCount = null;
			ViewData["pageIndex"] = pageIndex;
				
			if(String.IsNullOrEmpty(Model))
				Model = "-1";
			if(String.IsNullOrEmpty(Zip))
				Zip = "-1";

			carsCount = _productService.SearchStandardCount(hiddenSearchKey, 1, 1, ref carsCount, null, ref Price, ref Mileage,
				                                            ref Make, ref Model, ref Year, ref Body, Convert.ToInt32(Zip), null,
				                                            Warranty);

			return Json(carsCount);
		}

		[Authorize]
		[HttpGet]
		public ActionResult AdvanceSearch()
		{
			InitSearchFilters();

			return View(new SearchFilter());
		}

		public ActionResult SearchNoResults()
		{
			InitSearchFilters("NoResults");

			return View(new NoResultsSearchFilter());
		}

		[HttpPost]
		public ActionResult SearchNoResults(NoResultsSearchFilter filter, FormCollection collection)
		{
			if(String.IsNullOrWhiteSpace(collection["IsNoResultSubmit"]))
			{
				InitSearchFilters("NoResults");
				ModelState.Clear();
				return View(new NoResultsSearchFilter());
			}

			if(ModelState.IsValid)
			{
				var dealers = _dealerService.GetDealersByZip(filter.NoResultsZip.ToString());
				
				try
				{
					MessageManager.SendVehicleRequestToDealers(filter, dealers, 0);
				}
				catch
				{
					return Content("<span class='thank-you-message'>Error occurred while sending an email, please try again later.</span>");
				}

				return Content("<span class='thank-you-message'>Thank you!</span>");
			}

			InitSearchFilters("NoResults");
			return View(new NoResultsSearchFilter());
		}

		private void InitSearchFilters(String formName = "")
		{
			ViewData[formName + "Make"] = GetMakeList();
			ViewData[formName + "Body"] = GetBodyTypeList();
			var model = new List<SelectListItem>();
			model.Insert(0, new SelectListItem { Text = "All", Value = "-1" });
			ViewData[formName + "Model"] = model;
			var transmission = _productService.GetTransmissionsList().Select(m => new SelectListItem { Text = m, Value = m }).ToList();
			transmission.Insert(0, new SelectListItem { Text = "All", Value = "-1" });
			ViewData[formName + "Transmission"] = transmission;
			var engine = _productService.GetEnginesList().Select(m => new SelectListItem { Text = m, Value = m }).ToList();
			engine.Insert(0, new SelectListItem { Text = "All", Value = "-1" });
			ViewData[formName + "Engine"] = engine;
			var driveType = _productService.GetDriveTypesList().Select(m => new SelectListItem { Text = m, Value = m }).ToList();
			driveType.Insert(0, new SelectListItem { Text = "All", Value = "-1" });
			ViewData[formName + "DriveType"] = driveType;
			var type = GetProductTypeList();
			ViewData[formName + "Type"] = type;
		}

		private void BindFilters(int makeID = -1, int modelID = -1, string type = "-1", String mileage = "-1", 
			String body = "-1", String transmission = "", String engine = "", String minYear = "", 
			String maxYear = "", String minPrice = "", String maxPrice = "", IEnumerable<SelectListItem> selectedPrices = null)
		{
			var lstPrices = new List<SelectListItem>();
			lstPrices.Add(new SelectListItem { Value = "-1", Text = "All" });
			lstPrices.Add(new SelectListItem { Value = "0 - 5000", Text = "0 - 5000" });
			
			for(int i = 5001; i <= 45001; i += 5000)
			{
				var str = string.Format("{0} - {1}", i, (i + 5000 - 1));
				lstPrices.Add(new SelectListItem { Value = str, Text = str });
			}
			
			lstPrices.Add(new SelectListItem { Value = "50001 - 2000000", Text = "50001 and above" });

			if(String.IsNullOrEmpty(minPrice) && String.IsNullOrEmpty(maxPrice) && selectedPrices == null)//--no price range is selected
			{
				var price = new SelectList(lstPrices, "Value", "Text", "-1");
				ViewData["Price"] = price;
			}
			else
			{
				if(selectedPrices != null)
				{
					foreach(var i in selectedPrices)
						lstPrices.ForEach(c =>
						{
							if(c.Value == i.Value)
							{
								c.Selected = true;
							}
						});
				}

				ViewData["Price"] = lstPrices;
			}

			var mileages = new SelectList(GetMileAgeList(), "Value", "Text", mileage);
			ViewData["Mileage"] = mileages;
			ViewData["Make"] = new SelectList(GetMakeList(), "Value", "Text", makeID);

			if(makeID > 0)
			{
				ViewData["Model"] = new SelectList(GetModelList(makeID), "Value", "Text", modelID);
				ViewData["MakeSelected"] = "yes";
			}
			else
			{
				ViewData["Model"] = modelID;
			}
			
			ViewData["Year"] = GetYearList(minYear.ConvertTo(0), maxYear.ConvertTo(0));
			ViewData["Body"] = new SelectList(GetBodyTypeList(), "Value", "Text", body);
			ViewData["Type"] = new SelectList(GetProductTypeList(), "Value", "Text", type);
			ViewData["Warranty"] = new SelectList(GetWarrantyList(), "Value", "Text");
		}
		
		#endregion

		#region Search Filter Dropdown, Checkbox and Radios

		private IEnumerable<SelectListItem> GetWarrantyList()
		{
			List<SelectListItem> warranty = new List<SelectListItem>
			                                	{
			                                		new SelectListItem {Text = "All", Value = "-1"},
			                                		new SelectListItem {Text = "No", Value = "0"},
			                                		new SelectListItem {Text = "Yes", Value = "1"}
			                                	};

			return warranty;
		}

		private IEnumerable<SelectListItem> GetProductTypeList()
		{
			var productTypes = _productTypeService.GetAll()
				.Select(m => new SelectListItem
				             	{
				             		Value = m.id.ToString(),
				             		Text = m.type
				             	}).ToList();

			productTypes.Insert(0, new SelectListItem { Text = "All", Value = "-1" });

			return productTypes;
		}
		
		private IEnumerable<SelectListItem> GetBodyTypeList()
		{
			var bodyList = _productBodyService.GetAll().Select(m => new SelectListItem
			                                                        	{
			                                                        		Value = m.id.ToString(),
			                                                        		Text = m.body
			                                                        	}).ToList();

			bodyList.Insert(0, new SelectListItem { Text = "All", Value = "-1" });

			return bodyList;
		}

		private IEnumerable<SelectListItem> GetYearList(int minYear = 0, int maxYear = 0)
		{
			List<SelectListItem> listYear = new List<SelectListItem>();
			listYear.Add(new SelectListItem {Text = "All", Value = "-1"});
			{
				for (int i = 1998; i <= DateTime.Now.Year; i++)
				{
					listYear.Add(new SelectListItem {Value = i.ToString(), Text = i.ToString()});
				}

				if(minYear > 0 && maxYear > 0)
					foreach (var item in listYear)
					{
						item.Selected = minYear <= int.Parse(item.Value) && int.Parse(item.Value) <= maxYear;
					}

				if (!listYear.Any(m => m.Selected))
					listYear[0].Selected = true;
			}

			return listYear;
		}

		private IEnumerable<SelectListItem> GetModelList(int makeID)
		{
			var model = _productModelService.GetModelsByMake(makeID).Select(m => new SelectListItem
			                                                                     	{
			                                                                     		Text = m.modelName,
			                                                                     		Value = m.id.ToString()
			                                                                     	}).ToList();

			model.Insert(0, new SelectListItem { Value = "-1", Text = "All" });

			return model;
		}

		private IEnumerable<SelectListItem> GetMakeList()
		{
			var makes = _productMakeService.GetAll().Select(m => new SelectListItem
			                                                     	{
			                                                     		Text = m.make,
			                                                     		Value = m.id.ToString()
			                                                     	}).ToList();

			makes.Insert(0, new SelectListItem { Value = "-1", Text = "All" });

			return makes;
		}

		private IEnumerable<SelectListItem> GetMileAgeList()
		{
			var mileages = new List<SelectListItem>
			               	{
			               		new SelectListItem {Value = "-1", Text = "All"},
			               		new SelectListItem {Value = "0 - 10000", Text = "0 - 10000"}
			               	};

			for(int i = 10001; i <= 90001; i += 10000)
			{
				var value = string.Format("{0} - {1}", i, (i + 10000 - 1));
				mileages.Add(new SelectListItem { Value = value, Text = value });
			}

			return mileages;
		}

		#endregion

		[ChildActionOnly]
		public PartialViewResult FeaturedDealersVehicle()
		{
			var products = _productService.GetDealerFeaturedVehicles(3);
			return PartialView(products);
		}

		[OutputCache(Duration = 3600)]
		public ActionResult RegistrationDetailsPopup()
		{
			ViewBag.TotalVehicleCount = _productService.GetTotalVehiclesCount().ToString("###,###,###");
			
			return View();
		}
	}
}
