using Moq;
using MultiSourceConfiguration.Config.ConfigSource;
using NUnit.Framework;
using System;

namespace MultiSourceConfiguration.Config.Tests
{
	[TestFixture]
	public class ConfigurationBuilderTests
	{
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
    }
}
