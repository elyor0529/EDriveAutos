using System.Configuration;

namespace Edrive.Security
{
    public class ErrorPagesSection : ConfigurationSection
    {
        [ConfigurationProperty("useFriendlyErrors", IsRequired = true)]
        public bool UseFriendlyErrors
        {
            get { return (bool)base["useFriendlyErrors"]; }
            set { base["useFriendlyErrors"] = value; }
        }
    }
}