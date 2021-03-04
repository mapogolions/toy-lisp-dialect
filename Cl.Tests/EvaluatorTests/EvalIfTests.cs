using System;
using System.Collections.Generic;
using Cl.Contracts;
using Cl.Errors;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalIfTests
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
        public void EvalIf_EvalOnlyElseBranch_WhenConditionIsFalse()
        {
            Func<ClSymbol, ClCell> defineVar = it => BuiltIn.ListOf(ClSymbol.Define, it, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.False, defineVar(Var.Foo), defineVar(Var.Bar));

            var ctx = expr.Reduce(_ctx);

            Assert.That(ctx.Env.Lookup(Var.Bar), Is.EqualTo(Value.One));
            Assert.That(() => ctx.Env.Lookup(Var.Foo),
                Throws.Exception.TypeOf<UnboundVariableError>().With.Message.EqualTo("Unbound variable foo"));
        }

        [Test]
        public void EvalIf_EvalOnlyThenBranch_WhenConditionIsTrue()
        {
            Func<ClSymbol, ClCell> defineVar = it => BuiltIn.ListOf(ClSymbol.Define, it, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.True, defineVar(Var.Foo), defineVar(Var.Bar));

            var ctx = expr.Reduce(_ctx);

            Assert.That(ctx.Env.Lookup(Var.Foo), Is.EqualTo(Value.One));
            Assert.That(() => ctx.Env.Lookup(Var.Bar),
                Throws.Exception.TypeOf<UnboundVariableError>().With.Message.EqualTo("Unbound variable bar"));
        }

        [Test]
        [TestCaseSource(nameof(FalsyTestCases))]
        public void EvalIf_EvalElseBranch_WhenConditionIsFalse(ClObj predicate)
        {
            var expr = BuiltIn.ListOf(ClSymbol.If, predicate, ClBool.False, Value.One);
            var ctx = expr.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(Value.One));
        }

        static IEnumerable<ClObj> FalsyTestCases()
        {
            yield return ClBool.False;
            yield return ClCell.Nil;
        }

        [Test]
        [TestCaseSource(nameof(TruthyTestCases))]
        public void EvalIf_EvalThenBranch_WhenConditionIsTrue(ClObj predicate)
        {
            var expr = BuiltIn.ListOf(ClSymbol.If, predicate, Value.One, ClBool.False);
            var ctx = expr.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(Value.One));
        }

        static IEnumerable<ClObj> TruthyTestCases()
        {
            yield return new ClInt(0);
            yield return new ClDouble(0.0);
            yield return new ClString(string.Empty);
            yield return ClBool.True;
            yield return new ClChar('\0');
            yield return BuiltIn.Quote(BuiltIn.ListOf(ClBool.False)); // (quote . (false . nil))
            yield return BuiltIn.Quote(BuiltIn.ListOf(ClCell.Nil)); // (quote . (nil . nil))
        }

        [Test]
        public void EvalIf_ReturnNil_WhenConditionIsFalseAndElseBranchIsMissed()
        {
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.False, Value.One);
            var ctx = expr.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(ClCell.Nil));
        }
    }
}
