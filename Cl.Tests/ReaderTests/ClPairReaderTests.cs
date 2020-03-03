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
        public void ReadPair_ThrowException_WhenAfterReadElementSomething()
        {
            using var reader = new Reader(new FilteredSource("(#f #\\foo)"));

            Assert.That(() => reader.ReadPair(out var _),
                Throws.InvalidOperationException.With.Message.EqualTo(Errors.UnknownLiteral(nameof(ClPair))));
        }

        [Test]
        public void ReadPair_ReturnBoolAndChar()
        {
            using var reader = new Reader(new FilteredSource("(#f #\\f)"));

            var result = reader.ReadPair(out var cell);
            var car = cell.Car as ClBool;
            var cdr = cell.Cdr as ClChar;

            Assert.That(result, Is.True);
            Assert.That(car?.Value, Is.False);
            Assert.That(cdr?.Value, Is.EqualTo('f'));
        }

        [Test]
        public void ReadPair_ReturnPairOfNumbers()
        {
            using var reader = new Reader(new FilteredSource("(1.2 2)"));

            var result = reader.ReadPair(out var cell);
            var car = cell.Car as ClFloatingPoint;
            var cdr = cell.Cdr as ClFixnum;

            Assert.That(result, Is.True);
            Assert.That(car?.Value, Is.EqualTo(1.2).Within(double.Epsilon));
            Assert.That(cdr?.Value, Is.EqualTo(2));
        }

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
