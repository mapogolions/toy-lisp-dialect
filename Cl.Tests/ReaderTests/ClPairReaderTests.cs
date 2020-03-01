using System.Collections.Generic;
using Cl.Constants;
using Cl.Input;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClPairReaderTests
    {
        [Test]
        public void ReadPair_ThrowException_WhenOnlyOneElementInsideBrackets()
        {
            using var reader = new Reader(new FilteredSource("(1)"));

            Assert.That(() => reader.ReadPair(out var _),
                Throws.InvalidOperationException.And.Message.EqualTo(Errors.ReadIllegalState));
        }

        [Test]
        public void ReadPair_ThrowException_WhenSourceContainsOnlyOpenBracket()
        {
            using var reader = new Reader(new FilteredSource("("));

            Assert.That(() => reader.ReadPair(out var _),
                Throws.InvalidOperationException.And.Message.EqualTo(Errors.ReadIllegalState));
        }

        [Test]
        public void ReadPair_ReturnFalse_WhenSourceStartsWithInvalidSymbol_ButContainsList()
        {
            using var reader = new Reader(new FilteredSource("  ()"));

            Assert.That(reader.ReadPair(out var _), Is.False);
        }

        [TestCaseSource(nameof(EmptyListCases))]
        public void ReadPair_ReturnEmptyList(string source)
        {
            using var reader = new Reader(new FilteredSource(source));

            Assert.That(reader.ReadPair(out var cell), Is.True);
            Assert.That(cell, Is.EqualTo(Nil.Given));
        }

        static IEnumerable<string> EmptyListCases()
        {
            yield return "()";
            yield return "()  ";
            yield return "(  )  ";
        }

        [Test]
        public void ReadPair_ReturnFalse_WhenSourceContainSomethingElse()
        {
            using var reader = new Reader(new FilteredSource("something else"));

            Assert.That(reader.ReadPair(out var cell), Is.False);
            Assert.That(cell, Is.Null);
        }

        [Test]
        public void ReadPair_ReturnFalse_WhenSourceIsEmpty()
        {
            using var reader = new Reader(new FilteredSource(string.Empty));

            Assert.That(reader.ReadPair(out var cell), Is.False);
            Assert.That(cell, Is.Null);
        }
    }
}
