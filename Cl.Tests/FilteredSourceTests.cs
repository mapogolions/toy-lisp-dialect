using System.Collections.Generic;
using Cl.SourceCode;
using NUnit.Framework;

namespace Cl.Tests
{
    [TestFixture]
    public class FilteredSourceTests
    {
        [Test]
        public void SkipWhiteSpaces_SkipOnlyWhitespaces()
        {
            using var source = new FilteredSource("  \tsome ");

            Assert.That(source.SkipWhitespaces(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo("some "));
        }

        [Test]
        public void SkipWhitespaces_SkipAllSymbols_WhenSourceContainsOnlyWhitspaces()
        {
            using var source = new FilteredSource("\t\n \f");

            Assert.That(source.SkipWhitespaces(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void SkipWhitespaces_ReturnFalse_WhenSourceStartsWithNonSpace()
        {
            using var source = new FilteredSource("b r");

            Assert.That(source.SkipWhitespaces(), Is.False);
            Assert.That(source.ToString(), Is.EqualTo("b r"));
        }

        [Test]
        public void SkipWhitespaces_ReturnFalse_WhenSourceIsEmpty()
        {
            using var source = new FilteredSource(string.Empty);

            Assert.That(source.SkipWhitespaces(), Is.False);
        }

        [Test]
        public void SkipLine_SkipAllSymbols_WhenSourceDoesNotContainEol()
        {
            using var source = new FilteredSource("foo");

            Assert.That(source.SkipLine(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void SkipLine_RegognizeCarriageReturn()
        {
            using var source = new FilteredSource("fi\rst\nsecond\r\nthird\n\r");

            Assert.That(source.SkipLine(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo("second\r\nthird\n\r"));
        }

        [TestCaseSource(nameof(EndOfLines))]
        public void SkipLine_SkipAllSymbols_UntilEolAppears(string eol)
        {
            using var source = new FilteredSource($"foo{eol}bar");

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
            using var source = new FilteredSource("foo\r");

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
