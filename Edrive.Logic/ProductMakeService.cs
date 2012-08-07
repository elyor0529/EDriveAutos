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
	}
}