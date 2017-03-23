using System;

namespace MultiSourceConfiguration.Config
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PropertyAttribute : Attribute
    {
        private readonly string property;
		private bool required;

        public PropertyAttribute(string property)
        {
            this.property = property;
			required = false;
        }

        public string Property
        {
            get { return property; }
        }

		public bool Required { 
			get { return required; }

			set{ required = value; }
		}
    }
}