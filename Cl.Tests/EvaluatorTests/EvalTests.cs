using System;
using System.Collections.Generic;
using Cl.Contracts;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalTests
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
        public void Eval_IdentifiersAndKeywordsCoexistIndependently()
        {
            _env.Bind(ClSymbol.And, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, Value.Foo);

            Assert.That(ClSymbol.And.Reduce(_context).Value, Is.EqualTo(Value.One));
            Assert.That(expr.Reduce(_context).Value, Is.EqualTo(Value.Foo));
        }

        [Test]
        [TestCaseSource(nameof(DoesNotCreateNewScopeTestCases))]
        public void Eval_DoesNotCreateNewScope(Func<ClCell, ClCell> expr)
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.Foo);
            var context = expr.Invoke(define).Reduce(_context);
            Assert.That(context.Env.Lookup(Var.Foo), Is.EqualTo(Value.Foo));
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

            var context = expr.Reduce(_context);

            Assert.That(context.Value, Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Eval_ReturnUnreducedList_WhenItemOfListHasComplicatedForm()
        {
            var begin = BuiltIn.ListOf(ClSymbol.Begin, ClBool.True, Value.Foo, Value.Bar);
            var logicAnd = BuiltIn.ListOf(ClSymbol.And, ClBool.True, Value.One);
            var expr = BuiltIn.Quote(BuiltIn.ListOf(begin, logicAnd));

            var context = expr.Reduce(_context);

            Assert.That(BuiltIn.First(context.Value), Is.EqualTo(begin));
            Assert.That(BuiltIn.Second(context.Value), Is.EqualTo(logicAnd));
        }

        [Test]
        public void Eval_ReturnUnreducedExpression_WhenItIsQuoted()
        {
            var cell = new ClCell(new ClFixnum(1), new ClFixnum(2));
            var expr = BuiltIn.Quote(cell);

            var context = expr.Reduce(_context);

            Assert.That(Object.ReferenceEquals(context.Value, cell), Is.True);
        }

        [Test]
        public void Eval_Variable_ThrowUnboundException_WhenBindingIsMissed()
        {
            Assert.That(() => Var.Foo.Reduce(_context),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void Eval_Variable_ReturnBinding_WhenEnvironmentContainsVariable()
        {
            _env.Bind(Var.Foo, Value.One);
            var context = Var.Foo.Reduce(_context);
            Assert.That(context.Value, Is.EqualTo(Value.One));
        }

        [Test]
        [TestCaseSource(nameof(SelfEvaluatingTestCases))]
        public void Eval_SelfEvaluatingExpression(IClObj expr)
        {
            var context = expr.Reduce(_context);
            Assert.That(context.Value, Is.EqualTo(expr));
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
