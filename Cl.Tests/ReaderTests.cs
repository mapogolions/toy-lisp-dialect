using NUnit.Framework;

namespace Cl.Tests
{
    [TestFixture]
    public class ReaderTests
    {
        [Test]
        public void SkipEol_IgnoreWinEol()
        {
            var source = new Source("\r\n");
            using var reader = new Reader(source);

            reader.SkipEol();

            Assert.That(source.Eof(), Is.True);
        }

        [Test]
        public void SkipEol_IgnoreOsxEol()
        {
            var source = new Source("\n\r");
            using var reader = new Reader(source);

            reader.SkipEol();

            Assert.That(source.Eof(), Is.True);
        }

        [Test]
        public void SkipEol_IgnoreUnixEol()
        {
            var source = new Source("\n");
            using var reader = new Reader(source);

            reader.SkipEol();

            Assert.That(source.Eof(), Is.True);
        }

        [Test]
        public void Eof_ReturnFalse_WhenSourceContainAtLeastOneItem()
        {
            var source = new Source("foo");
            using var reader = new Reader(source);

            reader.SkipEol();

            Assert.That(source.Eof(), Is.False);
        }
    }
}
