using NUnit.Framework;
using Cl.Input;

namespace Cl.Tests
{
    [TestFixture]
    public class SourceTests
    {
        [Test]
        public void Peek_Return_MinusOne_WhenSourceIsEmpty()
        {
            using var source = new Source(string.Empty);
            Assert.That(source.Peek(), Is.EqualTo(-1));
        }

        [Test]
        public void Peek_FullInnerBuffer()
        {
            using var source = new Source("ab");
            source.Peek();
            source.Peek();

            Assert.That((char) source.Read(), Is.EqualTo('a'));
            Assert.That((char) source.Read(), Is.EqualTo('b'));
        }

        [Test]
        public void Peek_ReturnValueFromStreamAndSaveValueToInnerBuffer()
        {
            using var source = new Source("foo");

            Assert.That((char) source.Peek(), Is.EqualTo('f'));
            Assert.That((char) source.Read(), Is.EqualTo('f'));
        }

        [Test]
        public void Read_Return_MinusOne_WhenSourceCodeIsEmpty()
        {
            using var source = new Source(string.Empty);

            Assert.That(source.Read(), Is.EqualTo(-1));
        }

        [Test]
        public void Read_ReturnAllCharsStepByStep()
        {
            using var source = new Source("foo");

            Assert.That("foo", Is.EqualTo(source.ToString()));
        }

        [Test]
        public void Read_ReturnFirstCharFromString()
        {
            using var source = new Source("foo");

            Assert.That((char) source.Read(), Is.EqualTo('f'));
        }
    }
}
