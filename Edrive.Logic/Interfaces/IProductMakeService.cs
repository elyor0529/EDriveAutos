using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IProductMakeService
	{
		List<Product_Make> GetAll();
	}
}