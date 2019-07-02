using Moq;
using MultiSourceConfiguration.Config.ConfigSource;
using NUnit.Framework;
using System;
using System.Threading;

namespace MultiSourceConfiguration.Config.Tests
{
	[TestFixture]
	public class ConfigurationBuilderTests
	{
        private class StringPropertyDto
        {
            [Property("test.string.property", Required = false)]
            public string StringProperty { get; set; }
        }

        [Test]
        public void CacheDisabledByDefault()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            configurationBuilder.AddSources(memorySource);

            memorySource.Add("test.string.property", "test");
            var testConfigInstance = configurationBuilder.Build<StringPropertyDto>();
            Assert.AreEqual("test", testConfigInstance.StringProperty);

            memorySource.Add("test.string.property", "test2");
            testConfigInstance = configurationBuilder.Build<StringPropertyDto>();
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
            var testConfigInstance = configurationBuilder.Build<StringPropertyDto>();
            Assert.AreEqual("test", testConfigInstance.StringProperty);

            memorySource.Add("test.string.property", "test2");
            Thread.Sleep(500);
            testConfigInstance = configurationBuilder.Build<StringPropertyDto>();
            Assert.AreEqual("test", testConfigInstance.StringProperty);

            Thread.Sleep(1500);
            testConfigInstance = configurationBuilder.Build<StringPropertyDto>();
            Assert.AreEqual("test2", testConfigInstance.StringProperty);
        }

        [Test]
		public void CacheExpirationIsPropagatedToAddedPropertySources()
		{
            var testCacheExpiration = TimeSpan.FromMinutes(5);
            var propertySourceMock = new Mock<IStringConfigSource>();

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.CacheExpiration = testCacheExpiration;
            configurationBuilder.AddSources(propertySourceMock.Object);

            propertySourceMock.VerifySet(x => x.CacheExpiration = testCacheExpiration);
        }

        [Test]
        public void CacheExpirationIsPropagatedWhenModified()
        {
            var testCacheExpiration = TimeSpan.FromMinutes(5);
            var propertySourceMock = new Mock<IStringConfigSource>();

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySourceMock.Object);
            configurationBuilder.CacheExpiration = testCacheExpiration;

