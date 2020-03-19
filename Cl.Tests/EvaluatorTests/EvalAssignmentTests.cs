using Cl.Abs;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalAssignmentTests
    {
        [Test]
        public void EvalAssignment_ReturnNil_WhenAssignmentSuccess()
        {
            var env = new Env();
            env.Bind(new ClSymbol("a"), ClBool.True);
            var evaluator = new Evaluator(env);

            var expr = BuiltIn.ListOf(ClSymbol.Set, new ClSymbol("a"), ClBool.False);
            var actual = evaluator.EvalAssigment(expr);

            Assert.That(actual, Is.EqualTo(Nil.Given));
            Assert.That(env.Lookup(new ClSymbol("a")), Is.EqualTo(ClBool.False));
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
