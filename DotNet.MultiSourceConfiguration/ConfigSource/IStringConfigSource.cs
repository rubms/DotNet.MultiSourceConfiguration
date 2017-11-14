using System;

namespace MultiSourceConfiguration.Config.ConfigSource
{
    /// <summary>
    /// Configuration source for string properties.
    /// </summary>
    public interface IStringConfigSource
    {
        /// <summary>
        /// Expiration of the cache of read configuration properties.
        /// </summary>
        TimeSpan CacheExpiration { set; }

        /// <summary>
        /// Try and get a string property from the configuration source.
        /// </summary>
        /// <param name="property">The name of the property that must be retrieved.</param>
        /// <param name="value">Output parameter containing the read value for the property.</param>
        /// <returns>True when the property was successfully found in the source. False otherwise.</returns>
        bool TryGetString(string property, out string value);
    }
}