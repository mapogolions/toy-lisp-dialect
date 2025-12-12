using Cl.Readers;
using Cl.IO;

namespace Cl.Tests.ReaderTests;

[TestFixture]
public class ClSymbolReaderTests
{
    private readonly ClSymbolReader _reader = new();

    [Test]
    public void ReadSymbol_SkipOnlyPartOfSource()
    {
        using var source = new Source("foo bar");
        _reader.Read(source);
        Assert.That(source.ToString(), Is.EqualTo(" bar"));
    }

    [TestCaseSource(nameof(ValidSymbolsTestCases))]
    public void ReadSymbol_ReturnSymbol(string input, string expected)
    {
        using var source = new Source(input);
        Assert.That(_reader.Read(source)?.Value, Is.EqualTo(expected));
    }

    static object[] ValidSymbolsTestCases =
        {
            new object[] { "foo bar", "foo" },
            new object[] { "bar-foo", "bar-foo"},
            new object[] { "s1--??--", "s1--??--" },
            new object[] { "null-?", "null-?" },
            new object[] { "s-o-m-e-?-", "s-o-m-e-?-" },
            new object[] { "foo", "foo" },
            new object[] { "f", "f" },
            new object[] { "bar1", "bar1" },
            new object[] { "a1b", "a1b" },
            new object[] { "set!", "set!"},
            // special builtin functions
            new object[] { "+", "+" },
            new object[] { "*", "*" },
            new object[] { "-", "-" },
            new object[] { "/", "/" },
        };

    [TestCaseSource(nameof(InvalidSymbolsTestCases))]
    public void ReadString_ReturnNull_WhenSourceStartsWithInvalidSymbol(string input)
    {
        using var source = new Source("\"some");
        Assert.That(() => _reader.Read(source),Is.Null);
    }

    static IEnumerable<string> InvalidSymbolsTestCases()
    {
        yield return string.Empty;
        yield return "1foo";
        yield return "-bar";
        yield return "+foo";
        yield return "?bar";
        yield return ".x";
        yield return "~y";
    }
}
