using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.MultiSourceConfiguration.ConfigSource;
using DotNet.MultiSourceConfiguration.Implementation;

namespace DotNet.MultiSourceConfiguration
{
    public class ConfigurationServiceBuilder
    {
        private readonly Dictionary<Type,object> builtTypes = new Dictionary<Type, object>();
        private readonly Dictionary<Type, UnifiedConverter> converters = new Dictionary<Type, UnifiedConverter>();
        private bool seal = false;

        public ConfigurationServiceBuilder()
        {
            AddTypeConverter(new LambdaConverter<int?>(null, s => Int32.Parse(s)));
            AddTypeConverter(new LambdaConverter<int[]>(new int[0], s => s.Split(',').Select(Int32.Parse).ToArray()));
            AddTypeConverter(new LambdaConverter<string>(null, s => s));
            AddTypeConverter(new LambdaConverter<string[]>(new string[0], s => s.Split(',')));
        }

        public void AddTypeConverter<T>(ITypeConverter<T> converter)
        {
            converters.Add(typeof(T), new TypeConverterWrapper<T>(converter));
        }

        public void RegisterConfigInterface<T>(params IStringConfigSource[] stringConfigSources)
        {
            if(seal)
                throw new InvalidOperationException("Build is called, no further modification allowed, use new instance instead");
            ConfigInterfaceImplBase configInterfaceImplBase = ConfigBuilder.Instance.BuildInterface<T>();
            configInterfaceImplBase.Init(stringConfigSources, converters);
            builtTypes.Add(typeof(T), configInterfaceImplBase);
        }

        public ConfigurationService Build()
        {
            if (seal)
                throw new InvalidOperationException("Build is called, use new instance instead");
            seal = true;
            return new ConfigurationService(builtTypes);
        }
    }
}