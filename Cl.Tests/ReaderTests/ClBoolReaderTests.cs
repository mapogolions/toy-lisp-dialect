using Cl.Readers;
using Cl.Errors;
using Cl.IO;
using Cl.Types;

namespace Cl.Tests.ReaderTests;

[TestFixture]
public class ClBoolReaderTests
{
    private readonly ClBoolReader _reader = new();

    [Test]
    public void ReadBool_SkipOnlyPartOfSource()
    {
        var source = new Source("#ttf");
        _reader.Read(source);
        Assert.That(source.ToString(), Is.EqualTo("tf"));
    }

    [Test]
    public void ReadBool_ReturnTheFalse()
    {
        using var source = new Source("#fi");
        Assert.That(_reader.Read(source)?.Value, Is.False);
    }

    [Test]
    public void ReadBool_ReturnTheTrue()
    {
        using var source = new Source("#ti");
        Assert.That(_reader.Read(source)?.Value, Is.True);
    }

    [Test]
    public void ReadBool_ThrowException_WhenBooleanSignificantSymbolDoesNotFollowAfterHash()
    {
        using var source = new Source("#w");
        var errorMessage = $"Invalid format of the {nameof(ClBool)} literal";
        Assert.That(() => _reader.Read(source),
            Throws.Exception.TypeOf<SyntaxError>().And.Message.EqualTo(errorMessage));
    }

    [Test]
    public void ReadBool_ThrowException_WhenSourceContainsOnlyHash()
    {
        using var source = new Source("#");
        var errorMessage = $"Invalid format of the {nameof(ClBool)} literal";
        Assert.That(() => _reader.Read(source),
            Throws.Exception.TypeOf<SyntaxError>().And.Message.EqualTo(errorMessage));
    }

    [Test]
    public void ReadBool_ReturnFalse_WhenSourceDoesNotStartWithHash()
    {
        using var source = new Source(" #f");
        Assert.That(_reader.Read(source)?.Value, Is.Null);
    }
}
