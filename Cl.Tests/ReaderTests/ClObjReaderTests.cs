using Cl.Types;
using Cl.Errors;
using Cl.Readers;
using Cl.IO;

namespace Cl.Tests
{
    [TestFixture]
    public class ClObjReaderTests
    {
        private readonly ClObjReader _reader = new();

        [Test]
        public void Read_ReturnInteger()
        {
            var input = $"112;after";
            using var source = new Source(input);

            var atom = _reader.Read(source) as ClInt;

            Assert.That(atom?.Value, Is.EqualTo(112));
        }

        [Test]
        public void Read_ReturnChar()
        {
            using var source = new Source("#\\N");
            var atom = _reader.Read(source) as ClChar;
            Assert.That(atom?.Value, Is.EqualTo('N'));
        }

        [Test]
        public void Read_ReturnString()
        {
            using var source = new Source("'foo'");
            var atom = _reader.Read(source) as ClString;
            Assert.That(atom?.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void Read_ReturnBool()
        {
            using var source = new Source("#t");
            var atom = _reader.Read(source) as ClBool;
            Assert.That(atom?.Value, Is.EqualTo(true));
        }

        [Test]
        public void Read_ThrowException_WhenAfterSignificandAndDotInvalidSymbol()
        {
            using var source = new Source("11.");
            var errorMessage = $"Invalid format of the {nameof(ClDouble)} literal";

            Assert.That(() => _reader.Read(source),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void Read_ReturnNil_WhenSourceIsEmpty()
        {
            using var source = new Source(string.Empty);
            Assert.AreSame(ClCell.Nil, _reader.Read(source));
        }
    }
}
