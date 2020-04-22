using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalAndTests
    {
        private IEnv _env;
        private Evaluator _evaluator;

        [SetUp]
        public void BeforeEach()
        {
            _env = new Env();
            _evaluator = new Evaluator(_env);
        }

        [Test]
        public void EvalAnd_MustBeLazy()
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, ClBool.False, define, ClBool.True);

            Ignore(_evaluator.EvalAnd(expr));

            Assert.That(() => _env.Lookup(Var.Foo),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void EvalAnd_ReturnLastItem_WhenEachItemIsTrue()
        {
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, Value.Foo);

            Assert.That(_evaluator.EvalAnd(expr), Is.EqualTo(Value.Foo));
        }

        [Test]
        [TestCaseSource(nameof(AtLeastOneItemIsFalseTestCases))]
        public void EvalAnd_ReturnLastItem_WhenAtLeastOneItemIsFalse(ClCell items, IClObj expected)
        {
            var expr = new ClCell(ClSymbol.And, items);

            Assert.That(_evaluator.EvalAnd(expr), Is.EqualTo(expected));
        }

        static object[] AtLeastOneItemIsFalseTestCases =
            {
                new object[] { BuiltIn.ListOf(Value.One, Nil.Given, ClBool.True), Nil.Given },
                new object[] { BuiltIn.ListOf(ClBool.True, ClBool.False), ClBool.False }
            };

        [Test]
        public void EvalAnd_ReturnTrue_WhenTailIsEmptyList()
        {
            var expr = BuiltIn.ListOf(ClSymbol.And);

            Assert.That(_evaluator.EvalAnd(expr), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void EvalAnd_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Or);

            Assert.That(_evaluator.EvalAnd(expr), Is.Null);
        }
    }
}
