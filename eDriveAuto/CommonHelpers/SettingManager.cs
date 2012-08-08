using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.CommonHelpers
{
    public static class SettingManager
    {
        #region "Variable"
        /// <summary>
        /// Gets or sets a store name
        /// </summary>
        public static string StoreName
        {
            get
            {
                string storeName = SettingManager.GetSettingValue("Common.StoreName");
                return storeName;
            }
            set
            {
                SettingManager.SetParam("Common.StoreName", value);
            }
        }

        private static void SetParam(string settingName, string value)
        {
            using (eDriveAutoWebEntities _entity=new Models.eDriveAutoWebEntities() )
             {
                  _entity.Settings.FirstOrDefault(m=>m.Name==settingName).Value=value; 
                _entity.SaveChanges();
             }
           
        }

        public static string GetSettingValue(string settingName)
        {
             using (eDriveAutoWebEntities _entity=new Models.eDriveAutoWebEntities() )
             {
                 return  _entity.Settings.FirstOrDefault(m=>m.Name==settingName).Value; 
             }
        }

        /// <summary>
        /// Gets or sets a store URL (ending with "/")
        /// </summary>
        public static string StoreUrl
        {
            get
            {
                //string storeUrl = SettingManager.GetSettingValue("Common.StoreURL");
                //if (!storeUrl.EndsWith("/"))
                //    storeUrl += "/";
                //return storeUrl;
                return "http://www.edriveautos.com/";
            }
            set
            {
                SettingManager.SetParam("Common.StoreURL", value);
            }
        }

        #endregion
    }
}