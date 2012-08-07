using System.Collections.Generic;
using Edrive.Data;
using System.Linq;
using Edrive.Logic.Interfaces;
using Product_Body = Edrive.Core.Model.Product_Body;

namespace Edrive.Logic
{
	public class ProductBodyService : IProductBodyService
	{
		public List<Product_Body> GetAll()
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				var productBodies = entities.Product_Body.Where(m => m.Body != null && m.Body != "")
					.Select(m => new Product_Body
					     	{
					     		id = m.id,
					     		body = m.Body.Trim()
					     	}).OrderBy(m => m.body).ToList();

				return productBodies;
			}
		}
	}
}