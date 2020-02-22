using System.Collections.Generic;
using Cl.Input;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests
{
    [TestFixture]
    public class ReaderTests
    {
        [Test]
        public void TryReadBool_SkipOnlyPartOfSource()
        {
            var source = new FilteredSource("#ttf");
            using var reader = new Reader(source);

            reader.TryReadBool(out var _);

            Assert.That(source.ToString(), Is.EqualTo("tf"));
        }

        [Test]
        public void TryReadBool_ReturnTheFalse()
        {
            using var reader = new Reader(new FilteredSource("#fi"));

            Assert.That(reader.TryReadBool(out var atom), Is.True);
            Assert.That(atom, Is.InstanceOf(typeof(ClBool)));
            Assert.That(atom.Value, Is.False);
        }

        [Test]
        public void TryReadBool_ReturnTheTrue()
        {
            using var reader = new Reader(new FilteredSource("#ti"));

            Assert.That(reader.TryReadBool(out var atom), Is.True);
            Assert.That(atom, Is.InstanceOf(typeof(ClBool)));
            Assert.That(atom.Value, Is.True);
        }

        [Test]
        public void TryReadBool_ThrowException_WhenSourceStartWithHashButNextSymbolIsNotBoolPredefinedValue()
        {
            using var reader = new Reader(new FilteredSource("#i"));

            Assert.That(() => reader.TryReadBool(out var _), Throws.InvalidOperationException);
        }

        [Test]
        public void TryReadBool_ReturnFalse_WhenSourceDoesNotStartWithHash()
        {
            using var reader = new Reader(new FilteredSource("t"));

            Assert.That(reader.TryReadBool(out var atom), Is.False);
            Assert.That(atom, Is.Null);
        }

        [TestCaseSource(nameof(CommentTestCases))]
        public void Read_ThrowException_WhenSourceContainsOnlyCommentLine(string source)
        {
            using var reader = new Reader(new FilteredSource(source));

            Assert.That(() => reader.Read(), Throws.InvalidOperationException);
        }

        static IEnumerable<string> CommentTestCases()
        {
            yield return ";single line comment";
            yield return ";first line\n;second line";
            yield return ";first;second;third";
            yield return ";first\n;second\r\n;third\n\r";
        }

        [Test]
        public void Read_ThrowException_WhenSourceContainsOnlySpaces()
        {
            using var reader = new Reader(new FilteredSource("   \t"));

            Assert.That(() => reader.Read(), Throws.InvalidOperationException);
        }

        [Test]
        public void Read_ThrowException_WhenSourceIsEmpty()
        {
            using var reader = new Reader(new FilteredSource(string.Empty));

            Assert.That(() => reader.Read(), Throws.InvalidOperationException);
        }
    }
}
