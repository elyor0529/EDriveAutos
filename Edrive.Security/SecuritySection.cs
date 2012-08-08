using System.Configuration;

namespace Edrive.Security
{
    public class SecuritySection : ConfigurationSection
    {
        [ConfigurationProperty("useSsl", IsRequired = true)]
        public SecurityOption UseSecurity
        {
            get { return (SecurityOption) base["useSsl"]; }
            set { base["useSsl"] = value; }
        }
    }
}
