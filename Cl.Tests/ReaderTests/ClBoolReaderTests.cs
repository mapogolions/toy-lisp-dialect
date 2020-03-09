using Cl.Constants;
using Cl.Input;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClBoolReaderTests
    {
        [Test]
        public void ReadBool_SkipOnlyPartOfSource()
        {
            var source = new FilteredSource("#ttf");
            using var reader = new Reader(source);

            Ignore(reader.ReadBool());

            Assert.That(source.ToString(), Is.EqualTo("tf"));
        }

        [Test]
        public void ReadBool_ReturnTheFalse()
        {
            using var reader = new Reader("#fi");

            Assert.That(reader.ReadBool()?.Value, Is.False);
        }

        [Test]
        public void ReadBool_ReturnTheTrue()
        {
            using var reader = new Reader("#ti");

            Assert.That(reader.ReadBool()?.Value, Is.True);
        }

        [Test]
        public void ReadBool_ThrowException_WhenBooleanSignificantSymbolDoesNotFollowAfterHash()
        {
            using var reader = new Reader("#w");

            Assert.That(() => reader.ReadBool(),
                Throws.InvalidOperationException.And.Message.EqualTo(Errors.UnknownLiteral(nameof(ClBool))));
        }

        [Test]
        public void ReadBool_ThrowException_WhenSourceContainsOnlyHash()
        {
            using var reader = new Reader("#");

            Assert.That(() => reader.ReadBool(),
                Throws.InvalidOperationException.And.Message.EqualTo(Errors.UnknownLiteral(nameof(ClBool))));
        }

        [Test]
        public void ReadBool_ReturnFalse_WhenSourceDoesNotStartWithHash()
        {
            using var reader = new Reader(" #f");

            Assert.That(reader.ReadBool()?.Value, Is.Null);
        }
    }
}
