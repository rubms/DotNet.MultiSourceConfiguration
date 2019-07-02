using MultiSourceConfiguration.Config.Implementation;
using System;

namespace DotNet.MultiSourceConfiguration.Implementation
{
    class EnumConverter : UnifiedConverter
    {
        private Type type;

        public EnumConverter(Type type)
        {
            this.type = type;
        }

        public override Type Type => type;

        public override object FromString(string value)
        {
            return Enum.Parse(type, value);
        }

        public override object GetDefaultValue()
        {
            return Enum.GetValues(type).GetValue(0);
        }
    }
}
