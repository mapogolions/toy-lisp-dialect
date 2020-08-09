using Cl.Contracts;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalCondTest
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
        public void EvalCond_ThrowException_WhenAtLeastOneClauseIsNoCell()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Cond,
                BuiltIn.ListOf(ClBool.False, Value.Foo),
                BuiltIn.ListOf(ClBool.True, Value.Bar),
                Value.One);
            var errorMessage = Errors.BuiltIn.ClauseMustBeCell;

            Assert.That(() => expr.Reduce(_context),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void EvalCond_MustBeLazy()
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.Foo);
            var truthy = BuiltIn.ListOf(ClBool.True, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, truthy, define);

            var context = expr.Reduce(_context);

            Assert.That(context.Result, Is.EqualTo(Value.One));
            Assert.That(() => context.Env.Lookup(Var.Foo),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void EvalCond_ReturnNil_WhenEachClausePredicateIsFalse()
        {
            var clause1 = BuiltIn.ListOf(ClBool.False, Value.Foo);
            var clause2 = BuiltIn.ListOf(Nil.Given, Value.Bar);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, clause1, clause2);

            var context = expr.Reduce(_context);

            Assert.That(context.Result, Is.EqualTo(ClBool.False));
        }

        [Test]
        public void EvalCond_ReturnLastEvaluatedResult_WhenOnlyElseClauseIsProvided()
        {
            var elseClause = BuiltIn.ListOf(ClSymbol.Else, Value.Foo, Value.Bar, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, elseClause);

            var context = expr.Reduce(_context);

           Assert.That(context.Result, Is.EqualTo(Value.One));
        }

        [Test]
        public void EvalCond_ReturnResultOfElseClause()
        {
            var elseClause = BuiltIn.ListOf(ClSymbol.Else, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, elseClause);

            var context = expr.Reduce(_context);

            Assert.That(context.Result, Is.EqualTo(Value.One));
        }

        [Test]
        public void EvalCond_ThrowException_WhenElseClauseIsNotLast()
        {
            var elseClause = BuiltIn.ListOf(ClSymbol.Else, ClBool.True);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, elseClause, BuiltIn.ListOf(ClSymbol.Else, ClBool.True));
            var errorMessage = Errors.BuiltIn.ElseClauseMustBeLast;

            Assert.That(() => expr.Reduce(_context),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void EvalCond_ReturnFalse_WhenClausesAreMissed()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Cond);
            var context = expr.Reduce(_context);
            Assert.That(context.Result, Is.EqualTo(ClBool.False));
        }
    }
}
