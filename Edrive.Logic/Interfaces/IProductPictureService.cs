using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IProductPictureService
	{
		List<Product_Picture> GetProductPicture_By_ProductID(int productID);

		void AddPicsToProduct(string[] pics, int productID);

		void GetProductsPictures(IEnumerable<Products> lstModel);
	}
}