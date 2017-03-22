using System;

namespace DotNet.MultiSourceConfiguration.Implementation
{
    class TypeConverterWrapper<T> : UnifiedConverter, ITypeConverter<T>
    {
        private readonly ITypeConverter<T> wrapped;

        public TypeConverterWrapper(ITypeConverter<T> wrapped)
        {
            this.wrapped = wrapped;
        }

        public override Type Type
        {
            get { return typeof (T); }
        }

        public override object FromString(string value)
        {
            return wrapped.FromString(value);
        }

        public override object GetDefaultValue()
        {
            return wrapped.GetDefaultValue();
        }

        T ITypeConverter<T>.GetDefaultValue()
        {
            return wrapped.GetDefaultValue();
        }

        T ITypeConverter<T>.FromString(string value)
        {
            return wrapped.FromString(value);
        }
    }
}