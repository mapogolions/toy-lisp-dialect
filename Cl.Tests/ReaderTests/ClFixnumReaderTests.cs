using Cl.Sources;
using NUnit.Framework;
using static Cl.Helpers.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClFixnumReaderTests
    {
        [Test]
        public void ReadFixnum_SkipOnlyPartOfSource()
        {
            var source = new Source("120rest");
            using var reader = new Reader(source);

            Ignore(reader.ReadFixnum());

            Assert.That(source.ToString(), Is.EqualTo("rest"));
        }

        [Test]
        public void ReadFixnum_CanNotBeAbleReadNegativeNum()
        {
            using var reader = new Reader("-120...");
            Assert.That(reader.ReadFixnum(), Is.Null);
        }

        [Test]
        public void ReadFixnum_ReturnInteger_WhenDotAppears()
        {
            using var reader = new Reader("1.");
            Assert.That(reader.ReadFixnum()?.Value, Is.EqualTo(1));
        }

        [Test]
        public void ReadFixnum_ReturnPositiveNum()
        {
            using var reader = new Reader("12");
            Assert.That(reader.ReadFixnum()?.Value, Is.EqualTo(12));
        }

        [Test]
        public void ReadFixnum_ReturnNull_WhenSourceDoesNotStartWithDigit()
        {
            using var reader = new Reader(" 12");
            Assert.That(reader.ReadFixnum()?.Value, Is.Null);
        }
    }
}
