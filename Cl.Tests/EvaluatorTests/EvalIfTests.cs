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
        [Test]
        public void EvalIf_EvalOnlyElseBranch_WhenConditionIsFalse()
        {
            var env = new Env();
            var evaluator = new Evaluator(env);
            Func<ClSymbol, ClCell> spawnBranch = it => BuiltIn.ListOf(ClSymbol.Define, it, new ClFixnum(1));
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.False, spawnBranch(new ClSymbol("a")), spawnBranch(new ClSymbol("b")));

            Ignore(evaluator.EvalIf(expr));

            Assert.That(env.Lookup(new ClSymbol("b")), Is.EqualTo(new ClFixnum(1)));
            Assert.That(() => env.Lookup(new ClSymbol("a")),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void EvalIf_EvalOnlyThenBranch_WhenConditionIsTrue()
        {
            var env = new Env();
            var evaluator = new Evaluator(env);
            Func<ClSymbol, ClCell> spawnBranch = it => BuiltIn.ListOf(ClSymbol.Define, it, new ClFixnum(1));
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.True, spawnBranch(new ClSymbol("a")), spawnBranch(new ClSymbol("b")));

            Ignore(evaluator.EvalIf(expr));

            Assert.That(env.Lookup(new ClSymbol("a")), Is.EqualTo(new ClFixnum(1)));
            Assert.That(() => env.Lookup(new ClSymbol("b")),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        [TestCaseSource(nameof(FalsyTestCases))]
        public void EvalIf_EvalElseBranch_WhenConditionIsFalse(IClObj predicate)
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.If, predicate, ClBool.False, ClBool.True);

            Assert.That(evaluator.EvalIf(expr), Is.EqualTo(ClBool.True));
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
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.If, predicate, ClBool.True, ClBool.False);

            Assert.That(evaluator.Eval(expr), Is.EqualTo(ClBool.True));
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
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.False, new ClFixnum(1));

            Assert.That(evaluator.Eval(expr), Is.EqualTo(Nil.Given));
        }

        [Test]
        public void EvalIf_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Cond);

            Assert.That(evaluator.EvalIf(expr), Is.Null);
        }
    }
}
