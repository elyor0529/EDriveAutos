using System.Configuration;

namespace Edrive.Security
{
    public class EnvironmentSection : ConfigurationSection
    {
        [ConfigurationProperty("level", IsRequired = true)]
        public DevLevel Level
        {
            get { return (DevLevel)base["level"]; }
            set { base["level"] = value; }
        }
    }
}