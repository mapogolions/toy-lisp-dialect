using Cl.Errors;
using Cl.IO;
using Cl.Types;
using NUnit.Framework;
using static Cl.Helpers.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClBoolReaderTests
    {
        [Test]
        public void ReadBool_SkipOnlyPartOfSource()
        {
            var source = new Source("#ttf");
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
            var errorMessage = $"Invalid format of the {nameof(ClBool)} literal";

            Assert.That(() => reader.ReadBool(),
                Throws.Exception.TypeOf<SyntaxError>().And.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadBool_ThrowException_WhenSourceContainsOnlyHash()
        {
            using var reader = new Reader("#");
            var errorMessage = $"Invalid format of the {nameof(ClBool)} literal";

            Assert.That(() => reader.ReadBool(),
                Throws.Exception.TypeOf<SyntaxError>().And.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadBool_ReturnFalse_WhenSourceDoesNotStartWithHash()
        {
            using var reader = new Reader(" #f");
            Assert.That(reader.ReadBool()?.Value, Is.Null);
        }
    }
}
