using System;
using System.Collections.Generic;
using Cl;
using Cl.Errors;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalTests
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
        public void Eval_IdentifiersAndKeywordsCoexistIndependently()
        {
            _env.Bind(ClSymbol.And, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, Value.Foo);

            Assert.That(ClSymbol.And.Reduce(_ctx).Value, Is.EqualTo(Value.One));
            Assert.That(expr.Reduce(_ctx).Value, Is.EqualTo(Value.Foo));
        }

        [Test]
        [TestCaseSource(nameof(DoesNotCreateNewScopeTestCases))]
        public void Eval_DoesNotCreateNewScope(Func<ClCell, ClCell> expr)
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.Foo);
            var ctx = expr.Invoke(define).Reduce(_ctx);
            Assert.That(ctx.Env.Lookup(Var.Foo), Is.EqualTo(Value.Foo));
        }

        static IEnumerable<Func<ClCell, ClCell>> DoesNotCreateNewScopeTestCases()
        {
            yield return new Func<ClCell, ClCell>(x => BuiltIn.ListOf(ClSymbol.If, x, ClBool.True, ClBool.False));
            yield return new Func<ClCell, ClCell>(x => BuiltIn.ListOf(ClSymbol.And, x, ClBool.False));
            yield return new Func<ClCell, ClCell>(x => BuiltIn.ListOf(ClSymbol.Or, x, ClBool.True));
        }

        [Test]
        public void Eval_ReturnTrue_WhenEachLogicExpressionIsTrue()
        {
            var ifThenElse = BuiltIn.ListOf(ClSymbol.If, ClBool.True, ClBool.True);
            var logicOr = BuiltIn.ListOf(ClSymbol.Or, ClBool.False, ClBool.True);
            var logicAnd = BuiltIn.ListOf(ClSymbol.And, ClBool.True);
            var expr = BuiltIn.ListOf(ClSymbol.And, ifThenElse, logicOr, logicAnd);

            var ctx = expr.Reduce(_ctx);

            Assert.That(ctx.Value, Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Eval_ReturnUnreducedList_WhenItemOfListHasComplicatedForm()
        {
            var begin = BuiltIn.ListOf(ClSymbol.Begin, ClBool.True, Value.Foo, Value.Bar);
            var logicAnd = BuiltIn.ListOf(ClSymbol.And, ClBool.True, Value.One);
            var expr = BuiltIn.Quote(BuiltIn.ListOf(begin, logicAnd));

            var ctx = expr.Reduce(_ctx);

            Assert.That(BuiltIn.First(ctx.Value), Is.EqualTo(begin));
            Assert.That(BuiltIn.Second(ctx.Value), Is.EqualTo(logicAnd));
        }

        [Test]
        public void Eval_ReturnUnreducedExpression_WhenItIsQuoted()
        {
            var cell = new ClCell(new ClInt(1), new ClInt(2));
            var expr = BuiltIn.Quote(cell);

            var ctx = expr.Reduce(_ctx);

            Assert.That(Object.ReferenceEquals(ctx.Value, cell), Is.True);
        }

        [Test]
        public void Eval_Variable_ThrowUnboundException_WhenBindingIsMissed()
        {
            Assert.That(() => Var.Foo.Reduce(_ctx),
                Throws.Exception.TypeOf<UnboundVariableError>().With.Message.EqualTo("Unbound variable foo"));
        }

        [Test]
        public void Eval_Variable_ReturnBinding_WhenEnvironmentContainsVariable()
        {
            _env.Bind(Var.Foo, Value.One);
            var ctx = Var.Foo.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(Value.One));
        }

        [Test]
        [TestCaseSource(nameof(SelfEvaluatingTestCases))]
        public void Eval_SelfEvaluatingExpression(ClObj expr)
        {
            var ctx = expr.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(expr));
        }

        static IEnumerable<ClObj> SelfEvaluatingTestCases()
        {
            yield return ClBool.True;
            yield return ClBool.False;
            yield return new ClInt(10);
            yield return new ClDouble(10.2);
            yield return new ClChar('a');
        }
    }
}
