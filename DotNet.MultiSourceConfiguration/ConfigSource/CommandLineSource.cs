using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;

namespace DotNet.MultiSourceConfiguration.ConfigSource
{
    public class CommandLineSource : IStringConfigSource
    {
        Dictionary<string, string> parsedProperties;
        public CommandLineSource(string[] args)
        {
            parsedProperties = parseProperties(args);
        }

        public bool TryGetString(string property, out string value)
        {
            value = null;
            return parsedProperties.TryGetValue(property, out value);
        }

        private Dictionary<string, string> parseProperties(string[] args)
        {
            var result = new Dictionary<string, string>();
            var regex = new Regex("--(?<Property>[^\\s=]*)=(?<Value>.*)");
            foreach (string arg in args)
            {
                var results = regex.Matches(arg);
                foreach (Match match in results)
                {
                    result.Add(match.Groups["Property"].Value, match.Groups["Value"].Value);
                }
            }
            return result;
        }
    }
}