using System;

namespace MultiSourceConfiguration.Config
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PropertyAttribute : Attribute
    {
        private readonly string property;

        public PropertyAttribute(string property)
        {
            this.property = property;
			Required = false;
        }

        public string Property
        {
            get { return property; }
        }

		public bool Required { get; set; }

        public string Default { get; set; }
    }
}