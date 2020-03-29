using System.Collections.Generic;
using Cl.Abs;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalAndTests
    {
        [Test]
        public void EvalAnd_MustBeLazy()
        {
            var env = new Env();
            var evaluator = new Evaluator(env);
            var define = BuiltIn.ListOf(ClSymbol.Define, new ClSymbol("a"), new ClFixnum(2));
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, ClBool.False, define, ClBool.True);

            Assert.That(evaluator.Eval(expr), Is.EqualTo(ClBool.False));
            Assert.That(() => env.Lookup(new ClSymbol("a")),
                Throws.InvalidOperationException.With.Message.EqualTo("Unbound variable"));
        }

        [Test]
        public void EvalAnd_ReturnTrue_WhenEachItemIsTrue()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, new ClString(string.Empty)); // (and . (true . ("" . nil)))

            Assert.That(evaluator.EvalAnd(expr), Is.EqualTo(ClBool.True));
        }

        [Test]
        [TestCaseSource(nameof(AtLeastOneItemIsFalseTestCases))]
        public void EvalAnd_ReturnFalse_WhenAtLeastOneItemIsFalse(ClCell items)
        {
            var evaluator = new Evaluator(new Env());
            var expr = new ClCell(ClSymbol.And, items);

            Assert.That(evaluator.EvalAnd(expr), Is.EqualTo(ClBool.False));
        }

        static IEnumerable<ClCell> AtLeastOneItemIsFalseTestCases()
        {
            yield return BuiltIn.ListOf(new ClFixnum(10), Nil.Given, ClBool.True);
            yield return BuiltIn.ListOf(ClBool.True, ClBool.False);
        }

        [Test]
        public void EvalAnd_ReturnTrue_WhenTailIsEmptyList()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.And); // (and . nil)

            Assert.That(evaluator.EvalAnd(expr), Is.EqualTo(ClBool.True));
        }
    }
}
