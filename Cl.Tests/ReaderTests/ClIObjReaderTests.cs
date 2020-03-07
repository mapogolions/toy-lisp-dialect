using System.Collections.Generic;
using Cl.Input;
using NUnit.Framework;
using System;
using Cl.Types;

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
            using var reader = new Reader(new FilteredSource($"{startsWith}{endsWith}"));

            var cell = reader.Read() as ClPair;
            var first = BuiltIn.Car(cell) as ClBool;
            var second = BuiltIn.Cadr(cell) as ClString;

            Assert.That(first?.Value, Is.True);
            Assert.That(second?.Value, Is.EqualTo("bar"));
        }

        [Test]
        public void Read_Treat_CommentsBetweenCarAndCdrAsDelimiter()
        {
            var startsWith = $"\t;comment{Environment.NewLine}(\t;comment{Environment.NewLine}#f"; // ClBool.False
            var endsWith = $";comment{Environment.NewLine}\"foo\";comment\t{Environment.NewLine});comment\t"; // ClString("foo")
            using var reader = new Reader(new FilteredSource($"{startsWith}{endsWith}"));

            var cell = reader.Read() as ClPair;
            var first = BuiltIn.Car(cell) as ClBool;
            var second = BuiltIn.Cadr(cell) as ClString;

            Assert.That(first?.Value, Is.False);
            Assert.That(second?.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void Read_ReturnInteger_WhenCommentsAround()
        {
            var input = $";before{Environment.NewLine}112;after";
            using var reader = new Reader(new FilteredSource(input));

            var obj = reader.Read() as ClFixnum;

            Assert.That(obj?.Value, Is.EqualTo(112));
        }

        [Test]
        public void Read_ReturnChar()
        {
            using var reader = new Reader(new FilteredSource("#\\N"));

            var atom = reader.Read() as ClChar;

            Assert.That(atom?.Value, Is.EqualTo('N'));
        }

        [Test]
        public void Read_ReturnString()
        {
            using var reader = new Reader(new FilteredSource("\"foo\""));

            var atom = reader.Read() as ClString;

            Assert.That(atom?.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void Read_ReturnBool()
        {
            using var reader = new Reader(new FilteredSource("#t"));

            var atom = reader.Read() as ClBool;

            Assert.That(atom?.Value, Is.EqualTo(true));
        }

        [Test]
        public void Read_ReturnInteger()
        {
            using var reader = new Reader(new FilteredSource("12"));

            var atom = reader.Read() as ClFixnum;

            Assert.That(atom?.Value, Is.EqualTo(12));
        }

        [TestCaseSource(nameof(CommentTestCases))]
        public void Read_ThrowException_WhenSourceContainsOnlyCommentLine(string source)
        {
            using var reader = new Reader(new FilteredSource(source));

            Assert.That(() => reader.Read(), Throws.InvalidOperationException);
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
            using var reader = new Reader(new FilteredSource("11."));

            Assert.That(() => reader.Read(), Throws.InvalidOperationException);
        }

        [Test]
        public void Read_ThrowException_WhenSourceContainsOnlySpaces()
        {
            using var reader = new Reader(new FilteredSource("   \t"));

            Assert.That(() => reader.Read(), Throws.InvalidOperationException);
        }

        [Test]
        public void Read_ThrowException_WhenSourceIsEmpty()
        {
            using var reader = new Reader(new FilteredSource(string.Empty));

            Assert.That(() => reader.Read(), Throws.InvalidOperationException);
        }
    }
}
