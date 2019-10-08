﻿using System;
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
        /// Flag indicating whether the properties, in the configuration object, that have not
        /// been decorated with the Property annotation must be populated or not. For those properties,
        /// without an annotation, the name of the property itself will be used as configuration property name.
        /// The default value is false (non-decorated properties are ignored).
        /// </summary>
        bool HandleNonDecoratedProperties { get; set; }

        /// <summary>
        /// Adds more configuration sources to this configuration builder with a default priority of 1000. 
        /// Higher priority numbers indicate higher priority (that is, the configuration provided by them will overwrite the one
        /// read from sources with lower priority). Given the same priority, sources will be evaluated in the order they are provided:
        ///  - The properties provided by the first configuration source will be read.
        ///  - The properties provided by the second confiration source will be read, overwriting whatever was read for previous sources, and so on.
        /// Configuration sources are implementations of <see cref="IStringConfigSource"/> able to retrieve string
        /// property values given a property name. You can provide your own configuration 
        /// sources, though some sources are available out-of-the-box for the reading of
        /// application settings, command line arguments and environment variables.
        /// </summary>
        /// <param name="stringConfigSources">List of configuration sources to add to this configuration builder.</param>
        void AddSources(params IStringConfigSource[] stringConfigSources);

        /// <summary>
        /// Adds more configuration sources to this configuration builder, specifying a priority for them.
        /// Higher priority numbers indicate higher priority (that is, the configuration provided by them will overwrite the one
        /// read from sources with lower priority). Given the same priority, sources will be evaluated in the order they are provided:
        ///  - The properties provided by the first configuration source will be read.
        ///  - The properties provided by the second confiration source will be read, overwriting whatever was read for previous sources, and so on.
        /// Configuration sources are implementations of <see cref="IStringConfigSource"/> able to retrieve string
        /// property values given a property name. You can provide your own configuration 
        /// sources, though some sources are available out-of-the-box for the reading of
        /// application settings, command line arguments and environment variables.
        /// </summary>
        /// <param name="stringConfigSources">List of configuration sources to add to this configuration builder.</param>
        void AddSources(int priority, params IStringConfigSource[] stringConfigSources);

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
        /// <param name="prefix">A prefix to apply to each property name when finding a match in the property sources.</param>
        /// <param name="handleNonDecoratedProperties">Whether non-decorated config DTO properties should be populated (matched by property name) or not.</param>
        /// <returns>New instance of T, populated with the values read form configuration.</returns>
        T Build<T>(string prefix = null, bool handleNonDecoratedProperties = false) where T : class, new();

        /// <summary>
        /// Tries to get the string value of the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property that has to be retrieved.</param>
        /// <param name="propertyValue">Retrieved value of the property.</param>
        /// <returns>True when the property was successfully retrieved. False otherwise.</returns>
        bool TryGetStringValue(string propertyName, out string propertyValue);
    }
}