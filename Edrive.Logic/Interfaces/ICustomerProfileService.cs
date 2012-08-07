using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface ICustomerProfileService
	{
		_CustomerProfile GetProfile(int dealerID);
	}
}