using System.Collections.Generic;
using Cl.Abs;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalTests
    {
        [Test]
        public void Eval_Variable_ThrowUnboundException_WhenBindingIsMissed()
        {
            var evaluator = new Evaluator(new Env());

            Assert.That(() => evaluator.Eval(new ClSymbol("a")),
                Throws.InvalidOperationException.With.Message.EqualTo("Unbound variable"));
        }

        [Test]
        public void Eval_Variable_ReturnBinding_WhenEnvironmentContainsVariable()
        {
            var env = new Env();
            env.Bind(new ClSymbol("a"), ClBool.False);
            var evaluator = new Evaluator(env);

            Assert.That(evaluator.Eval(new ClSymbol("a")), Is.EqualTo(ClBool.False));
        }

        [Test]
        [TestCaseSource(nameof(SelfEvaluatingTestCases))]
        public void Eval_SelfEvaluatingExpression_ReturnTheSameExpression(IClObj expr)
        {
            var evaluator = new Evaluator(new Env());

            Assert.That(evaluator.Eval(expr), Is.EqualTo(expr));
        }

        static IEnumerable<IClObj> SelfEvaluatingTestCases()
        {
            yield return ClBool.True;
            yield return ClBool.False;
            yield return new ClFixnum(10);
            yield return new ClFloat(10.2);
            yield return new ClChar('a');
            // TODO: '(1 2 3) quoted case
        }
    }
}
