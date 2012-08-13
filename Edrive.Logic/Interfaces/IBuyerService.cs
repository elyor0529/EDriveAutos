using Edrive.Core.Enums;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IBuyerService
	{
		Buyer GetByID(int id);

		Buyer GetByUsername(string username);

		bool SaveBuyer(Buyer item);

		Buyer Authenticate(string username, string password, UserType role);

		bool ChangePassword(int id, string newPassword);
	}
}