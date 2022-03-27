using Cl.Core;
using Cl.Errors;
using Cl.IO;
using Cl.Types;
using NUnit.Framework;
using static Cl.Core.Helpers.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClStringReaderTests
    {
        [Test]
        public void ReadString_SkipOnlyPartOfSource()
        {
            var source = new Source("'foo'bar");
            using var reader = new Reader(source);

            Ignore(reader.ReadString());

            Assert.That(source.ToString(), Is.EqualTo("bar"));
        }

        [TestCaseSource(nameof(ValidStringTestCases))]
        public void ReadString_ReturnString(string input, string expected)
        {
            using var reader = new Reader(input);
            Assert.That(reader.ReadString()?.Value, Is.EqualTo(expected));
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
            using var reader = new Reader("'some");
            var errorMessage = $"Invalid format of the {nameof(ClString)} literal";

            Assert.That(() => reader.ReadString(),
                Throws.Exception.TypeOf<SyntaxError>().And.Message.EqualTo(errorMessage));
        }

        [Test]
        public void ReadString_ReturnNull_WhenSourceDoesNotStartWithDoubleQuotes()
        {
            using var reader = new Reader(" 'foo'");
            Assert.That(reader.ReadString(), Is.Null);
        }
    }
}
