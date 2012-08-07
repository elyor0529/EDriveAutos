using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Edrive.Core.Enums;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class ProductService : BaseService, IProductService
	{
		public int AddProduct(Products product)
		{
			using(Context = GetDataContext())
			{
				VEHICLE newProduct = new VEHICLE
				{
					PRICE_AVERAGERETAIL = product.averageRetailPrice,
					PRICE_AVERAGETRADEIN = product.averageTradeinPrice,
					BODY_ID = product.bodyID,
					FUEL_CITY = product.city_Fuel,
					CONDITION = product.condition,
					DATE_CREATED = DateTime.Now,
					SALESPERSON_ID = product.customerId,
					STOCKNUMBER = product.stock,
					TRIM = product.trim,
					TRANSMISSION = product.transmission,
					ISWARRANTY = product.warranty,
					RESERVED = product.ReservedStr,
					TITLE = product.Title,
					DATE_INSTOCK = DateTime.Now,
					ISDELETED = product.deleted,
					MODEL_ID = product.model,
					ISOFFER = product.Offer,
					DESCRIPTION = product.descriptiont,
					DRIVE_TYPE = product.drive_Type,
					ENGINE = product.engine,
					COLOR_EXTERIOR = product.exterior,
					FILENAME = product.fileName,
					FUEL_TYPE = product.fuel_Type,
					FUEL_HIGHWAY = product.highWay_Fuel,
					COLOR_INTERIOR = product.interior,
					ISFEATURE = product.isfeature,
					ISNEW = product.isNew,
					MILEAGE = product.mileage,
					OWNERDETAIL = product.OwnerDetail,
					PICS = product.pics,
					DATE_UPDATED = DateTime.Now,
					PRICE = product.price_Current,
					VIN = product.vin,
					YEAR = product.Year,
					TYPE_ID = product.type,
					ZIP = product.ZipCode
				};

				Context.VEHICLEs.AddObject(newProduct);
				Context.SaveChanges();

				return newProduct.ID;
			}
		}

		public Products GetProductByID(int productID)
		{
			var adPics = new ProductPictureService().GetProductPicture_By_ProductID(productID);

			using(Context = GetDataContext())
			{
				var product = Context.VEHICLEs.FirstOrDefault(m => m.ID == productID);

				string pictureUrl = string.Empty;

				foreach (var item in adPics)
				{
					if (String.IsNullOrEmpty(item.PictureURL) == false)
						pictureUrl += item.PictureURL + ";";
				}

				if (pictureUrl.IndexOf(';') > 0)
				{
					pictureUrl = pictureUrl.Substring(0, pictureUrl.LastIndexOf(';'));
				}

				product.PICS = pictureUrl;
				var lst = new Products
				          	{
				          		bodyID = product.BODY_ID,
				          		productId = product.ID,
				          		pics = product.PICS,
				          		mileage = product.MILEAGE ?? 0,
				          		body = product.LST_VEHICLEBODY.NAME,
				          		price_Current = product.PRICE ?? 0,
				          		price_WholeSale = Convert.ToDecimal(product.PRICE_WHOLESALE ?? 0),
				          		price_Cost = Convert.ToDecimal(product.PRICE_COST ?? 0),
				          		VehicleType = product.LST_VEHICLETYPE.NAME,
				          		transmission = product.TRANSMISSION,
				          		exterior = product.COLOR_EXTERIOR,
				          		OwnerDetail = product.OWNERDETAIL,
				          		drive_Type = product.DRIVE_TYPE,
				          		vin = product.VIN,
				          		//zip = prd.zip??0,
				          		ZipCode = product.ZIP,
				          		Year = product.YEAR ?? 0,
				          		type = product.TYPE_ID,
				          		Make = product.LST_VEHICLEMODEL.LST_VEHICLEMAKE.NAME,
				          		MakeName = product.LST_VEHICLEMODEL.LST_VEHICLEMAKE.NAME,
				          		ModelName = product.LST_VEHICLEMODEL.NAME,
				          		model = product.MODEL_ID,
				          		descriptiont = product.DESCRIPTION ?? "",
				          		updatedOn = product.DATE_UPDATED,
				          		stock = product.STOCKNUMBER,
				          		doors = product.DOORS ?? 0,
				          		ReservedStr = product.RESERVED,
				          		fuel_Type = product.FUEL_TYPE,
				          		Title = product.TITLE,
				          		condition = product.CONDITION,
				          		isfeature = product.ISFEATURE ?? false,
				          		customerId = product.SALESPERSON_ID ?? 0,
				          		ShowOnDealerProfile = product.ISONDEALERPROFILE ?? false,
				          		engine = product.ENGINE,
				          		date_in_Stock = product.DATE_INSTOCK ?? DateTime.Now,
				          		interior = product.COLOR_INTERIOR,
				          		trim = product.TRIM,
				          		savingAmount = product.SAVINGS ?? 0,
				          		averageRetailPrice = product.PRICE_AVERAGERETAIL ?? 0,
				          		averageTradeinPrice = product.PRICE_AVERAGETRADEIN ?? 0,
				          		qualifyPrice = product.PRICE_QUALIFY ?? 0,
				          		city_Fuel = product.FUEL_CITY ?? 0,
				          		highWay_Fuel = product.FUEL_HIGHWAY ?? 0,
				          		fileName = product.FILENAME,
				          		warranty = product.ISWARRANTY ?? false
				          	};

				if (lst.customerId == 0) //then add admin id
				{
					int userType = (int)UserType.Admin;
					var admin = Context.DEALER_SALESPERSON.FirstOrDefault(m => m.ISDELETED == false && m.DEALERTYPE_ID == userType);

					if (admin != null)
					{
						lst.customerId = admin.ID;
					}
				}

				return lst;
			}
		}

		public List<Products> GetProductsByIDs(IEnumerable<int> productIDs)
		{
			List<Products> lstProducts = new List<Products>();

			using(Context = GetDataContext())
			{
				var lst = Context.VEHICLEs.Where(m => productIDs.Contains(m.ID));
				if (lst.Any())
				{
					lstProducts = lst.Select(prd => new Products
					                                	{
					                                		productId = prd.ID,
					                                		pics = prd.PICS,
					                                		mileage = prd.MILEAGE ?? 0,
					                                		body = prd.LST_VEHICLEBODY.NAME,
					                                		price_Current = prd.PRICE ?? 0,
															price_WholeSale = prd.PRICE_WHOLESALE ?? 0,
															price_Cost = prd.PRICE_COST ?? 0,
					                                		VehicleType = prd.LST_VEHICLETYPE.NAME,
					                                		transmission = prd.TRANSMISSION,
					                                		exterior = prd.COLOR_EXTERIOR,
					                                		OwnerDetail = prd.OWNERDETAIL,
					                                		drive_Type = prd.DRIVE_TYPE,
					                                		vin = prd.VIN,
					                                		//zip = prd.zip??0,
					                                		ZipCode = prd.ZIP,
					                                		Year = prd.YEAR ?? 0,
					                                		type = prd.TYPE_ID,
					                                		Make = prd.LST_VEHICLEMODEL.LST_VEHICLEMAKE.NAME,
					                                		MakeName = prd.LST_VEHICLEMODEL.LST_VEHICLEMAKE.NAME,
					                                		ModelName = prd.LST_VEHICLEMODEL.NAME,
					                                		model = prd.MODEL_ID,
					                                		descriptiont = prd.DESCRIPTION ?? "",
					                                		updatedOn = prd.DATE_UPDATED,
					                                		stock = prd.STOCKNUMBER,
					                                		doors = prd.DOORS ?? 0,
					                                		ReservedStr = prd.RESERVED,
					                                		fuel_Type = prd.FUEL_TYPE,
					                                		Title = prd.TITLE,
					                                		condition = prd.CONDITION,
					                                		isfeature = prd.ISFEATURE ?? false,
					                                		customerId = prd.SALESPERSON_ID ?? 0,
					                                		ShowOnDealerProfile = prd.ISONDEALERPROFILE ?? false,
					                                		engine = prd.ENGINE,
					                                		date_in_Stock = prd.DATE_INSTOCK ?? DateTime.Now,
					                                		interior = prd.COLOR_INTERIOR,
					                                		trim = prd.TRIM
					                                	}).ToList();
				}
			}

			new ProductPictureService().GetProductsPictures(lstProducts);
			
			return lstProducts;
		} 
		
		public List<Products> GetProductsByDealerID(int dealerID)
		{
			List<Products> lstModel;

			using(Context = GetDataContext())
			{
				lstModel = Context.VEHICLEs.Where(m => m.SALESPERSON_ID == dealerID && m.ISDELETED == false)
					.OrderByDescending(m => m.DATE_CREATED)
					.ThenByDescending(m => m.ID)
					.Take(8).Select(m => new Products
					                     	{
					                     		productId = m.ID,
					                     		pics = m.PICS,
					                     		mileage = m.MILEAGE ?? 0,
					                     		ModelName = m.LST_VEHICLEMODEL.NAME,
					                     		Year = m.YEAR ?? 0,
					                     		model = m.MODEL_ID,
					                     		descriptiont = m.DESCRIPTION ?? "",
					                     		updatedOn = m.DATE_UPDATED,
					                     		stock = m.STOCKNUMBER,
					                     		engine = m.ENGINE,
					                     		Make = m.LST_VEHICLEMODEL.LST_VEHICLEMAKE.NAME,
					                     		price_Current = m.PRICE ?? 0,
					                     		interior = m.COLOR_INTERIOR,
					                     		trim = m.TRIM,
					                     		body = m.LST_VEHICLEBODY.NAME,
					                     		bodyID = m.BODY_ID
					                     	}
					).ToList();
			}

			new ProductPictureService().GetProductsPictures(lstModel);
			return lstModel;
		}

		public List<Products> SearchStandard(string searchText, int pageSize, int pageIndex, ref int? carsCount, string sortByColumn,
		  ref string price, ref string mileage, ref string make,
		  ref string model, ref String year, ref string body, int zip, int? radius, String warrantyStr)
		{
			if(pageIndex < 1)
				pageIndex = 1;
			else
				pageIndex += 1;

			sortByColumn = GetSortByString(sortByColumn);

			if(String.IsNullOrWhiteSpace(searchText))
				searchText = null;
			else
				searchText = searchText.Trim().Replace(" and ", " ");

			model = GetValue(model);
			make = GetValue(make);
			year = GetValue(year);
			body = GetValue(body);
			price = GetValue(price);
			mileage = GetValue(mileage);
			string zipCode = zip > 0 ? zip.ToString().PadLeft(5, '0') : null;
			string sortColumn = sortByColumn.Split(' ')[0].Trim();
			string sortOrder = sortByColumn.EndsWith(" desc") ? "desc" : null;
			bool? warranty = null;
			radius = radius.HasValue && radius > 0 ? radius : null;

			if(!String.IsNullOrWhiteSpace(zipCode) && radius.GetValueOrDefault(0) <= 0)
				radius = 100;//default radius in miles.
			if(String.IsNullOrWhiteSpace(zipCode))
				radius = null;

			if(!String.IsNullOrWhiteSpace(warrantyStr) && !warrantyStr.StartsWith("-1"))
				warranty = warrantyStr == "1";

			ObjectParameter totalCount = new ObjectParameter("tOTALRESULTS", 0);
			ObjectParameter totalPages = new ObjectParameter("tOTALPAGES", 0);
			ObjectParameter exactMatch = new ObjectParameter("eXACTMATCH", 0);

			using(Context = GetDataContext())
			{
				var result = Context.SearchStandard(searchText, zipCode, radius, price, year, mileage, make, model,
				                                     warranty, body, true, sortColumn, sortOrder, pageSize, pageIndex, totalCount,
				                                     totalPages, exactMatch).Select(ConvertType).ToList();

				carsCount = (int) totalCount.Value;

				return result;
			}
		}

		public int SearchStandardCount(string searchText, int pageSize, int pageIndex, ref int? carsCount, string sortByColumn,
			ref string price, ref string mileage, ref string make,
			ref string model, ref String year, ref string body, int zip, int? radius, String warrantyStr)
		{
			int? count = 0;

			SearchStandard(searchText, 1, 1, ref count,
						   null, ref price, ref mileage, ref make, ref model, ref year, ref body, zip, radius, warrantyStr);

			carsCount = count;

			return count.GetValueOrDefault(0);
		}

		public List<Products> SearchAdvanced(AdvancedSearchAttributes attributes, int pageSize, int pageIndex, string sortByColumn, out int count)
		{
			if (pageIndex < 1)
				pageIndex = 1;
			else
				pageIndex += 1;

			sortByColumn = GetSortByString(sortByColumn);

			string zipCode = attributes._zip > 0 ? attributes._zip.ToString().PadLeft(5, '0') : null;
			string sortColumn = sortByColumn.Split(' ')[0].Trim();
			string sortOrder = sortByColumn.EndsWith(" desc") ? "desc" : null;
			string vin = !String.IsNullOrWhiteSpace(attributes._vin) ? attributes._vin.Trim() : null;
			int? radius = Convert.ToInt32(attributes._radius);
			radius = radius > 0 ? radius : null;

			ObjectParameter totalCount = new ObjectParameter("tOTALRESULTS", 0);
			ObjectParameter totalPages = new ObjectParameter("tOTALPAGES", 0);
			List<Products> result;

			using(Context = GetDataContext())
			{
				result = Context.SearchAdvanced(attributes._make, attributes._model, attributes._mileageTo, attributes._minYaer,
				                                 attributes._maxYear,
				                                 attributes._minPrice, attributes._maxPrice, attributes._body,
				                                 attributes._transmission,
				                                 attributes._engine, attributes._driveType,
				                                 vin, zipCode, true, radius, sortColumn, sortOrder, pageSize, pageIndex,
				                                 totalCount, totalPages)
					.Select(ConvertType).ToList();

				count = (int) totalCount.Value;
			}

			new ProductPictureService().GetProductsPictures(result);

			return result;
	}

		public int SearchAdvancedCount(AdvancedSearchAttributes attributes, int pageSize, int pageIndex, string sortByColumn, out int count)
		{
			SearchAdvanced(attributes, 1, 1, null, out count);

			return count;
		}

		public bool AddProductRating(int productID, int score, string username)
		{
			using(Context = GetDataContext())
			{
				var rating = Context.VEHICLE_RATING.FirstOrDefault(m => m.VEHICLE_ID == productID);
				
				if (rating != null)
				{
					var averageRating = rating.AVE_RATING;
					var totalVotes = rating.TOTAL_VOTES;
					rating.AVE_RATING = ((averageRating * totalVotes) + score) / (totalVotes + 1);
					rating.TOTAL_VOTES = totalVotes + 1;
				}
				else
				{
					rating = new VEHICLE_RATING
					         	{
					         		AVE_RATING = score,
					         		TOTAL_VOTES = 1,
					         		VEHICLE_ID = productID
					         	};

					Context.VEHICLE_RATING.AddObject(rating);
				}

				Context.SaveChanges();

				return true;
			}
		}

		public bool IsProductExists(string vin)
		{
			using(Context = GetDataContext())
			{
				vin = vin.Trim();

				return Context.VEHICLEs.Any(m => m.VIN == vin && m.ISDELETED == false);
			}
		}

		public int GetTotalVehiclesCount()
		{
			using(Context = GetDataContext())
			{
				int totalCount = Context.VEHICLEs.Count(c => c.ISQUALIFIED && c.ISDELETED == false);

				return totalCount;
			}
		}

		public List<Products> GetDealerFeaturedVehicles(int pageSize)
		{
			using(Context = GetDataContext())
			{
				List<Products> result = new List<Products>();
				var customer = Context.DEALER_SALESPERSON.FirstOrDefault(m => m.ISFEATURED == true);

				if(customer == null)
					return result;
				
				string cityName = customer.DEALER.STATE;
				var ownerDet = customer.NAME + ", " + cityName;
				
					result = Context.VEHICLEs.Where(m => m.SALESPERSON_ID == customer.ID).Take(pageSize).
						Select(m => new Products
							            {
							            	productId = m.ID,
							            	Year = m.YEAR ?? 0,
							            	MakeName = m.LST_VEHICLEMODEL.LST_VEHICLEMAKE.NAME,
							            	ModelName = m.LST_VEHICLEMODEL.NAME,
							            	body = m.LST_VEHICLEBODY.NAME,
							            	mileage = m.MILEAGE ?? 0,
							            	exterior = m.COLOR_EXTERIOR,
							            	customerId = m.SALESPERSON_ID ?? 0,
							            	OwnerDetail = ownerDet,
							            	price_Current = m.PRICE ?? 0,
											savingAmount = m.SAVINGS ?? 0
							            }).ToList();

					foreach (var item in result)
					{
						if (item.exterior.Length > 15)
						{
							item.exterior = item.exterior.Substring(0, 14) + "...";
						}
					}

				return result;
			}
		}

		public List<Products> GetFeaturedVehicles(int totalCount)
		{
			List<Products> vehicles;

			using(Context = GetDataContext())
			{
				vehicles = Context.FeaturedVehicles().Select(c => new Products
				{
					productId = c.ID,
					pics = c.PICS,
					mileage = c.MILEAGE ?? 0,
					body = c.BODY_NAME,
					price_Current = c.PRICE ?? 0,
					transmission = c.TRANSMISSION ?? "",
					exterior = c.COLOR_EXTERIOR ?? "",
					OwnerDetail = c.OWNERDETAIL,
					drive_Type = c.DRIVE_TYPE,
					vin = c.VIN,
					MakeName = c.MAKE,
					ModelName = c.MODEL,
					ZipCode = c.ZIP,
					Year = c.YEAR ?? 0,
					model = c.MODEL_ID,
					updatedOn = c.DATE_UPDATED,
					customerId = c.SALESPERSON_ID ?? 0,
					savingAmount = c.SAVINGS ?? 0,
					CustomerCity = c.CITY ?? "",
					CustomerState = c.STATE ?? ""
				}).ToList();
			}

			// to update the product picture by sort order
			new ProductPictureService().GetProductsPictures(vehicles);
			foreach(var item in vehicles)
			{
				if(!String.IsNullOrEmpty(item.pics))
				{
					if(item.pics.Contains(';'))
					{
						item.pics = item.pics.Split(';')[0];
					}
				}

			}
			return vehicles;
		}

		public List<string> GetTransmissionsList()
		{
			using(Context = GetDataContext())
			{
				var query = Context.VEHICLEs.Where(c => c.TRANSMISSION != null && c.TRANSMISSION.Trim() != "")
					.Select(c => c.TRANSMISSION).Distinct().ToList();

				return query;
			}
		}

		public List<string> GetEnginesList()
		{
			using(Context = GetDataContext())
			{
				var query = Context.VEHICLEs.Where(c => c.ENGINE != null && c.ENGINE.Trim() != "")
					.Select(c => c.ENGINE).Distinct().ToList();

				return query;
			}
		}

		public List<string> GetDriveTypesList()
		{
			using(Context = GetDataContext())
			{
				var query = Context.VEHICLEs.Where(c => c.DRIVE_TYPE != null && c.DRIVE_TYPE.Trim() != "")
					.Select(c => c.DRIVE_TYPE).OrderBy(c => c).Distinct().ToList();

				return query;
			}
		}

		public List<Products> HomePageSearchHint(string searchKey, int pageSize)
		{
			using(Context = GetDataContext())
			{
				if(String.IsNullOrWhiteSpace(searchKey))
					return new List<Products>();

				var searcharray = searchKey.Trim().Split(' ');

				var query = Context.LST_VEHICLEMODEL.Select(m => new
				                                               	{
				                                               		name = m.LST_VEHICLEMAKE.NAME + " " + m.NAME,
				                                               		model = m.ID,
				                                               		modelName = m.NAME,
				                                               		makeName = m.LST_VEHICLEMAKE.NAME
				                                               	});

				query = searcharray.Select(q => q.Trim()).Aggregate(query, (current, key) => current.Where(m => m.name.Contains(key.Trim())));
				var result = query.OrderBy(m => m.makeName).ThenBy(m => m.modelName).Take(pageSize).ToList();

				return result.Select(m => new Products { name = m.name }).ToList();
			}
		} 

		#region private methods
		
		private static Products ConvertType(VEHICLE entity)
		{
			var item = new Products
			{
				productId = entity.ID,
				pics = entity.PICS,
				mileage = entity.MILEAGE ?? 0,
				body = entity.LST_VEHICLEBODY.NAME,
				price_Current = entity.PRICE ?? 0,
				transmission = entity.TRANSMISSION ?? "",
				exterior = entity.COLOR_EXTERIOR ?? "",
				OwnerDetail = entity.OWNERDETAIL,
				drive_Type = entity.DRIVE_TYPE,
				vin = entity.VIN,
				MakeName = entity.LST_VEHICLEMODEL.LST_VEHICLEMAKE.NAME,
				ModelName = entity.LST_VEHICLEMODEL.NAME,
				ZipCode = entity.ZIP,
				Year = entity.YEAR ?? 0,
				model = entity.MODEL_ID,
				interior = entity.COLOR_INTERIOR,
				updatedOn = entity.DATE_UPDATED,
				descriptiont = entity.DESCRIPTION
			};

			return item;
		}

		private string GetValue(string input)
		{
			return !String.IsNullOrWhiteSpace(input) && !input.StartsWith("-1") ? input.Trim(',').Replace(" ", "") : null;
		}

		private string GetSortByString(string sortByColumn)
		{
			if(String.IsNullOrWhiteSpace(sortByColumn) ||
				(!sortByColumn.StartsWith("Year") && !sortByColumn.StartsWith("Mileage") && !sortByColumn.StartsWith("Price") && !sortByColumn.StartsWith("savingAmount")))
				sortByColumn = "ProductId";
			if(sortByColumn.StartsWith("Price"))
			{
				if(sortByColumn.ToLower().EndsWith("desc"))
					sortByColumn = "Price_Current desc";
				else
					sortByColumn = "Price_Current";
			}

			return sortByColumn;
		}

		#endregion
	}
}