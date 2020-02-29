using Cl.Input;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClFixnumReaderTests
    {
        [Test]
        public void ReadFixnum_SkipOnlyPartOfSource()
        {
            var source = new FilteredSource("120rest");
            using var reader = new Reader(source);

            Ignore(reader.ReadFixnum(out var _));

            Assert.That(source.ToString(), Is.EqualTo("rest"));
        }

        [Test]
        public void ReadFixnum_CanNotBeAbleReadNegativeNum()
        {
            using var reader = new Reader(new FilteredSource("-120..."));

            Assert.That(reader.ReadFixnum(out var _), Is.False);
        }

        [Test]
        public void ReadFixnum_ReturnInteger_WhenDotAppears()
        {
            using var reader = new Reader(new FilteredSource("1."));

            Assert.That(reader.ReadFixnum(out var atom), Is.True);
            Assert.That(atom.Value, Is.EqualTo(1));
        }

        [Test]
        public void ReadFixnum_ReturnPositiveNum()
        {
            using var reader = new Reader(new FilteredSource("12"));

            Assert.That(reader.ReadFixnum(out var atom), Is.True);
            Assert.That(atom.Value, Is.EqualTo(12));
        }
    }
}
