using System;
using System.Collections.Generic;
using System.Configuration;

namespace DotNet.MultiSourceConfiguration.ConfigSource
{
    public class MemorySource : IStringConfigSource
    {
        Dictionary<string, string> properties;
        public MemorySource()
        {
            properties = new Dictionary<string, string>();
        }

        public void Add(string property, string value)
        {
            properties.Add(property, value);
        }

        public bool TryGetString(string property, out string value)
        {
            value = null;
            return properties.TryGetValue(property, out value);
        }
    }
}