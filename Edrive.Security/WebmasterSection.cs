using System.Configuration;

namespace Edrive.Security
{
    public class WebmasterSection : ConfigurationSection
    {
        [ConfigurationProperty("email", IsRequired = true)]
        public string EmailAddress
        {
            get { return (string)base["email"]; }
            set { base["email"] = value; }
        }
    }
}