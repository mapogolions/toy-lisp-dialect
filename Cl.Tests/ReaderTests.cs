using System.Collections.Generic;
using NUnit.Framework;

namespace Cl.Tests
{
    [TestFixture]
    public class ReaderTests
    {
        [Test]
        public void SkipEol_ReturnFalse_WhenSourceIsEmpty()
        {
            var source = new Source(string.Empty);
            using var reader = new Reader(source);

            Assert.That(reader.SkipEol(), Is.False);
        }

        [TestCaseSource(nameof(EndOfLineCases))]
        public void SkipEol_ReturnRestOfTheSource(string eol)
        {
            var source = new Source($"{eol}foo");
            using var reader = new Reader(source);

            Assert.That(reader.SkipEol(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo("foo"));
        }

        [TestCaseSource(nameof(EndOfLineCases))]
        public void SkipEol_IsCrossPlatform(string eol)
        {
            var source = new Source(eol);
            using var reader = new Reader(source);

            Assert.That(reader.SkipEol(), Is.True);
            Assert.That(source.Eof(), Is.True);
        }

        [TestCaseSource(nameof(EndOfLineCases))]
        public void SkipEol_ReturnFalse_WhenSourceDoesNotContainEol(string eol)
        {
            var source = new Source($"foo{eol}");
            using var reader = new Reader(source);

            Assert.That(reader.SkipEol(), Is.False);
            Assert.That(source.ToString(), Is.EqualTo($"foo{eol}"));
        }

        static IEnumerable<string> EndOfLineCases()
        {
            yield return "\r\n";
            yield return "\n\r";
            yield return "\n";
        }
    }
}
