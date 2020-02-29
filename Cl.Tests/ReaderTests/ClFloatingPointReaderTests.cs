using Cl.Input;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClFloatingPointReaderTests
    {
        [Test]
        public void ReadFloatingPoint_SkipOnlyPartOfSource()
        {
            var source = new FilteredSource("120.0rest");
            using var reader = new Reader(source);

            Ignore(reader.ReadFloatingPoint(out var _));

            Assert.That(source.ToString(), Is.EqualTo("rest"));
        }

        [Test]
        public void ReadFloatingPoint_CanNotBeAbleReadNegativeNum()
        {
            using var reader = new Reader(new FilteredSource("-120.0"));

            Assert.That(reader.ReadFloatingPoint(out var _), Is.False);
        }

        [Test]
        public void ReadFloatingPoint_ReturnNumber()
        {
            using var reader = new Reader(new FilteredSource("0.45rest"));

            Assert.That(reader.ReadFloatingPoint(out var atom), Is.True);
            Assert.That(atom.Value, Is.EqualTo(0.45).Within(double.Epsilon));
        }

        [Test]
        public void ReadFloatingPoint_ThrowException_WhenAfterDotInvaidSymbol()
        {
            using var reader = new Reader(new FilteredSource("0. "));

            Assert.That(() => reader.ReadFloatingPoint(out var _), Throws.InvalidOperationException);
        }

        [Test]
        public void ReadFloatingPoint_ReturnFalse_WhenSourceContainsInteger()
        {
            using var reader = new Reader(new FilteredSource("23"));

            Assert.That(reader.ReadFloatingPoint(out var _), Is.False);
        }
    }
}
