using Cl.Extensions;
using Cl.Types;
using Cl.Tests.TestDataSources;
using NUnit.Framework;

namespace Cl.Tests
{
    public class CodeSnippetTests
    {
        [Test]
        [TestCaseSource(typeof(CodeSnippetsDataSource))]
        public void SnippetsTest(string snippet, ClObj expected)
        {
            using var reader = new Reader(snippet);
            var (actual, _) = BuiltIn.Eval(reader.Read());
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
