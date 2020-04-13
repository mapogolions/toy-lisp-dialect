using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalCondTest
    {
        [Test]
        public void EvalCond_ThrowException_WhenAtLeastOneClauseIsNoCell()
        {
            var evaluator = new Evaluator(new Env());
            var invalidExpr = BuiltIn.ListOf(ClSymbol.Cond,
                BuiltIn.ListOf(ClBool.False, new ClFixnum(1)),
                BuiltIn.ListOf(ClBool.True, new ClFixnum(2)),
                new ClFixnum(3));
            var errorMessage = Errors.BuiltIn.ClauseMustBeCell;

            Assert.That(() => evaluator.Eval(invalidExpr),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void EvalCond_MustBeLazy()
        {
            var env = new Env();
            var evaluator = new Evaluator(env);
            var define = BuiltIn.ListOf(ClSymbol.Define, new ClSymbol("a"), new ClFixnum(2));
            var truthy = BuiltIn.ListOf(ClBool.True, new ClString("foo"));
            var expr = BuiltIn.ListOf(ClSymbol.Cond, truthy, define);

            Assert.That(evaluator.Eval(expr), Is.EqualTo(new ClString("foo")));
            Assert.That(() => env.Lookup(new ClSymbol("a")),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void EvalCond_ReturnNil_WhenEachClausePredicateIsFalse()
        {
            var evaluator = new Evaluator(new Env());
            var clause1 = BuiltIn.ListOf(ClBool.False, new ClChar('a'));
            var clause2 = BuiltIn.ListOf(Nil.Given, new ClChar('b'));
            var expr = BuiltIn.ListOf(ClSymbol.Cond, clause1, clause2);

            Assert.That(evaluator.Eval(expr), Is.EqualTo(ClBool.False));
        }

        [Test]
        public void EvalCond_ReturnLastEvaluatedResult_WhenOnlyElseClauseIsProvided()
        {
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
            var errorMessage = Errors.BuiltIn.ElseClauseMustBeLast;

            Assert.That(() => evaluator.Eval(expr),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
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
