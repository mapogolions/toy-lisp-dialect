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

            Ignore(reader.ReadString(out var _));

            Assert.That(source.ToString(), Is.EqualTo("bar"));
        }

        [Test]
        public void ReadString_ReturnString()
        {
            using var reader = new Reader(new FilteredSource("\"foo\""));

            Assert.That(reader.ReadString(out var atom), Is.True);
            Assert.That(atom.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void ReadString_ThrowException_WhenSourceDoesNotContainPairDoubleQuotes()
        {
            using var reader = new Reader(new FilteredSource("\"some"));

            Assert.That(() => reader.ReadString(out var _),
                Throws.InvalidOperationException.And.Message.EqualTo(Errors.UnknownLiteral(nameof(ClString))));
        }

        [Test]
        public void ReadString_ReturnFalse_WhenSourceDoesNotStartWithDoubleQuotes()
        {
            using var reader = new Reader(new FilteredSource("_"));

            Assert.That(reader.ReadString(out var atom), Is.False);
            Assert.That(atom, Is.Null);
        }
    }
}
