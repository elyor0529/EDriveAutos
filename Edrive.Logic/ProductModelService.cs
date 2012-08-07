using System.Collections.Generic;
using System.Linq;
using Edrive.Data;
using Edrive.Logic.Interfaces;
using Product_Model = Edrive.Core.Model.Product_Model;

namespace Edrive.Logic
{
	public class ProductModelService : BaseService, IProductModelService
	{
		public List<Product_Model> GetModelsByMake(int makeID)
		{
			using(Context = GetDataContext())
			{
				var models = Context.LST_VEHICLEMODEL.Where(m => m.MAKE_ID == makeID)
					.Select(m => new Product_Model
					             	{
					             		id = m.ID,
					             		modelName = m.NAME
					             	}).ToList();
				
				return models;
			}
		}

		public Product_Model AddProductModel(string modelName, int makeID)
		{
			using(Context = GetDataContext())
			{
				var model = Context.LST_VEHICLEMODEL.FirstOrDefault(m => m.NAME == modelName && m.MAKE_ID == makeID);

				if(model == null)
				{
					model = new LST_VEHICLEMODEL
					{
						ID = makeID,
						NAME = modelName
					};

					Context.LST_VEHICLEMODEL.AddObject(model);
					Context.SaveChanges();
				}

				var result = new Product_Model
				{
					id = model.ID,
					makeID = model.MAKE_ID,
					modelName = model.NAME
				};

				return result;
			}
		}
	}
}