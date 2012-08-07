using System.Collections.Generic;
using Edrive.Data;
using System.Linq;
using Edrive.Logic.Interfaces;
using Product_Type = Edrive.Core.Model.Product_Type;

namespace Edrive.Logic
{
	public class ProductTypeService : IProductTypeService
	{
		public List<Product_Type> GetAll()
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				var result = entities.Product_Type.Select(c => new Product_Type
				                                               	{
				                                               		id = c.id,
																	type = c.Type
				                                               	}).ToList();

				return result;
			}
		}
	}
}