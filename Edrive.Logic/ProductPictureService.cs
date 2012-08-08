using System;
using System.Collections.Generic;
using System.Linq;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class ProductPictureService : IProductPictureService
	{
		public List<Product_Picture> GetProductPicture_By_ProductID(int productID)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				if(entities.ProductPicture.Any(m => m.ProductID == productID) == false)
				{
					var pics = entities.Product.FirstOrDefault(m => m.ProductId == productID);

					if(pics != null && pics.Pics != null)
					{
						var picsarray = pics.Pics.Split(';');
						AddPicsToProduct(picsarray, productID);
					}
				}

				return entities.ProductPicture.Where(m => m.ProductID == productID)
					.OrderBy(m => m.DisplayOrder)
					.Select(m => new Product_Picture
					{
						ProductPictureID = m.ProductPictureID,
						ProductID = m.ProductID,
						PictureURL = m.PictureURL,
						DisplayOrder = m.DisplayOrder
					}).ToList();
			}
		}

		public void AddPicsToProduct(string[] pics, int productID)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				for(int i = 0; i < pics.Length; i++)
				{
					if(!String.IsNullOrWhiteSpace(pics[i]))
						entities.ProductPicture.AddObject(new ProductPicture { DisplayOrder = i + 1, PictureURL = pics[i], ProductID = productID });
				}

				entities.SaveChanges();
			}
		}

		public void GetProductsPictures(IEnumerable<Products> lstModel)
		{
			foreach(var item in lstModel)
			{
				List<Product_Picture> picture = GetProductPicture_By_ProductID(item.productId);

				if(picture != null)
				{
					var picUrls = "";
					foreach(var picitem in picture)
					{
						if(String.IsNullOrEmpty(picitem.PictureURL) == false)
						{
							picUrls += picitem.PictureURL + ";";
						}
					}
					if(picUrls != "")
					{
						item.pics = picUrls;
					}
				}
			}
		}
	}
}