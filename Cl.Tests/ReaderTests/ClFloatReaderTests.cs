using Cl.Constants;
using Cl.Input;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClFloatReaderTests
    {
        [Test]
        public void ReadFloat_SkipOnlyPartOfSource()
        {
            var source = new FilteredSource("120.0rest");
            using var reader = new Reader(source);

            Ignore(reader.ReadFloat(out var _));

            Assert.That(source.ToString(), Is.EqualTo("rest"));
        }

        [Test]
        public void ReadFloat_CanNotBeAbleReadNegativeNum()
        {
            using var reader = new Reader(new FilteredSource("-120.0"));

            Assert.That(reader.ReadFloat(out var _), Is.False);
        }

        [Test]
        public void ReadFloat_ReturnNumber()
        {
            using var reader = new Reader(new FilteredSource("0.45rest"));

            Assert.That(reader.ReadFloat(out var atom), Is.True);
            Assert.That(atom.Value, Is.EqualTo(0.45).Within(double.Epsilon));
        }

        [Test]
        public void ReadFloat_ThrowException_WhenAfterDotInvalidSymbol()
        {
            using var reader = new Reader(new FilteredSource("0. "));

            Assert.That(() => reader.ReadFloat(out var _),
                Throws.InvalidOperationException.And.Message.EqualTo(Errors.UnknownLiteral(nameof(ClFloat))));
        }

        [Test]
        public void ReadFloat_ReturnFalse_WhenSourceContainsInteger()
        {
            using var reader = new Reader(new FilteredSource("23"));

            Assert.That(reader.ReadFloat(out var _), Is.False);
        }
    }
}
