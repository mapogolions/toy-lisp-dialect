using Cl.Core;
using Cl.Errors;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClDottedPairReaderTests
    {
        [Test]
        public void ReadDottedCell_TailCanBeFlattenList()
        {
            using var reader = new Reader("(1 . ('foo' #\\w ))");

            var cell = reader.ReadCell();
            var first = BuiltIn.First(cell) as ClInt;
            var second = BuiltIn.Second(cell) as ClString;
            var third = BuiltIn.Third(cell) as ClChar;

            Assert.That(first?.Value, Is.EqualTo(1));
            Assert.That(second?.Value, Is.EqualTo("foo"));
            Assert.That(third?.Value, Is.EqualTo('w'));
            Assert.That(BuiltIn.Cdddr(cell), Is.EqualTo(ClCell.Nil));
        }

        [Test]
        public void ReadDottedCell_CanRepresentTraditionalImmutableLinkedList()
        {
            using var reader = new Reader("(1 . ('foo' . (#\\w . ())))");

            var cell = reader.ReadCell();
            var first = BuiltIn.First(cell) as ClInt;
            var second = BuiltIn.Second(cell) as ClString;
            var third = BuiltIn.Third(cell) as ClChar;

            Assert.That(first?.Value, Is.EqualTo(1));
            Assert.That(second?.Value, Is.EqualTo("foo"));
            Assert.That(third?.Value, Is.EqualTo('w'));
            Assert.That(BuiltIn.Cdddr(cell), Is.EqualTo(ClCell.Nil));
        }

        [Test]
        public void ReadDottedCell_ThrowException_WhenAfterReadCdrInvalidSymbol()
        {
            using var reader = new Reader("(#f . #\\foo)");
            var errorMessage = $"Invalid format of the {nameof(ClCell)} literal";

            Assert.That(() => reader.ReadCell(),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadDottedCell_ReturnBoolAndChar()
        {
            using var reader = new Reader("(#f . #\\f)");

            var cell = reader.ReadCell();
            var car = cell.Car as ClBool;
            var cdr = cell.Cdr as ClChar;

            Assert.That(car?.Value, Is.False);
            Assert.That(cdr?.Value, Is.EqualTo('f'));
        }

        [Test]
        public void ReadDottedCell_ThrowException_CanNotReadMultipleValues()
        {
            using var reader = new Reader("(1.2 . 2 . #)");
            var errorMessage = $"Invalid format of the {nameof(ClCell)} literal";

            Assert.That(() => reader.ReadCell(),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [TestCaseSource(nameof(DottedPairTestCases))]
        public void ReadDottedCell_ReturnFloatIntegerCell(string input, double expectedCar, int expectedCdr)
        {
            using var reader = new Reader(input);

            var cell = reader.ReadCell();
            var car = cell.Car as ClDouble;
            var cdr = cell.Cdr as ClInt;

            Assert.That(car?.Value, Is.EqualTo(expectedCar).Within(double.Epsilon));
            Assert.That(cdr?.Value, Is.EqualTo(expectedCdr));
        }

        static object[] DottedPairTestCases =
            {
                new object[] { "(1.34 . 2)", 1.34, 2 },
                new object[] { "( 1.0 . 2 )", 1.0, 2 },
                new object[] { "(1.34.2)", 1.34, 2 },
            };
    }
}
