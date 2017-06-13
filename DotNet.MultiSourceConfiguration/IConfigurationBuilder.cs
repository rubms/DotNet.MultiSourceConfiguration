using System;
using MultiSourceConfiguration.Config.ConfigSource;

namespace MultiSourceConfiguration.Config
{
    public interface IConfigurationBuilder
    {
        /// <summary>
        /// Amount of time in which configuration objects stored in the cache will expire.
        /// After expiration the configuration will again be read from the configuration sources.
        /// </summary>
        TimeSpan CacheExpiration { get; set; }

        /// <summary>
        /// Adds more configuration sources to this configuration builder. Configuration sources
        /// are implementations of <see cref="IStringConfigSource"/> able to retrieve string
        /// property values given a property name. You can provide your own configuration 
        /// sources, though some sources are available out-of-the-box for the reading of
        /// application settings, command line arguments and environment variables.
        /// </summary>
        /// <param name="stringConfigSources">List of configuration sources to add to this configuration builder.</param>
        void AddSources(params IStringConfigSource[] stringConfigSources);

        /// <summary>
        /// Adds more type converters to this configuration builder. Converters are used to
        /// convert string values to primitive or object data types.  You can provide your own  
        /// converters, though some are available out-of-the-box for most of the primitive
        /// data types.
        /// </summary>
        /// <typeparam name="T">Type to which to convert from a string value.</typeparam>
        /// <param name="converter">Converter, able to convert string values to primitive or object values of type T.</param>
        void AddTypeConverter<T>(ITypeConverter<T> converter);

        /// <summary>
        /// Builds a new instance of T, populating it with the corresponding values read from 
        /// the configuration sources.
        /// </summary>
        /// <typeparam name="T">Type of the configuration object to instance and populate.</typeparam>
        /// <returns>New instance of T, populated with the values read form configuration.</returns>
        T Build<T>() where T : class, new();
    }
}