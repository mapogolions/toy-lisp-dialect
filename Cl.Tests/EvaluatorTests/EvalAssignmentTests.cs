using System;
using Cl.Abs;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalAssignmentTests
    {
        [Test]
        public void EvalAssigment_Assignment_IsNotSoSmart_LikePython()
        {
            /*
                Python REPL
                a = 1
                b = 1
                a is b // True
             */

            var env = new Env();
            var a = new ClSymbol("a");
            var b = new ClSymbol("b");
            env.Bind(a, new ClFixnum(10));
            env.Bind(b, new ClFixnum(10));

            Assert.That(Object.ReferenceEquals(env.Lookup(a), env.Lookup(b)), Is.False);
        }

        [Test]
        public void EvalAssignment_SharedReference()
        {
            /*
                Python REPL
                a = 1
                b = a
                a is b // True
             */
            var env = new Env();
            var a = new ClSymbol("a");
            var b = new ClSymbol("b");
            env.Bind(a, ClBool.True);
            env.Bind(b, new ClFixnum(10));
            var evaluator = new Evaluator(env);

            var expr = BuiltIn.ListOf(ClSymbol.Set, a, b);
            var actual = evaluator.EvalAssigment(expr);

            Assert.That(Object.ReferenceEquals(env.Lookup(a), env.Lookup(b)), Is.True);
        }

        [Test]
        public void EvalAssignment_Reassign()
        {
            var env = new Env();
            var a = new ClSymbol("a");
            env.Bind(a, new ClFixnum(1));
            var evaluator = new Evaluator(env);

            var expr = BuiltIn.ListOf(ClSymbol.Set, a, a);
            var actual = evaluator.EvalAssigment(expr);

            Assert.That(actual, Is.EqualTo(Nil.Given));
            Assert.That(env.Lookup(a), Is.EqualTo(new ClFixnum(1)));
        }

        [Test]
        public void EvalAssignment_CanAssignOneVariableToAnother()
        {
            var env = new Env();
            var a = new ClSymbol("a");
            var b = new ClSymbol("b");
            env.Bind(a, new ClFixnum(10));
            env.Bind(b, new ClString("foo"));
            var evaluator = new Evaluator(env);


            var expr = BuiltIn.ListOf(ClSymbol.Set, a, b);
            var actual = evaluator.EvalAssigment(expr);

            Assert.That(actual, Is.EqualTo(Nil.Given));
            Assert.That(env.Lookup(a), Is.EqualTo(new ClString("foo")));
        }

        [Test]
        public void EvalAssignment_ReturnNil_WhenOperationIsSuccessful()
        {
            var env = new Env();
            var a = new ClSymbol("a");
            env.Bind(a, ClBool.True);
            var evaluator = new Evaluator(env);

            var expr = BuiltIn.ListOf(ClSymbol.Set, a, ClBool.False);

            Assert.That(evaluator.EvalAssigment(expr), Is.EqualTo(Nil.Given));
        }

        [Test]
        public void EvalAssignment_ThrowException_WhenEnvironmentDoesNotContainBinding()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Set, new ClSymbol("a"), ClBool.True);

            Assert.That(() => evaluator.EvalAssigment(expr), Throws.InvalidOperationException);
        }
    }
}
