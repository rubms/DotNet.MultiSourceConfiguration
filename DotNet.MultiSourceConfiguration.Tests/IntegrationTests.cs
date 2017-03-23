using System;
using MultiSourceConfiguration.Config.ConfigSource;
using NUnit.Framework;

namespace MultiSourceConfiguration.Config.Tests
{
	[TestFixture]
	public class IntegrationTests
	{
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

		[Test]
		public void IntValuesAreCorrectlyRetrieved()
		{
			var configurationBuilder = new ConfigurationBuilder();
			var memorySource = new MemorySource();
			memorySource.Add("test.int.property", "123");
			configurationBuilder.AddSources(memorySource);
			ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
			Assert.AreEqual(123, testConfigInstance.IntProperty);
		}

		[Test]
		public void StringValuesAreCorrectlyRetrieved()
		{
			var configurationBuilder = new ConfigurationBuilder();
			var memorySource = new MemorySource();
			memorySource.Add("test.string.propery", "test");
			configurationBuilder.AddSources(memorySource);
			ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
			Assert.AreEqual("test", testConfigInstance.StringProperty);
		}

		[Test]
		public void NonExistingPropertiesThrowException()
		{
			var configurationBuilder = new ConfigurationBuilder();
			var memorySource = new MemorySource();
			configurationBuilder.AddSources(memorySource);
			ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
			Assert.Throws(typeof(InvalidOperationException), () => {var foo = testConfigInstance.IntProperty;});
		}

		[Test]
		public void LongValuesAreCorrectlyRetrieved()
		{
			var configurationBuilder = new ConfigurationBuilder();
			var memorySource = new MemorySource();
			memorySource.Add("test.long.property", "123123123123");
			configurationBuilder.AddSources(memorySource);
			ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
			Assert.AreEqual(123123123123L, testConfigInstance.LongProperty);
		}

		[Test]
		public void DecimalValuesAreCorrectlyRetrieved()
		{
			var configurationBuilder = new ConfigurationBuilder();
			var memorySource = new MemorySource();
			memorySource.Add("test.decimal.property", "123123.123123");
			configurationBuilder.AddSources(memorySource);
			ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
			Assert.AreEqual(123123.123123m, testConfigInstance.DecimalProperty);
		}

		[Test]
		public void DoubleValuesAreCorrectlyRetrieved()
		{
			var configurationBuilder = new ConfigurationBuilder();
			var memorySource = new MemorySource();
			memorySource.Add("test.double.property", "123123.123123");
			configurationBuilder.AddSources(memorySource);
			ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
			Assert.AreEqual(123123.123123d, testConfigInstance.DoubleProperty);
		}

		[Test]
		public void FloatValuesAreCorrectlyRetrieved()
		{
			var configurationBuilder = new ConfigurationBuilder();
			var memorySource = new MemorySource();
			memorySource.Add("test.float.property", "123.123");
			configurationBuilder.AddSources(memorySource);
			ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
			Assert.AreEqual(123.123f, testConfigInstance.FloatProperty);
		}
	}
}
