using System;

namespace MultiSourceConfiguration.Config.Implementation
{
    public abstract class UnifiedConverter
    {
        public abstract Type Type { get; }
        public abstract object FromString(string value);
        public abstract object GetDefaultValue();
    }
}