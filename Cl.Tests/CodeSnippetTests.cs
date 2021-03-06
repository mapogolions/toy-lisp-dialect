using Cl.Extensions;
using Cl.Tests.TestDataSources;
using NUnit.Framework;

namespace Cl.Tests
{
    public class CodeSnippetTests
    {
        [Test]
        [TestCaseSource(typeof(CodeSnippetsDataSource))]
        public void SnippetsTest(string snippet, string expected)
        {
            using var reader = new Reader(snippet);
            var (actual, _) = BuiltIn.Eval(reader.Read());
            Assert.That(actual.ToString(), Is.EqualTo(expected.ToString()));
        }
    }
}
