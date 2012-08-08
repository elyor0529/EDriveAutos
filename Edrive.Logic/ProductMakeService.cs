using System;
using System.Collections.Generic;
using System.Linq;
using Edrive.Data;
using Edrive.Logic.Interfaces;
using Product_Make = Edrive.Core.Model.Product_Make;

namespace Edrive.Logic
{
	public class ProductMakeService : IProductMakeService
	{
		public List<Product_Make> GetAll()
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				var makes = entities.Product_Make.OrderBy(m => m.Make).Select(
					m => new Product_Make
					     	{
					     		id = m.id,
					     		make = m.Make
					     	}).ToList();
				
				return makes;
			}
		}

		public Product_Make AddProductMake(string productMake)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				if(!String.IsNullOrWhiteSpace(productMake))
				{
					var make = entities.Product_Make.FirstOrDefault(m => m.Make == productMake);

					if(make == null)
					{
						make = new Data.Product_Make
						{
							Make = productMake
						};

						entities.Product_Make.AddObject(make);
						entities.SaveChanges();
					}

					var result = new Product_Make
					{
						id = make.id,
						make = make.Make
					};

					return result;
				}

				return null;
			}
		}
	}
}