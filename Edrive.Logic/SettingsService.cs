using System.Linq;
using Edrive.Core.Model;
using Edrive.Data;
using Edrive.Logic.Interfaces;

namespace Edrive.Logic
{
	public class SettingsService : BaseService, ISettingsService
	{
		public string GetValue(string settingsName)
		{
			using(Context = GetDataContext())
			{
				string result = string.Empty;
				var query = (from c in Context.SITE_SETTINGS where c.NAME.ToLower() == settingsName.ToLower() select c).FirstOrDefault();

				if(query != null)
					result = query.VALUE;

				return result;
			}
		}

		public bool SaveSettings(SiteSetting settings)
		{
			using (Context = GetDataContext())
			{
				if(string.IsNullOrWhiteSpace(settings.Name) || string.IsNullOrWhiteSpace(settings.Value))
					return false;

				var query = (from c in Context.SITE_SETTINGS where c.NAME.ToLower() == settings.Name.ToLower() select c).FirstOrDefault();

				if(query != null)
				{
					query.VALUE = settings.Value;
					Context.SaveChanges();
				}
				else
				{
					SITE_SETTINGS newSettings = new SITE_SETTINGS
					                            	{
					                            		NAME = settings.Name,
														VALUE = settings.Value
					                            	};

					Context.SITE_SETTINGS.AddObject(newSettings);
					Context.SaveChanges();
				}

				return true;
			}
		}
	}
}