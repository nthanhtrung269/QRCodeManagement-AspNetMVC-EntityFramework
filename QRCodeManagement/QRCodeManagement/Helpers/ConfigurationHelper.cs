using System.Configuration;
using System.Configuration.Provider;
using System.Runtime.CompilerServices;

namespace QRCodeManagement.Helpers
{
    public static class ConfigurationHelper
    {
        public static string GetAppSettingValueString(string appSettingKey, bool required = true)
        {
            string text = string.Empty;

            if (ConfigurationManager.AppSettings[appSettingKey] != null)
            {
                text = ConfigurationManager.AppSettings[appSettingKey];
            }

            if (required && string.IsNullOrEmpty(text))
            {
                throw new ProviderException(string.Format("Please add an appSetting key/value for {0}.", appSettingKey));
            }

            return text;
        }

        public static bool GetAppSettingValueBool(string appSettingKey, bool required = true, bool defaultValue = false)
        {
            bool result = defaultValue;
            string value = string.Empty;

            if (ConfigurationManager.AppSettings[appSettingKey] != null)
            {
                value = ConfigurationManager.AppSettings[appSettingKey];
            }

            if ((!bool.TryParse(value, out result) || string.IsNullOrEmpty(value)) && required)
            {
                throw new ProviderException(string.Format("Please add an appSetting key/value for {0}.", appSettingKey));
            }

            return result;
        }

        public static int GetAppSettingValueInt(string appSettingKey, bool required = true, int defaultValue = 0)
        {
            int result = defaultValue;
            string text = string.Empty;

            if (ConfigurationManager.AppSettings[appSettingKey] != null)
            {
                text = ConfigurationManager.AppSettings[appSettingKey];
            }

            if ((!int.TryParse(text, out result) || string.IsNullOrEmpty(text)) && required)
            {
                throw new ProviderException(string.Format("Please add an appSetting key/value for {0}.", appSettingKey));
            }

            return result;
        }

        public static decimal GetAppSettingValueDecimal(string appSettingKey, bool required = true, [DecimalConstant(0, 0, 0u, 0u, 0u)] decimal defaultValue = default(decimal))
        {
            decimal result = defaultValue;
            string text = string.Empty;

            if (ConfigurationManager.AppSettings[appSettingKey] != null)
            {
                text = ConfigurationManager.AppSettings[appSettingKey];
            }

            if ((!decimal.TryParse(text, out result) || string.IsNullOrEmpty(text)) && required)
            {
                throw new ProviderException(string.Format("Please add an appSetting key/value for {0}.", appSettingKey));
            }

            return result;
        }
    }
}