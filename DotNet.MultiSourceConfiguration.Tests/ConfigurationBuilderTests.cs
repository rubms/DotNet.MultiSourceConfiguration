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
        private class IntPropertyDto
        {
            [Property("test.int.property", Required = false)]
            public int IntProperty { get; set; }
        }
        [Test]
        public void IntValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.int.property", "123");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<IntPropertyDto>();
            Assert.AreEqual(123, testConfigInstance.IntProperty);
        }

        private class NullableIntPropertyDto
        {
            [Property("test.int.property", Required = false)]
            public int? IntProperty { get; set; }
        }
        [Test]
        public void NullableIntValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.int.property", "123");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<NullableIntPropertyDto>();
            Assert.AreEqual(123, testConfigInstance.IntProperty);
        }

        private class StringPropertyDto
        {
            [Property("test.string.property", Required = false)]
            public string StringProperty { get; set; }
        }
        [Test]
        public void StringValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.string.property", "test");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<StringPropertyDto>();
            Assert.AreEqual("test", testConfigInstance.StringProperty);
        }

        private class LongPropertyDto
        {
            [Property("test.long.property", Required = false)]
            public long LongProperty { get; set; }
        }
        [Test]
        public void LongValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.long.property", "123123123123");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<LongPropertyDto>();
            Assert.AreEqual(123123123123L, testConfigInstance.LongProperty);
        }

        private class NullableLongPropertyDto
        {
            [Property("test.long.property", Required = false)]
            public long? LongProperty { get; set; }
        }
        [Test]
        public void NullableLongValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.long.property", "123123123123");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<NullableLongPropertyDto>();
            Assert.AreEqual(123123123123L, testConfigInstance.LongProperty);
        }

        private class DecimalPropertyDto
        {
            [Property("test.decimal.property", Required = false)]
            public decimal DecimalProperty { get; set; }
        }
        [Test]
        public void DecimalValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.decimal.property", "123123.123123");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<DecimalPropertyDto>();
            Assert.AreEqual(123123.123123m, testConfigInstance.DecimalProperty);
        }

        private class NullableDecimalPropertyDto
        {
            [Property("test.decimal.property", Required = false)]
            public decimal? DecimalProperty { get; set; }
        }
        [Test]
        public void NullableDecimalValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.decimal.property", "123123.123123");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<NullableDecimalPropertyDto>();
            Assert.AreEqual(123123.123123m, testConfigInstance.DecimalProperty);
        }

        private class DoublePropertyDto
        {
            [Property("test.double.property", Required = false)]
            public double DoubleProperty { get; set; }
        }
        [Test]
        public void DoubleValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.double.property", "123123.123123");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<DoublePropertyDto>();
            Assert.AreEqual(123123.123123d, testConfigInstance.DoubleProperty);
        }

        private class NullableDoublePropertyDto
        {
            [Property("test.double.property", Required = false)]
            public double? DoubleProperty { get; set; }
        }
        [Test]
        public void NullableDoubleValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.double.property", "123123.123123");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<NullableDoublePropertyDto>();
            Assert.AreEqual(123123.123123d, testConfigInstance.DoubleProperty);
        }

        private class FloatPropertyDto
        {
            [Property("test.float.property", Required = false)]
            public float FloatProperty { get; set; }
        }
        [Test]
        public void FloatValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.float.property", "123.123");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<FloatPropertyDto>();
            Assert.AreEqual(123.123f, testConfigInstance.FloatProperty);
        }

        private class NullableFloatPropertyDto
        {
            [Property("test.float.property", Required = false)]
            public float? FloatProperty { get; set; }
        }
        [Test]
        public void NullableFloatValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.float.property", "123.123");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<NullableFloatPropertyDto>();
            Assert.AreEqual(123.123f, testConfigInstance.FloatProperty);
        }

        private class BoolPropertyDto
        {
            [Property("test.bool.property", Required = false)]
            public bool BoolProperty { get; set; }
        }
        [Test]
        public void TrueBoolValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.bool.property", "true");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<BoolPropertyDto>();
            Assert.AreEqual(true, testConfigInstance.BoolProperty);
        }

        private class NullableBoolPropertyDto
        {
            [Property("test.bool.property", Required = false)]
            public bool? BoolProperty { get; set; }
        }
        [Test]
        public void NullableTrueBoolValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.bool.property", "true");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<NullableBoolPropertyDto>();
            Assert.AreEqual(true, testConfigInstance.BoolProperty);
        }

        [Test]
        public void FalseBoolValuesAreCorrectlyRetrieved()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var memorySource = new MemorySource();
            memorySource.Add("test.bool.property", "false");
            configurationBuilder.AddSources(memorySource);
            var testConfigInstance = configurationBuilder.Build<BoolPropertyDto>();
            Assert.AreEqual(false, testConfigInstance.BoolProperty);
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
    }
}
