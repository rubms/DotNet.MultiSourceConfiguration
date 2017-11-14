using System;
using System.Configuration;

namespace MultiSourceConfiguration.Config.ConfigSource
{
    public class EnvironmentVariableSource : IStringConfigSource
    {
        public TimeSpan CacheExpiration { private get; set; }

        public bool TryGetString(string property, out string value)
        {
            value = null;
            string str = Environment.GetEnvironmentVariable(property);
            if (str == null)
                return false;
            value = str;
            return true;
        }
    }
}