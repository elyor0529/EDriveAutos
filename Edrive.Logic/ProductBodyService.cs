using System.Collections.Generic;
using System.Linq;
using Edrive.Data;
using Edrive.Logic.Interfaces;
using Product_Body = Edrive.Core.Model.Product_Body;

namespace Edrive.Logic
{
	public class ProductBodyService : BaseService, IProductBodyService
	{
		public List<Product_Body> GetAll()
		{
			using(Context = GetDataContext())
			{
				var productBodies = Context.LST_VEHICLEBODY.Where(m => m.NAME != null && m.NAME != "").OrderBy(c => c.NAME)
					.Select(m => new Product_Body
					             	{
					             		id = m.ID,
					             		body = m.NAME.Trim()
					             	}).ToList();

				return productBodies;
			}
		}

		public Product_Body AddProductBody(string bodyName)
		{
			using(Context = GetDataContext())
			{
				var body = Context.LST_VEHICLEBODY.FirstOrDefault(m => m.NAME == bodyName);

				if(body == null)
				{
					body = new LST_VEHICLEBODY
					{
						NAME = bodyName
					};

					Context.LST_VEHICLEBODY.AddObject(body);
					Context.SaveChanges();
				}

				var result = new Product_Body
				{
					id = body.ID,
					body = body.NAME
				};

				return result;
			}
		}
	}
}