using Cl.Extensions;
using Cl.Readers;
using Cl.IO;
using Cl.Tests.TestDataSources;

namespace Cl.Tests
{
    public class StdLibTests
    {
        [Test]
        [TestCaseSource(typeof(StdLibDataSource))]
        public void Test(string snippet, string expected)
        {
            using var source = new Source(snippet);
            var reader = new Reader();
            var (actual, _) = BuiltIn.Eval(_ctx, reader.Read(source));
            Assert.That(actual.ToString(), Is.EqualTo(expected));
        }

        private static IContext _ctx = BuiltIn.StdLib();
    }
}
