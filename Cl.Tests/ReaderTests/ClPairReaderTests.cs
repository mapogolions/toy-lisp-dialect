// using System;
// using System.Collections.Generic;
// using Cl;
// using Cl.Readers;
// using Cl.Errors;
// using Cl.IO;
// using Cl.Types;
// using NUnit.Framework;

// namespace Cl.Tests.ReaderTests
// {
//     [TestFixture]
//     public class ClPairReaderTests
//     {
//         private readonly ClCellReader _reader = new(new ClObjReader());

//         [Test]
//         public void ReadCell_CanReadMultipleValues()
//         {
//             using var source = new Source("(#f #t 6 #\\a)");

//             var cell = _reader.Read(source);
//             var first = BuiltIn.First(cell) as ClBool;
//             var second = BuiltIn.Second(cell) as ClBool;
//             var third = BuiltIn.Third(cell) as ClInt;
//             var fourth = BuiltIn.Fourth(cell) as ClChar;

//             Assert.That(first?.Value, Is.False);
//             Assert.That(second?.Value, Is.True);
//             Assert.That(third?.Value, Is.EqualTo(6));
//             Assert.That(fourth?.Value, Is.EqualTo('a'));
//         }

//         [Test]
//         public void ReadCell_SkipAllWhitespacesBetweenCarAndCdr()
//         {
//             using var source = new Source("(1.34\t  #\\a)");
//             var cell = _reader.Read(source);

//             var first = BuiltIn.First(cell) as ClDouble;
//             var second = BuiltIn.Second(cell) as ClChar;

//             Assert.That(first?.Value, Is.EqualTo(1.34).Within(double.Epsilon));
//             Assert.That(second?.Value, Is.EqualTo('a'));
//         }


//         [Test]
//         public void ReadCell_ThrowException_WhenSpaceIsMissed()
//         {
//             using var source = new Source("(#t1)");
//             var errorMessage = $"Invalid format of the {nameof(ClCell)} literal";

//             Assert.That(() => _reader.Read(source),
//                 Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
//         }

//         [Test]
//         public void ReadCell_ThrowException_WhenAfterReadCdrInvalidSymbol()
//         {
//             using var source = new Source("(#f #\\foo)");
//             var errorMessage = $"Invalid format of the {nameof(ClCell)} literal";

//             Assert.That(() => _reader.Read(source),
//                 Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
//         }

//         [Test]
//         public void ReadCell_ReturnBoolAndChar()
//         {
//             using var source = new Source("(#f #\\f)");

//             var cell = _reader.Read(source);
//             var first = BuiltIn.First(cell) as ClBool;
//             var second = BuiltIn.Second(cell) as ClChar;

//             Assert.That(first?.Value, Is.False);
//             Assert.That(second?.Value, Is.EqualTo('f'));
//         }

//         [Test]
//         public void ReadCell_ReturnPairOfNumbers()
//         {
//             using var source = new Source("(1.2 2)");

//             var cell = _reader.Read(source);
//             var first = BuiltIn.First(cell) as ClDouble;
//             var second = BuiltIn.Second(cell) as ClInt;

//             Assert.That(first?.Value, Is.EqualTo(1.2).Within(double.Epsilon));
//             Assert.That(second?.Value, Is.EqualTo(2));
//         }

//         [Test]
//         public void ReadCell_ReturnOneElementList()
//         {
//             using var source = new Source("(1)");

//             var cell = _reader.Read(source);
//             var first = cell.Car as ClInt;

//             Assert.That(first?.Value, Is.EqualTo(1));
//             Assert.That(cell.Cdr, Is.EqualTo(ClCell.Nil));
//         }

//         [Test]
//         public void ReadCell_ThrowException_WhenSourceContainsOnlyOpenBracket()
//         {
//             using var source = new Source("(");
//             Assert.That(() => _reader.Read(source),
//                 Throws.Exception.TypeOf<SyntaxError>().And.Message.EqualTo($"Invalid format of the {nameof(ClCell)} literal"));
//         }

//         [Test]
//         public void ReadCell_ReturnNull_WhenSourceStartsWithInvalidSymbol_ButContainsList()
//         {
//             using var source = new Source("  ()");
//             Assert.That(_reader.Read(source), Is.Null);
//         }

//         [TestCaseSource(nameof(EmptyListTestCases))]
//         public void ReadCell_ReturnEmptyList(string input)
//         {
//             using var source = new Source(input);
//             Assert.That(_reader.Read(source), Is.EqualTo(ClCell.Nil));
//         }

//         static IEnumerable<string> EmptyListTestCases()
//         {
//             yield return "()";
//             yield return "()  ";
//             yield return "(  )  ";
//             yield return $"(;comment{Environment.NewLine});comment";
//         }

//         [Test]
//         public void ReadCell_ReturnFalse_WhenSourceDoesNotStartWithBracket()
//         {
//             using var source = new Source("should be bracket");
//             Assert.That(_reader.Read(source), Is.Null);
//         }

//         [Test]
//         public void ReadCell_ReturnFalse_WhenSourceIsEmpty()
//         {
//             using var source = new Source(string.Empty);
//             Assert.That(_reader.Read(source), Is.Null);
//         }
//     }
// }
