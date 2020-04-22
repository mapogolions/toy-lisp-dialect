using System;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalAssignmentTests
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
        public void EvalAssignment_SharedReference()
        {
            _env.Bind(Var.Foo, Value.Foo);
            _env.Bind(Var.Bar, Value.Bar);
            var expr = BuiltIn.ListOf(ClSymbol.Set, Var.Foo, Var.Bar);

            Ignore(_evaluator.EvalAssigment(expr));

            Assert.That(Object.ReferenceEquals(_env.Lookup(Var.Foo), _env.Lookup(Var.Bar)), Is.True);
        }

        [Test]
        public void EvalAssignment_Reassign()
        {
            _env.Bind(Var.Foo, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.Set, Var.Foo, Var.Foo);

            Assert.That(_evaluator.EvalAssigment(expr), Is.EqualTo(Nil.Given));
            Assert.That(_env.Lookup(Var.Foo), Is.EqualTo(Value.One));
        }

        [Test]
        public void EvalAssignment_CanAssignOneVariableToAnother()
        {
            _env.Bind(Var.Foo, Value.Foo);
            _env.Bind(Var.Bar, Value.Bar);
            var expr = BuiltIn.ListOf(ClSymbol.Set, Var.Foo, Var.Bar);

            Assert.That(_evaluator.EvalAssigment(expr), Is.EqualTo(Nil.Given));
            Assert.That(_env.Lookup(Var.Foo), Is.EqualTo(Value.Bar));
        }

        [Test]
        public void EvalAssignment_ReturnNil_WhenOperationIsSuccessful()
        {
            _env.Bind(Var.Foo, ClBool.True);
            var expr = BuiltIn.ListOf(ClSymbol.Set, Var.Foo, ClBool.False);

            Assert.That(_evaluator.EvalAssigment(expr), Is.EqualTo(Nil.Given));
        }

        [Test]
        public void EvalAssignment_ThrowException_WhenEnvironmentDoesNotContainBinding()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Set, Var.Foo, Value.Foo);

            Assert.That(() => _evaluator.EvalAssigment(expr), Throws.InvalidOperationException);
        }

        [Test]
        public void EvalAssignment_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Define);

            Assert.That(_evaluator.EvalAssigment(expr), Is.Null);
        }
    }
}
