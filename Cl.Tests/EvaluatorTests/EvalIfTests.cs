using System;
using System.Collections.Generic;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalIfTests
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
        public void EvalIf_EvalOnlyElseBranch_WhenConditionIsFalse()
        {
            Func<ClSymbol, ClCell> spawnBranch = it => BuiltIn.ListOf(ClSymbol.Define, it, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.False, spawnBranch(Var.Foo), spawnBranch(Var.Bar));

            Ignore(_evaluator.EvalIf(expr));

            Assert.That(_env.Lookup(Var.Bar), Is.EqualTo(Value.One));
            Assert.That(() => _env.Lookup(Var.Foo),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void EvalIf_EvalOnlyThenBranch_WhenConditionIsTrue()
        {
            Func<ClSymbol, ClCell> spawnBranch = it => BuiltIn.ListOf(ClSymbol.Define, it, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.True, spawnBranch(Var.Foo), spawnBranch(Var.Bar));

            Ignore(_evaluator.EvalIf(expr));

            Assert.That(_env.Lookup(Var.Foo), Is.EqualTo(Value.One));
            Assert.That(() => _env.Lookup(Var.Bar),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        [TestCaseSource(nameof(FalsyTestCases))]
        public void EvalIf_EvalElseBranch_WhenConditionIsFalse(IClObj predicate)
        {
            var expr = BuiltIn.ListOf(ClSymbol.If, predicate, ClBool.False, ClBool.True);

            Assert.That(_evaluator.EvalIf(expr), Is.EqualTo(ClBool.True));
        }

        static IEnumerable<IClObj> FalsyTestCases()
        {
            yield return ClBool.False;
            yield return Nil.Given;
        }

        [Test]
        [TestCaseSource(nameof(TruthyTestCases))]
        public void EvalIf_EvalThenBranch_WhenConditionIsTrue(IClObj predicate)
        {
            var expr = BuiltIn.ListOf(ClSymbol.If, predicate, ClBool.True, ClBool.False);

            Assert.That(_evaluator.Eval(expr), Is.EqualTo(ClBool.True));
        }

        static IEnumerable<IClObj> TruthyTestCases()
        {
            yield return new ClFixnum(0);
            yield return new ClFloat(0.0);
            yield return new ClString(string.Empty);
            yield return ClBool.True;
            yield return new ClChar('\0');
            yield return BuiltIn.Quote(BuiltIn.ListOf(ClBool.False)); // (quote . (false . nil))
            yield return BuiltIn.Quote(BuiltIn.ListOf(Nil.Given)); // (quote . (nil . nil))
        }

        [Test]
        public void EvalIf_ReturnNil_WhenConditionIsFalseAndElseBranchIsSkipped()
        {
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.False, Value.One);

            Assert.That(_evaluator.Eval(expr), Is.EqualTo(Nil.Given));
        }

        [Test]
        public void EvalIf_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Cond);

            Assert.That(_evaluator.EvalIf(expr), Is.Null);
        }
    }
}
