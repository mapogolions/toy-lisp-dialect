using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalBeginTests
    {
        private IEnv _env;
        private Evaluator _evaluator;

        [SetUp]
        public void BeforeEach()
        {
            _env = new Env();
            _evaluator = new Evaluator(_env);
        }

        [Test]
        public void EvalBegin_ReturnLastEvaluatedValue()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Begin, ClBool.False, ClBool.True);

            Assert.That(_evaluator.EvalBegin(expr), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void EvalBegin_ReturnNil_WhenTailIsEmptyList()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Begin);

            Assert.That(_evaluator.EvalBegin(expr), Is.EqualTo(Nil.Given));
        }

        [Test]
        public void EvalBegin_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var expr = BuiltIn.ListOf(ClSymbol.If);

            Assert.That(_evaluator.EvalBegin(expr), Is.Null);
        }
    }
}
