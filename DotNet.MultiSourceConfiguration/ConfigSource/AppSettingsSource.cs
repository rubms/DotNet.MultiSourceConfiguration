using System;
using System.Configuration;

namespace MultiSourceConfiguration.Config.ConfigSource
{
    public class AppSettingsSource : IStringConfigSource
    {
        public bool TryGetString(string property, out string value)
        {
            value = null;
            string str = ConfigurationManager.AppSettings.Get(property);
            if (str == null)
                return false;
            value = str;
            return true;
        }
    }
}