            propertySourceMock.VerifySet(x => x.CacheExpiration = testCacheExpiration);
        }

        [Test]
        public void ConfigurationIsOverwrittenInTheProvidedSourceOrder()
        {
            string testPropertyName = "testPropertyName";
            var propertySource1 = new MemorySource();
            propertySource1.Add(testPropertyName, "1");

            var propertySource2 = new MemorySource();
            propertySource2.Add(testPropertyName, "2");

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1, propertySource2);

            string obtainedValue;
            Assert.IsTrue(configurationBuilder.TryGetStringValue(testPropertyName, out obtainedValue));
            Assert.AreEqual("2", obtainedValue);
        }

        [Test]
        public void ConfigurationSetByASourceIsNotMistakenlyOverwrittenBySubsequentSources()
        {
            string testPropertyName = "testPropertyName";
            var propertySource1 = new MemorySource();
            propertySource1.Add(testPropertyName, "1");

            var propertySource2 = new MemorySource();

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1, propertySource2);

            string obtainedValue;
            Assert.IsTrue(configurationBuilder.TryGetStringValue(testPropertyName, out obtainedValue));
            Assert.AreEqual("1", obtainedValue);
        }

        [Test]
        public void TryGetReturnsFalseWithNotFoundProperty()
        {
            string testPropertyName = "testPropertyName";
            var propertySource1 = new MemorySource();

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);

            string obtainedValue;
            Assert.IsFalse(configurationBuilder.TryGetStringValue(testPropertyName, out obtainedValue));
        }

        private class RequiredParameterWithoutDefaultValue
        {
            [Property("testPropertyName", Required = true)]
            public int requiredProperty { get; set; }
        }
        [Test]
        public void RequiredParametersWithoutDefaultValueFail()
        {
            var propertySource1 = new MemorySource();

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);

            Assert.Throws<InvalidOperationException>(() => configurationBuilder.Build<RequiredParameterWithoutDefaultValue>());
        }

        private class RequiredParameterWithDefaultValue
        {
            [Property("testPropertyName", Required = true, Default = "1")]
            public int requiredProperty { get; set; }
        }
        [Test]
        public void RequiredParametersWithDefaultValueReceiveDefaultValue()
        {
            var propertySource1 = new MemorySource();

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);

            var configDto = configurationBuilder.Build<RequiredParameterWithDefaultValue>();
            Assert.AreEqual(1, configDto.requiredProperty);
        }

        private class ParameterWithAlreadySetValue
        {
            public ParameterWithAlreadySetValue()
            {
                AlreadySetProperty = 3;
            }

            [Property("testPropertyName")]
            public int AlreadySetProperty { get; set; }
        }
        [Test]
        public void NotFoundPropertiesAreNotOverwritten()
        {
            var propertySource1 = new MemorySource();

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);

            var configDto = configurationBuilder.Build<ParameterWithAlreadySetValue>();
            Assert.AreEqual(3, configDto.AlreadySetProperty);
        }

        private class PropertyWithDefaultRequiredProperty
        {
            [Property("testPropertyName")]
            public int testProperty { get; set; }
        }
        [Test]
        public void PropertiesAreNotRequiredByDefault()
        {
            var propertySource1 = new MemorySource();

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);

            Assert.DoesNotThrow(() => {
                var configDto = configurationBuilder.Build<PropertyWithDefaultRequiredProperty>();
                Assert.AreEqual(0, configDto.testProperty);
            });
        }

        private class NonDecoratedPropertyDto
        {
            public string NonDecoratedTestProperty { get; set; }
        }
        [Test]
        public void NonDecoratedPropertiesAreByDefaultIgnored()
        {
            var propertySource1 = new MemorySource();
            propertySource1.Add("NonDecoratedTestProperty", "testValue");

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);

            var configDto = configurationBuilder.Build<NonDecoratedPropertyDto>();
            Assert.IsNull(configDto.NonDecoratedTestProperty);
        }

        [Test]
        public void WhenActiveNonDecoratedPropertiesArePopulatedUsingTheNameOfThePropertyItself()
        {
            var propertySource1 = new MemorySource();
            propertySource1.Add("NonDecoratedTestProperty", "testValue");

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);
            configurationBuilder.HandleNonDecoratedProperties = true;

            var configDto = configurationBuilder.Build<NonDecoratedPropertyDto>();
            Assert.AreEqual("testValue", configDto.NonDecoratedTestProperty);
        }

        private class PropertyWithPrefixDto
        {
            [Property("testPropertyName")]
            public string testProperty { get; set; }
        }

        [Test]
        public void WhenSpecifiedThePropertiesPrefixIsUsed()
        {
            var propertySource1 = new MemorySource();
            propertySource1.Add("testPrefix.testPropertyName", "testValue");

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);

            var configDto = configurationBuilder.Build<PropertyWithPrefixDto>(propertiesPrefix: "testPrefix.");
            Assert.AreEqual("testValue", configDto.testProperty);
        }

        private class UndecoratedPropertyDto
        {
            public string testProperty { get; set; }
        }
        [Test]
        public void WhenHandleUndecoratedPropertiesIsUsedTheUndecoratedPropertiesArePopulated()
        {
            var propertySource1 = new MemorySource();
            propertySource1.Add("testProperty", "testValue");

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);

            var configDto = configurationBuilder.Build<UndecoratedPropertyDto>(handleNonDecoratedProperties: true);
            Assert.AreEqual("testValue", configDto.testProperty);
        }

        [Test]
        public void ByDefaultTheUndecoratedPropertiesAreNotPopulated()
        {
            var propertySource1 = new MemorySource();
            propertySource1.Add("testProperty", "testValue");

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);

            var configDto = configurationBuilder.Build<UndecoratedPropertyDto>();
            Assert.IsNull(configDto.testProperty);
        }

        [Test]
        public void BothHandleUndecoratedPropertiesAndPrefixCorectlyWorkTogether()
        {
            var propertySource1 = new MemorySource();
            propertySource1.Add("someprefix.testProperty", "testValue");

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);

            var configDto = configurationBuilder.Build<UndecoratedPropertyDto>(propertiesPrefix: "someprefix.", handleNonDecoratedProperties: true);
            Assert.AreEqual("testValue", configDto.testProperty);
        }

        private class DtoWithEnumProperties
        {
            [Property("testProperty")]
            public DayOfWeek testProperty { get; set; }
        }
        [Test]
        public void EnumPropertiesAreAutomaticallyConverted()
        {
            var propertySource1 = new MemorySource();
            propertySource1.Add("testProperty", "Monday");

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddSources(propertySource1);

            var configDto = configurationBuilder.Build<DtoWithEnumProperties>();
            Assert.AreEqual(DayOfWeek.Monday, configDto.testProperty);
        }
    }
}
