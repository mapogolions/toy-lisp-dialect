using System.Collections.Generic;
using Cl.Input;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClPairReaderTests
    {
        [TestCaseSource(nameof(PairWithoutClosedBracket))]
        public void ReadPair_ThrowException_WhenSouceContainsUnbalancedBrackets(string source)
        {
            using var reader = new Reader(new FilteredSource(source));

            Assert.That(() => reader.ReadPair(out var _), Throws.InnerException);
        }
        static IEnumerable<string> PairWithoutClosedBracket()
        {
            yield return "(";
            yield return "(1";
            yield return "(1 23";
        }

        public void ReadPair_ReturnFalse_WhenSourceStartsWithInvalidSymbol_ButContainsList()
        {
            using var reader = new Reader(new FilteredSource("  ()"));

            Assert.That(() => reader.ReadPair(out var _), Throws.InvalidOperationException);
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
