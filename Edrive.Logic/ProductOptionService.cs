using System.Linq;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class ProductOptionService : IProductOptionService
	{
		public ProductOptions AddProductOption(int productID, string productOptionName)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				ProductOptions options = null;
				var productOption = entities.ProductOption.FirstOrDefault(m => m.OptionName == productOptionName);

				if(productOption == null)
				{
					productOption = new ProductOption
					                	{
					                		OptionName = productOptionName
					                	};

					entities.ProductOption.AddObject(productOption);
					entities.SaveChanges();
				}

				if(!entities.ProductOptionMapping.Any(m => m.ProductID == productID && m.OptionID == productOption.id))
				{
					var newProductOption = new ProductOptionMapping
					                       	{
					                       		OptionID = productOption.id,
					                       		ProductID = productID
					                       	};

					entities.ProductOptionMapping.AddObject(newProductOption);
					entities.SaveChanges();

					options = new ProductOptions
					                         	{
					                         		id = productOption.id,
					                         		OptionName = productOption.OptionName
					                         	};

					
				}

				return options;
			}
		}
	}
}