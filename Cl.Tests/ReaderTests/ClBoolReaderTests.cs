using Cl.Input;
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

            Ignore(reader.ReadBool(out var _));

            Assert.That(source.ToString(), Is.EqualTo("tf"));
        }

        [Test]
        public void ReadBool_ReturnTheFalse()
        {
            using var reader = new Reader(new FilteredSource("#fi"));

            Assert.That(reader.ReadBool(out var atom), Is.True);
            Assert.That(atom.Value, Is.False);
        }

        [Test]
        public void ReadBool_ReturnTheTrue()
        {
            using var reader = new Reader(new FilteredSource("#ti"));

            Assert.That(reader.ReadBool(out var atom), Is.True);
            Assert.That(atom.Value, Is.True);
        }

        [Test]
        public void ReadBool_ThrowException_WhenSourceIsEqualToHash()
        {
            using var reader = new Reader(new FilteredSource("#"));

            Assert.That(() => reader.ReadBool(out var _), Throws.InvalidOperationException);
        }

        [Test]
        public void ReadBool_ThrowException_WhenSourceStartWithHashButNextSymbolIsNotBoolPredefinedValue()
        {
            using var reader = new Reader(new FilteredSource("#i"));

            Assert.That(() => reader.ReadBool(out var _), Throws.InvalidOperationException);
        }

        [Test]
        public void ReadBool_ReturnFalse_WhenSourceDoesNotStartWithHash()
        {
            using var reader = new Reader(new FilteredSource("t"));

            Assert.That(reader.ReadBool(out var atom), Is.False);
            Assert.That(atom, Is.Null);
        }
    }
}
