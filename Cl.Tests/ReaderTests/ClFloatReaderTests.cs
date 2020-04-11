using Cl.Input;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClFloatReaderTests
    {
        [Test]
        public void ReadFloat_SkipOnlyPartOfSource()
        {
            var source = new FilteredSource("120.0rest");
            using var reader = new Reader(source);

            Ignore(reader.ReadFloat());

            Assert.That(source.ToString(), Is.EqualTo("rest"));
        }

        [Test]
        public void ReadFloat_CanNotBeAbleReadNegativeNum()
        {
            using var reader = new Reader("-120.0");

            Assert.That(reader.ReadFloat(), Is.Null);
        }

        [Test]
        public void ReadFloat_ReturnFloatingPointNumber()
        {
            using var reader = new Reader("0.45rest");

            Assert.That(reader.ReadFloat()?.Value, Is.EqualTo(0.45).Within(double.Epsilon));
        }

        [Test]
        public void ReadFloat_ThrowException_WhenAfterDotInvalidSymbol()
        {
            using var reader = new Reader("0. ");
            var errorMessage = Errors.Reader.UnknownLiteral(nameof(ClFloat));

            Assert.That(() => reader.ReadFloat(),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadFloat_ReturnNull_WhenSourceContainsInteger()
        {
            using var reader = new Reader("23");

            Assert.That(reader.ReadFloat(), Is.Null);
        }

         [Test]
        public void ReadFloat_ReturnNull_WhenSourceDoesNotStartWithDigit()
        {
            using var reader = new Reader(" 1.12");

            Assert.That(reader.ReadFloat()?.Value, Is.Null);
        }
    }
}
