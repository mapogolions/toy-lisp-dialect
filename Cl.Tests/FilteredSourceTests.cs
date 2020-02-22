using System.Collections.Generic;
using Cl.SourceCode;
using NUnit.Framework;

namespace Cl.Tests
{
    [TestFixture]
    public class FilteredSourceTests
    {
        [Test]
        public void SkipLine_SkipAllSymbolsUntilEolAppears()
        {
            var source = new FilteredSource("fi\rst\nsecond\r\nthird\n\r");

            Assert.That(source.SkipLine(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo("second\r\nthird\n\r"));
        }

        [TestCaseSource(nameof(EndOfLines))]
        public void SkipLine_SkipAllSymbols_WhenSourceDoesNotContainEol(string eol)
        {
            var source = new FilteredSource($"foo{eol}bar");

            Assert.That(source.SkipLine(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo("bar"));
        }

        [Test]
        public void SkipLine_ReturnFalse_WhenSourceIsEmpty()
        {
            using var source = new FilteredSource(string.Empty);

            Assert.That(source.SkipLine(), Is.False);
        }

        [Test]
        public void SkipEol_ReturnFalse_WhenSourceContainOnlyCarriageReturn()
        {
            var source = new FilteredSource("foo\r");

            Assert.That(source.SkipEol(), Is.False);
            Assert.That(source.ToString(), Is.EqualTo("foo\r"));
        }

        [Test]
        public void SkipEol_ReturnFalse_WhenSourceIsEmpty()
        {
            using var source = new FilteredSource(string.Empty);

            Assert.That(source.SkipEol(), Is.False);
        }

        [TestCaseSource(nameof(EndOfLines))]
        public void SkipEol_ReturnRestOfTheSource(string eol)
        {
            using var source = new FilteredSource($"{eol}foo");

            Assert.That(source.SkipEol(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo("foo"));
        }

        [TestCaseSource(nameof(EndOfLines))]
        public void SkipEol_IsCrossPlatform(string eol)
        {
            using var source = new FilteredSource(eol);

            Assert.That(source.SkipEol(), Is.True);
            Assert.That(source.Eof(), Is.True);
        }

        [TestCaseSource(nameof(EndOfLines))]
        public void SkipEol_ReturnFalse_WhenSourceDoesNotContainEol(string eol)
        {
            using var source = new FilteredSource($"foo{eol}");

            Assert.That(source.SkipEol(), Is.False);
            Assert.That(source.ToString(), Is.EqualTo($"foo{eol}"));
        }

        static IEnumerable<string> EndOfLines()
        {
            yield return "\r\n";
            yield return "\n\r";
            yield return "\n";
        }
    }
}
