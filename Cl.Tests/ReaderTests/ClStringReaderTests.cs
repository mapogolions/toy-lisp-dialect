using Cl.Constants;
using Cl.Input;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.ReaderTests
{
    [TestFixture]
    public class ClStringReaderTests
    {
        [Test]
        public void ReadString_SkipOnlyPartOfSource()
        {
            var source = new FilteredSource("\"foo\"bar");
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
                new object[] { "\"foo\"", "foo" },
                new object[] { "\"\"", string.Empty },
                new object[] { "\"foo\"bar", "foo" }
            };

        [Test]
        public void ReadString_ThrowException_DoubleQuotesAreUnbalanced()
        {
            using var reader = new Reader("\"some");

            Assert.That(() => reader.ReadString(),
                Throws.InvalidOperationException.And.Message.EqualTo(Errors.UnknownLiteral(nameof(ClString))));
        }

        [Test]
        public void ReadString_ReturnNull_WhenSourceDoesNotStartWithDoubleQuotes()
        {
            using var reader = new Reader(" \"foo\"");

            Assert.That(reader.ReadString(), Is.Null);
        }
    }
}
