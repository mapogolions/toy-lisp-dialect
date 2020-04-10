using System;
using System.Collections.Generic;
using Cl.Abs;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalTests
    {
        [Test]
        [TestCaseSource(nameof(DoesNotCreateNewScopeTestCases))]
        public void Eval_DoesNotCreateNewScope(Func<ClCell, ClCell> expr)
        {
            var env = new Env();
            var evaluator = new Evaluator(env);
            var a = new ClSymbol("a");
            var value = new ClString("foo");
            var define = BuiltIn.ListOf(ClSymbol.Define, a, value);

            Ignore(evaluator.Eval(expr.Invoke(define)));

            Assert.True(env.AtTheTopLevel);
            Assert.That(env.Lookup(a), Is.EqualTo(value));
        }

        static IEnumerable<Func<ClCell, ClCell>> DoesNotCreateNewScopeTestCases()
        {
            yield return new Func<ClCell, ClCell>(it => BuiltIn.ListOf(ClSymbol.IfThenElse, it, ClBool.True, ClBool.False));
            yield return new Func<ClCell, ClCell>(it => BuiltIn.ListOf(ClSymbol.And, it, ClBool.False));
            yield return new Func<ClCell, ClCell>(it => BuiltIn.ListOf(ClSymbol.Or, it, ClBool.True));
        }

        [Test]
        public void Eval_ReturnTrue_WhenEachLogicExpressionIsTrue()
        {
            var evaluator = new Evaluator(new Env());
            var ifThenElse = BuiltIn.ListOf(ClSymbol.IfThenElse, ClBool.True, ClBool.True);
            var logicOr = BuiltIn.ListOf(ClSymbol.Or, ClBool.False, ClBool.True);

            var logicAnd = BuiltIn.ListOf(ClSymbol.And, ClBool.True);
            var expr = BuiltIn.ListOf(ClSymbol.And, ifThenElse, logicOr, logicAnd);

            Assert.That(evaluator.Eval(expr), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Eval_ReturnUnreducedList_WhenItemOfListHasComplicatedForm()
        {
            var evaluator = new Evaluator(new Env());
            var begin = BuiltIn.ListOf(ClSymbol.Begin, ClBool.True, new ClString("foo"), new ClString("bar"));
            var logicAnd = BuiltIn.ListOf(ClSymbol.And, ClBool.True, new ClFixnum(1));
            var expr = BuiltIn.Quote(BuiltIn.ListOf(begin, logicAnd));

            var result = evaluator.Eval(expr);

            Assert.That(BuiltIn.First(result), Is.EqualTo(begin));
            Assert.That(BuiltIn.Second(result), Is.EqualTo(logicAnd));
        }

        [Test]
        public void Eval_ReturnUnreducedExpression_WhenItIsQuoted()
        {
            var evaluator = new Evaluator(new Env());
            var cell = new ClCell(new ClFixnum(1), new ClFixnum(2));
            var expr = BuiltIn.Quote(cell);

            Assert.That(Object.ReferenceEquals(evaluator.Eval(expr), cell), Is.True);
        }

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
        }
    }
}
