using System;
using Cl.Abs;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalDefinitionTests
    {
        [Test]
        public void TryEvalDefinition_CreateSharedReference()
        {
            var a = new ClSymbol("a");
            var b = new ClSymbol("b");
            var env = new Env();
            env.Bind(b, new ClString("foo"));
            var evaluator = new Evaluator(env);
            var expr = BuiltIn.ListOf(ClSymbol.Define, a, b);

            Ignore(evaluator.TryEvalDefinition(expr, out var _));

            Assert.That(Object.ReferenceEquals(env.Lookup(a), env.Lookup(b)), Is.True);
        }

        [Test]
        public void TryEvalDefinition_ThrowException_WhenRighSideVariableDoesNotExist()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Define, new ClSymbol("a"), new ClSymbol("b"));

            Assert.That(() => evaluator.TryEvalDefinition(expr, out var _),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void TryEvalDefinition_ScopeRules()
        {
            var a = new ClSymbol("a");
            var outerScope = new Env();
            outerScope.Bind(a, ClBool.True);
            var innerScope = new Env(outerScope);
            innerScope.Bind(a, ClBool.False);
            var evaluator = new Evaluator(innerScope);
            var expr = BuiltIn.ListOf(ClSymbol.Define, a, new ClFixnum(0));

            Ignore(evaluator.TryEvalDefinition(expr, out var _));

            Assert.That(innerScope.Lookup(a), Is.EqualTo(new ClFixnum(0)));
            Assert.That(outerScope.Lookup(a), Is.EqualTo(ClBool.True));
        }
        [Test]
        public void TryEvalDefinition_OverrideExistingBinding()
        {
            var env = new Env();
            var a = new ClSymbol("a");
            env.Bind(a, ClBool.True);
            var evaluator = new Evaluator(env);
            var expr = BuiltIn.ListOf(ClSymbol.Define, a, ClBool.False);

            Ignore(evaluator.TryEvalDefinition(expr, out var _));

            Assert.That(env.Lookup(a), Is.EqualTo(ClBool.False));
        }

        [Test]
        public void TryEvalDefinition_ReturnNil_WhenOperationIsSuccessful()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Define, new ClSymbol("a"), ClBool.False);

            Ignore(evaluator.TryEvalDefinition(expr, out var obj));

            Assert.That(obj, Is.EqualTo(Nil.Given));
        }

        [Test]
        public void TryEvalDefinition_CreateNewBinding_WhenEnvironmentDoesNotContainBinding()
        {
            var env = new Env();
            var evaluator = new Evaluator(env);
            var a = new ClSymbol("a");
            var expr = BuiltIn.ListOf(ClSymbol.Define, a, ClBool.True);

            Ignore(evaluator.TryEvalDefinition(expr, out var _));

            Assert.That(env.Lookup(a), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void TryTryEvalDefinition_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Set);

            Assert.That(evaluator.TryEvalDefinition(expr, out var _), Is.False);
        }
    }
}
