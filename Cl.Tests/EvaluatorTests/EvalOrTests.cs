using System.Collections.Generic;
using Cl.Abs;
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

            Assert.That(evaluator.Eval(expr), Is.EqualTo(ClBool.True));
            Assert.That(() => env.Lookup(new ClSymbol("a")),
                Throws.InvalidOperationException.With.Message.EqualTo("Unbound variable"));
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
        public void EvalOr_ReturnTrue_WhenAtLeastOneItemIsTrue(ClCell items)
        {
            var evaluator = new Evaluator(new Env());
            var expr = new ClCell(ClSymbol.Or, items);

            Assert.That(evaluator.EvalOr(expr), Is.EqualTo(ClBool.True));
        }

        static IEnumerable<ClCell> AtLeastOneItemIsTrueTestCases()
        {
            yield return BuiltIn.ListOf(ClBool.False, Nil.Given, ClBool.True);
            yield return BuiltIn.ListOf(ClBool.True, ClBool.False);
        }

        [Test]
        public void EvalOr_ReturnFalse_WhenTailIsEmptyList()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Or);

            Assert.That(evaluator.EvalOr(expr), Is.EqualTo(ClBool.False));
        }
    }
}
