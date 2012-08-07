using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IDealerService
	{
		Customer GetByID(int customerID);

		Customer GetDealerByProductID(int productID);

		_CustomerProfile GetProfile(int dealerID);

		Customer GetDealerByDealerEmail(string email);

		int GetTotalDealersCount();

		List<Customer> GetDealersByZip(string zipcode);
	}
}