using Cl.Abs;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalCondTest
    {
        [Test]
        public void EvalCond_ReturnNil_WhenEachClausePredicateIsFalse()
        {
            /*
                (cond
                  (#f #\a)
                  (nil #\b))
             */
            var evaluator = new Evaluator(new Env());
            var clause1 = BuiltIn.ListOf(ClBool.False, new ClChar('a'));
            var clause2 = BuiltIn.ListOf(Nil.Given, new ClChar('b'));
            var expr = BuiltIn.ListOf(ClSymbol.Cond, clause1, clause2);

            Assert.That(evaluator.Eval(expr), Is.EqualTo(ClBool.False));
        }

        [Test]
        public void EvalCond_ReturnLastEvaluatedResult_WhenOnlyElseClauseIsProvided()
        {
            /*
                (cond
                  (else \#a \#b))
                as
                (cond . ((else . (\#a . (\#b . nil))) . nil))
             */
            var evaluator = new Evaluator(new Env());
            var elseClause = BuiltIn.ListOf(ClSymbol.Else, new ClChar('a'), new ClChar('b'), new ClChar('c'));
            var expr = BuiltIn.ListOf(ClSymbol.Cond, elseClause);

           Assert.That(evaluator.Eval(expr), Is.EqualTo(new ClChar('c')));
        }

        [Test]
        public void EvalCond_ReturnResultOfElseClause_WhenItIsSingle()
        {
            var evaluator = new Evaluator(new Env());
            var result = new ClFixnum(10);
            var elseClause = BuiltIn.ListOf(ClSymbol.Else, result);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, elseClause);

            Assert.That(evaluator.Eval(expr), Is.EqualTo(result));
        }

        [Test]
        public void EvalCond_ThrowException_WhenElseClauseIsNotLast()
        {
            var evaluator = new Evaluator(new Env());
            var elseClause = BuiltIn.ListOf(ClSymbol.Else, ClBool.True);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, elseClause, BuiltIn.ListOf(ClSymbol.Else, ClBool.True));

            Assert.That(() => evaluator.Eval(expr),
                Throws.InvalidOperationException.With.Message.EqualTo("Else clause is not last condition"));
        }

        [Test]
        public void EvalCond_ReturnFalse_WhenParamsAreMissed()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Cond);

            Assert.That(evaluator.Eval(expr), Is.EqualTo(ClBool.False));
        }
    }
}
