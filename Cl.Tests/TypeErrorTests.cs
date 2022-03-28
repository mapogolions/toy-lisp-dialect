using Cl.Errors;
using NUnit.Framework;
using Cl.Tests.TestDataSources;
using Cl.IO;
using Cl.Readers;

namespace Cl.Tests
{
    public class TypeErrorTests
    {
        [Test]
        [TestCaseSource(typeof(TypeErrorDataSource))]
        public void ShouldThrowTypeErrorException(string snippet, string errorMessage)
        {
            using var source = new Source(snippet);
            var reader = new Reader();
            var obj = reader.Read(source);
            Assert.That(() => BuiltIn.Eval(obj),
                Throws.Exception.TypeOf<TypeError>().With.Message.EqualTo(errorMessage));

        }
    }
}
