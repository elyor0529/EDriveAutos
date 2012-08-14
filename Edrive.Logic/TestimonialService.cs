using System;
using System.Collections.Generic;
using Edrive.Core.Model;
using System.Linq;
using Edrive.Data;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class TestimonialService : BaseService, ITestimonialService
	{
		public List<Testimonial> GetAll(int pageSize, int pageNumber, out int count)
		{
			using (Context = GetDataContext())
			{
				var query = Context.EDRIVE_TESTIMONIALS
					.OrderBy(c => c.DATE_CREATED)
					.Skip(pageSize * pageNumber).Take(pageSize)
					.Select(ConvertType).ToList();

				count = Context.EDRIVE_TESTIMONIALS.Count();

				return query;
			}
		}

		public Testimonial GetByID(int id)
		{
			using(Context = GetDataContext())
			{
				var query = Context.EDRIVE_TESTIMONIALS.Where(c => c.ID == id).Select(ConvertType).FirstOrDefault();

				return query;
			}
		}

		public int Save(Testimonial item)
		{
			using(Context = GetDataContext())
			{
				int result;
				var query = Context.EDRIVE_TESTIMONIALS.FirstOrDefault(c => c.ID == item.ID);

				if(query != null)
				{
					query.ISACTIVE = item.IsActive;
					query.NAME = item.Name;
					query.PICTURE_ID = item.PictureID;
					query.ADDRESS = item.Address;
					query.CONTENT = item.Content;
					query.DATE_UPDATED = DateTime.Now;

					Context.SaveChanges();
					result = query.ID;
				}
				else
				{
					EDRIVE_TESTIMONIALS newItem = new EDRIVE_TESTIMONIALS
						{
							NAME = item.Name,
							ADDRESS = item.Address,
							CONTENT = item.Content,
							ISACTIVE = item.IsActive,
							PICTURE_ID = item.PictureID,
							DATE_CREATED = DateTime.Now
						};

					Context.EDRIVE_TESTIMONIALS.AddObject(newItem);
					Context.SaveChanges();
					result = newItem.ID;
				}

				return result;
			}
		}

		public bool Delete(int id)
		{
			using (Context = GetDataContext())
			{
				var result = false;
				var query = Context.EDRIVE_TESTIMONIALS.FirstOrDefault(c => c.ID == id);

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

		private Testimonial ConvertType(EDRIVE_TESTIMONIALS item)
		{
			var result = new Testimonial
				{
					ID = item.ID,
					Name = item.NAME,
					IsActive = item.ISACTIVE,
					Address = item.ADDRESS,
					Content = item.CONTENT,
					DateCreated = item.DATE_CREATED,
					DateUpdated = item.DATE_UPDATED,
					PictureID = item.PICTURE_ID
				};

			return result;
		}

		#endregion
	}
}