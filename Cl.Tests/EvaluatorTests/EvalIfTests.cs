using System;
using System.Collections.Generic;
using Cl.Contracts;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalIfTests
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
        public void EvalIf_EvalOnlyElseBranch_WhenConditionIsFalse()
        {
            Func<ClSymbol, ClCell> defineVar = it => BuiltIn.ListOf(ClSymbol.Define, it, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.False, defineVar(Var.Foo), defineVar(Var.Bar));

            var context = expr.Reduce(_context);

            Assert.That(context.Env.Lookup(Var.Bar), Is.EqualTo(Value.One));
            Assert.That(() => context.Env.Lookup(Var.Foo),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void EvalIf_EvalOnlyThenBranch_WhenConditionIsTrue()
        {
            Func<ClSymbol, ClCell> defineVar = it => BuiltIn.ListOf(ClSymbol.Define, it, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.True, defineVar(Var.Foo), defineVar(Var.Bar));

            var context = expr.Reduce(_context);

            Assert.That(context.Env.Lookup(Var.Foo), Is.EqualTo(Value.One));
            Assert.That(() => context.Env.Lookup(Var.Bar),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        [TestCaseSource(nameof(FalsyTestCases))]
        public void EvalIf_EvalElseBranch_WhenConditionIsFalse(IClObj predicate)
        {
            var expr = BuiltIn.ListOf(ClSymbol.If, predicate, ClBool.False, Value.One);
            var context = expr.Reduce(_context);
            Assert.That(context.Value, Is.EqualTo(Value.One));
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
            var expr = BuiltIn.ListOf(ClSymbol.If, predicate, Value.One, ClBool.False);
            var context = expr.Reduce(_context);
            Assert.That(context.Value, Is.EqualTo(Value.One));
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
        public void EvalIf_ReturnNil_WhenConditionIsFalseAndElseBranchIsMissed()
        {
            var expr = BuiltIn.ListOf(ClSymbol.If, ClBool.False, Value.One);
            var context = expr.Reduce(_context);
            Assert.That(context.Value, Is.EqualTo(Nil.Given));
        }
    }
}
