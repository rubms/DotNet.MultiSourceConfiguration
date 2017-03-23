using System;
using System.Collections.Generic;
using MultiSourceConfiguration.Config.ConfigSource;

namespace MultiSourceConfiguration.Config.Implementation
{
    public class ConfigInterfaceImplBase
    {
        private IEnumerable<IStringConfigSource> configSources;
        private Dictionary<Type, UnifiedConverter> typeConverters;

        private bool GetStringValue(string field, out string value)
        {
            foreach (var configSource in configSources)
            {
                if (configSource.TryGetString(field, out value))
                    return true;
            }
            value = null;
            return false;
        }

        protected T GetValue<T>(string fieldName)
        {
            var type = typeof(T);
            UnifiedConverter converter;
            string value;

            if (!typeConverters.TryGetValue(type, out converter))
                throw new InvalidOperationException(string.Format("Unsupported type {0} for field {1}", type.Name, fieldName));
            if (!GetStringValue(fieldName, out value))
                throw new InvalidOperationException(string.Format("No value found for field {0}", fieldName));
            try
            {
                return value == null ? (T)converter.GetDefaultValue() : (T)converter.FromString(value);
            }
            catch (Exception e)
            {
                e.Data.Add("FieldName:", fieldName);
                e.Data.Add("Type:", type.Name);
                throw;
            }
        }

        internal void Init(IStringConfigSource[] aStringConfigSources, Dictionary<Type, UnifiedConverter> aTypeConverters)
        {
            configSources = aStringConfigSources;
            typeConverters = aTypeConverters;
        }
    }
}