using Cl.Constants;
using Cl.Input;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClCharReaderTests
    {
        [Test]
        public void ReadChar_SkipOnlyPartOfSource()
        {
            var source = new FilteredSource("#\\foo");
            using var reader = new Reader(source);

            Ignore(reader.ReadChar());

            Assert.That(source.ToString(), Is.EqualTo("oo"));
        }

        [Test]
        public void ReadChar_Return_n_Character()
        {
            using var reader = new Reader(new FilteredSource("#\\new"));

            Assert.That(reader.ReadChar()?.Value, Is.EqualTo('n'));
        }

        [Test]
        public void ReadChar_Return_Space()
        {
            using var reader = new Reader(new FilteredSource("#\\space"));

            Assert.That(reader.ReadChar()?.Value, Is.EqualTo(' '));
        }

        [Test]
        public void ReadChar_Return_Tab()
        {
            using var reader = new Reader(new FilteredSource("#\\tab"));

            Assert.That(reader.ReadChar()?.Value, Is.EqualTo('\t'));
        }

        [Test]
        public void ReadChar_Return_Newline()
        {
            using var reader = new Reader(new FilteredSource("#\\newline"));

            Assert.That(reader.ReadChar()?.Value, Is.EqualTo('\n'));
        }

        [Test]
        public void ReadChar_ThrowException_WhenSourceIsEqualToHashAndBackslash()
        {
            using var reader = new Reader(new FilteredSource("#\\"));

            Assert.That(() => reader.ReadChar(),
                Throws.InvalidOperationException.And.Message.EqualTo(Errors.UnknownLiteral(nameof(ClChar))));
        }

        [Test]
        public void ReadChar_ReturnNull_WhenSourceStartsWithHashButNextSymbolIsNotBackslash()
        {
            using var reader = new Reader(new FilteredSource("#i"));

            Assert.That(reader.ReadChar(), Is.Null);
        }

        [Test]
        public void ReadChar_ReturnFalse_WhenSourceDoesNotStartWithHash()
        {
            using var reader = new Reader(new FilteredSource("t"));

            Assert.That(reader.ReadChar(), Is.Null);
        }
    }
}
