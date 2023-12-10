using Cl.Extensions;
using Cl.Readers;
using Cl.IO;
using Cl.Tests.TestDataSources;

namespace Cl.Tests
{
    public class EqualityComparisonTests
    {
        [Test]
        [TestCaseSource(typeof(EqualityComparisonDataSource))]
        public void ShouldDoEqualityComparison(string snippet, string expected)
        {
            using var source = new Source(snippet);
            var reader = new Reader();
            var (actual, _) = BuiltIn.Eval(reader.Read(source));
            Assert.That(actual.ToString(), Is.EqualTo(expected));
        }
    }
}
