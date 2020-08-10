using System;
using Cl.Contracts;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalAssignmentTests
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
        public void EvalAssignment_SharedReference()
        {
            _env.Bind(Var.Foo, Value.Foo);
            _env.Bind(Var.Bar, Value.Bar);
            var expr = BuiltIn.ListOf(ClSymbol.Set, Var.Foo, Var.Bar);

            var context = expr.Reduce(_context);

            Assert.That(Object.ReferenceEquals(context.Env.Lookup(Var.Foo), _env.Lookup(Var.Bar)), Is.True);
        }

        [Test]
        public void EvalAssignment_Reassign()
        {
            _env.Bind(Var.Foo, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.Set, Var.Foo, Var.Foo);

            var context = expr.Reduce(_context);

            Assert.That(context.Env.Lookup(Var.Foo), Is.EqualTo(Value.One));
        }

        [Test]
        public void EvalAssignment_CanAssignOneVariableToAnother()
        {
            _env.Bind(Var.Foo, Value.Foo);
            _env.Bind(Var.Bar, Value.Bar);
            var expr = BuiltIn.ListOf(ClSymbol.Set, Var.Foo, Var.Bar);

            var context = expr.Reduce(_context);

            Assert.That(context.Env.Lookup(Var.Foo), Is.EqualTo(Value.Bar));
        }

        [Test]
        public void EvalAssignment_ReturnNil_WhenOperationIsSuccessful()
        {
            _env.Bind(Var.Foo, ClBool.True);
            var expr = BuiltIn.ListOf(ClSymbol.Set, Var.Foo, ClBool.False);

            var context = expr.Reduce(_context);

            Assert.That(context.Value, Is.EqualTo(Nil.Given));
        }

        [Test]
        public void EvalAssignment_ThrowException_WhenEnvironmentDoesNotContainBinding()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Set, Var.Foo, Value.Foo);
            Assert.That(() => expr.Reduce(_context), Throws.InvalidOperationException);
        }
    }
}
