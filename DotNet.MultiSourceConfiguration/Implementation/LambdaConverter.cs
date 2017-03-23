using System;

namespace MultiSourceConfiguration.Config.Implementation
{
    public class LambdaConverter<T> : ITypeConverter<T>
    {
        private readonly T defaultValue;
        private readonly Func<string, T> conversion;

        public LambdaConverter(T defaultValue, Func<string, T> conversion)
        {
            this.defaultValue = defaultValue;
            this.conversion = conversion;
        }

        public T FromString(string value)
        {
            return conversion(value);
        }

        public T GetDefaultValue()
        {
            return defaultValue;
        }
    }
}