using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IProductOptionService
	{
		ProductOptions AddProductOption(int productID, string productOptionName);
	}
}