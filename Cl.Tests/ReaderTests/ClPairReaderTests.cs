using System.Collections.Generic;
using Cl.Constants;
using Cl.Extensions;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClPairReaderTests
    {
        [Test]
        public void ReadPair_CanReadMultipleValues()
        {
            using var reader = new Reader("(#f #t 6 #\\a)");

            var cell = reader.ReadPair();
            var first = BuiltIn.First(cell).TypeOf<ClBool>();
            var second = BuiltIn.Second(cell).TypeOf<ClBool>();
            var third = BuiltIn.Third(cell).TypeOf<ClFixnum>();
            var fourth = BuiltIn.Fourth(cell).TypeOf<ClChar>();

            Assert.That(first?.Value, Is.False);
            Assert.That(second?.Value, Is.True);
            Assert.That(third?.Value, Is.EqualTo(6));
            Assert.That(fourth?.Value, Is.EqualTo('a'));
        }

        [Test]
        public void ReadPair_SkipAllWhitespacesBetweenCarAndCdr()
        {
            using var reader = new Reader("(1.34\t  #\\a)");
            var cell = reader.ReadPair();

            var first = BuiltIn.First(cell).TypeOf<ClFloat>();
            var second = BuiltIn.Second(cell).TypeOf<ClChar>();

            Assert.That(first?.Value, Is.EqualTo(1.34).Within(double.Epsilon));
            Assert.That(second?.Value, Is.EqualTo('a'));
        }


        [Test]
        public void ReadPair_ThrowException_WhenSpaceIsMissed()
        {
            using var reader = new Reader("(#t1)");

            Assert.That(() => reader.ReadPair(),
                Throws.InvalidOperationException.With.Message.EqualTo(Errors.UnknownLiteral(nameof(ClPair))));
        }

        [Test]
        public void ReadPair_ThrowException_WhenAfterReadCdrInvalidSymbol()
        {
            using var reader = new Reader("(#f #\\foo)");

            Assert.That(() => reader.ReadPair(),
                Throws.InvalidOperationException.With.Message.EqualTo(Errors.UnknownLiteral(nameof(ClPair))));
        }

        [Test]
        public void ReadPair_ReturnBoolAndChar()
        {
            using var reader = new Reader("(#f #\\f)");

            var cell = reader.ReadPair();
            var first = BuiltIn.First(cell).TypeOf<ClBool>();
            var second = BuiltIn.Second(cell).TypeOf<ClChar>();

            Assert.That(first?.Value, Is.False);
            Assert.That(second?.Value, Is.EqualTo('f'));
        }

        [Test]
        public void ReadPair_ReturnPairOfNumbers()
        {
            using var reader = new Reader("(1.2 2)");

            var cell = reader.ReadPair();
            var first = BuiltIn.First(cell).TypeOf<ClFloat>();
            var second = BuiltIn.Second(cell).TypeOf<ClFixnum>();

            Assert.That(first?.Value, Is.EqualTo(1.2).Within(double.Epsilon));
            Assert.That(second?.Value, Is.EqualTo(2));
        }

        [Test]
        public void ReadPair_ReturnOneElementList()
        {
            using var reader = new Reader("(1)");

            var cell = reader.ReadPair();
            var first = cell.Car.TypeOf<ClFixnum>();

            Assert.That(first?.Value, Is.EqualTo(1));
            Assert.That(cell.Cdr, Is.EqualTo(Nil.Given));
        }

        [Test]
        public void ReadPair_ThrowException_WhenSourceContainsOnlyOpenBracket()
        {
            using var reader = new Reader("(");

            Assert.That(() => reader.ReadPair(),
                Throws.InvalidOperationException.And.Message.EqualTo(Errors.ReadIllegalState));
        }

        [Test]
        public void ReadPair_ReturnNull_WhenSourceStartsWithInvalidSymbol_ButContainsList()
        {
            using var reader = new Reader("  ()");

            Assert.That(reader.ReadPair(), Is.Null);
        }

        [TestCaseSource(nameof(EmptyListTestCases))]
        public void ReadPair_ReturnEmptyList(string source)
        {
            using var reader = new Reader(source);

            Assert.That(reader.ReadPair(), Is.EqualTo(Nil.Given));
        }

        static IEnumerable<string> EmptyListTestCases()
        {
            yield return "()";
            yield return "()  ";
            yield return "(  )  ";
        }

        [Test]
        public void ReadPair_ReturnFalse_WhenSourceContainSomethingElse()
        {
            using var reader = new Reader("something else");

            Assert.That(reader.ReadPair(), Is.Null);
        }

        [Test]
        public void ReadPair_ReturnFalse_WhenSourceIsEmpty()
        {
            using var reader = new Reader(string.Empty);

            Assert.That(reader.ReadPair(), Is.Null);
        }
    }
}
