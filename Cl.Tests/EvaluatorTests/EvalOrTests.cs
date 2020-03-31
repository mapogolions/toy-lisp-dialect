using System.Collections.Generic;
using Cl.Abs;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalOrTests
    {
        [Test]
        public void TryEvalOr_MustBeLazy()
        {
            var env = new Env();
            var evaluator = new Evaluator(env);
            var define = BuiltIn.ListOf(ClSymbol.Define, new ClSymbol("a"), new ClFixnum(2));
            var expr = BuiltIn.ListOf(ClSymbol.Or, ClBool.True, define);

            Ignore(evaluator.TryEvalOr(expr, out var obj));

            Assert.That(obj, Is.EqualTo(ClBool.True));
            Assert.That(() => env.Lookup(new ClSymbol("a")),
                Throws.InvalidOperationException.With.Message.EqualTo("Unbound variable"));
        }

        [Test]
        public void TryEvalOr_ReturnFalse_WhenEachItemIsFalse()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Or, Nil.Given, ClBool.False);

            Ignore(evaluator.TryEvalOr(expr, out var obj));

            Assert.That(obj, Is.EqualTo(ClBool.False));
        }


        [Test]
        [TestCaseSource(nameof(AtLeastOneItemIsTrueTestCases))]
        public void TryEvalOr_ReturnTrue_WhenAtLeastOneItemIsTrue(ClCell items)
        {
            var evaluator = new Evaluator(new Env());
            var expr = new ClCell(ClSymbol.Or, items);

            Ignore(evaluator.TryEvalOr(expr, out var obj));

            Assert.That(obj, Is.EqualTo(ClBool.True));
        }

        static IEnumerable<ClCell> AtLeastOneItemIsTrueTestCases()
        {
            yield return BuiltIn.ListOf(ClBool.False, Nil.Given, ClBool.True);
            yield return BuiltIn.ListOf(ClBool.True, ClBool.False);
        }

        [Test]
        public void TryEvalOr_ReturnFalse_WhenTailIsEmptyList()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Or);

            Ignore(evaluator.TryEvalOr(expr, out var obj));

            Assert.That(obj, Is.EqualTo(ClBool.False));
        }

        [Test]
        public void TryEvalOr_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.And);

            Assert.That(evaluator.TryEvalOr(expr, out var _), Is.False);
        }
    }
}
