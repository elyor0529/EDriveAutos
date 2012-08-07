using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IProductBodyService
	{
		List<Product_Body> GetAll();

		Product_Body AddProductBody(string bodyName);
	}
}