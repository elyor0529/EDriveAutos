using System;
using System.Collections.Generic;
using System.Linq;
using Edrive.Data;
using Edrive.Logic.Interfaces;
using Product_Make = Edrive.Core.Model.Product_Make;

namespace Edrive.Logic
{
	public class ProductMakeService : BaseService, IProductMakeService
	{
		public List<Product_Make> GetAll()
		{
			using(Context = GetDataContext())
			{
				var makes = Context.LST_VEHICLEMAKE.OrderBy(m => m.NAME).Select(
					m => new Product_Make
					     	{
					     		id = m.ID,
					     		make = m.NAME
					     	}).ToList();
				
				return makes;
			}
		}

		public Product_Make AddProductMake(string productMake)
		{
			using(Context = GetDataContext())
			{
				if(!String.IsNullOrWhiteSpace(productMake))
				{
					var make = Context.LST_VEHICLEMAKE.FirstOrDefault(m => m.NAME == productMake);

					if(make == null)
					{
						make = new LST_VEHICLEMAKE
						{
							NAME = productMake
						};

						Context.LST_VEHICLEMAKE.AddObject(make);
						Context.SaveChanges();
					}

					var result = new Product_Make
					{
						id = make.ID,
						make = make.NAME
					};

					return result;
				}

				return null;
			}
		}
	}
}