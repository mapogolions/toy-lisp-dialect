using System;
using System.Collections.Generic;
using Cl.Errors;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClPairReaderTests
    {
        [Test]
        public void ReadCell_CanReadMultipleValues()
        {
            using var reader = new Reader("(#f #t 6 #\\a)");

            var cell = reader.ReadCell();
            var first = BuiltIn.First(cell) as ClBool;
            var second = BuiltIn.Second(cell) as ClBool;
            var third = BuiltIn.Third(cell) as ClInt;
            var fourth = BuiltIn.Fourth(cell) as ClChar;

            Assert.That(first?.Value, Is.False);
            Assert.That(second?.Value, Is.True);
            Assert.That(third?.Value, Is.EqualTo(6));
            Assert.That(fourth?.Value, Is.EqualTo('a'));
        }

        [Test]
        public void ReadCell_SkipAllWhitespacesBetweenCarAndCdr()
        {
            using var reader = new Reader("(1.34\t  #\\a)");
            var cell = reader.ReadCell();

            var first = BuiltIn.First(cell) as ClDouble;
            var second = BuiltIn.Second(cell) as ClChar;

            Assert.That(first?.Value, Is.EqualTo(1.34).Within(double.Epsilon));
            Assert.That(second?.Value, Is.EqualTo('a'));
        }


        [Test]
        public void ReadCell_ThrowException_WhenSpaceIsMissed()
        {
            using var reader = new Reader("(#t1)");
            var errorMessage = $"Invalid format of the {nameof(ClCell)} literal";

            Assert.That(() => reader.ReadCell(),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadCell_ThrowException_WhenAfterReadCdrInvalidSymbol()
        {
            using var reader = new Reader("(#f #\\foo)");
            var errorMessage = $"Invalid format of the {nameof(ClCell)} literal";

            Assert.That(() => reader.ReadCell(),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadCell_ReturnBoolAndChar()
        {
            using var reader = new Reader("(#f #\\f)");

            var cell = reader.ReadCell();
            var first = BuiltIn.First(cell) as ClBool;
            var second = BuiltIn.Second(cell) as ClChar;

            Assert.That(first?.Value, Is.False);
            Assert.That(second?.Value, Is.EqualTo('f'));
        }

        [Test]
        public void ReadCell_ReturnPairOfNumbers()
        {
            using var reader = new Reader("(1.2 2)");

            var cell = reader.ReadCell();
            var first = BuiltIn.First(cell) as ClDouble;
            var second = BuiltIn.Second(cell) as ClInt;

            Assert.That(first?.Value, Is.EqualTo(1.2).Within(double.Epsilon));
            Assert.That(second?.Value, Is.EqualTo(2));
        }

        [Test]
        public void ReadCell_ReturnOneElementList()
        {
            using var reader = new Reader("(1)");

            var cell = reader.ReadCell();
            var first = cell.Car as ClInt;

            Assert.That(first?.Value, Is.EqualTo(1));
            Assert.That(cell.Cdr, Is.EqualTo(ClCell.Nil));
        }

        [Test]
        public void ReadCell_ThrowException_WhenSourceContainsOnlyOpenBracket()
        {
            using var reader = new Reader("(");
            Assert.That(() => reader.ReadCell(),
                Throws.Exception.TypeOf<SyntaxError>().And.Message.EqualTo("Unknown literal"));
        }

        [Test]
        public void ReadCell_ReturnNull_WhenSourceStartsWithInvalidSymbol_ButContainsList()
        {
            using var reader = new Reader("  ()");
            Assert.That(reader.ReadCell(), Is.Null);
        }

        [TestCaseSource(nameof(EmptyListTestCases))]
        public void ReadCell_ReturnEmptyList(string source)
        {
            using var reader = new Reader(source);
            Assert.That(reader.ReadCell(), Is.EqualTo(ClCell.Nil));
        }

        static IEnumerable<string> EmptyListTestCases()
        {
            yield return "()";
            yield return "()  ";
            yield return "(  )  ";
            yield return $"(;comment{Environment.NewLine});comment";
        }

        [Test]
        public void ReadCell_ReturnFalse_WhenSourceDoesNotStartWithBracket()
        {
            using var reader = new Reader("should be bracket");
            Assert.That(reader.ReadCell(), Is.Null);
        }

        [Test]
        public void ReadCell_ReturnFalse_WhenSourceIsEmpty()
        {
            using var reader = new Reader(string.Empty);
            Assert.That(reader.ReadCell(), Is.Null);
        }
    }
}
