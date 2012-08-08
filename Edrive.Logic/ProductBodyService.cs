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

		public Product_Body AddProductBody(string bodyName)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				var body = entities.Product_Body.FirstOrDefault(m => m.Body == bodyName);

				if(body == null)
				{
					body = new Data.Product_Body
					{
						Body = bodyName
					};

					entities.Product_Body.AddObject(body);
					entities.SaveChanges();
				}

				var result = new Product_Body
				{
					id = body.id,
					body = body.Body
				};

				return result;
			}
		}
	}
}