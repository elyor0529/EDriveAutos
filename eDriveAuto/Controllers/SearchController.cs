using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Edrive.CommonHelpers;
using Edrive.Logic.Interfaces;
using Edrive.Models;

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
		
		#region Search

		public ActionResult Index(String searchKey, String searchByDealer, string zipCode = "")
		{
			int? carsCount = 0;
			if(TempData["BackButton"] != null)
			{
				if(Session["SearchType"] != null && ((SearchSession)Session["SearchType"]).prpSearchType == SearchType.SearchonSearchPage)
				{
					SearchOnSearchPage objSearch = (SearchOnSearchPage)TempData["BackButton"];
					
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
					objSearch.Zip = zipCode.PadLeft(5, '0').Trim(',');

					BindSearchPageFilter(objSearch.Price,
					                     objSearch.Mileage,
					                     objSearch.Make,
					                     objSearch.Model,
					                     objSearch.Year,
					                     objSearch.Body,
					                     objSearch.Type,
					                     objSearch.Warranty
						);

					int pageIndex = 0;

					if(String.IsNullOrWhiteSpace(objSearch.PageIndex) || Convert.ToInt32(objSearch.PageIndex) == 0)
						pageIndex = -1;

					var lstPRoducts = GetSearchOnSearchPage(
						objSearch.Price, objSearch.Mileage, objSearch.Make, objSearch.Model, objSearch.Year,
						objSearch.Vin, objSearch.DriveType, objSearch.Transmission, objSearch.Engine,
						objSearch.Body, objSearch.Type, objSearch.Zip, objSearch.Warranty, objSearch.sortByColumn,
						objSearch.pageSize, pageIndex.ToString(), objSearch.hiddenSearchKey, objSearch.SearchByDealerID, carsCount);

					return View(lstPRoducts);
				}
			}
			var homePageSearchSession = new SearchSession(SearchType.HomePageSearch, searchKey, searchByDealer, zipCode);
			Session["SearchType"] = homePageSearchSession;

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

					if(String.IsNullOrWhiteSpace(searchKey) == false)
					{
						return RedirectToAction("Index", "Search", new { SearchKey = searchKey });
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

			ViewData["metatitle"] = SettingManager.GetSettingValue("SEO.Search-metatitle");
			ViewData["description"] = SettingManager.GetSettingValue("SEO.Search-description");
			ViewData["keywords"] = SettingManager.GetSettingValue("SEO.Search-keywords");
			int countDownDays = GetCountDownDays();
			ViewData["CountDownDays"] = countDownDays;
			
			if(!String.IsNullOrEmpty(searchByDealer))
			{
				ViewData["SearchByDealerID"] = Request.QueryString["DealerID"].ConvertTo(-1);
			}
			
			String price = string.Empty;
			String mileage = string.Empty;
			String make = string.Empty;
			String model = string.Empty;
			String year = string.Empty;
			String body = string.Empty;
			String warranty = string.Empty;
			int zip = zipCode.ConvertTo(-1);
			
			if(zip > 0)
				ViewData["Zip"] = zip;

			var vehicles = _productService.SearchStandard(searchKey, 25, 0,
				                                            ref carsCount, null, ref price, ref mileage, ref make, ref model,
				                                            ref year, ref body, zip, null, warranty);

			ViewData["CarsCount"] = carsCount;

			if((carsCount ?? 0) > 0)
				ViewData["pageIndex"] = 0;

			#region Set Filters for Search summary page using Home page SearchKey
			int makeid = -1;
			int modelId = -1;
			String minYear = "-1";
			String maxYear = "-1";

			if(Request.QueryString["SearchKey"] != null)
			{
				var searchString = Request.QueryString["SearchKey"].ToLower();
				var searchValue = searchString.Replace(" ", "");
				var lstMakes = GetMakeList();
				foreach(var item in lstMakes)
				{
					var ismakeSpecified = searchValue.Contains(item.Text.Replace(" ", "").ToLower());
					if(ismakeSpecified)
					{
						makeid = item.Value.ConvertTo<int>();
						// remove the make name from search string
						if(searchString.IndexOf(item.Text.ToLower(), StringComparison.Ordinal) != -1)
							searchValue = searchString.Remove(searchString.IndexOf(item.Text.ToLower(), StringComparison.Ordinal), item.Text.Length);

						#region checkforModel
						if(String.IsNullOrEmpty(searchValue.Trim()) == false)
						{
							var lstModel = GetModelList(makeid);
							foreach(var itemModel in lstModel)
							{
								// remove blan spaces
								searchValue = searchValue.Replace(" ", "").Trim();
								var itemModelName = itemModel.Text.Replace(" ", "").ToLower();
								var isModelSpecified = searchValue.Contains(itemModelName);
								if(isModelSpecified)
								{
									modelId = itemModel.Value.ConvertTo<int>();

									if(searchValue.IndexOf(itemModelName, StringComparison.Ordinal) != -1)
										searchValue = searchValue.Remove(searchValue.IndexOf(itemModelName, StringComparison.Ordinal), itemModelName.Length);
									
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
						if(searchValue.IndexOf(strYear, StringComparison.Ordinal) != -1)
							searchValue = searchValue.Remove(searchValue.IndexOf(strYear, StringComparison.Ordinal), strYear.Length);
						
						minYear = maxYear = strYear;
						break;
					}
				}

				// modified the searh key
				searchKey = GetFullTextSearchKey(searchValue);
			}

			#endregion

			BindFilters(makeid, modelId, minYear: minYear, maxYear: maxYear);
			// if cars count is zero then remove search key
			if((carsCount ?? 0) > 0)
			{
				ViewData["searchKey"] = searchKey;
			}
			if((carsCount ?? 0) > 0)
			{
				ViewData["PageCounts"] = carsCount.GetValueOrDefault(0) / 25 + 1;
			}
			else
			{
				ViewData["PageCounts"] = 0;
			}

			if(vehicles != null)
			{
				return View("Index", vehicles.ToList());
			}
			else
			{
				return View("Index", null);
			}
		}

		[HttpPost]
		public ActionResult Index(
			String body, String driveType, String engine,
			String priceMax, String yearMax, String mileage, String priceMin, String yearMin, int make,
			int? model, int radius, String transmission, int type, String vin, String zip
			)
		{
			var advancePageSearchSession = new SearchSession();
			advancePageSearchSession.prpSearchType = SearchType.AdvanceSearch;
			advancePageSearchSession.prpAdvSearchParameter = new AdvanceSearch
			{
				Body = body,
				DriveType = driveType
				,
				Engine = engine,
				PriceMax = priceMax,
				YearMax = yearMax,
				Mileage = mileage,
				PriceMin = priceMin,
				YearMin = yearMin,
				Make = make,
				Model = model,
				Radius = radius,
				Transmission = transmission,
				Type = type,
				Vin = vin,
				Zip = zip
			};
			Session["SearchType"] = advancePageSearchSession;
			// end of  Search Type Session 
			return SearchAdvance(body, driveType, engine,
						   priceMax, yearMax, mileage, priceMin, yearMin, make,
						   model, radius, transmission, type, vin,
						   zip);

		}
		
		public string DownloadFromFtp(string url, string username, string password)
		{
			// Get the object used to communicate with the server.
			FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
			request.Method = WebRequestMethods.Ftp.DownloadFile;

			// This example assumes the FTP site uses anonymous logon.
			request.Credentials = new NetworkCredential(username, password);

			string content = String.Empty;
			using (FtpWebResponse response = (FtpWebResponse) request.GetResponse())
			{
				Stream responseStream = response.GetResponseStream();

				if(responseStream != null)
					using (StreamReader reader = new StreamReader(responseStream))
					{
						content = reader.ReadToEnd();
					}
			}

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

		private void BindSearchPageFilter(String prices, String mileage, String make, String model, String year, String body, String type, String warranties)
		{
			var lstPrices = new List<SelectListItem>();
			lstPrices.Add(new SelectListItem {Value = "-1", Text = "All"});

			lstPrices.Add(new SelectListItem {Value = "0 - 5000", Text = "0 - 5000"});
			for (int i = 5001; i <= 50001; i += 5000)
			{
				var str = i.ToString() + " - " + (i + 5000 - 1).ToString();
				lstPrices.Add(new SelectListItem {Value = str, Text = str});
			}

			String[] arPrices = prices.Split(',');
			if (arPrices.Any())
			{
				foreach (var item in lstPrices)
				{
					if (arPrices.Contains(item.Value))
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
			var lstMileages = new List<SelectListItem>
			                  	{
			                  		new SelectListItem {Value = "-1", Text = "All"},
			                  		new SelectListItem {Value = "0 - 10000", Text = "0 - 10000"}
			                  	};

			for (int i = 10001; i <= 90001; i += 10000)
			{
				var str = i.ToString() + " - " + (i + 10000 - 1).ToString();
				lstMileages.Add(new SelectListItem {Value = str, Text = str});
			}

			var mileages = new SelectList(lstMileages, "Value", "Text", mileage);
			ViewData["Mileage"] = mileages;
			var makes = GetMakeList();
			var makeID = Convert.ToInt32(make);
			ViewData["Make"] = new SelectList(makes, "Value", "Text", makeID);

			if (makeID > 0)
			{
				var lstmodel = GetModelList(makeID);
				var arModel = model.Split(',');
				foreach (var item in lstmodel)
				{
					if (arModel.Contains(item.Value))
						item.Selected = true;

				}
				ViewData["Model"] = lstmodel;
				ViewData["MakeSelected"] = "yes";
			}
			else
			{
				ViewData["Model"] = null;
			}
			//-- bind Year
			List<SelectListItem> lstYear = new List<SelectListItem>();
			lstYear.Add(new SelectListItem {Text = "All", Value = "-1"});
			{
				for (int i = 1998; i <= DateTime.Now.Year; i++)
				{
					lstYear.Add(new SelectListItem {Value = i.ToString(), Text = i.ToString()});
				}

				String[] arYears = year.Split(',');
				if (arYears.Any())
				{
					foreach (var item in lstYear)
					{
						if (arYears.Contains(item.Value))
							item.Selected = true;
					}
				}
				else
				{
					if (!lstYear.Any(m => m.Selected))
						lstYear[0].Selected = true;

				}
				ViewData["Year"] = lstYear;
			}
			
			var bodyList = GetBodyTypeList().ToArray();
			String[] arBody = body.Split(',');
			foreach (var item in bodyList)
			{
				if (arBody.Contains(item.Value))
					item.Selected = true;

			}
			//if none is selected then all should be checked
			if (bodyList.Any(m => m.Selected) == false)
				bodyList[0].Selected = true;
			
			ViewData["Body"] = bodyList;

			var productTypes = GetProductTypeList();
			ViewData["Type"] = new SelectList(productTypes, "Value", "Text", type);
			List<SelectListItem> warranty = GetWarrantyList().ToList();

			if (warranty.Any(m => m.Value == warranties))
			{
				warranty.First(m => m.Value == warranties).Selected = true;
			}

			ViewData["Warranty"] = new SelectList(warranty, "Value", "Text");
		}

		public List<Core.Model.Products> GetSearchOnSearchPage(String price, String mileage, String make, String model, String year,
		   String vin, string driveType, string transmission, string engine,
			String body, String type, String zip, string warranty, String sortByColumn,
		   String pageSize, String pageNumber, string hiddenSearchKey, int searchByDealerID, int? carsCount)
		{
			string searchKey = string.Empty;
			if(!String.IsNullOrWhiteSpace(hiddenSearchKey))
			{
				searchKey = hiddenSearchKey;
			}
			if(String.IsNullOrEmpty(model))
				model = "-1";
			if(String.IsNullOrEmpty(zip))
				zip = "-1";

			int pageIndex = Convert.ToInt32(pageNumber);
			var vehicles = _productService.SearchStandard(searchKey, Convert.ToInt32(pageSize), pageIndex,
				                                            ref carsCount, sortByColumn, ref price, ref mileage, ref make,
				                                            ref model, ref year, ref body, Convert.ToInt32(zip), null, warranty);

			ViewData["CarsCount"] = carsCount;
			int countDownDays = GetCountDownDays();
			ViewData["CountDownDays"] = countDownDays;
			Session["sortByColumn"] = sortByColumn;
			ViewData["pageIndex"] = pageIndex < 0 ? 0 : pageIndex;
			ViewData["PageCounts"] = carsCount.GetValueOrDefault(0) / Convert.ToInt32(pageSize) + 1;
			ViewData["PageSize"] = pageSize;

			return vehicles;
		}

		private static String GetFullTextSearchKey(String searchKey)
		{
			searchKey = searchKey.Trim();
			var tempSearchKey = searchKey.Split(' ');
			var tempSrch = "";
			for(int i = 0; i < tempSearchKey.Length; i++)
			{
				if(String.IsNullOrEmpty(tempSearchKey[i].Trim()) == false)
				{
					tempSrch += tempSearchKey[i].Trim() + " and ";
				}

			}
			if(tempSrch.LastIndexOf(" and", StringComparison.Ordinal) > 0)
			{
				tempSrch = tempSrch.Substring(0, tempSrch.LastIndexOf(" and", StringComparison.Ordinal));
			}
			searchKey = tempSrch;
			return searchKey;
		}

		public ActionResult SearchCars()
		{
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult SearchCars(SearchType searchType, bool isPreAuction = false)
		{
			String mileage = "-1";
			String make = "-1";
			String model = "-1";
			String year = "-1";
			String vin = "";
			String driveType = "-1";
			String transmission = "-1";
			String engine = "-1";
			String body = "-1";
			String type = "-1";
			String zip = "";
			String warranty = "-1";
			String sortByColumn = "";
			String pageSize = "25";
			String pageIndex = "0";
			String hiddenSearchKey = "";
			string price = "0 - 10000";

			var selectedPrices = new List<SelectListItem>
			                     	{
			                     		new SelectListItem {Value = "0 - 5000", Text = "0 - 5000"},
			                     		new SelectListItem {Value = "5001 - 10000", Text = "5001 - 10000"}
			                     	};

			if(searchType == SearchType.Luxury)
			{
				price = "50001 - 2000000";
				selectedPrices.Clear();
				selectedPrices.Add(new SelectListItem { Value = "50001 - 2000000", Text = "50001 and above" });
			}

			var searchPageSearchSession = new SearchSession();
			searchPageSearchSession.prpSearchType = SearchType.SearchonSearchPage;
			var objSearch = new SearchOnSearchPage
			{
				Price = price,
				Mileage = mileage,
				Make = make,
				Model = model,
				Year = year,
				Vin = vin,
				DriveType = driveType,
				Transmission = transmission,
				Engine = engine,
				Body = body,
				Type = type,
				Zip = zip,
				Warranty = warranty,
				sortByColumn = sortByColumn,
				pageSize = pageSize,
				PageIndex = pageIndex,
				hiddenSearchKey = hiddenSearchKey,
				SearchByDealerID = 0
			};

			searchPageSearchSession.prpSearchonSearchPageParameter = objSearch;
			Session["SearchType"] = searchPageSearchSession;

			BindSearchPageFilter(objSearch.Price,
											objSearch.Mileage,
											objSearch.Make,
											objSearch.Model,
											objSearch.Year,
												objSearch.Body,
											objSearch.Type,
											objSearch.Warranty
											);

			int? carsCount = null;
			string searchKey = string.Empty;
			ViewData["pageIndex"] = pageIndex;
			if(!String.IsNullOrWhiteSpace(hiddenSearchKey))
			{
				searchKey = hiddenSearchKey;
			}
			if(String.IsNullOrEmpty(model))
				model = "-1";
			if(String.IsNullOrEmpty(zip))
				zip
					= "-1";
				
			var vehicles = _productService.SearchStandard(searchKey, Convert.ToInt32(pageSize), Convert.ToInt32(pageIndex),
				                                            ref carsCount, sortByColumn, ref price, ref mileage, ref make,
				                                            ref model, ref year, ref body, Convert.ToInt32(zip), null, warranty);

			ViewData["CarsCount"] = carsCount;
			if((carsCount ?? 0) > 0)
				ViewData["pageIndex"] = 0;

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

						if(searchString.IndexOf(item.make.ToLower(), StringComparison.Ordinal) != -1)
							searchValue = searchString.Remove(searchString.IndexOf(item.make.ToLower(), StringComparison.Ordinal), item.make.Length);

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

									if(searchValue.IndexOf(itemModelName, StringComparison.Ordinal) != -1)
										searchValue = searchValue.Remove(searchValue.IndexOf(itemModelName, StringComparison.Ordinal), itemModelName.Length);
									
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
						minYear = maxYear = strYear;
						break;
					}

				}
			}

			#endregion

			BindFilters(makeid, modelId, minYear: minYear, maxYear: maxYear, selectedPrices: selectedPrices);
			// if cars count is zero then remove search key
			if((carsCount ?? 0) > 0)
			{
				ViewData["PageCounts"] = carsCount.GetValueOrDefault(0) / 25 + 1;
			}
			else
			{
				ViewData["PageCounts"] = 0;
			}

			ViewData["CarsCount"] = carsCount;
			int countDownDays = GetCountDownDays();
			ViewData["CountDownDays"] = countDownDays;
			return PartialView("Index", vehicles);
		}

		public ActionResult SearchAdvance(String body, String driveType, String engine,
			String priceMax, String yearMax, String mileage, String priceMin, String yearMin, int? make,
			int? model, int? radius, String transmission, int? type, String vin, String zip)
		{
			// if it is back button clicked on  prodcut detail page
			if(TempData["BackButton"] != null)
			{
				AdvanceSearch advSearch = (AdvanceSearch)TempData["BackButton"];
				body = advSearch.Body;
				driveType = advSearch.DriveType;
				engine = advSearch.Engine;
				priceMax = advSearch.PriceMax;
				yearMax = advSearch.YearMax;
				mileage = advSearch.Mileage;
				priceMin = advSearch.PriceMin;
				yearMin = advSearch.YearMin;
				make = advSearch.Make;
				model = advSearch.Model;
				radius = advSearch.Radius;
				transmission = advSearch.Transmission;
				type = advSearch.Type;
				vin = advSearch.Vin;
				zip = advSearch.Zip;
			}

			int countDownDays = GetCountDownDays();
			ViewData["CountDownDays"] = countDownDays;
			
			Core.Model.AdvancedSearchAttributes attr = new Core.Model.AdvancedSearchAttributes
				                                        {
				                                           	_body = Convert.ToInt32(body),
				                                           	_driveType = driveType,
				                                           	_engine = engine,
				                                           	_make = make ?? -1,
				                                           	_maxYear = yearMax.ConvertTo(-1)
				                                        };

			if(mileage != null)
			{
				if(mileage == "-1" || String.IsNullOrEmpty(mileage))
				{
					attr._mileageFrom = -1; attr._mileageTo = -1;
				}
				else
				{
					attr._mileageFrom = 0;
					attr._mileageTo = Convert.ToInt32(mileage);
					ViewData["AdvSearchMileageRange"] = attr._mileageFrom + " - " + attr._mileageTo;
				}
			}
			else
			{
				attr._mileageFrom = -1;
				attr._mileageTo = -1;
			}
			if(!String.IsNullOrEmpty(priceMin) && !String.IsNullOrEmpty(priceMax))
			{
				int minPrice, maxPrice;
				if(int.TryParse(priceMin, out minPrice) && int.TryParse(priceMax, out maxPrice))
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
			attr._maxPrice = priceMax.ConvertTo<decimal>(-1);
			attr._minPrice = priceMin.ConvertTo<decimal>(-1);
			attr._minYaer = yearMin.ConvertTo(-1);
			attr._maxYear = yearMax.ConvertTo(-1);
			var modelID = model.GetValueOrDefault(-1);
			attr._model = modelID;
			attr._radius = radius.ToString();
			attr._transmission = transmission;
			attr._Type = type ?? -1;
			attr._vin = vin ?? "";
			attr._zip = zip.ConvertTo(-1);
			int count;
			List<Core.Model.Products> collection = _productService.SearchAdvanced(attr, 25, 0, null, out count);
			ViewData["pageIndex"] = 0;
			mileage = mileage == "" ? "-1" : mileage;
			BindFilters(make ?? -1, modelID, type.ToString(), mileage, body, attr._minYaer.ToString(), attr._maxYear.ToString(), priceMin, priceMax);
			ViewData["Vin"] = vin;
			ViewData["Zip"] = zip;
			ViewData["Transmission"] = transmission;
			ViewData["Engine"] = engine;
			ViewData["Radius"] = radius;
			var carsCount = _productService.SearchAdvancedCount(attr, 1, 0, null, out count);
			ViewData["CarsCount"] = carsCount;
			ViewData["PageCounts"] = carsCount / 25 + 1;
			return View("Index", collection);
		}

		[HttpPost]
		public ActionResult SearchOnSearchPage(
			String price, String mileage, String make, String model, String year,
			String vin, string driveType, string transmission, string engine,
			String body, String type, String zip, string warranty, String sortByColumn,
			String pageSize, String pageIndex, string hiddenSearchKey, int searchByDealerID, bool isSort = false, int? radius = null)
		{
			var searchPageSearchSession = new SearchSession();
			searchPageSearchSession.prpSearchType = SearchType.SearchonSearchPage;
			searchPageSearchSession.prpSearchonSearchPageParameter = new SearchOnSearchPage
			{
				Price = price,
				Mileage = mileage,
				Make = make,
				Model = model,
				Year = year,
				Vin = vin,
				DriveType = driveType,
				Transmission = transmission,
				Engine = engine,
				Body = body,
				Type = type,
				Zip = zip,
				Warranty = warranty,
				sortByColumn = sortByColumn,
				pageSize = pageSize,
				PageIndex = pageIndex,
				hiddenSearchKey = hiddenSearchKey,
				SearchByDealerID = searchByDealerID
			};
			Session["SearchType"] = searchPageSearchSession;
		
			int? carsCount = null;
			ViewData["pageIndex"] = pageIndex;

			if(String.IsNullOrEmpty(model))
				model = "-1";
			if(String.IsNullOrEmpty(zip))
				zip
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

			var vehicles = _productService.SearchStandard(hiddenSearchKey, Convert.ToInt32(pageSize), Convert.ToInt32(pageIndex),
				                            ref carsCount, sortByColumn, ref price, ref mileage, ref make, ref model, ref year,
				                            ref body, Convert.ToInt32(zip), radius, warranty);

			ViewData["CarsCount"] = carsCount;
			int countDownDays = GetCountDownDays();
			ViewData["CountDownDays"] = countDownDays;
			return PartialView("_SearchResult", vehicles);
		}

		private static int GetCountDownDays()
		{
			int countDownDays = 0;
			using(eDriveAutoWebEntities entity = new eDriveAutoWebEntities())
			{
				var set = entity.Settings.FirstOrDefault(m => m.Name == "CountDownDays");
				if(set != null)
				{
					countDownDays = Convert.ToInt32(set.Value);
				}
			}
			return countDownDays;
		}

		[HttpPost]
		public JsonResult SearchOnSearchPage_CarsCount(String price, String mileage, String make, String model, String year,
			String vin, string driveType, string transmission, string engine,
			 String body, String type, String zip, string warranty, String sortByColumn,
			String pageSize, String pageIndex, string hiddenSearchKey, int searchByDealerID, int? radius = null)
		{
			int? carsCount = null;
			ViewData["pageIndex"] = pageIndex;
				
			if(String.IsNullOrEmpty(model))
				model = "-1";
			if(String.IsNullOrEmpty(zip))
				zip = "-1";

			carsCount = _productService.SearchStandardCount(hiddenSearchKey, 1, 1, ref carsCount, null, ref price, ref mileage,
				                                            ref make, ref model, ref year, ref body, Convert.ToInt32(zip), null,
				                                            warranty);

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
			String body = "-1", String minYear = "", 
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
