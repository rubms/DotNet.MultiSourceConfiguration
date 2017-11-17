# DotNet.MultiSourceConfiguration
[![NuGet Version](https://img.shields.io/nuget/v/DotNet.MultiSourceConfiguration.svg?style=flat)](https://www.nuget.org/packages/DotNet.MultiSourceConfiguration)
[![Build Status](https://travis-ci.org/rubms/DotNet.MultiSourceConfiguration.svg?branch=master)](https://travis-ci.org/rubms/DotNet.MultiSourceConfiguration)

Configuration library with multiple sources for .NET. 

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
        // By default properties are not required
        [Property("test.int.property")]
        public int? IntProperty { get; set; }

        // The required condition of a property can be explicitly included
        [Property("test.string.property", Required = true)]
        public string StringProperty { get; set; }

        // Properties can be marked as not required. The default value of the
        // given type converter will be applied (typically, null).
        [Property("test.long.property", Required = false)]
        public long? LongProperty { get; set; }

        // The "Default" property can be used to provide a default value in 
        // case it is not provided via configuration.
        [Property("test.bool.property", Default = "true")]
        public bool? BoolProperty { get; set; }
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
            TestConfigurationDto configurationDto = configurationBuilder.Build<TestConfigurationDto>();
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

### Properties

Properties in configuration objects that you want to populate with DotNet.MultiSourceConfiguration must be decorated with the `Property` annotation. This annotation receives the name of the configuration property that must be read from configuration in order to set the value to the configuration object property. 

```C#
    public class TestConfigurationDto
    {
    	[Property("test.int.property")]
        public int IntProperty { get; set; }
    }
    
    [Test]
    public void Test()
    {
	    var memoryConfigurationSource = new MemorySource();
    	memoryConfigurationSource.Add("test.int.property", "123");

		IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
		configurationBuilder.AddSources(memoryConfigurationSource);

		TestConfigurationDto configurationDto = configurationBuilder.Build<TestConfigurationDto>();
        
        Assert.AreEqual(123, configurationDto.IntProperty);
	}
    
```

The `Property` annotation accepts the following attributes:

* _Required (bool)_: When set to true, the corresponding configuration property will be mandatory. An `InvalidOperationException` will be thrown if it is not possible to read the property value from at least one configuration source. Configuration properties are not required by default.
* _Default (string)_: A default value to set to the configuration property in case the it was not possible to read the property value from any configuration source.

Properties in the configuration object that are not decorated with the `Property` annotation will not be populated by default. 

```C#
    public class TestConfigurationDto
    {
        // By default only properties decorated with the Property annotation are populated
        [Property("test.int.property")]
        public int? IntProperty { get; set; }
        
        // This property will be ignored, unless HandleNonDecoratedProperties is set to true
        public int? IgnoredProperty { get; set; }
    }
```

In case you want DotNet.MultiSourceConfiguration to also populate non-decorated configuration object properties you may set the `HandleNonDecoratedProperties` property of `IConfigurationBuilder` to `true`:
```C#
configurationBuilder.HandleNonDecoratedProperties = true;
```
In this case, the name of the configuration object property itself will be used as property name when retrieving the configuration from the configuration sources. 
```C#
    public class TestConfigurationDto
    {
        public int NonDecoratedProperty { get; set; }
    }
    
    [Test]
    public void Test()
    {
	    var memoryConfigurationSource = new MemorySource();
    	memoryConfigurationSource.Add("NonDecoratedProperty", "123");

		IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
		configurationBuilder.AddSources(memoryConfigurationSource);
		configurationBuilder.HandleNonDecoratedProperties = true;

		TestConfigurationDto configurationDto = configurationBuilder.Build<TestConfigurationDto>();
        
        Assert.AreEqual(123, configurationDto.NonDecoratedProperty);
	}
    
```
This behavior is useful when you are introducing DotNet.MultiSourceConfiguration in an already existing application that has a big number of configuration objects.


#### Property Prefixes

It is possible to indicate a prefix when building a configuration object in the `IConfigurationBuilder.Build<T>(string propertiesPrefix)` function. This will add the specified prefix to each one of the property names when trying to find the property value in the different configuration sources: 
```C#
    public class TestConfigurationDto
    {
    	[Property("testProperty")]
        public int IntProperty { get; set; }
    }
    
    [Test]
    public void Test()
    {
	    var memoryConfigurationSource = new MemorySource();
    	memoryConfigurationSource.Add("myComponent1.testProperty", "1");
    	memoryConfigurationSource.Add("myComponent2.testProperty", "2");

		IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
		configurationBuilder.AddSources(memoryConfigurationSource);

		TestConfigurationDto configurationDto1 = configurationBuilder.Build<TestConfigurationDto>(propertiesPrefix: "myComponent1.");
		TestConfigurationDto configurationDto2 = configurationBuilder.Build<TestConfigurationDto>(propertiesPrefix: "myComponent2.");
        
        Assert.AreEqual(1, configurationDto1.IntProperty);
        Assert.AreEqual(2, configurationDto2.IntProperty);
	}
    
```

This is useful when you want to reuse the same configuration object in different contexts, each one having a different prefix.

### Property Sources

The available, out-of-the-box, property sources are:
* AppSettingsSource: looks for the properties in the .NET application settings file.
* EnvironmentVariableSource: looks for the properties in the system environment variables.
* CommandLineSource: tries to match the properties with arguments in the command line, with the format `--<property>=<value>`.
* MemorySource: allows to define a series of properties in memory as use them as source of configuration.

There are some additional projects that provide property sources for some common configuration services, like [DotNet.MultisourceConfiguration.Zookeeper](https://github.com/rubms/DotNet.MultisourceConfiguration.Zookeeper), that provides a configuration source for [ZooKeeper](https://zookeeper.apache.org/).

In addition to these property sources you can implement your own by providing implementations of the `IStringConfigSource` interface:
```C#
    public interface IStringConfigSource
    {
		TimeSpan CacheExpiration { set; }
        bool TryGetString(string property, out string value);
    }
```

The configuration properties are overwritten by the given property sources in the order they are specified in the `AddSources` call. In the example above, the properties will be first read in the application settings file. Subsequently they will be overwritten with the properties found in environment variables (in case they are found). Finally, the properties will be overwritten with the values found in command-line arguments.

This provides a very convenient deployment behavior (specially for applications running in containers), in which applications take some default configuration from application settings, that is overwritten by the environment variables set in the machine (or container) and are finally overwritten with whatever has been provided in command line arguments.

It is also possible to perform several calls to `AddSources`. The given configuration sources are appended to the already existing list. This way, the new provided sources will overwrite configuration properties already set by configuration sources set in previous cllas to `AddSources`.

### Property Types

The following types for configuration properties are available:
* bool, bool?, bool[], List<bool>
* string, string[], List<string>
* int, int?, int[], List<int>
* long, long?, long[], List<long>
* decimal, decimal?, decimal[], List<decimal>
* float, float?, float[], List<float>
* double, double?, double[], List<double>

In addition to these types, you can add your own type converters by providing implementations of the `ITypeConverter` interface to the `AddTypeConverter<T>()` method of `ConfigurationBuilder`. For convenience, the `LambdaConverter` is provided, that makes it easier to implement your own type converter:
```C#
    configurationBuilder.AddTypeConverter(new LambdaConverter<MyType>(null /* Default value */, s => MyType.Parse(s) /* Converter lambda */));
```

