using System.Collections.Generic;
using NUnit.Framework;
using System;
using Cl.Types;
using Cl.Errors;
using Cl.Core;

namespace Cl.Tests
{
    [TestFixture]
    public class ClObjReaderTests
    {
        [Test]
        public void ReadExpression_Treat_WhitespacesBetweenCarAndCdrAsDelimiter()
        {
            var startsWith = $"\t;comment{Environment.NewLine}(\t;comment{Environment.NewLine}#t"; // ClBool.True
            var endsWith = $"\t 'bar';comment\t{Environment.NewLine});comment\t"; // ClString("bar")
            using var reader = new Reader($"{startsWith}{endsWith}");

            var cell = reader.ReadExpression() as ClCell;
            var first = BuiltIn.First(cell) as ClBool;
            var second = BuiltIn.Second(cell) as ClString;

            Assert.That(first?.Value, Is.True);
            Assert.That(second?.Value, Is.EqualTo("bar"));
        }

        [Test]
        public void ReadExpression_Treat_CommentsBetweenCarAndCdrAsDelimiter()
        {
            var startsWith = $"\t;comment{Environment.NewLine}(\t;comment{Environment.NewLine}#f"; // ClBool.False
            var endsWith = $";comment{Environment.NewLine}'foo';comment\t{Environment.NewLine});comment\t"; // ClString("foo")
            using var reader = new Reader($"{startsWith}{endsWith}");

            var cell = reader.ReadExpression() as ClCell;
            var first = BuiltIn.Car(cell) as ClBool;
            var second = BuiltIn.Cadr(cell) as ClString;

            Assert.That(first?.Value, Is.False);
            Assert.That(second?.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void ReadExpression_ReturnInteger_WhenCommentsAround()
        {
            var input = $";before{Environment.NewLine}112;after";
            using var reader = new Reader(input);

            var atom = reader.ReadExpression() as ClInt;

            Assert.That(atom?.Value, Is.EqualTo(112));
        }

        [Test]
        public void ReadExpression_ReturnChar()
        {
            using var reader = new Reader("#\\N");
            var atom = reader.ReadExpression() as ClChar;
            Assert.That(atom?.Value, Is.EqualTo('N'));
        }

        [Test]
        public void ReadExpression_ReturnString()
        {
            using var reader = new Reader("'foo'");
            var atom = reader.ReadExpression() as ClString;
            Assert.That(atom?.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void ReadExpression_ReturnBool()
        {
            using var reader = new Reader("#t");
            var atom = reader.ReadExpression() as ClBool;
            Assert.That(atom?.Value, Is.EqualTo(true));
        }

        [Test]
        public void ReadExpression_ReturnInteger()
        {
            using var reader = new Reader("12");
            var atom = reader.ReadExpression() as ClInt;
            Assert.That(atom?.Value, Is.EqualTo(12));
        }

        [TestCaseSource(nameof(CommentTestCases))]
        public void ReadExpression_ReturnNil_WhenSourceContainsOnlyComments(string source)
        {
            using var reader = new Reader(source);
            Assert.AreSame(ClCell.Nil, reader.ReadExpression());
        }

        static IEnumerable<string> CommentTestCases()
        {
            yield return ";single line comment";
            yield return $";first line{Environment.NewLine};second line";
            yield return ";chunk-1;chunk-2;chunk-3";
            yield return $";first{Environment.NewLine};second{Environment.NewLine};third{Environment.NewLine}";
        }

        [Test]
        public void ReadExpression_ThrowException_WhenAfterSignificandAndDotInvalidSymbol()
        {
            using var reader = new Reader("11.");
            var errorMessage = $"Invalid format of the {nameof(ClDouble)} literal";

            Assert.That(() => reader.ReadExpression(),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadExpression_ReturnNil_WhenSourceContainsOnlySpaces()
        {
            using var reader = new Reader("   \t");
            Assert.AreSame(ClCell.Nil, reader.ReadExpression());
        }

        [Test]
        public void ReadExpression_ReturnNil_WhenSourceIsEmpty()
        {
            using var reader = new Reader(string.Empty);
            Assert.AreSame(ClCell.Nil, reader.ReadExpression());
        }
    }
}
