using MultiSourceConfiguration.Config;
using MultiSourceConfiguration.Config.Implementation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DotNet.MultiSourceConfiguration.Implementation
{
    public static class DefaultConverterFactory
    {
        public static Dictionary<Type, UnifiedConverter> GetDefaultConverters()
        {
            Dictionary<Type, UnifiedConverter> converters = new Dictionary<Type, UnifiedConverter>();
            converters.AddTypeConverter(new LambdaConverter<bool?>(null, s => Boolean.Parse(s)));
            converters.AddTypeConverter(new LambdaConverter<bool[]>(new bool[0], s => s.Split(',').Select(Boolean.Parse).ToArray()));
            converters.AddTypeConverter(new LambdaConverter<bool>(false, s => Boolean.Parse(s)));
            converters.AddTypeConverter(new LambdaConverter<List<bool>>(new List<bool>(), s => s.Split(',').Select(Boolean.Parse).ToList()));

            converters.AddTypeConverter(new LambdaConverter<int?>(null, s => Int32.Parse(s, CultureInfo.InvariantCulture)));
            converters.AddTypeConverter(new LambdaConverter<int[]>(new int[0], s => s.Split(',').Select(x => Int32.Parse(x, CultureInfo.InvariantCulture)).ToArray()));
            converters.AddTypeConverter(new LambdaConverter<int>(0, s => Int32.Parse(s, CultureInfo.InvariantCulture)));
            converters.AddTypeConverter(new LambdaConverter<List<int>>(new List<int>(), s => s.Split(',').Select(x => Int32.Parse(x, CultureInfo.InvariantCulture)).ToList()));

            converters.AddTypeConverter(new LambdaConverter<long?>(null, s => long.Parse(s, CultureInfo.InvariantCulture)));
            converters.AddTypeConverter(new LambdaConverter<long[]>(new long[0], s => s.Split(',').Select(x => long.Parse(x, CultureInfo.InvariantCulture)).ToArray()));
            converters.AddTypeConverter(new LambdaConverter<long>(0, s => long.Parse(s, CultureInfo.InvariantCulture)));
            converters.AddTypeConverter(new LambdaConverter<List<long>>(new List<long>(), s => s.Split(',').Select(x => long.Parse(x, CultureInfo.InvariantCulture)).ToList()));

            converters.AddTypeConverter(new LambdaConverter<string>(null, s => s));
            converters.AddTypeConverter(new LambdaConverter<string[]>(new string[0], s => s.Split(',')));
            converters.AddTypeConverter(new LambdaConverter<List<string>>(new List<string>(), s => s.Split(',').ToList()));

            converters.AddTypeConverter(new LambdaConverter<double?>(null, s => double.Parse(s, CultureInfo.InvariantCulture)));
            converters.AddTypeConverter(new LambdaConverter<double[]>(new double[0], s => s.Split(',').Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToArray()));
            converters.AddTypeConverter(new LambdaConverter<double>(0, s => double.Parse(s, CultureInfo.InvariantCulture)));
            converters.AddTypeConverter(new LambdaConverter<List<double>>(new List<double>(), s => s.Split(',').Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList()));

            converters.AddTypeConverter(new LambdaConverter<decimal?>(null, s => decimal.Parse(s, CultureInfo.InvariantCulture)));
            converters.AddTypeConverter(new LambdaConverter<decimal[]>(new decimal[0], s => s.Split(',').Select(x => decimal.Parse(x, CultureInfo.InvariantCulture)).ToArray()));
            converters.AddTypeConverter(new LambdaConverter<decimal>(0, s => decimal.Parse(s, CultureInfo.InvariantCulture)));
            converters.AddTypeConverter(new LambdaConverter<List<decimal>>(new List<decimal>(), s => s.Split(',').Select(x => decimal.Parse(x, CultureInfo.InvariantCulture)).ToList()));

            converters.AddTypeConverter(new LambdaConverter<float?>(null, s => float.Parse(s, CultureInfo.InvariantCulture)));
            converters.AddTypeConverter(new LambdaConverter<float[]>(new float[0], s => s.Split(',').Select(x => float.Parse(x, CultureInfo.InvariantCulture)).ToArray()));
            converters.AddTypeConverter(new LambdaConverter<float>(0, s => float.Parse(s, CultureInfo.InvariantCulture)));
            converters.AddTypeConverter(new LambdaConverter<List<float>>(new List<float>(), s => s.Split(',').Select(x => float.Parse(x, CultureInfo.InvariantCulture)).ToList()));
            return converters;
        }

        public static void AddTypeConverter<T>(this Dictionary<Type, UnifiedConverter> converters, ITypeConverter<T> converter)
        {
            converters.Add(typeof(T), new TypeConverterWrapper<T>(converter));
        }
    }
}
