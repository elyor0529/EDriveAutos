using System;
using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IProductService
	{
		Products GetProductByID(int productID);

		List<Products> GetProductsByIDs(IEnumerable<int> productIDs);

		List<Products> GetProductsByDealerID(int dealerID);

		List<Products> SearchStandard(string searchText, int pageSize, int pageIndex, ref int? carsCount, string sortByColumn,
		                                              ref string price, ref string mileage, ref string make,
		                                              ref string model, ref String year, ref string body, int zip, int? radius, String warrantyStr);

		int SearchStandardCount(string searchText, int pageSize, int pageIndex, ref int? carsCount, string sortByColumn,
		                                        ref string price, ref string mileage, ref string make,
		                                        ref string model, ref String year, ref string body, int zip, int? radius, String warrantyStr);

		List<Products> SearchAdvanced(AdvancedSearchAttributes attributes, int pageSize, int pageIndex, string sortByColumn, out int count);

		int SearchAdvancedCount(AdvancedSearchAttributes attributes, int pageSize, int pageIndex, string sortByColumn, out int count);

		bool AddProductRating(int productID, int score, string username);

		bool IsProductExists(string vin);

		int GetTotalVehiclesCount();

		List<Products> GetDealerFeaturedVehicles(int pageSize);

		List<string> GetTransmissionsList();

		List<string> GetEnginesList();

		List<string> GetDriveTypesList();
	}
}