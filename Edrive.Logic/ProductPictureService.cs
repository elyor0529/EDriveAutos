using System;
using System.Collections.Generic;
using System.Linq;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class ProductPictureService : BaseService, IProductPictureService
	{
		public List<Product_Picture> GetProductPicture_By_ProductID(int productID)
		{
			using(Context = GetDataContext())
			{
				if(Context.VEHICLE_IMAGES.Any(m => m.VEHICLE_ID == productID) == false)
				{
					var pics = Context.VEHICLEs.FirstOrDefault(m => m.ID == productID);

					if(pics != null && pics.PICS != null)
					{
						var picsarray = pics.PICS.Split(';');
						AddPicsToProduct(picsarray, productID);
					}
				}

				return Context.VEHICLE_IMAGES.Where(m => m.VEHICLE_ID == productID)
					.OrderBy(m => m.DISPLAY_ORDER)
					.Select(m => new Product_Picture
					{
						ProductPictureID = m.ID,
						ProductID = m.VEHICLE_ID,
						PictureURL = m.URL,
						DisplayOrder = m.DISPLAY_ORDER
					}).ToList();
			}
		}

		public void AddPicsToProduct(string[] pics, int productID)
		{
			using(Context = GetDataContext())
			{
				for(int i = 0; i < pics.Length; i++)
				{
					if(!String.IsNullOrWhiteSpace(pics[i]))
						Context.VEHICLE_IMAGES.AddObject(new VEHICLE_IMAGES
						                                 	{
						                                 		DISPLAY_ORDER = i + 1,
						                                 		URL = pics[i],
						                                 		VEHICLE_ID = productID
						                                 	});
				}

				Context.SaveChanges();
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