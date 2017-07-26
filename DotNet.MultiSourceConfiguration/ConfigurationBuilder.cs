using System;
using System.Collections.Generic;
using System.Linq;
using MultiSourceConfiguration.Config.ConfigSource;
using MultiSourceConfiguration.Config.Implementation;
using System.Runtime.Caching;

namespace MultiSourceConfiguration.Config
{
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        private readonly Dictionary<Type, UnifiedConverter> converters = new Dictionary<Type, UnifiedConverter>();
        private List<IStringConfigSource> stringConfigSources;
        private MemoryCache memoryCache;

        public TimeSpan CacheExpiration { get; set; }

        public ConfigurationBuilder()
        {
            stringConfigSources = new List<IStringConfigSource>();

            AddTypeConverter(new LambdaConverter<bool?>(null, s => Boolean.Parse(s)));
            AddTypeConverter(new LambdaConverter<bool[]>(new bool[0], s => s.Split(',').Select(Boolean.Parse).ToArray()));
            AddTypeConverter(new LambdaConverter<int?>(null, s => Int32.Parse(s)));
            AddTypeConverter(new LambdaConverter<int[]>(new int[0], s => s.Split(',').Select(Int32.Parse).ToArray()));
            AddTypeConverter(new LambdaConverter<string>(null, s => s));
            AddTypeConverter(new LambdaConverter<string[]>(new string[0], s => s.Split(',')));
            AddTypeConverter(new LambdaConverter<long?>(null, s => Int64.Parse(s)));
            AddTypeConverter(new LambdaConverter<long[]>(new long[0], s => s.Split(',').Select(Int64.Parse).ToArray()));
            AddTypeConverter(new LambdaConverter<double?>(null, s => double.Parse(s)));
            AddTypeConverter(new LambdaConverter<double[]>(new double[0], s => s.Split(',').Select(double.Parse).ToArray()));
            AddTypeConverter(new LambdaConverter<decimal?>(null, s => decimal.Parse(s)));
            AddTypeConverter(new LambdaConverter<decimal[]>(new decimal[0], s => s.Split(',').Select(decimal.Parse).ToArray()));
            AddTypeConverter(new LambdaConverter<float?>(null, s => float.Parse(s)));
            AddTypeConverter(new LambdaConverter<float[]>(new float[0], s => s.Split(',').Select(float.Parse).ToArray()));

            memoryCache = new MemoryCache("MultiSourceConfiguration");

            CacheExpiration = TimeSpan.FromSeconds(0);
        }

        public void AddTypeConverter<T>(ITypeConverter<T> converter)
        {
            converters.Add(typeof(T), new TypeConverterWrapper<T>(converter));
        }

        public void AddSources(params IStringConfigSource[] stringConfigSources)
        {
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
                    var propertyAttribute = dtoProperty.GetCustomAttributes(typeof(PropertyAttribute), false).FirstOrDefault() as PropertyAttribute;
                    dtoProperty.SetValue(result, GetValue(propertyAttribute.Property, dtoProperty.PropertyType, propertyAttribute.Required, propertyAttribute.Default));
                }
            }

            memoryCache.Add(typeof(T).FullName, result, DateTimeOffset.Now.Add(CacheExpiration));

            return result;
        }

        private bool GetStringValue(string field, out string value)
        {
            bool found = false;
            value = null;
            foreach (var configSource in this.stringConfigSources)
            {
                string obtainedValue;
                if (configSource.TryGetString(field, out obtainedValue))
                {
                    found = true;
                    value = obtainedValue;
                }
            }
            return found;
        }

        private object GetValue(string fieldName, Type type, bool required, string defaultValue)
        {
            UnifiedConverter converter;
            string value;

            if (!converters.TryGetValue(type, out converter))
                throw new InvalidOperationException(string.Format("Unsupported type {0} for field {1}", type.Name, fieldName));
            if (!GetStringValue(fieldName, out value)) {
                if (defaultValue != null)
                {
                    value = defaultValue;
                }
                else if (required)
                {
                    throw new InvalidOperationException(string.Format("No value found for field {0}", fieldName));
                }
            }
                
            try
            {
                return value == null ? converter.GetDefaultValue() : converter.FromString(value);
            }
            catch (Exception e)
            {
                e.Data.Add("FieldName:", fieldName);
                e.Data.Add("Type:", type.Name);
                throw;
            }
        }
    }
}