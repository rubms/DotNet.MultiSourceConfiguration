using System;
using MultiSourceConfiguration.Config.ConfigSource;
using NUnit.Framework;

namespace MultiSourceConfiguration.Config.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        public class TestConfigurationDto
        {
            [Property("test.int.property", Required = false)]
            public int? IntProperty { get; set; }

            [Property("test.string.property", Required = false)]
            public string StringProperty { get; set; }

            [Property("test.long.property", Required = false)]
            public long? LongProperty { get; set; }

            [Property("test.decimal.property", Required = false)]
            public decimal? DecimalProperty { get; set; }

            [Property("test.double.property", Required = false)]
            public double? DoubleProperty { get; set; }

            [Property("test.float.property", Required = false)]
            public float? FloatProperty { get; set; }
        }

        public class TestConfigurationDtoWithRequiredField
        {
            [Property("test.string.property", Required = true)]
            public string StringProperty { get; set; }
        }

        [Test]
        public void IntValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.int.property", "123");
            configurationBuilder.AddSources(memorySource);
            TestConfigurationDto testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual(123, testConfigInstance.IntProperty);
        }

        [Test]
        public void StringValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.string.property", "test");
            configurationBuilder.AddSources(memorySource);
            TestConfigurationDto testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual("test", testConfigInstance.StringProperty);
        }

        [Test]
        public void NonExistingPropertiesThrowException()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            configurationBuilder.AddSources(memorySource);
            Assert.Throws(typeof(InvalidOperationException), () => configurationBuilder.Build<TestConfigurationDtoWithRequiredField>());
        }

        [Test]
        public void LongValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.long.property", "123123123123");
            configurationBuilder.AddSources(memorySource);
            TestConfigurationDto testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual(123123123123L, testConfigInstance.LongProperty);
        }

        [Test]
        public void DecimalValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.decimal.property", "123123.123123");
            configurationBuilder.AddSources(memorySource);
            TestConfigurationDto testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual(123123.123123m, testConfigInstance.DecimalProperty);
        }

        [Test]
        public void DoubleValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.double.property", "123123.123123");
            configurationBuilder.AddSources(memorySource);
            TestConfigurationDto testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual(123123.123123d, testConfigInstance.DoubleProperty);
        }

        [Test]
        public void FloatValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.float.property", "123.123");
            configurationBuilder.AddSources(memorySource);
            TestConfigurationDto testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual(123.123f, testConfigInstance.FloatProperty);
        }

        [Test]
        public void ConfigurationIsOverwrittenBySubsequentSources()
        {
            var memorySource1 = new MemorySource();
            memorySource1.Add("test.string.property", "source1");
            var memorySource2 = new MemorySource();
            memorySource2.Add("test.string.property", "source2");

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(memorySource1, memorySource2);
            TestConfigurationDto testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();

            Assert.AreEqual("source2", testConfigInstance.StringProperty);
        }
    }
}
