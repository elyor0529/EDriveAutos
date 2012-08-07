using System.Collections.Generic;
using System.Linq;
using Edrive.Logic.Interfaces;
using Product_Type = Edrive.Core.Model.Product_Type;

namespace Edrive.Logic
{
	public class ProductTypeService : BaseService, IProductTypeService
	{
		public List<Product_Type> GetAll()
		{
			using(Context = GetDataContext())
			{
				var result = Context.LST_VEHICLETYPE.Select(c => new Product_Type
				                                               	{
				                                               		id = c.ID,
																	type = c.NAME
				                                               	}).ToList();

				return result;
			}
		}

		public Product_Type GetProductTypeByType(string type)
		{
			using(Context = GetDataContext())
			{
				var productType = Context.LST_VEHICLETYPE
					.Where(m => m.NAME.ToLower() == type.ToLower())
					.Select(c => new Product_Type
					{
						id = c.ID,
						type = c.NAME
					}).FirstOrDefault();

				return productType;
			}
		}
	}
}