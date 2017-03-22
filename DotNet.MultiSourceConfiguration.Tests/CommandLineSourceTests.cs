using DotNet.MultiSourceConfiguration.ConfigSource;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNet.MultiSourceConfiguration.Tests
{
    [TestClass]
    public class CommandLineSourceTests
    {
        [TestMethod]
        public void PropertiesAreCorrectlyParsed()
        {
            string[] args = new string[] { "--test.property=123", "" };
            var commandLineSource = new CommandLineSource(args);
            string value;
            Assert.IsTrue(commandLineSource.TryGetString("test.property", out value));
            Assert.AreEqual("123", value);
        }

        [TestMethod]
        public void PropertiesMayContainValues()
        {
            string[] args = new string[] { "--test.property=test string property with spaces", "" };
            var commandLineSource = new CommandLineSource(args);
            string value;
            Assert.IsTrue(commandLineSource.TryGetString("test.property", out value));
            Assert.AreEqual("test string property with spaces", value);
        }
    }
}
