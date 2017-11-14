using System;
using System.Collections.Generic;
using System.Configuration;

namespace MultiSourceConfiguration.Config.ConfigSource
{
    public class MemorySource : IStringConfigSource
    {
        Dictionary<string, string> properties;
        public MemorySource()
        {
            properties = new Dictionary<string, string>();
        }

        public TimeSpan CacheExpiration { private get; set; }

        public void Add(string property, string value)
        {
            properties[property] = value;
        }

        public bool TryGetString(string property, out string value)
        {
            value = null;
            return properties.TryGetValue(property, out value);
        }
    }
}