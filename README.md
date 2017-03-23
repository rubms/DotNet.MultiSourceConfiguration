# DotNet.MultiSourceConfiguration
Configuration library with multiple sources for .NET.

## Why DotNet.MultiSourceConfiguration
A very typical scenario in microservices (which typically run in containers) is to configure a service via a configuration file, and overwrite that configuration with whatever you can find in environemnt variables and command line. Used to [Spring Boot's approach](http://docs.spring.io/spring-boot/docs/current/reference/html/boot-features-external-config.html) to configuration based in properties and property sources, I have struggled to find a simple library in .NET allowing to read configuration from different sources and overwrite it in a specified source order.

## How to use it
The approach followed by DotNet.MultiSourceConfiguration is the population of configuration interfaces, that can subsequently be registered on an IOC container or made avaialable as a static property. The properties of the interface must be decorated with the `Property` attribute, indicating the name of the property that must be mapped to the property:

```C#
    public interface ITestInterface
	{
		[Property("test.int.property")]
		int? IntProperty { get; }

		[Property("test.string.propery")]
		string StringProperty { get; }

		[Property("test.long.property")]
		long? LongProperty { get; }

		[Property("test.decimal.property")]
		decimal? DecimalProperty { get; }

		[Property("test.double.property")]
		double? DoubleProperty { get; }

		[Property("test.float.property")]
		float? FloatProperty { get; }
	}
```

Configuration interfaces are populated via a configuration builder, which can be specified a series of sources:
```C#
    class Program
    {
        static void Main(string[] args)
        {
            var configurationServiceBuilder = new ConfigurationServiceBuilder();
            configurationServiceBuilder.AddSources<AppConfiguration>(
                new AppSettingsSource(), new EnvironmentVariableSource(), new CommandLineSource(args));
			ITestInterface configurationInterface = configurationServiceBuilder.Build<ITestInterface>();
			...
        }
	}
```

The available property sources are:
* AppSettingsSource: looks for the properties in the .NET application settings file.
* EnvironmentVariableSource: looks for the properties in the system environment variables.
* CommandLineSource: tries to match the properties with arguments in the command line, with the format `--<property>=<value>`.
* MemorySource: allows to define a series of properties in memory as use them as source of configuration.