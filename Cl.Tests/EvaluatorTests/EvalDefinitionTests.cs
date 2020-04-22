using System;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalDefinitionTests
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
        public void EvalDefinition_CreateSharedReference()
        {
            _env.Bind(Var.Bar, Value.Bar);
            var expr = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Var.Bar);

            Ignore(_evaluator.EvalDefinition(expr));

            Assert.That(Object.ReferenceEquals(_env.Lookup(Var.Foo), _env.Lookup(Var.Bar)), Is.True);
        }

        [Test]
        public void EvalDefinition_ThrowException_WhenRighSideVariableDoesNotExist()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Var.Bar);

            Assert.That(() => _evaluator.EvalDefinition(expr),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void EvalDefinition_ScopeRules()
        {
            var outerScope = new Env();
            outerScope.Bind(Var.Foo, Value.Foo);
            var innerScope = new Env(outerScope);
            var evaluator = new Evaluator(innerScope);
            var expr = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.One);

            Ignore(evaluator.EvalDefinition(expr));

            Assert.That(innerScope.Lookup(Var.Foo), Is.EqualTo(Value.One));
            Assert.That(outerScope.Lookup(Var.Foo), Is.EqualTo(Value.Foo));
        }
        [Test]
        public void EvalDefinition_OverrideExistingBinding()
        {
            _env.Bind(Var.Foo, ClBool.True);
            var expr = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.Foo);

            Ignore(_evaluator.EvalDefinition(expr));

            Assert.That(_env.Lookup(Var.Foo), Is.EqualTo(Value.Foo));
        }

        [Test]
        public void EvalDefinition_ReturnNil_WhenOperationIsSuccessful()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, ClBool.False);

            Assert.That(_evaluator.EvalDefinition(expr), Is.EqualTo(Nil.Given));
        }

        [Test]
        public void EvalDefinition_CreateNewBinding_WhenEnvironmentDoesNotContainBinding()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, ClBool.True);

            Ignore(_evaluator.EvalDefinition(expr));

            Assert.That(_env.Lookup(Var.Foo), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void EvalDefinition_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Set);

            Assert.That(_evaluator.EvalDefinition(expr), Is.Null);
        }
    }
}
