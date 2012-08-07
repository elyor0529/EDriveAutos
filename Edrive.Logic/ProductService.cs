using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
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
					RESERVED = product.Reserved,
					Title = product.Title,
					Date_in_Stock = DateTime.Now,
					Deleted = product.deleted,
					Model = product.model,
					Offer = product.Offer,
					Description = product.descriptiont,
					Drive_Type = product.drive_Type,
					Engine = product.engine,
					Exterior_Color = product.exterior,
					FileName = product.fileName,
					Free_Text = product.free_Text,
					Fuel_Type = product.fuel_Type,
					Highway_Fuel = product.highWay_Fuel,
					Interior_Color = product.interior,
					IsFeature = product.isfeature,
					IsNew = product.isNew,
					Mileage = product.mileage,
					Name = string.Format("{0} {1} {2} {3} {4}", product.Make, product.ModelName, product.vin, product.Year, product.SellerZip.ToString().PadLeft(5)),
					OwnerDetail = product.OwnerDetail,
					Pics = product.pics,
					UpdatedOn = DateTime.Now,
					Price_Current = product.price_Current,
					VIN = product.vin,
					Year = product.Year,
					Type = product.type,
					SellerEmail = product.SellerEmail,
					SellerName = product.SellerName,
					zip = product.SellerZip
				};

				Context.Product.AddObject(newProduct);
				Context.SaveChanges();

				return newProduct.ProductId;
			}
		}

		public Products GetProductByID(int productID)
		{
			var adPics = new ProductPictureService().GetProductPicture_By_ProductID(productID);

			using(Context = GetDataContext())
			{
				Product product = Context.Product.FirstOrDefault(m => m.ProductId == productID);

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

				product.Pics = pictureUrl;
				var lst = new Products
				          	{
				          		bodyID = product.Body,
				          		productId = product.ProductId,
				          		pics = product.Pics,
				          		mileage = product.Mileage ?? 0,
				          		body = product.Product_Body.Body,
				          		price_Current = product.Price_Current ?? 0,
				          		price_WholeSale = Convert.ToDecimal(product.Price_WholeSale ?? 0),
				          		price_Cost = Convert.ToDecimal(product.Price_Cost ?? 0),
				          		VehicleType = product.Product_Type.Type,
				          		transmission = product.Transmission,
				          		exterior = product.Exterior_Color,
				          		OwnerDetail = product.OwnerDetail,
				          		drive_Type = product.Drive_Type,
				          		vin = product.VIN,
				          		//zip = prd.zip??0,
				          		zip = product.Customer.ZipPostalCode ?? 0,
				          		Year = product.Year ?? 0,
				          		type = product.Type,
				          		Make = product.Product_Model.Product_Make.Make,
				          		MakeName = product.Product_Model.Product_Make.Make,
				          		ModelName = product.Product_Model.ModeLName,
				          		model = product.Model,
				          		descriptiont = product.Description ?? "",
				          		updatedOn = product.UpdatedOn,
				          		stock = product.Stock,
				          		doors = product.Doors ?? 0,
				          		Reserved = product.Reserved,
				          		fuel_Type = product.Fuel_Type,
				          		Title = product.Title,
				          		condition = product.Condition,
				          		free_Text = product.Free_Text,
				          		isfeature = product.IsFeature ?? false,
				          		customerId = product.CustomerID ?? 0,
				          		ShowOnDealerProfile = product.ShowOnDealerProfile ?? false,
				          		engine = product.Engine,
				          		date_in_Stock = product.Date_in_Stock ?? DateTime.Now,
				          		interior = product.Interior_Color,
				          		trim = product.Trim,
				          		savingAmount = product.SavingAmount ?? 0,
				          		averageRetailPrice = product.AverageRetailPrice ?? 0,
				          		averageTradeinPrice = product.AverageTradeinPrice ?? 0,
				          		qualifyPrice = product.QualifyPrice ?? 0,
				          		city_Fuel = product.City_Fuel ?? 0,
				          		highWay_Fuel = product.Highway_Fuel ?? 0,
				          		name = product.Name,
				          		fileName = product.FileName,
				          		warranty = product.Warranty ?? false
				          	};
				if (lst.customerId == 0) //then add admin id
				{
					var admin = Context.Customer.FirstOrDefault(m => m.Deleted == false && m.Customer_Type.Role == "Admin");

					if (admin != null)
					{
						lst.customerId = admin.CustomerID;
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
				var lst = Context.Product.Where(m => productIDs.Contains(m.ProductId));
				if (lst.Any())
				{
					lstProducts = lst.Select(prd => new Products
					                                	{
					                                		productId = prd.ProductId,
					                                		pics = prd.Pics,
					                                		mileage = prd.Mileage ?? 0,
					                                		body = prd.Product_Body.Body,
					                                		price_Current = prd.Price_Current ?? 0,
					                                		WholeSalePrice = prd.Price_WholeSale,
															PriceCost = prd.Price_Cost,
					                                		VehicleType = prd.Product_Type.Type,
					                                		transmission = prd.Transmission,
					                                		exterior = prd.Exterior_Color,
					                                		OwnerDetail = prd.OwnerDetail,
					                                		drive_Type = prd.Drive_Type,
					                                		vin = prd.VIN,
					                                		//zip = prd.zip??0,
					                                		zip = prd.Customer.ZipPostalCode ?? 0,
					                                		Year = prd.Year ?? 0,
					                                		type = prd.Type,
					                                		Make = prd.Product_Model.Product_Make.Make,
					                                		MakeName = prd.Product_Model.Product_Make.Make,
					                                		ModelName = prd.Product_Model.ModeLName,
					                                		model = prd.Model,
					                                		descriptiont = prd.Description ?? "",
					                                		updatedOn = prd.UpdatedOn,
					                                		stock = prd.Stock,
					                                		doors = prd.Doors ?? 0,
					                                		Reserved = prd.Reserved,
					                                		fuel_Type = prd.Fuel_Type,
					                                		Title = prd.Title,
					                                		condition = prd.Condition,
					                                		free_Text = prd.Free_Text,
					                                		isfeature = prd.IsFeature ?? false,
					                                		customerId = prd.CustomerID ?? 0,
					                                		ShowOnDealerProfile = prd.ShowOnDealerProfile ?? false,
					                                		engine = prd.Engine,
					                                		date_in_Stock = prd.Date_in_Stock ?? DateTime.Now,
					                                		interior = prd.Interior_Color,
					                                		trim = prd.Trim
					                                	}).ToList();
				}
			}

			new ProductPictureService().GetProductsPictures(lstProducts);
			
			return lstProducts;
		} 
		
		public List<Products> GetProductsByDealerID(int dealerID)
		{
			List<Products> lstModel = null;

			using(Context = GetDataContext())
			{
				lstModel = Context.Product.Where(m => m.CustomerID == dealerID && m.Deleted == false)
					.OrderByDescending(m => m.CreatedOn)
					.ThenByDescending(m => m.ProductId)
					.Take(8).Select(m => new Products
					                     	{
					                     		productId = m.ProductId,
					                     		pics = m.Pics,
					                     		mileage = m.Mileage ?? 0,
					                     		ModelName = m.Product_Model.ModeLName,
					                     		Year = m.Year ?? 0,
					                     		model = m.Model,
					                     		descriptiont = m.Description ?? "",
					                     		updatedOn = m.UpdatedOn,
					                     		stock = m.Stock,
					                     		engine = m.Engine,
					                     		Make = m.Product_Model.Product_Make.Make,
					                     		price_Current = m.Price_Current ?? 0,
					                     		interior = m.Interior_Color,
					                     		trim = m.Trim,
					                     		body = m.Product_Body.Body,
					                     		bodyID = m.Body
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
			List<Products> result = new List<Products>();

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
				var rating = Context.Product_Rating.FirstOrDefault(m => m.ProductID == productID);
				
				if (rating != null)
				{
					var averageRating = rating.AvgRating ?? 0;
					var totalVotes = rating.TotaVotes ?? 0;
					rating.AvgRating = ((averageRating * totalVotes) + score) / (totalVotes + 1);
					rating.TotaVotes = totalVotes + 1;
				}
				else
				{
					rating = new Product_Rating
					         	{
					         		AvgRating = score,
					         		TotaVotes = 1,
					         		ProductID = productID
					         	};

					Context.Product_Rating.AddObject(rating);
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

				return Context.Product.Any(m => m.VIN == vin && m.Deleted == false);
			}
		}

		public int GetTotalVehiclesCount()
		{
			using(Context = GetDataContext())
			{
				int totalCount = Context.QualifiedVehiclesCount().FirstOrDefault().GetValueOrDefault(0);

				return totalCount;
			}
		}

		public List<Products> GetDealerFeaturedVehicles(int pageSize)
		{
			using(Context = GetDataContext())
			{
				List<Products> result = new List<Products>();
				var customer = Context.Customer.FirstOrDefault(m => m.IsFeatured == true);

				if(customer == null)
					return result;
				
				string cityName = "";

				if (customer.Stateid != null)
					cityName = customer.StateProvince.Abbreviation;
				
				var ownerDet = customer.FirstName + ", " + cityName;
				
					result = Context.Product.Where(m => m.CustomerID == customer.CustomerID).Take(pageSize).
						Select(m => new Products
							            {
							            	productId = m.ProductId,
							            	Year = m.Year ?? 0,
							            	MakeName = m.Product_Model.Product_Make.Make,
							            	ModelName = m.Product_Model.ModeLName,
							            	body = m.Product_Body.Body,
							            	mileage = m.Mileage ?? 0,
							            	exterior = m.Exterior_Color,
							            	customerId = m.CustomerID ?? 0,
							            	OwnerDetail = ownerDet,
							            	price_Current = m.Price_Current ?? 0,
											savingAmount = m.SavingAmount ?? 0
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
					productId = c.ProductId,
					pics = c.Pics,
					mileage = c.Mileage ?? 0,
					body = c.ProductBodyName,
					price_Current = c.Price_Current ?? 0,
					transmission = c.Transmission ?? "",
					exterior = c.Exterior_Color ?? "",
					OwnerDetail = c.OwnerDetail,
					drive_Type = c.Drive_Type,
					vin = c.VIN,
					MakeName = c.MakeName,
					ModelName = c.ModelName,
					//zip = c.zip??0,
					zip = c.ZipPostalCode ?? 0,
					Year = c.Year ?? 0,
					model = c.Model,
					updatedOn = c.UpdatedOn,
					customerId = c.CustomerID ?? 0,
					savingAmount = c.SavingAmount ?? 0,
					CustomerCity = c.City ?? "",
					CustomerState = c.StateProvince ?? ""
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
				var query = Context.Product.Where(c => c.Transmission != null && c.Transmission.Trim() != "")
					.Select(c => c.Transmission).Distinct().ToList();

				return query;
			}
		}

		public List<string> GetEnginesList()
		{
			using(Context = GetDataContext())
			{
				var query = Context.Product.Where(c => c.Engine != null && c.Engine.Trim() != "")
					.Select(c => c.Engine).Distinct().ToList();

				return query;
			}
		}

		public List<string> GetDriveTypesList()
		{
			using(Context = GetDataContext())
			{
				var query = Context.Product.Where(c => c.Drive_Type != null && c.Drive_Type.Trim() != "")
					.Select(c => c.Drive_Type).OrderBy(c => c).Distinct().ToList();

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

				var query = Context.Product_Model.Select(m => new
				                                               	{
				                                               		name = m.Product_Make.Make + " " + m.ModeLName,
				                                               		model = m.id,
				                                               		modelName = m.ModeLName,
				                                               		makeName = m.Product_Make.Make
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
				//zip 		  = entity.zip??0,
				ZipCode = entity.DEALER_SALESPERSON.DEALER.ZIP,
				Year = entity.YEAR ?? 0,
				model = entity.MODEL_ID,
				interior = entity.COLOR_INTERIOR,
				updatedOn = entity.DATE_UPDATED,
				descriptiont = entity.DESCRIPTION,
				CustomerCity = entity.DEALER_SALESPERSON.DEALER.CITY
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