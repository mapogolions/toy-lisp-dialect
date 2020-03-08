using Cl.Constants;
using Cl.Input;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClStringReaderTests
    {
        [Test]
        public void ReadString_SkipOnlyPartOfSource()
        {
            var source = new FilteredSource("\"foo\"bar");
            using var reader = new Reader(source);

            Ignore(reader.ReadString());

            Assert.That(source.ToString(), Is.EqualTo("bar"));
        }

        [Test]
        public void ReadString_ReturnString()
        {
            using var reader = new Reader("\"foo\"");

            Assert.That(reader.ReadString()?.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void ReadString_ThrowException_WhenSourceDoesNotContainPairDoubleQuotes()
        {
            using var reader = new Reader("\"some");

            Assert.That(() => reader.ReadString(),
                Throws.InvalidOperationException.And.Message.EqualTo(Errors.UnknownLiteral(nameof(ClString))));
        }

        [Test]
        public void ReadString_ReturnFalse_WhenSourceDoesNotStartWithDoubleQuotes()
        {
            using var reader = new Reader("_");

            Assert.That(reader.ReadString(), Is.Null);
        }
    }
}
