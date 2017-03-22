using System;
using System.Collections.Generic;

namespace DotNet.MultiSourceConfiguration
{
    public class ConfigurationService
    {
        private readonly Dictionary<Type, object> builtTypes;

        internal ConfigurationService(Dictionary<Type, object> builtTypes)
        {
            this.builtTypes = builtTypes;
        }

        public T For<T>()
        {
            object impl;
            if(builtTypes.TryGetValue(typeof (T), out impl))
                return (T)impl;
            throw new InvalidOperationException(string.Format("Config interface  {0} is not registred", typeof(T).Name));
        }
    }
}