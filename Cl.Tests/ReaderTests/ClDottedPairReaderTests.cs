using Cl.Readers;
using Cl.Errors;
using Cl.IO;
using Cl.Types;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClDottedPairReaderTests
    {
        private readonly ClCellReader _reader = new(new ClObjReader());

        [Test]
        public void ReadDottedCell_TailCanBeFlattenList()
        {
            using var source = new Source("(1 . ('foo' #\\w ))");

            var cell = _reader.Read(source)!;
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
            using var source = new Source("(1 . ('foo' . (#\\w . ())))");

            var cell = _reader.Read(source)!;
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
            using var source = new Source("(#f . #\\foo)");
            var errorMessage = $"Invalid format of the {nameof(ClCell)} literal";

            Assert.That(() => _reader.Read(source),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadDottedCell_ReturnBoolAndChar()
        {
            using var source = new Source("(#f . 2)");
            var cell = _reader.Read(source)!;
            var car = cell.Car as ClBool;
            var cdr = cell.Cdr as ClInt;

            Assert.That(car?.Value, Is.False);
            Assert.That(cdr?.Value, Is.EqualTo(2));
        }

        [Test]
        public void ReadDottedCell_ThrowException_CanNotReadMultipleValues()
        {
            using var source = new Source("(1.2 . 2 . #)");
            var errorMessage = $"Invalid format of the {nameof(ClCell)} literal";

            Assert.That(() => _reader.Read(source),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [TestCaseSource(nameof(DottedPairTestCases))]
        public void ReadDottedCell_ReturnFloatIntegerCell(string input, double expectedCar, int expectedCdr)
        {
            using var source = new Source(input);

            var cell = _reader.Read(source)!;
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
