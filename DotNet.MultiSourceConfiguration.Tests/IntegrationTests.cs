using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.MultiSourceConfiguration.ConfigSource;

namespace DotNet.MultiSourceConfiguration.Tests
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
            var configurationServiceBuilder = new ConfigurationServiceBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.int.property", "123");
            configurationServiceBuilder.RegisterConfigInterface<ITestInterface>(memorySource);
            ConfigurationService configService = configurationServiceBuilder.Build();
            Assert.AreEqual(123, configService.For<ITestInterface>().IntProperty);
        }

        [TestMethod]
        public void StringValuesAreCorrectlyRetrieved()
        {
            var configurationServiceBuilder = new ConfigurationServiceBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.string.propery", "test");
            configurationServiceBuilder.RegisterConfigInterface<ITestInterface>(memorySource);
            ConfigurationService configService = configurationServiceBuilder.Build();
            Assert.AreEqual("test", configService.For<ITestInterface>().StringProperty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NonExistingPropertiesThrowException()
        {
            var configurationServiceBuilder = new ConfigurationServiceBuilder();
            var memorySource = new MemorySource();
            configurationServiceBuilder.RegisterConfigInterface<ITestInterface>(memorySource);
            ConfigurationService configService = configurationServiceBuilder.Build();
            int? result = configService.For<ITestInterface>().IntProperty;
        }

        [TestMethod]
        public void LongValuesAreCorrectlyRetrieved()
        {
            var configurationServiceBuilder = new ConfigurationServiceBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.long.property", "123123123123");
            configurationServiceBuilder.RegisterConfigInterface<ITestInterface>(memorySource);
            ConfigurationService configService = configurationServiceBuilder.Build();
            Assert.AreEqual(123123123123L, configService.For<ITestInterface>().LongProperty);
        }

        [TestMethod]
        public void DecimalValuesAreCorrectlyRetrieved()
        {
            var configurationServiceBuilder = new ConfigurationServiceBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.decimal.property", "123123.123123");
            configurationServiceBuilder.RegisterConfigInterface<ITestInterface>(memorySource);
            ConfigurationService configService = configurationServiceBuilder.Build();
            Assert.AreEqual(123123.123123m, configService.For<ITestInterface>().DecimalProperty);
        }

        [TestMethod]
        public void DoubleValuesAreCorrectlyRetrieved()
        {
            var configurationServiceBuilder = new ConfigurationServiceBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.double.property", "123123.123123");
            configurationServiceBuilder.RegisterConfigInterface<ITestInterface>(memorySource);
            ConfigurationService configService = configurationServiceBuilder.Build();
            Assert.AreEqual(123123.123123d, configService.For<ITestInterface>().DoubleProperty);
        }

        [TestMethod]
        public void FloatValuesAreCorrectlyRetrieved()
        {
            var configurationServiceBuilder = new ConfigurationServiceBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.float.property", "123.123");
            configurationServiceBuilder.RegisterConfigInterface<ITestInterface>(memorySource);
            ConfigurationService configService = configurationServiceBuilder.Build();
            Assert.AreEqual(123.123f, configService.For<ITestInterface>().FloatProperty);
        }
    }
}
