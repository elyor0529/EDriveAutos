using System.Collections.Generic;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;
using System.Linq;

namespace Edrive.Logic
{
	public class EmailTemplateService : BaseService, IEmailTemplateService
	{
		public List<EmailTemplate> GetAll()
		{
			using(Context = GetDataContext())
			{
				var query = Context.EMAIL_TEMPLATE.Where(c => c.ISACTIVE == true || c.ISACTIVE == null).Select(ConvertType).ToList();

				return query;
			}
		}

		public EmailTemplate GetByID(int id)
		{
			using (Context = GetDataContext())
			{
				var query = Context.EMAIL_TEMPLATE.Where(c => c.ID == id).Select(ConvertType).FirstOrDefault();

				return query;
			}
		}

		public EmailTemplate GetByName(string name)
		{
			using (Context = GetDataContext())
			{
				var query = Context.EMAIL_TEMPLATE.Where(c => c.NAME.ToLower() == name.ToLower()).Select(ConvertType).FirstOrDefault();

				return query;
			}
		}

		public int Save(EmailTemplate template)
		{
			using (Context = GetDataContext())
			{
				int result;
				var query = Context.EMAIL_TEMPLATE.FirstOrDefault(c => c.NAME.ToLower() == template.Name.ToLower() || c.ID == template.ID);

				if(query != null)
				{
					result = query.ID;

					query.NAME = template.Name;
					query.SUBJECT = template.Subject;
					query.BODY = template.Body;
					query.BCC = template.Bcc;
					query.ISACTIVE = template.IsActive;

					Context.SaveChanges();
				}
				else
				{
					EMAIL_TEMPLATE newItem = new EMAIL_TEMPLATE
						{
							BODY = template.Body,
							NAME = template.Name,
							BCC = template.Bcc,
							SUBJECT = template.Subject,
							ISACTIVE = template.IsActive
						};

					Context.EMAIL_TEMPLATE.AddObject(newItem);
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
				var query = Context.EMAIL_TEMPLATE.FirstOrDefault(c => c.ID == id);

				if(query != null)
				{
					Context.EMAIL_TEMPLATE.DeleteObject(query);
					Context.SaveChanges();

					return true;
				}

				return false;
			}
		}

		#region private methods

		private EmailTemplate ConvertType(EMAIL_TEMPLATE item)
		{
			var result = new EmailTemplate
				{
					ID = item.ID,
					Name = item.NAME,
					IsActive = item.ISACTIVE,
					Bcc = item.BCC,
					Body = item.BODY,
					Subject = item.SUBJECT
				};

			return result;
		}

		#endregion
	}
}