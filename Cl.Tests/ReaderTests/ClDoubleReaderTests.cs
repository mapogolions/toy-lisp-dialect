using Cl.Readers;
using Cl.Errors;
using Cl.IO;
using Cl.Types;
using NUnit.Framework;
using static Cl.Helpers.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClDoubleReaderTests
    {
        private readonly ClDoubleReader _reader = new();

        [Test]
        public void ReadDouble_SkipOnlyPartOfSource()
        {
            using var source = new Source("120.0rest");
            Ignore(_reader.Read(source));
            Assert.That(source.ToString(), Is.EqualTo("rest"));
        }

        [Test]
        public void ReadDouble_CanNotBeAbleReadNegativeNum()
        {
            using var source = new Source("-120.0");
            Assert.That(_reader.Read(source), Is.Null);
        }

        [Test]
        public void ReadDouble_ReturnFloatingPointNumber()
        {
            using var source = new Source("0.45rest");
            Assert.That(_reader.Read(source)?.Value, Is.EqualTo(0.45).Within(double.Epsilon));
        }

        [Test]
        public void ReadDouble_ThrowException_WhenAfterDotInvalidSymbol()
        {
            using var source = new Source("0. ");
            var errorMessage = $"Invalid format of the {nameof(ClDouble)} literal";

            Assert.That(() => _reader.Read(source),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadDouble_ReturnNull_WhenSourceContainsInteger()
        {
            using var source = new Source("23");
            Assert.That(_reader.Read(source), Is.Null);
        }

         [Test]
        public void ReadDouble_ReturnNull_WhenSourceDoesNotStartWithDigit()
        {
            using var source = new Source(" 1.12");
            Assert.That(_reader.Read(source)?.Value, Is.Null);
        }
    }
}
