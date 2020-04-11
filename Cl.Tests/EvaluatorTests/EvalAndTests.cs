using System.Collections.Generic;
using Cl.Abs;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalAndTests
    {
        [Test]
        public void TryEvalAnd_MustBeLazy()
        {
            var env = new Env();
            var evaluator = new Evaluator(env);
            var define = BuiltIn.ListOf(ClSymbol.Define, new ClSymbol("a"), new ClFixnum(2));
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, ClBool.False, define, ClBool.True);

            Ignore(evaluator.TryEvalAnd(expr, out var obj));

            Assert.That(obj, Is.EqualTo(ClBool.False));
            Assert.That(() => env.Lookup(new ClSymbol("a")),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void TryEvalAnd_ReturnTrue_WhenEachItemIsTrue()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, new ClString(string.Empty)); // (and . (true . ("" . nil)))

            Ignore(evaluator.TryEvalAnd(expr, out var obj));

            Assert.That(obj, Is.EqualTo(ClBool.True));
        }

        [Test]
        [TestCaseSource(nameof(AtLeastOneItemIsFalseTestCases))]
        public void TryEvalAnd_ReturnFalse_WhenAtLeastOneItemIsFalse(ClCell items)
        {
            var evaluator = new Evaluator(new Env());
            var expr = new ClCell(ClSymbol.And, items);

            Ignore(evaluator.TryEvalAnd(expr, out var obj));

            Assert.That(obj, Is.EqualTo(ClBool.False));
        }

        static IEnumerable<ClCell> AtLeastOneItemIsFalseTestCases()
        {
            yield return BuiltIn.ListOf(new ClFixnum(10), Nil.Given, ClBool.True);
            yield return BuiltIn.ListOf(ClBool.True, ClBool.False);
        }

        [Test]
        public void TryEvalAnd_ReturnTrue_WhenTailIsEmptyList()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.And); // (and . nil)

            Ignore(evaluator.TryEvalAnd(expr, out var obj));

            Assert.That(obj, Is.EqualTo(ClBool.True));
        }

        [Test]
        public void TryEvalAnd_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Or);

            Assert.That(evaluator.TryEvalAnd(expr, out var _), Is.False);
        }
    }
}
