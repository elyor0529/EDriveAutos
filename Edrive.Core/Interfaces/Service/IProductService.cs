using System;
using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Core.Interfaces.Service
{
	public partial interface IProductService : IService<Products>
	{
		List<Products> SearchStandard(string searchText, int pageSize, int pageIndex, ref int? carsCount, string sortByColumn,
		                              ref string price, ref string mileage, ref string make,
		                              ref string model, ref String year, ref string body, int zip, int? radius, String warrantyStr);

		int SearchStandardCount(string searchText, int pageSize, int pageIndex, ref int? carsCount, string sortByColumn,
		                        ref string price, ref string mileage, ref string make,
		                        ref string model, ref String year, ref string body, int zip, int? radius, String warrantyStr);

		List<Products> SearchAdvanced(AdvancedSearchAttributes attributes, int pageSize, int pageIndex, string sortByColumn, out int count);

		int SearchAdvancedCount(AdvancedSearchAttributes attributes, int pageSize, int pageIndex, string sortByColumn, out int count);

		List<Product_Picture> GetProductPicture_By_ProductID(int productID);

		void AddPicsToProduct(string[] pics, int productID);
	}
}