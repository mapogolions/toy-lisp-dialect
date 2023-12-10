using Cl.Readers;
using Cl.IO;
using static Cl.Helpers.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClFixnumReaderTests
    {
        private readonly ClIntReader _reader = new();

        [Test]
        public void ReadFixnum_SkipOnlyPartOfSource()
        {
            using var source = new Source("120rest");
            Ignore(_reader.Read(source));
            Assert.That(source.ToString(), Is.EqualTo("rest"));
        }

        [Test]
        public void ReadFixnum_CanNotBeAbleReadNegativeNum()
        {
            using var source = new Source("-120...");
            Assert.That(_reader.Read(source), Is.Null);
        }

        [Test]
        public void ReadFixnum_ReturnInteger_WhenDotAppears()
        {
            using var source = new Source("1.");
            Assert.That(_reader.Read(source)?.Value, Is.EqualTo(1));
        }

        [Test]
        public void ReadFixnum_ReturnPositiveNum()
        {
            using var source = new Source("12");
            Assert.That(_reader.Read(source)?.Value, Is.EqualTo(12));
        }

        [Test]
        public void ReadFixnum_ReturnNull_WhenSourceDoesNotStartWithDigit()
        {
            using var source = new Source(" 12");
            Assert.That(_reader.Read(source)?.Value, Is.Null);
        }
    }
}
