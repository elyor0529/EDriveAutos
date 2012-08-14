using System.Collections.Generic;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;
using System.Linq;

namespace Edrive.Logic
{
	public class PictureService : BaseService, IPictureService
	{
		public Picture GetByID(int id)
		{
			using(Context = GetDataContext())
			{
				var query = Context.EDRIVE_PICTURE.Where(c => c.ID == id).Select(ConvertType).FirstOrDefault();

				return query;
			}
		}

		public int Save(Picture item)
		{
			using(Context = GetDataContext())
			{
				int result;
				var query = Context.EDRIVE_PICTURE.FirstOrDefault(c => c.ID == item.ID);

				if(query != null)
				{
					query.BINARY = item.Binary;
					query.EXTENSION = item.Extension;
					query.ISNEW = item.IsNew;

					Context.SaveChanges();
					result = query.ID;
				}
				else
				{
					var newItem = new EDRIVE_PICTURE
						{
							BINARY = item.Binary,
							EXTENSION = item.Extension,
							ISNEW = item.IsNew
						};

					Context.EDRIVE_PICTURE.AddObject(newItem);
					Context.SaveChanges();
					result = newItem.ID;
				}

				return result;
			}
		}

		public bool Delete(int id)
		{
			using(Context = GetDataContext())
			{
				bool result = false;
				var query = Context.EDRIVE_PICTURE.FirstOrDefault(c => c.ID == id);

				if(query != null)
				{
					Context.DeleteObject(query);
					Context.SaveChanges();
					result = true;
				}

				return result;
			}
		}

		#region private methods

		private Picture ConvertType(EDRIVE_PICTURE item)
		{
			var result = new Picture
				{
					ID = item.ID,
					Binary = item.BINARY,
					Extension = item.EXTENSION,
					IsNew = item.ISNEW
				};

			return result;
		}

		#endregion
	}
}