using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalOrTests
    {
        [Test]
        public void EvalOr_MustBeLazy()
        {
            var env = new Env();
            var evaluator = new Evaluator(env);
            var define = BuiltIn.ListOf(ClSymbol.Define, new ClSymbol("a"), new ClFixnum(2));
            var expr = BuiltIn.ListOf(ClSymbol.Or, ClBool.True, define);

            Assert.That(evaluator.EvalOr(expr), Is.EqualTo(ClBool.True));
            Assert.That(() => env.Lookup(new ClSymbol("a")),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void EvalOr_ReturnFalse_WhenEachItemIsFalse()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Or, Nil.Given, ClBool.False);

            Assert.That(evaluator.EvalOr(expr), Is.EqualTo(ClBool.False));
        }


        [Test]
        [TestCaseSource(nameof(AtLeastOneItemIsTrueTestCases))]
        public void EvalOr_ReturnTrue_WhenAtLeastOneItemIsTrue(ClCell items, IClObj expected)
        {
            var evaluator = new Evaluator(new Env());
            var expr = new ClCell(ClSymbol.Or, items);

            Assert.That(evaluator.EvalOr(expr), Is.EqualTo(expected));
        }

        static object[] AtLeastOneItemIsTrueTestCases =
            {
                new object[] { BuiltIn.ListOf(new ClString("bar"), new ClString("foo")), new ClString("bar") },
                new object[] { BuiltIn.ListOf(ClBool.False, Nil.Given, new ClFixnum(1)), new ClFixnum(1) },
                new object[] { BuiltIn.ListOf(ClBool.True, ClBool.False), ClBool.True }
            };

        [Test]
        public void EvalOr_ReturnFalse_WhenTailIsEmptyList()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Or);

            Assert.That(evaluator.EvalOr(expr), Is.EqualTo(ClBool.False));
        }

        [Test]
        public void EvalOr_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.And);

            Assert.That(evaluator.EvalOr(expr), Is.Null);
        }
    }
}
