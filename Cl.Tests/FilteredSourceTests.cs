using System.Collections.Generic;
using Cl.SourceCode;
using NUnit.Framework;

namespace Cl.Tests
{
    // [TestFixture]
    public class FilteredSourceTests
    {
        // [Test]
        // public void SkipLine_DrainSource()
        // {
        //     var source = new FilteredSource("fi\rst\nsecond\r\nthird\n\r");
        //     using var reader = new Reader(source);

        //     source.SkipLine();
        //     Assert.That(source.ToString(), Is.EqualTo("second\r\nthird\n\r"));
        // }

        // [TestCaseSource(nameof(EndOfLineCases))]
        // public void SkipLine_IgnoreEntireStringUntilEol(string eol)
        // {
        //     var source = new FilteredSource($"foo{eol}bar");
        //     using var reader = new Reader(source);

        //     Assert.That(reader.SkipLine(), Is.True);
        //     Assert.That(source.ToString(), Is.EqualTo("bar"));
        // }

        // [Test]
        // public void SkipLine_ReturnTrue_WhenSourceIsEmpty()
        // {
        //     using var reader = new Reader(new FilteredSource(string.Empty));

        //     Assert.That(reader.SkipLine(), Is.True);
        // }


        // // SkipEol
        // [Test]
        // public void SkipEol_ReturnFalse_WhenSourceContainOnlyCarriageReturn()
        // {
        //     var source = new FilteredSource("foo\r");
        //     using var reader = new Reader(source);

        //     Assert.That(reader.SkipEol(), Is.False);
        //     Assert.That(source.ToString(), Is.EqualTo("foo\r"));
        // }

        // [Test]
        // public void SkipEol_ReturnTrue_WhenSourceIsEmpty()
        // {
        //     using var reader = new Reader(new FilteredSource(string.Empty));

        //     Assert.That(reader.SkipEol(), Is.True);
        // }

        // [TestCaseSource(nameof(EndOfLineCases))]
        // public void SkipEol_ReturnRestOfTheSource(string eol)
        // {
        //     var source = new FilteredSource($"{eol}foo");
        //     using var reader = new Reader(source);

        //     Assert.That(reader.SkipEol(), Is.True);
        //     Assert.That(source.ToString(), Is.EqualTo("foo"));
        // }

        // [TestCaseSource(nameof(EndOfLineCases))]
        // public void SkipEol_IsCrossPlatform(string eol)
        // {
        //     var source = new FilteredSource(eol);
        //     using var reader = new Reader(source);

        //     Assert.That(reader.SkipEol(), Is.True);
        //     Assert.That(source.Eof(), Is.True);
        // }

        // [TestCaseSource(nameof(EndOfLineCases))]
        // public void SkipEol_ReturnFalse_WhenSourceDoesNotContainEol(string eol)
        // {
        //     var source = new FilteredSource($"foo{eol}");
        //     using var reader = new Reader(source);

        //     Assert.That(reader.SkipEol(), Is.False);
        //     Assert.That(source.ToString(), Is.EqualTo($"foo{eol}"));
        // }

        // static IEnumerable<string> EndOfLineCases()
        // {
        //     yield return "\r\n";
        //     yield return "\n\r";
        //     yield return "\n";
        // }
    }
}
