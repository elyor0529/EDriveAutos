using System.Collections.Generic;
using System.Linq;
using Edrive.Data;
using Edrive.Logic.Interfaces;
using Product_Model = Edrive.Core.Model.Product_Model;

namespace Edrive.Logic
{
	public class ProductModelService : IProductModelService
	{
		public List<Product_Model> GetModelsByMake(int makeID)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				var models = entities.Product_Model.Where(m => m.MakeID == makeID)
					.Select(m => new Product_Model
					             	{
					             		id = m.id,
					             		modelName = m.ModeLName
					             	}).ToList();
				
				return models;
			}
		}

		public Product_Model AddProductModel(string modelName, int makeID)
		{
			using(EDriveEntities entities = new EDriveEntities())
			{
				var model = entities.Product_Model.FirstOrDefault(m => m.ModeLName == modelName && m.MakeID == makeID);

				if(model == null)
				{
					model = new Data.Product_Model
					{
						MakeID = makeID,
						ModeLName = modelName
					};

					entities.Product_Model.AddObject(model);
					entities.SaveChanges();
				}

				var result = new Product_Model
				{
					id = model.id,
					makeID = model.MakeID,
					modelName = model.ModeLName
				};

				return result;
			}
		}
	}
}