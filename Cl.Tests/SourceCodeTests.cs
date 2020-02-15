using NUnit.Framework;

namespace Cl.Tests
{
    [TestFixture]
    public class SourceCodeTests
    {
        [Test]
        public void Read_Return_MinusOne_WhenSourceCodeIsEmpty()
        {
            using var source = new SourceCode(string.Empty);

            Assert.That(source.Read(), Is.EqualTo(-1));
        }

        [Test]
        public void Read_ReturnAllCharsStepByStep()
        {
            using var source = new SourceCode("foo");

            Assert.That("foo", Is.EqualTo(source.ToString()));
        }

        [Test]
        public void Read_ReturnFirstCharFromString()
        {
            using var source = new SourceCode("foo");

            Assert.That((char) source.Read(), Is.EqualTo('f'));
        }
    }
}
