using System.Configuration;

namespace Edrive.Security
{
    public class MonitoringSection : ConfigurationSection
    {
        [ConfigurationProperty("exceptionsEmail", IsRequired = true)]
        public string EmailAddress
        {
            get { return (string)base["exceptionsEmail"]; }
            set { base["exceptionsEmail"] = value; }
        }
    }
}