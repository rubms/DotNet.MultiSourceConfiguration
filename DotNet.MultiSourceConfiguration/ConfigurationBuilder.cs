using System;
using System.Collections.Generic;
using System.Linq;
using MultiSourceConfiguration.Config.ConfigSource;
using MultiSourceConfiguration.Config.Implementation;
using System.Runtime.Caching;
using System.Globalization;
using System.Reflection;

namespace MultiSourceConfiguration.Config
{
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        private readonly Dictionary<Type, UnifiedConverter> converters = new Dictionary<Type, UnifiedConverter>();
        private List<IStringConfigSource> stringConfigSources;
        private MemoryCache memoryCache;
        private TimeSpan _cacheExpiration;

        public ConfigurationBuilder()
        {
            stringConfigSources = new List<IStringConfigSource>();

            AddTypeConverter(new LambdaConverter<bool?>(null, s => Boolean.Parse(s)));
            AddTypeConverter(new LambdaConverter<bool[]>(new bool[0], s => s.Split(',').Select(Boolean.Parse).ToArray()));
            AddTypeConverter(new LambdaConverter<bool>(false, s => Boolean.Parse(s)));
            AddTypeConverter(new LambdaConverter<int?>(null, s => Int32.Parse(s, CultureInfo.InvariantCulture)));
            AddTypeConverter(new LambdaConverter<int[]>(new int[0], s => s.Split(',').Select(x => Int32.Parse(x, CultureInfo.InvariantCulture)).ToArray()));
            AddTypeConverter(new LambdaConverter<int>(0, s => Int32.Parse(s, CultureInfo.InvariantCulture)));
            AddTypeConverter(new LambdaConverter<string>(null, s => s));
            AddTypeConverter(new LambdaConverter<string[]>(new string[0], s => s.Split(',')));
            AddTypeConverter(new LambdaConverter<long?>(null, s => Int64.Parse(s, CultureInfo.InvariantCulture)));
            AddTypeConverter(new LambdaConverter<long[]>(new long[0], s => s.Split(',').Select(x => Int64.Parse(x, CultureInfo.InvariantCulture)).ToArray()));
            AddTypeConverter(new LambdaConverter<long>(0, s => Int64.Parse(s, CultureInfo.InvariantCulture)));
            AddTypeConverter(new LambdaConverter<double?>(null, s => double.Parse(s, CultureInfo.InvariantCulture)));
            AddTypeConverter(new LambdaConverter<double[]>(new double[0], s => s.Split(',').Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToArray()));
            AddTypeConverter(new LambdaConverter<double>(0, s => double.Parse(s, CultureInfo.InvariantCulture)));
            AddTypeConverter(new LambdaConverter<decimal?>(null, s => decimal.Parse(s, CultureInfo.InvariantCulture)));
            AddTypeConverter(new LambdaConverter<decimal[]>(new decimal[0], s => s.Split(',').Select(x => decimal.Parse(x, CultureInfo.InvariantCulture)).ToArray()));
            AddTypeConverter(new LambdaConverter<decimal>(0, s => decimal.Parse(s, CultureInfo.InvariantCulture)));
            AddTypeConverter(new LambdaConverter<float?>(null, s => float.Parse(s, CultureInfo.InvariantCulture)));
            AddTypeConverter(new LambdaConverter<float[]>(new float[0], s => s.Split(',').Select(x => float.Parse(x, CultureInfo.InvariantCulture)).ToArray()));
            AddTypeConverter(new LambdaConverter<float>(0, s => float.Parse(s, CultureInfo.InvariantCulture)));

            memoryCache = new MemoryCache("MultiSourceConfiguration");

            _cacheExpiration = TimeSpan.FromSeconds(0);
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

        public void AddTypeConverter<T>(ITypeConverter<T> converter)
        {
            converters.Add(typeof(T), new TypeConverterWrapper<T>(converter));
        }

        public void AddSources(params IStringConfigSource[] stringConfigSources)
        {
            foreach (var stringConfigSource in stringConfigSources)
                stringConfigSource.CacheExpiration = _cacheExpiration;
            this.stringConfigSources.AddRange(stringConfigSources);
        }

        public T Build<T>() where T : class, new()
        {
            T result = memoryCache.Get(typeof(T).FullName) as T;
            if (result != null)
                return result;

            result = new T();
            var dtoProperties = typeof(T).GetProperties();
            foreach (var dtoProperty in dtoProperties)
            {
                if (dtoProperty.IsDefined(typeof(PropertyAttribute), false))
                {
                    SetPropertyValue<T>(result, dtoProperty);
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

        private void SetPropertyValue<T>(T configurationObject, PropertyInfo dtoProperty)
        {
            var propertyAttribute = dtoProperty.GetCustomAttributes(typeof(PropertyAttribute), false).FirstOrDefault() as PropertyAttribute;

            UnifiedConverter converter;
            string value;

            if (!converters.TryGetValue(dtoProperty.PropertyType, out converter))
                throw new InvalidOperationException(string.Format("Unsupported type {0} for field {1}", dtoProperty.PropertyType.Name, propertyAttribute.Property));

            if (TryGetStringValue(propertyAttribute.Property, out value)) {
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