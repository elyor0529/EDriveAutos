using System.Linq;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class ProductOptionService : BaseService, IProductOptionService
	{
		public ProductOptions AddProductOption(int productID, string productOptionName)
		{
			using(Context = GetDataContext())
			{
				ProductOptions options = null;
				var productOption = Context.LST_VEHICLEOPTION.FirstOrDefault(m => m.NAME == productOptionName);

				if(productOption == null)
				{
					productOption = new LST_VEHICLEOPTION
					                	{
					                		NAME = productOptionName
					                	};

					Context.LST_VEHICLEOPTION.AddObject(productOption);
					Context.SaveChanges();
				}

				if(!Context.VEHICLE_OPTIONS.Any(m => m.VEHICLE_ID == productID && m.OPTION_ID == productOption.ID))
				{
					var newProductOption = new VEHICLE_OPTIONS
					                       	{
					                       		OPTION_ID = productOption.ID,
					                       		VEHICLE_ID = productID
					                       	};

					Context.VEHICLE_OPTIONS.AddObject(newProductOption);
					Context.SaveChanges();

					options = new ProductOptions
					                         	{
					                         		id = productOption.ID,
					                         		OptionName = productOption.NAME
					                         	};
				}

				return options;
			}
		}
	}
}