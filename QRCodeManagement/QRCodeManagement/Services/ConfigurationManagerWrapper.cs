using QRCodeManagement.Common;
using QRCodeManagement.Helpers;

namespace QRCodeManagement.Services
{
    public class ConfigurationManagerWrapper : IConfigurationManagerWrapper
    {
        public string DisplayEmailName
        {
            get { return ConfigurationHelper.GetAppSettingValueString("DisplayEmailName"); }
        }

        public string FromEmail
        {
            get { return ConfigurationHelper.GetAppSettingValueString("FromEmail"); }
        }

        public string StatisticDomain
        {
            get { return ConfigurationHelper.GetAppSettingValueString("StatisticDomain"); }
        }
    }
}