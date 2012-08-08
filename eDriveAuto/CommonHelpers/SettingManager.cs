using Edrive.Core.Model;
using Edrive.Logic;
using Edrive.Logic.Interfaces;

namespace Edrive.CommonHelpers
{
    public static class SettingManager
    {
		private readonly static ISettingsService Service;

		static SettingManager()
		{
			Service = new SettingsService();
		}

        #region "Variable"
        /// <summary>
        /// Gets or sets a store name
        /// </summary>
        public static string StoreName
        {
            get
            {
				string storeName = GetSettingValue("Common.StoreName");
                return storeName;
            }
            set
            {
                SetParam("Common.StoreName", value);
            }
        }

        private static void SetParam(string settingName, string value)
        {
        	var setting = new SiteSetting
        	              	{
        	              		Name = settingName,
        	              		Value = value
        	              	};

        	Service.SaveSettings(setting);
        }

        public static string GetSettingValue(string settingName)
        {
        	string result = Service.GetValue(settingName);

        	return result;
        }

        /// <summary>
        /// Gets or sets a store URL (ending with "/")
        /// </summary>
        public static string StoreUrl
        {
            get
            {
                return "http://www.edriveautos.com/";
            }
            set
            {
                SetParam("Common.StoreURL", value);
            }
        }

        #endregion
    }
}