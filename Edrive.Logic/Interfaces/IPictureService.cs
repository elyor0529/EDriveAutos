using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IPictureService
	{
		Picture GetByID(int id);

		int Save(Picture item);

		bool Delete(int id);
	}
}