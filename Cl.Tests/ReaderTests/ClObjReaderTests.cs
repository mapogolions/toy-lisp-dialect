using System.Collections.Generic;
using NUnit.Framework;
using System;
using Cl.Types;
using Cl.Errors;
using Cl.Core;
using Cl.Core.Readers;
using Cl.IO;

namespace Cl.Tests
{
    [TestFixture]
    public class ClObjReaderTests
    {
        private readonly ClObjReader _reader = new();

        [Test]
        public void ReadExpression_Treat_WhitespacesBetweenCarAndCdrAsDelimiter()
        {
            var startsWith = $"\t;comment{Environment.NewLine}(\t;comment{Environment.NewLine}#t"; // ClBool.True
            var endsWith = $"\t 'bar';comment\t{Environment.NewLine});comment\t"; // ClString("bar")
            using var source = new Source($"{startsWith}{endsWith}");

            var cell = _reader.Read(source) as ClCell;
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
            using var source = new Source($"{startsWith}{endsWith}");

            var cell = _reader.Read(source) as ClCell;
            var first = BuiltIn.Car(cell) as ClBool;
            var second = BuiltIn.Cadr(cell) as ClString;

            Assert.That(first?.Value, Is.False);
            Assert.That(second?.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void ReadExpression_ReturnInteger_WhenCommentsAround()
        {
            var input = $";before{Environment.NewLine}112;after";
            using var source = new Source(input);

            var atom = _reader.Read(source) as ClInt;

            Assert.That(atom?.Value, Is.EqualTo(112));
        }

        [Test]
        public void ReadExpression_ReturnChar()
        {
            using var source = new Source("#\\N");
            var atom = _reader.Read(source) as ClChar;
            Assert.That(atom?.Value, Is.EqualTo('N'));
        }

        [Test]
        public void ReadExpression_ReturnString()
        {
            using var source = new Source("'foo'");
            var atom = _reader.Read(source) as ClString;
            Assert.That(atom?.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void ReadExpression_ReturnBool()
        {
            using var source = new Source("#t");
            var atom = _reader.Read(source) as ClBool;
            Assert.That(atom?.Value, Is.EqualTo(true));
        }

        [Test]
        public void ReadExpression_ReturnInteger()
        {
            using var source = new Source("12");
            var atom = _reader.Read(source) as ClInt;
            Assert.That(atom?.Value, Is.EqualTo(12));
        }

        [TestCaseSource(nameof(CommentTestCases))]
        public void ReadExpression_ReturnNil_WhenSourceContainsOnlyComments(string input)
        {
            using var source = new Source(input);
            Assert.AreSame(ClCell.Nil, _reader.Read(source));
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
            using var source = new Source("11.");
            var errorMessage = $"Invalid format of the {nameof(ClDouble)} literal";

            Assert.That(() => _reader.Read(source),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadExpression_ReturnNil_WhenSourceContainsOnlySpaces()
        {
            using var source = new Source("   \t");
            Assert.AreSame(ClCell.Nil, _reader.Read(source));
        }

        [Test]
        public void ReadExpression_ReturnNil_WhenSourceIsEmpty()
        {
            using var source = new Source(string.Empty);
            Assert.AreSame(ClCell.Nil, _reader.Read(source));
        }
    }
}
