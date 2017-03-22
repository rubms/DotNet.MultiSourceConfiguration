using System;

namespace DotNet.MultiSourceConfiguration
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PropertyAttribute : Attribute
    {
        private readonly string property;

        public PropertyAttribute(string property)
        {
            this.property = property;
        }

        public string Property
        {
            get { return property; }
        }
    }
}