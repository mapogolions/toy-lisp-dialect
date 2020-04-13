using Cl.Abs;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalBeginTests
    {
        [Test]
        public void EvalBegin_ReturnLastEvaluatedValue()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Begin, ClBool.False, ClBool.True);

            Assert.That(evaluator.EvalBegin(expr), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void EvalBegin_ReturnNil_WhenTailIsEmptyList()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Begin); // (begin) -> (begin . nil)

            Assert.That(evaluator.EvalBegin(expr), Is.EqualTo(Nil.Given));
        }

        [Test]
        public void EvalBegin_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.If);

            Assert.That(evaluator.EvalBegin(expr), Is.Null);
        }
    }
}
