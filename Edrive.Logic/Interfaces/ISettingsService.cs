using Edrive.Core.Model;

namespace Edrive.Logic.Interfaces
{
	public interface ISettingsService
	{
		string GetValue(string settingsName);

		bool SaveSettings(SiteSetting settings);
	}
}