using Cl.Extensions;
using Cl.IO;

namespace Cl.Tests.SourceTests
{
    [TestFixture]
    public class RewindableSourceTests
    {
        [Test]
        public void RewindMatched_ReturnOriginalSource_AfterPartialMatch()
        {
            using var source = new Source("partial");

            Assert.That(source.Skip("parts"), Is.False);
            Assert.That(source.ToString(), Is.EqualTo("partial"));
        }

        [Test]
        public void RewindMatched_ThrowException_WhenSourceDoesNotMatchPattern()
        {
            using var source = new Source("foo");

            Assert.That(source.Skip("bar"), Is.False);
            Assert.That(source.ToString(), Is.EqualTo("foo"));
        }

        [Test]
        public void RewindMatched_ReturnTrueWhenSourceStartsWithPattern()
        {
            using var source = new Source("foobar");

            Assert.That(source.Skip("foo"), Is.True);
            Assert.That(source.ToString(), Is.EqualTo("bar"));
        }

        [Test]
        public void RewindMatched_ReturnTrueWhenSourceEqualToPattern()
        {
            using var source = new Source("foo");

            Assert.That(source.Skip("foo"), Is.True);
            Assert.That(source.ToString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void RewindMatched_ThrowExceptionWhenSourceIsEmpty()
        {
            using var source = new Source(string.Empty);

            Assert.That(source.Skip("foo"), Is.False);
        }

        [Test]
        public void RewindMatched_ReturnFalseWhenSourceAndPatternAreEmpty()
        {
            using var source = new Source(string.Empty);

            Assert.That(source.Skip(string.Empty), Is.True);
        }

        [Test]
        public void RewindWhiteSpaces_RewindOnlyWhitespaces()
        {
            using var source = new Source("  \tsome ");

            Assert.That(source.SkipSpaces(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo("some "));
        }

        [Test]
        public void RewindWhitespaces_RewindAllSymbols_WhenSourceContainsOnlyWhitspaces()
        {
            using var source = new Source("\t\n \f");

            Assert.That(source.SkipSpaces(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void RewindWhitespaces_ReturnFalse_WhenSourceStartsWithNonSpace()
        {
            using var source = new Source("b r");

            Assert.That(source.SkipSpaces(), Is.False);
            Assert.That(source.ToString(), Is.EqualTo("b r"));
        }

        [Test]
        public void RewindWhitespaces_ReturnFalse_WhenSourceIsEmpty()
        {
            using var source = new Source(string.Empty);

            Assert.That(source.SkipSpaces(), Is.False);
        }

        [Test]
        public void RewindLine_RewindAllSymbols_WhenSourceDoesNotContainEol()
        {
            using var source = new Source("foo");

            Assert.That(source.SkipLine(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void RewindLine_RegognizeCarriageReturn()
        {
            using var source = new Source($"fi\rst\nsecond");

            Assert.That(source.SkipLine(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo("second"));
        }

        [Test]

        public void RewindLine_RewindAllSymbols_UntilEolAppears()
        {
            using var source = new Source($"foo\nbar");

            Assert.That(source.SkipLine(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo("bar"));
        }

        [Test]
        public void RewindLine_ReturnFalse_WhenSourceIsEmpty()
        {
            using var source = new Source(string.Empty);

            Assert.That(source.SkipLine(), Is.False);
        }

        [Test]
        public void RewindEol_ReturnFalse_WhenSourceContainOnlyCarriageReturn()
        {
            using var source = new Source("foo\r");

            Assert.That(source.SkipEol(), Is.False);
            Assert.That(source.ToString(), Is.EqualTo("foo\r"));
        }

        [Test]
        public void RewindEol_ReturnFalse_WhenSourceIsEmpty()
        {
            using var source = new Source(string.Empty);

            Assert.That(source.SkipEol(), Is.False);
        }

        [Test]

        public void RewindEol_ReturnRestOfTheSource()
        {
            using var source = new Source($"\nfoo");

            Assert.That(source.SkipEol(), Is.True);
            Assert.That(source.ToString(), Is.EqualTo("foo"));
        }

        [Test]

        public void RewindEol_IsCrossPlatform()
        {
            using var source = new Source("\n");

            Assert.That(source.SkipEol(), Is.True);
            Assert.That(source.Eof(), Is.True);
        }

        [Test]

        public void RewindEol_ReturnFalse_WhenSourceDoesNotStartWithEol()
        {
            using var source = new Source($"foo\n");

            Assert.That(source.SkipEol(), Is.False);
            Assert.That(source.ToString(), Is.EqualTo($"foo\n"));
        }
    }
}
