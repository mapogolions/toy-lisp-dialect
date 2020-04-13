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
        public void EvalAnd_MustBeLazy()
        {
            var env = new Env();
            var evaluator = new Evaluator(env);
            var define = BuiltIn.ListOf(ClSymbol.Define, new ClSymbol("a"), new ClFixnum(2));
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, ClBool.False, define, ClBool.True);

            Ignore(evaluator.EvalAnd(expr));

            Assert.That(() => env.Lookup(new ClSymbol("a")),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void EvalAnd_ReturnLastItem_WhenEachItemIsTrue()
        {
            var evaluator = new Evaluator(new Env());
            var lastItem = new ClString("");
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, lastItem);

            Assert.That(evaluator.EvalAnd(expr), Is.EqualTo(lastItem));
        }

        [Test]
        [TestCaseSource(nameof(AtLeastOneItemIsFalseTestCases))]
        public void EvalAnd_ReturnLastItem_WhenAtLeastOneItemIsFalse(ClCell items, IClObj expected)
        {
            var evaluator = new Evaluator(new Env());
            var expr = new ClCell(ClSymbol.And, items);

            Assert.That(evaluator.EvalAnd(expr), Is.EqualTo(expected));
        }

        static object[] AtLeastOneItemIsFalseTestCases =
            {
                new object[] { BuiltIn.ListOf(new ClFixnum(10), Nil.Given, ClBool.True), Nil.Given },
                new object[] { BuiltIn.ListOf(ClBool.True, ClBool.False), ClBool.False }
            };

        [Test]
        public void EvalAnd_ReturnTrue_WhenTailIsEmptyList()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.And);

            Assert.That(evaluator.EvalAnd(expr), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void EvalAnd_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Or);

            Assert.That(evaluator.EvalAnd(expr), Is.Null);
        }
    }
}
