using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IInterestedCustomerService
	{
		bool IntrestedCustomer(IntrestedCustomer customer, int productID);

		List<IntrestedCustomer> Get_InterestedCustomer(int interestType);
	}
}