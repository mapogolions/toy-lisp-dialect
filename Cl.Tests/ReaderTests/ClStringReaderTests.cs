using Cl.Readers;
using Cl.Errors;
using Cl.IO;
using Cl.Types;
using NUnit.Framework;
using static Cl.Helpers.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClStringReaderTests
    {
        private readonly ClStringReader _reader = new();

        [Test]
        public void ReadString_SkipOnlyPartOfSource()
        {
            using var source = new Source("'foo'bar");
            Ignore(_reader.Read(source));
            Assert.That(source.ToString(), Is.EqualTo("bar"));
        }

        [TestCaseSource(nameof(ValidStringTestCases))]
        public void ReadString_ReturnString(string input, string expected)
        {
            using var source = new Source(input);
            Assert.That(_reader.Read(source)?.Value, Is.EqualTo(expected));
        }

        static object[] ValidStringTestCases =
            {
                new object[] { "'foo'", "foo" },
                new object[] { "''", string.Empty },
                new object[] { "'foo'bar", "foo" }
            };

        [Test]
        public void ReadString_ThrowException_DoubleQuotesAreUnbalanced()
        {
            using var source = new Source("'some");
            var errorMessage = $"Invalid format of the {nameof(ClString)} literal";

            Assert.That(() => _reader.Read(source),
                Throws.Exception.TypeOf<SyntaxError>().And.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadString_ReturnNull_WhenSourceDoesNotStartWithDoubleQuotes()
        {
            using var source = new Source(" 'foo'");
            Assert.That(_reader.Read(source), Is.Null);
        }
    }
}
