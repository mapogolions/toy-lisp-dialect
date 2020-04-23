using System;
using System.Collections.Generic;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalTests
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
        public void Eval_IdentifiersAndKeywordsCoexistIndependently()
        {
            _env.Bind(ClSymbol.And, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, Value.Foo);

            Assert.That(_evaluator.Eval(ClSymbol.And), Is.EqualTo(Value.One));
            Assert.That(_evaluator.Eval(expr), Is.EqualTo(Value.Foo));
        }

        [Test]
        [TestCaseSource(nameof(DoesNotCreateNewScopeTestCases))]
        public void Eval_DoesNotCreateNewScope(Func<ClCell, ClCell> expr)
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.Foo);

            Ignore(_evaluator.Eval(expr.Invoke(define)));

            Assert.That(_env.Lookup(Var.Foo), Is.EqualTo(Value.Foo));
        }

        static IEnumerable<Func<ClCell, ClCell>> DoesNotCreateNewScopeTestCases()
        {
            yield return new Func<ClCell, ClCell>(it => BuiltIn.ListOf(ClSymbol.If, it, ClBool.True, ClBool.False));
            yield return new Func<ClCell, ClCell>(it => BuiltIn.ListOf(ClSymbol.And, it, ClBool.False));
            yield return new Func<ClCell, ClCell>(it => BuiltIn.ListOf(ClSymbol.Or, it, ClBool.True));
        }

        [Test]
        public void Eval_ReturnTrue_WhenEachLogicExpressionIsTrue()
        {
            var ifThenElse = BuiltIn.ListOf(ClSymbol.If, ClBool.True, ClBool.True);
            var logicOr = BuiltIn.ListOf(ClSymbol.Or, ClBool.False, ClBool.True);

            var logicAnd = BuiltIn.ListOf(ClSymbol.And, ClBool.True);
            var expr = BuiltIn.ListOf(ClSymbol.And, ifThenElse, logicOr, logicAnd);

            Assert.That(_evaluator.Eval(expr), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Eval_ReturnUnreducedList_WhenItemOfListHasComplicatedForm()
        {
            var begin = BuiltIn.ListOf(ClSymbol.Begin, ClBool.True, Value.Foo, Value.Bar);
            var logicAnd = BuiltIn.ListOf(ClSymbol.And, ClBool.True, Value.One);
            var expr = BuiltIn.Quote(BuiltIn.ListOf(begin, logicAnd));

            var actual = _evaluator.Eval(expr);

            Assert.That(BuiltIn.First(actual), Is.EqualTo(begin));
            Assert.That(BuiltIn.Second(actual), Is.EqualTo(logicAnd));
        }

        [Test]
        public void Eval_ReturnUnreducedExpression_WhenItIsQuoted()
        {
            var cell = new ClCell(new ClFixnum(1), new ClFixnum(2));
            var expr = BuiltIn.Quote(cell);

            Assert.That(Object.ReferenceEquals(_evaluator.Eval(expr), cell), Is.True);
        }

        [Test]
        public void Eval_Variable_ThrowUnboundException_WhenBindingIsMissed()
        {
            Assert.That(() => _evaluator.Eval(Var.Foo),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void Eval_Variable_ReturnBinding_WhenEnvironmentContainsVariable()
        {
            _env.Bind(Var.Foo, ClBool.False);

            Assert.That(_evaluator.Eval(Var.Foo), Is.EqualTo(ClBool.False));
        }

        [Test]
        [TestCaseSource(nameof(SelfEvaluatingTestCases))]
        public void Eval_SelfEvaluatingExpression_ReturnTheSameExpression(IClObj expr)
        {
            Assert.That(_evaluator.Eval(expr), Is.EqualTo(expr));
        }

        static IEnumerable<IClObj> SelfEvaluatingTestCases()
        {
            yield return ClBool.True;
            yield return ClBool.False;
            yield return new ClFixnum(10);
            yield return new ClFloat(10.2);
            yield return new ClChar('a');
        }
    }
}
