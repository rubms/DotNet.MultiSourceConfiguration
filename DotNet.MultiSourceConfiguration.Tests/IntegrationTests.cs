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
    }
}
