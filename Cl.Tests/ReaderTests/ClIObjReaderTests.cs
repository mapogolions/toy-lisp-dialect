using System.Collections.Generic;
using NUnit.Framework;
using System;
using Cl.Types;
using Cl.Extensions;

namespace Cl.Tests
{
    [TestFixture]
    public class ClIObjReaderTests
    {
        [Test]
        public void Read_Treat_WhitespacesBetweenCarAndCdrAsDelimiter()
        {
            var startsWith = $"\t;comment{Environment.NewLine}(\t;comment{Environment.NewLine}#t"; // ClBool.True
            var endsWith = $"\t \"bar\";comment\t{Environment.NewLine});comment\t"; // ClString("bar")
            using var reader = new Reader($"{startsWith}{endsWith}");

            var cell = reader.Ast().TypeOf<ClPair>();
            var first = BuiltIn.First(cell).TypeOf<ClBool>();
            var second = BuiltIn.Second(cell).TypeOf<ClString>();

            Assert.That(first?.Value, Is.True);
            Assert.That(second?.Value, Is.EqualTo("bar"));
        }

        [Test]
        public void Read_Treat_CommentsBetweenCarAndCdrAsDelimiter()
        {
            var startsWith = $"\t;comment{Environment.NewLine}(\t;comment{Environment.NewLine}#f"; // ClBool.False
            var endsWith = $";comment{Environment.NewLine}\"foo\";comment\t{Environment.NewLine});comment\t"; // ClString("foo")
            using var reader = new Reader($"{startsWith}{endsWith}");

            var cell = reader.Ast().TypeOf<ClPair>();
            var first = BuiltIn.Car(cell).TypeOf<ClBool>();
            var second = BuiltIn.Cadr(cell).TypeOf<ClString>();

            Assert.That(first?.Value, Is.False);
            Assert.That(second?.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void Read_ReturnInteger_WhenCommentsAround()
        {
            var input = $";before{Environment.NewLine}112;after";
            using var reader = new Reader(input);

            var atom = reader.Ast().TypeOf<ClFixnum>();

            Assert.That(atom?.Value, Is.EqualTo(112));
        }

        [Test]
        public void Read_ReturnChar()
        {
            using var reader = new Reader("#\\N");

            var atom = reader.Ast().TypeOf<ClChar>();

            Assert.That(atom?.Value, Is.EqualTo('N'));
        }

        [Test]
        public void Read_ReturnString()
        {
            using var reader = new Reader("\"foo\"");

            var atom = reader.Ast().TypeOf<ClString>();

            Assert.That(atom?.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void Read_ReturnBool()
        {
            using var reader = new Reader("#t");

            var atom = reader.Ast().TypeOf<ClBool>();

            Assert.That(atom?.Value, Is.EqualTo(true));
        }

        [Test]
        public void Read_ReturnInteger()
        {
            using var reader = new Reader("12");

            var atom = reader.Ast().TypeOf<ClFixnum>();

            Assert.That(atom?.Value, Is.EqualTo(12));
        }

        [TestCaseSource(nameof(CommentTestCases))]
        public void Read_ThrowException_WhenSourceContainsOnlyCommentLine(string source)
        {
            using var reader = new Reader(source);

            Assert.That(() => reader.Ast(), Throws.InvalidOperationException);
        }

        static IEnumerable<string> CommentTestCases()
        {
            yield return ";single line comment";
            yield return $";first line{Environment.NewLine};second line";
            yield return ";chunk-1;chunk-2;chunk-3";
            yield return $";first{Environment.NewLine};second{Environment.NewLine};third{Environment.NewLine}";
        }

        [Test]
        public void Read_ThrowException_WhenAfterSignificandAndDotInvalidSymbol()
        {
            using var reader = new Reader("11.");

            Assert.That(() => reader.Ast(), Throws.InvalidOperationException);
        }

        [Test]
        public void Read_ThrowException_WhenSourceContainsOnlySpaces()
        {
            using var reader = new Reader("   \t");

            Assert.That(() => reader.Ast(), Throws.InvalidOperationException);
        }

        [Test]
        public void Read_ThrowException_WhenSourceIsEmpty()
        {
            using var reader = new Reader(string.Empty);

            Assert.That(() => reader.Ast(), Throws.InvalidOperationException);
        }
    }
}
