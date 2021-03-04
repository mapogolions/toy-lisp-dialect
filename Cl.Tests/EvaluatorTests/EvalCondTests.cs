using Cl.Contracts;
using Cl.Errors;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalCondTest
    {
        private IEnv _env;
        private IContext _ctx;

        [SetUp]
        public void BeforeEach()
        {
            _env = new Env();
            _ctx = new Context(_env);
        }

        [Test]
        public void EvalCond_ThrowException_WhenAtLeastOneClauseIsNoCell()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Cond,
                BuiltIn.ListOf(ClBool.False, Value.Foo),
                BuiltIn.ListOf(ClBool.True, Value.Bar),
                Value.One);
            var errorMessage = "Clause must be a cell";

            Assert.That(() => expr.Reduce(_ctx),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void EvalCond_MustBeLazy()
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.Foo);
            var truthy = BuiltIn.ListOf(ClBool.True, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, truthy, define);

            var ctx = expr.Reduce(_ctx);

            Assert.That(ctx.Value, Is.EqualTo(Value.One));
            Assert.That(() => ctx.Env.Lookup(Var.Foo),
                Throws.Exception.TypeOf<UnboundVariableError>().With.Message.EqualTo("Unbound variable foo"));
        }

        [Test]
        public void EvalCond_ReturnNil_WhenEachClausePredicateIsFalse()
        {
            var clause1 = BuiltIn.ListOf(ClBool.False, Value.Foo);
            var clause2 = BuiltIn.ListOf(ClCell.Nil, Value.Bar);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, clause1, clause2);

            var ctx = expr.Reduce(_ctx);

            Assert.That(ctx.Value, Is.EqualTo(ClBool.False));
        }

        [Test]
        public void EvalCond_ReturnLastEvaluatedResult_WhenOnlyElseClauseIsProvided()
        {
            var elseClause = BuiltIn.ListOf(ClSymbol.Else, Value.Foo, Value.Bar, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, elseClause);

            var ctx = expr.Reduce(_ctx);

            Assert.That(ctx.Value, Is.EqualTo(Value.One));
        }

        [Test]
        public void EvalCond_ReturnResultOfElseClause()
        {
            var elseClause = BuiltIn.ListOf(ClSymbol.Else, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, elseClause);

            var ctx = expr.Reduce(_ctx);

            Assert.That(ctx.Value, Is.EqualTo(Value.One));
        }

        [Test]
        public void EvalCond_ThrowException_WhenElseClauseIsNotLast()
        {
            var elseClause = BuiltIn.ListOf(ClSymbol.Else, ClBool.True);
            var expr = BuiltIn.ListOf(ClSymbol.Cond, elseClause, BuiltIn.ListOf(ClSymbol.Else, ClBool.True));
            var errorMessage = "Else clause must be last condition";

            Assert.That(() => expr.Reduce(_ctx),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void EvalCond_ReturnFalse_WhenClausesAreMissed()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Cond);
            var ctx = expr.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(ClBool.False));
        }
    }
}
