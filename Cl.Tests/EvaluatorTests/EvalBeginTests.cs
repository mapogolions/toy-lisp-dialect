using Cl.Abs;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalBeginTests
    {
        [Test]
        public void TryEvalBegin_ReturnLastEvaluatedValue()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Begin, ClBool.False, ClBool.True);

            Ignore(evaluator.TryEvalBegin(expr, out var obj));

            Assert.That(obj, Is.EqualTo(ClBool.True));
        }

        [Test]
        public void TryEvalBegin_ReturnNil_WhenTailIsEmptyList()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Begin); // (begin) -> (begin . nil)

            Ignore(evaluator.TryEvalBegin(expr, out var obj));

            Assert.That(obj, Is.EqualTo(Nil.Given));
        }

        [Test]
        public void TryEvalBegin_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.IfThenElse);

            Assert.That(evaluator.TryEvalBegin(expr, out var _), Is.False);
        }
    }
}
