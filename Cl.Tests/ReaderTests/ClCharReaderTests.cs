using Cl.Readers;
using Cl.Errors;
using Cl.IO;
using Cl.Types;
using static Cl.Helpers.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClCharReaderTests
    {
        private readonly ClCharReader _reader = new();

        [Test]
        public void ReadChar_SkipOnlyPartOfSource()
        {
            using var source = new Source("#\\foo");
            Ignore(_reader.Read(source));
            Assert.That(source.ToString(), Is.EqualTo("oo"));
        }

        [Test]
        public void ReadChar_Return_n_Character()
        {
            using var source = new Source("#\\new");
            Assert.That(_reader.Read(source)?.Value, Is.EqualTo('n'));
        }

        [Test]
        public void ReadChar_Return_Space()
        {
            using var source = new Source("#\\space");
            Assert.That(_reader.Read(source)?.Value, Is.EqualTo(' '));
        }

        [Test]
        public void ReadChar_Return_Tab()
        {
            using var source = new Source("#\\tab");
            Assert.That(_reader.Read(source)?.Value, Is.EqualTo('\t'));
        }

        [Test]
        public void ReadChar_Return_Newline()
        {
            using var source = new Source("#\\newline");
            Assert.That(_reader.Read(source)?.Value, Is.EqualTo('\n'));
        }

        [Test]
        public void ReadChar_ThrowException_WhenSourceIsEqualToHashAndBackslash()
        {
            using var source = new Source("#\\");
            var errorMessage = $"Invalid format of the {nameof(ClChar)} literal";

            Assert.That(() => _reader.Read(source),
                Throws.Exception.TypeOf<SyntaxError>().And.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadChar_ReturnNull_WhenBackslashIsMissed()
        {
            using var source = new Source("#i");
            Assert.That(_reader.Read(source), Is.Null);
        }

        [Test]
        public void ReadChar_ReturnNull_WhenSourceDoesNotStartWithHash()
        {
            using var source = new Source(" #\\d");
            Assert.That(_reader.Read(source), Is.Null);
        }
    }
}
