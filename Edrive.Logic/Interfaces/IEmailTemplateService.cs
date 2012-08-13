using System.Collections.Generic;
using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface IEmailTemplateService
	{
		List<EmailTemplate> GetAll();

		EmailTemplate GetByID(int id);

		EmailTemplate GetByName(string name);

		int Save(EmailTemplate template);

		bool Delete(int id);
	}
}