using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IProductTypeService
	{
		List<Product_Type> GetAll();

		Product_Type GetProductTypeByType(string type);
	}
}