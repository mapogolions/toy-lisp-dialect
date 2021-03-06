using Cl.Errors;
using NUnit.Framework;
using Cl.Tests.TestDataSources;

namespace Cl.Tests
{
    public class TypeErrorTests
    {
        [Test]
        [TestCaseSource(typeof(TypeErrorDataSource))]
        public void ShouldThrowTypeErrorException(string snippet, string errorMessage)
        {
            using var reader = new Reader(snippet);
            var ast = reader.Read();
            Assert.That(() => BuiltIn.Eval(ast, new Context(BuiltIn.Env)),
                Throws.Exception.TypeOf<TypeError>().With.Message.EqualTo(errorMessage));

        }
    }
}
