using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Edrive.Core.Model;
using Edrive.Data;

namespace Edrive.Service
{
	public partial class ProductService
	{
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

			var result = Entities.SearchStandard(searchText, zipCode, radius, price, year, mileage, make, model,
				warranty, body, true, sortColumn, sortOrder, pageSize, pageIndex, totalCount, totalPages, exactMatch).Select(ConvertType).ToList();

			carsCount = (int)totalCount.Value;

			return result;
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
			if(pageIndex < 1)
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

			var result = Entities.SearchAdvanced(attributes._make, attributes._model, attributes._mileageTo, attributes._minYaer,
										attributes._maxYear,
										attributes._minPrice, attributes._maxPrice, attributes._body, attributes._transmission,
										attributes._engine, attributes._driveType,
										vin, zipCode, true, radius, sortColumn, sortOrder, pageSize, pageIndex,
										totalCount, totalPages)
					.Select(ConvertType).ToList();

			count = (int)totalCount.Value;
			GetProductsPictures(result);

			return result;
		}

		public int SearchAdvancedCount(AdvancedSearchAttributes attributes, int pageSize, int pageIndex, string sortByColumn, out int count)
		{
			SearchAdvanced(attributes, 1, 1, null, out count);

			return count;
		}

		public List<Product_Picture> GetProductPicture_By_ProductID(int productID)
		{
			if(Entities.ProductPicture.Any(m => m.ProductID == productID) == false)
			{
				var pics = Entities.Product.FirstOrDefault(m => m.ProductId == productID);

				if(pics != null && pics.Pics != null)
				{
					var picsarray = pics.Pics.Split(';');
					AddPicsToProduct(picsarray, productID);
				}
			}

			return Entities.ProductPicture.Where(m => m.ProductID == productID)
				.OrderBy(m => m.DisplayOrder)
				.Select(m => new Product_Picture
				{
					ProductPictureID = m.ProductPictureID,
					ProductID = m.ProductID,
					PictureURL = m.PictureURL,
					DisplayOrder = m.DisplayOrder
				}).ToList();
		}

		public void AddPicsToProduct(string[] pics, int productID)
		{
			for(int i = 0; i < pics.Length; i++)
			{
				if(!String.IsNullOrWhiteSpace(pics[i]))
					Entities.ProductPicture.AddObject(new ProductPicture { DisplayOrder = i + 1, PictureURL = pics[i], ProductID = productID });
			}

			Entities.SaveChanges();
		}

		#region private methods

		private void GetProductsPictures(IEnumerable<Products> lstModel)
		{
			foreach(var item in lstModel)
			{
				List<Product_Picture> picture = GetProductPicture_By_ProductID(item.productId);

				if(picture != null)
				{
					var picUrls = "";
					foreach(var picitem in picture)
					{
						if(String.IsNullOrEmpty(picitem.PictureURL) == false)
						{
							picUrls += picitem.PictureURL + ";";
						}
					}
					if(picUrls != "")
					{
						item.pics = picUrls;
					}
				}
			}
		}

		private static Products ConvertType(Product entity)
		{
			var item = new Products
			           	{
							productId = entity.ProductId,
							pics = entity.Pics,
							mileage = entity.Mileage ?? 0,
							body = entity.Product_Body.Body,
							price_Current = entity.Price_Current ?? 0,
							transmission = entity.Transmission ?? "",
							exterior = entity.Exterior_Color ?? "",
							OwnerDetail = entity.OwnerDetail,
							drive_Type = entity.Drive_Type,
							vin = entity.VIN,
							MakeName = entity.Product_Model.Product_Make.Make,
							ModelName = entity.Product_Model.ModeLName,
							//zip 		  = entity.zip??0,
							zip = entity.Customer.ZipPostalCode ?? 0,
							Year = entity.Year ?? 0,
							model = entity.Model,
							interior = entity.Interior_Color,
							updatedOn = entity.UpdatedOn,
							descriptiont = entity.Description ?? ""
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