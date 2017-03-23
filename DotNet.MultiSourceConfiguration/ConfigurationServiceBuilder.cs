using System;
using System.Collections.Generic;
using System.Linq;
using MultiSourceConfiguration.Config.ConfigSource;
using MultiSourceConfiguration.Config.Implementation;

namespace MultiSourceConfiguration.Config
{
    public class ConfigurationBuilder
    {
        private readonly Dictionary<Type, UnifiedConverter> converters = new Dictionary<Type, UnifiedConverter>();
        private IStringConfigSource[] stringConfigSources;

        public ConfigurationBuilder()
        {
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
        }

        public void AddTypeConverter<T>(ITypeConverter<T> converter)
        {
            converters.Add(typeof(T), new TypeConverterWrapper<T>(converter));
        }

        public void AddSources(params IStringConfigSource[] stringConfigSources)
        {
            this.stringConfigSources = stringConfigSources;
        }

        public T Build<T>() where T : class
        {
            ConfigInterfaceImplBase configInterfaceImplBase = ConfigBuilder.Instance.BuildInterface<T>();
            configInterfaceImplBase.Init(stringConfigSources, converters);
            return configInterfaceImplBase as T;
        }
    }
}