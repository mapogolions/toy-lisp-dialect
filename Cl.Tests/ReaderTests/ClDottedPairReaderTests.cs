using Cl.Extensions;
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
            using var reader = new Reader("(1 . (\"foo\" #\\w ))");

            var cell = reader.ReadCell();
            var first = BuiltIn.First(cell).TypeOf<ClFixnum>();
            var second = BuiltIn.Second(cell).TypeOf<ClString>();
            var third = BuiltIn.Third(cell).TypeOf<ClChar>();

            Assert.That(first?.Value, Is.EqualTo(1));
            Assert.That(second?.Value, Is.EqualTo("foo"));
            Assert.That(third?.Value, Is.EqualTo('w'));
            Assert.That(BuiltIn.Cdddr(cell), Is.EqualTo(Nil.Given));
        }

        [Test]
        public void ReadDottedCell_CanRepresentTraditionalImmutableLinkedList()
        {
            using var reader = new Reader("(1 . (\"foo\" . (#\\w . ())))");

            var cell = reader.ReadCell();
            var first = BuiltIn.First(cell).TypeOf<ClFixnum>();
            var second = BuiltIn.Second(cell).TypeOf<ClString>();
            var third = BuiltIn.Third(cell).TypeOf<ClChar>();

            Assert.That(first?.Value, Is.EqualTo(1));
            Assert.That(second?.Value, Is.EqualTo("foo"));
            Assert.That(third?.Value, Is.EqualTo('w'));
            Assert.That(BuiltIn.Cdddr(cell), Is.EqualTo(Nil.Given));
        }


        [Test]
        public void ReadDottedCell_ThrowException_WhenAfterReadCdrInvalidSymbol()
        {
            using var reader = new Reader("(#f . #\\foo)");
            var errorMessage = Errors.Reader.UnknownLiteral(nameof(ClCell));

            Assert.That(() => reader.ReadCell(),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadDottedCell_ReturnBoolAndChar()
        {
            using var reader = new Reader("(#f . #\\f)");

            var cell = reader.ReadCell();
            var car = cell.Car.TypeOf<ClBool>();
            var cdr = cell.Cdr.TypeOf<ClChar>();

            Assert.That(car?.Value, Is.False);
            Assert.That(cdr?.Value, Is.EqualTo('f'));
        }

        [Test]
        public void ReadDottedCell_ThrowException_CanNotReadMultipleValues()
        {
            using var reader = new Reader("(1.2 . 2 . #)");
            var errorMessage = Errors.Reader.UnknownLiteral(nameof(ClCell));

            Assert.That(() => reader.ReadCell(),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }

        [TestCaseSource(nameof(DottedPairTestCases))]
        public void ReadDottedCell_ReturnFloatIntegerCell(string input, double expectedCar, int expectedCdr)
        {
            using var reader = new Reader(input);

            var cell = reader.ReadCell();
            var car = cell.Car.TypeOf<ClFloat>();
            var cdr = cell.Cdr.TypeOf<ClFixnum>();

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
