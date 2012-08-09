using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IProductModelService
	{
		List<Product_Model> GetModelsByMake(int makeID);

		Product_Model AddProductModel(string modelName, int makeID);
	}
}