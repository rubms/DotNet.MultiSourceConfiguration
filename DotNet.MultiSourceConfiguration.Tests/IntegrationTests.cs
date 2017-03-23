using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiSourceConfiguration.Config.ConfigSource;

namespace MultiSourceConfiguration.Config.Tests
{
    [TestClass]
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

        [TestMethod]
        public void IntValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.int.property", "123");
            configurationBuilder.AddSources(memorySource);
            ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
            Assert.AreEqual(123, testConfigInstance.IntProperty);
        }

        [TestMethod]
        public void StringValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.string.propery", "test");
            configurationBuilder.AddSources(memorySource);
            ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
            Assert.AreEqual("test", testConfigInstance.StringProperty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NonExistingPropertiesThrowException()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            configurationBuilder.AddSources(memorySource);
            ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
            int? result = testConfigInstance.IntProperty;
        }

        [TestMethod]
        public void LongValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.long.property", "123123123123");
            configurationBuilder.AddSources(memorySource);
            ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
            Assert.AreEqual(123123123123L, testConfigInstance.LongProperty);
        }

        [TestMethod]
        public void DecimalValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.decimal.property", "123123.123123");
            configurationBuilder.AddSources(memorySource);
            ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
            Assert.AreEqual(123123.123123m, testConfigInstance.DecimalProperty);
        }

        [TestMethod]
        public void DoubleValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.double.property", "123123.123123");
            configurationBuilder.AddSources(memorySource);
            ITestInterface testConfigInstance = configurationBuilder.Build<ITestInterface>();
            Assert.AreEqual(123123.123123d, testConfigInstance.DoubleProperty);
        }

        [TestMethod]
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
