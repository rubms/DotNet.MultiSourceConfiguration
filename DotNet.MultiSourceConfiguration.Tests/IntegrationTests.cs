﻿using System;
using MultiSourceConfiguration.Config.ConfigSource;
using NUnit.Framework;
using System.Threading;

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

            [Property("test.bool.property", Required = false)]
            public bool? BoolProperty { get; set; }
        }

        public class TestConfigurationDtoWithRequiredField
        {
            [Property("test.string.property", Required = true)]
            public string StringProperty { get; set; }
        }

        public class TestConfigurationDtoWithDefaultValue
        {
            [Property("test.string.property", Default = "testValue")]
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
        public void TrueBoolValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.bool.property", "true");
            configurationBuilder.AddSources(memorySource);
            TestConfigurationDto testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual(true, testConfigInstance.BoolProperty);
        }

        [Test]
        public void FalseBoolValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.bool.property", "false");
            configurationBuilder.AddSources(memorySource);
            TestConfigurationDto testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual(false, testConfigInstance.BoolProperty);
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

        [Test]
        public void CacheDisabledByDefault()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            configurationBuilder.AddSources(memorySource);

            memorySource.Add("test.string.property", "test");
            TestConfigurationDto testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual("test", testConfigInstance.StringProperty);

            memorySource.Add("test.string.property", "test2");
            testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual("test2", testConfigInstance.StringProperty);
        }

        [Test]
        public void CacheAcceptsTheConfiguredExpiration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            configurationBuilder.AddSources(memorySource);
            configurationBuilder.CacheExpiration = TimeSpan.FromSeconds(1);

            memorySource.Add("test.string.property", "test");
            TestConfigurationDto testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual("test", testConfigInstance.StringProperty);

            memorySource.Add("test.string.property", "test2");
            Thread.Sleep(500);
            testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual("test", testConfigInstance.StringProperty);

            Thread.Sleep(1500);
            testConfigInstance = configurationBuilder.Build<TestConfigurationDto>();
            Assert.AreEqual("test2", testConfigInstance.StringProperty);
        }

        [Test]
        public void DefaultValuesAreCorrectlyRead()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            configurationBuilder.AddSources(memorySource);

            TestConfigurationDtoWithDefaultValue testConfigInstance = configurationBuilder.Build<TestConfigurationDtoWithDefaultValue>();
            Assert.AreEqual("testValue", testConfigInstance.StringProperty);
        }
    }
}
