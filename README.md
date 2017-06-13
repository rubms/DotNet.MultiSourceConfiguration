# DotNet.MultiSourceConfiguration
Configuration library with multiple sources for .NET. [![Build Status](https://travis-ci.org/rubms/DotNet.MultiSourceConfiguration.svg?branch=master)](https://travis-ci.org/rubms/DotNet.MultiSourceConfiguration)

## Why DotNet.MultiSourceConfiguration
A very typical scenario in microservices (which typically run in containers) is to configure a service via a configuration file, and overwrite that configuration with whatever you can find in environment variables and command line. Used to [Spring Boot's approach](http://docs.spring.io/spring-boot/docs/current/reference/html/boot-features-external-config.html) to configuration based in properties and property sources, I have struggled to find a simple library in .NET allowing to read configuration from different sources and overwrite it in a specified source order.

The Microsoft.Extensions.Configuration project follow a very similar approach but have some drawbacks:
* It has a huge amount of dependencies.
* At the moment of writing DotNet.MultiSourceConfiguration, the existing documentation was outdated and did not work with the last version of the library.

## How to use it
The approach followed by DotNet.MultiSourceConfiguration is the population of configuration classes, that can subsequently be registered on an IOC container or made avaialable as a static property. The properties of the configuration class must be decorated with the `Property` attribute, indicating the name of the property that must be mapped to the property:

```C#
    public class TestConfigurationDto
    {
        // By default properties are required
        [Property("test.int.property")]
        public int? IntProperty { get; set; }

        // The required condition of a property can be explicitly included
        [Property("test.string.property", Required = true)]
        public string StringProperty { get; set; }

        // Properties can be marked as not required. The default value of the
        // given type converter will be applied (typically, null).
        [Property("test.long.property", Required = false)]
        public long? LongProperty { get; set; }
    }
```

Configuration classes are populated via a configuration builder, which can be specified a series of sources:
```C#
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(
                new AppSettingsSource(), new EnvironmentVariableSource(), new CommandLineSource(args));
            TestConfigurationDto configurationInterface = configurationBuilder.Build<TestConfigurationDto>();
            ...
        }
    }
```

The configuration builder implements the IConfigurationBuilder interface. This way, the configuration builder is easier to mock in unit tests of classes that depend on it. A common pattern is to register the configuration builder in an IoC container and inject it in classes that need it.

The configuration builder has caching capabilities, allowing to re-use built configuration classes until a configurable cache expiration times out. When the cache expires and a configuration class is re-built, then the configuration is re-read from the sources. The cache expiration is configurable via the `CacheExpiration` property (by default the cache expiration is 0, i.e. no caching is done):
```C#
	IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
	configurationBuilder.AddSources(
		new AppSettingsSource(), new EnvironmentVariableSource(), new CommandLineSource(args));
	configurationBuilder.CacheExpiration = TimeSpan.FromMinutes(2);
```


### Property Sources

The available property sources are:
* AppSettingsSource: looks for the properties in the .NET application settings file.
* EnvironmentVariableSource: looks for the properties in the system environment variables.
* CommandLineSource: tries to match the properties with arguments in the command line, with the format `--<property>=<value>`.
* MemorySource: allows to define a series of properties in memory as use them as source of configuration.

In addition to these property sources you can implement your own by providing implementations of the `IStringConfigSource` interface:
```C#
    public interface IStringConfigSource
    {
        bool TryGetString(string property, out string value);
    }
```

The configuration properties are overwritten by the given property sources in the order they are specified in the `AddSources` call. In the example above, the properties will be first read in the application settings file. Subsequently they will be overwritten with the properties found in environment variables (in case they are found). Finally, the properties will be overwritten with the values found in command-line arguments.

This provides a very convenient deployment behavior (specially for applications running in containers), in which applications take some default configuration from application settings, that is overwritten by the environment variables set in the machine (or container) and are finally overwritten with whatever has been provided in command line arguments.

### Property Types

The following types for configuration properties are available:
* bool?, bool?[]
* string, string[]
* int?, int[]
* long?, long[]
* decimal?, decimal[]
* float?, float[]
* double?, double[]

In addition to these types, you can add your own type converters by providing implementations of the `ITypeConverter` interface to the `AddTypeConverter<T>()` method of `ConfigurationBuilder`. For convenience, the `LambdaConverter` is provided, that makes it easier to implement your own type converter:
```C#
    configurationBuilder.AddTypeConverter(new LambdaConverter<MyType>(null /* Default value */, s => MyType.Parse(s) /* Converter lambda */));
```

## Get DotNet.MultiSourceConfiguration
DotNet.MultiSourceConfiguration is available in NuGet [![NuGet Version](https://img.shields.io/nuget/v/DotNet.MultiSourceConfiguration.svg?style=flat)](https://www.nuget.org/packages/DotNet.MultiSourceConfiguration)