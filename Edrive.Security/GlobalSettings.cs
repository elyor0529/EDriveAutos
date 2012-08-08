using System.Web.Configuration;

namespace Edrive.Security
{
    public static class GlobalSettings
    {
        private static readonly SecuritySection _security = (SecuritySection) WebConfigurationManager.GetSection("Edrive/security");

        private static readonly EnvironmentSection _environment =
            (EnvironmentSection) WebConfigurationManager.GetSection("Edrive/environment");

        private static readonly ErrorPagesSection _errorPages =
    (ErrorPagesSection)WebConfigurationManager.GetSection("Edrive/errorPages");

        private static readonly WebmasterSection _webmasterSection =
(WebmasterSection)WebConfigurationManager.GetSection("Edrive/webmaster");

        private static readonly MonitoringSection _monitoringSection =
(MonitoringSection)WebConfigurationManager.GetSection("Edrive/monitoring");


        public static SecurityOption UseSecurity
        {
            get { return _security.UseSecurity; }
        }

        public static DevLevel Environment
        {
            get { return _environment.Level; }
        }

        public static bool UseFriendlyErrors
        {
            get { return _errorPages.UseFriendlyErrors; }
        }

        public static string WebmasterEmail
        {
            get { return _webmasterSection.EmailAddress; }
        }

        public static string ExceptionsEmail
        {
            get { return _monitoringSection.EmailAddress; }
        }
    }
}
