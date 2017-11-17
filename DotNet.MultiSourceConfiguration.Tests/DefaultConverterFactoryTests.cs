using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using DotNet.MultiSourceConfiguration.Implementation;

namespace DotNet.MultiSourceConfiguration.Tests
{
    [TestFixture]
    public class DefaultConverterFactoryTests
    {
        [Test]
        public void IntValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(123, converters[typeof(int)].FromString("123"));
        }

        [Test]
        public void NullableIntValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(123, converters[typeof(int?)].FromString("123"));
        }

        [Test]
        public void IntArrayValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new int[]{ 123,124,125 }, converters[typeof(int[])].FromString("123,124,125"));
        }

        [Test]
        public void IntListValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new int[] { 123, 124, 125 }.ToList(), converters[typeof(List<int>)].FromString("123,124,125"));
        }

        [Test]
        public void LongValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(123, converters[typeof(long)].FromString("123"));
        }

        [Test]
        public void NullableLongValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(123, converters[typeof(long?)].FromString("123"));
        }

        [Test]
        public void LongArrayValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new long[] { 123, 124, 125 }, converters[typeof(long[])].FromString("123,124,125"));
        }

        [Test]
        public void LongListValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new long[] { 123, 124, 125 }.ToList(), converters[typeof(List<long>)].FromString("123,124,125"));
        }

        [Test]
        public void DecimalValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(123.456M, converters[typeof(decimal)].FromString("123.456"));
        }

        [Test]
        public void NullableDecimalValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(123.456M, converters[typeof(decimal?)].FromString("123.456"));
        }

        [Test]
        public void DecimalArrayValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new decimal[] { 123.456M, 124.456M, 125.456M }, converters[typeof(decimal[])].FromString("123.456,124.456,125.456"));
        }

        [Test]
        public void DecimalListValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new decimal[] { 123.456M, 124.456M, 125.456M }.ToList(), converters[typeof(List<decimal>)].FromString("123.456,124.456,125.456"));
        }

        [Test]
        public void DoubleValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(123.456, converters[typeof(double)].FromString("123.456"));
        }

        [Test]
        public void NullableDoubleValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(123.456, converters[typeof(double?)].FromString("123.456"));
        }

        [Test]
        public void DoubleArrayValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new double[] { 123.456, 124.456, 125.456 }, converters[typeof(double[])].FromString("123.456,124.456,125.456"));
        }

        [Test]
        public void DoubleListValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new double[] { 123.456, 124.456, 125.456 }.ToList(), converters[typeof(List<double>)].FromString("123.456,124.456,125.456"));
        }

        [Test]
        public void FloatValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(123.456f, converters[typeof(float)].FromString("123.456"));
        }

        [Test]
        public void NullableFloatValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(123.456f, converters[typeof(float?)].FromString("123.456"));
        }

        [Test]
        public void FloatArrayValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new float[] { 123.456f, 124.456f, 125.456f }, converters[typeof(float[])].FromString("123.456,124.456,125.456"));
        }

        [Test]
        public void FloatListValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new float[] { 123.456f, 124.456f, 125.456f }.ToList(), converters[typeof(List<float>)].FromString("123.456,124.456,125.456"));
        }

        [Test]
        public void BoolValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(true, converters[typeof(bool)].FromString("true"));
        }

        [Test]
        public void NullableBoolValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(true, converters[typeof(bool?)].FromString("true"));
        }

        [Test]
        public void BoolArrayValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new bool[] { true, false, true, false }, converters[typeof(bool[])].FromString("true,false,true,false"));
        }

        [Test]
        public void BoolListValuesAreCorrectlyConverted()
        {
            var converters = DefaultConverterFactory.GetDefaultConverters();
            Assert.AreEqual(new bool[] { true, false, true, false }.ToList(), converters[typeof(List<bool>)].FromString("true,false,true,false"));
        }
    }
}
