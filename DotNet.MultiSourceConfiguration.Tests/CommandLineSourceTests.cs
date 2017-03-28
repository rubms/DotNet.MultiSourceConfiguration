using MultiSourceConfiguration.Config.ConfigSource;
using NUnit.Framework;

namespace MultiSourceConfiguration.Config.Tests
{
	[TestFixture]
	public class CommandLineSourceTests
	{
		[Test]
		public void PropertiesAreCorrectlyParsed()
		{
			string[] args = new string[] { "--test.property=123", "" };
			var commandLineSource = new CommandLineSource(args);
			string value;
			Assert.IsTrue(commandLineSource.TryGetString("test.property", out value));
			Assert.AreEqual("123", value);
		}

		[Test]
		public void PropertyValuesMayContainSpaces()
		{
			string[] args = new string[] { "--test.property=test string property with spaces", "" };
			var commandLineSource = new CommandLineSource(args);
			string value;
			Assert.IsTrue(commandLineSource.TryGetString("test.property", out value));
			Assert.AreEqual("test string property with spaces", value);
		}
	}
}
