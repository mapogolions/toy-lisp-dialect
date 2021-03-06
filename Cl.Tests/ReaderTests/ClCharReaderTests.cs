using Cl.Errors;
using Cl.DataSources;
using Cl.Types;
using NUnit.Framework;
using static Cl.Helpers.FpUniverse;

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
            using var reader = new Reader("#\\new");
            Assert.That(reader.ReadChar()?.Value, Is.EqualTo('n'));
        }

        [Test]
        public void ReadChar_Return_Space()
        {
            using var reader = new Reader("#\\space");
            Assert.That(reader.ReadChar()?.Value, Is.EqualTo(' '));
        }

        [Test]
        public void ReadChar_Return_Tab()
        {
            using var reader = new Reader("#\\tab");
            Assert.That(reader.ReadChar()?.Value, Is.EqualTo('\t'));
        }

        [Test]
        public void ReadChar_Return_Newline()
        {
            using var reader = new Reader("#\\newline");
            Assert.That(reader.ReadChar()?.Value, Is.EqualTo('\n'));
        }

        [Test]
        public void ReadChar_ThrowException_WhenSourceIsEqualToHashAndBackslash()
        {
            using var reader = new Reader("#\\");
            var errorMessage = $"Invalid format of the {nameof(ClChar)} literal";

            Assert.That(() => reader.ReadChar(),
                Throws.Exception.TypeOf<SyntaxError>().And.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadChar_ReturnNull_WhenBackslashIsMissed()
        {
            using var reader = new Reader("#i");
            Assert.That(reader.ReadChar(), Is.Null);
        }

        [Test]
        public void ReadChar_ReturnNull_WhenSourceDoesNotStartWithHash()
        {
            using var reader = new Reader(" #\\d");
            Assert.That(reader.ReadChar(), Is.Null);
        }
    }
}
