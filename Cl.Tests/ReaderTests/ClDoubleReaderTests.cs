using Cl.Errors;
using Cl.Input;
using Cl.Types;
using NUnit.Framework;
using static Cl.Helpers.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClDoubleReaderTests
    {
        [Test]
        public void ReadDouble_SkipOnlyPartOfSource()
        {
            var source = new FilteredSource("120.0rest");
            using var reader = new Reader(source);

            Ignore(reader.ReadDouble());

            Assert.That(source.ToString(), Is.EqualTo("rest"));
        }

        [Test]
        public void ReadDouble_CanNotBeAbleReadNegativeNum()
        {
            using var reader = new Reader("-120.0");
            Assert.That(reader.ReadDouble(), Is.Null);
        }

        [Test]
        public void ReadDouble_ReturnFloatingPointNumber()
        {
            using var reader = new Reader("0.45rest");
            Assert.That(reader.ReadDouble()?.Value, Is.EqualTo(0.45).Within(double.Epsilon));
        }

        [Test]
        public void ReadDouble_ThrowException_WhenAfterDotInvalidSymbol()
        {
            using var reader = new Reader("0. ");
            var errorMessage = $"Invalid format of the {nameof(ClDouble)} literal";

            Assert.That(() => reader.ReadDouble(),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadDouble_ReturnNull_WhenSourceContainsInteger()
        {
            using var reader = new Reader("23");
            Assert.That(reader.ReadDouble(), Is.Null);
        }

         [Test]
        public void ReadDouble_ReturnNull_WhenSourceDoesNotStartWithDigit()
        {
            using var reader = new Reader(" 1.12");
            Assert.That(reader.ReadDouble()?.Value, Is.Null);
        }
    }
}
