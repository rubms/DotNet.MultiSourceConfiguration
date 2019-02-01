using System;
using System.Collections.Generic;
using System.Linq;
using MultiSourceConfiguration.Config.ConfigSource;
using MultiSourceConfiguration.Config.Implementation;
using System.Runtime.Caching;
using System.Reflection;
using DotNet.MultiSourceConfiguration.Implementation;

namespace MultiSourceConfiguration.Config
{
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        private readonly Dictionary<Type, UnifiedConverter> converters;
        private List<IStringConfigSource> stringConfigSources;
        private MemoryCache memoryCache;
        private TimeSpan _cacheExpiration;

        public ConfigurationBuilder()
        {
            stringConfigSources = new List<IStringConfigSource>();
            converters = DefaultConverterFactory.GetDefaultConverters();

            memoryCache = new MemoryCache("MultiSourceConfiguration");
            _cacheExpiration = TimeSpan.FromSeconds(0);
            HandleNonDecoratedProperties = false;
        }

        public TimeSpan CacheExpiration {
            get
            {
                return _cacheExpiration;
            }

            set
            {
                _cacheExpiration = value;
                stringConfigSources.ForEach(configSource => configSource.CacheExpiration = value);
            }
        }

        public bool HandleNonDecoratedProperties { get; set; }

        public void AddTypeConverter<T>(ITypeConverter<T> converter)
        {
            converters.AddTypeConverter(new TypeConverterWrapper<T>(converter));
        }

        public void AddSources(params IStringConfigSource[] stringConfigSources)
        {
            foreach (var stringConfigSource in stringConfigSources)
                stringConfigSource.CacheExpiration = _cacheExpiration;
            this.stringConfigSources.AddRange(stringConfigSources);
        }

        public T Build<T>(string propertiesPrefix=null, bool handleNonDecoratedProperties=false) where T : class, new()
        {
            T result = memoryCache.Get(typeof(T).FullName) as T;
            if (result != null)
                return result;

            result = new T();
            var dtoProperties = typeof(T).GetProperties();
            foreach (var dtoProperty in dtoProperties)
            {
                if (HandleNonDecoratedProperties || handleNonDecoratedProperties || dtoProperty.IsDefined(typeof(PropertyAttribute), false))
                {
                    SetPropertyValue<T>(result, dtoProperty, propertiesPrefix);
                }
            }

            memoryCache.Add(typeof(T).FullName, result, DateTimeOffset.Now.Add(CacheExpiration));

            return result;
        }

        public bool TryGetStringValue(string propertyName, out string propertyValue)
        {
            bool found = false;
            propertyValue = null;
            foreach (var configSource in this.stringConfigSources)
            {
                string obtainedValue;
                if (configSource.TryGetString(propertyName, out obtainedValue))
                {
                    found = true;
                    propertyValue = obtainedValue;
                }
            }
            return found;
        }

        private void SetPropertyValue<T>(T configurationObject, PropertyInfo dtoProperty, string propertiesPrefix)
        {
            var propertyAttribute = dtoProperty.GetCustomAttributes(typeof(PropertyAttribute), false).FirstOrDefault() as PropertyAttribute;
            // If not explicit Property anotation is provided, take the object property name as configuration property.
            if (propertyAttribute == null)
                propertyAttribute = new PropertyAttribute(dtoProperty.Name);

            UnifiedConverter converter;
            string value;

            if (!converters.TryGetValue(dtoProperty.PropertyType, out converter))
                throw new InvalidOperationException(string.Format("Unsupported type {0} for field {1}", dtoProperty.PropertyType.Name, propertyAttribute.Property));

            if (TryGetStringValue((propertiesPrefix ?? "") + propertyAttribute.Property, out value)) {
                dtoProperty.SetValue(configurationObject, converter.FromString(value));
            }
            else { 
                if (propertyAttribute.Default != null)
                {
                    dtoProperty.SetValue(configurationObject, converter.FromString(propertyAttribute.Default));
                }
                else if (propertyAttribute.Required)
                {
                    throw new InvalidOperationException(string.Format("No value found for field {0}", propertyAttribute.Property));
                }
            }
        }
    }
}