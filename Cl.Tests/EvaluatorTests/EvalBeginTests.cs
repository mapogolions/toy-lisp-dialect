using Cl.Contracts;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalBeginTests
    {
        private IEnv _env;
        private IContext _context;

        [SetUp]
        public void BeforeEach()
        {
            _env = new Env();
            _context = new Context(_env);
        }

        [Test]
        public void EvalBegin_ReturnLastEvaluatedValue()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Begin, ClBool.False, ClBool.True, Value.One);
            var context = expr.Reduce(_context);
            Assert.That(context.Value, Is.EqualTo(Value.One));
        }

        [Test]
        public void EvalBegin_ReturnNil_WhenTailIsEmptyList()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Begin);
            var context = expr.Reduce(_context);
            Assert.That(context.Value, Is.EqualTo(Nil.Given));
        }
    }
}
