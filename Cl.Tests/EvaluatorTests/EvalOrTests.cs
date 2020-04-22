using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalOrTests
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
        public void EvalOr_MustBeLazy()
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.Foo);
            var expr = BuiltIn.ListOf(ClSymbol.Or, ClBool.True, define);

            Assert.That(_evaluator.EvalOr(expr), Is.EqualTo(ClBool.True));
            Assert.That(() => _env.Lookup(Var.Foo),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void EvalOr_ReturnFalse_WhenEachItemIsFalse()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Or, Nil.Given, ClBool.False);

            Assert.That(_evaluator.EvalOr(expr), Is.EqualTo(ClBool.False));
        }


        [Test]
        [TestCaseSource(nameof(AtLeastOneItemIsTrueTestCases))]
        public void EvalOr_ReturnTrue_WhenAtLeastOneItemIsTrue(ClCell items, IClObj expected)
        {
            var expr = new ClCell(ClSymbol.Or, items);

            Assert.That(_evaluator.EvalOr(expr), Is.EqualTo(expected));
        }

        static object[] AtLeastOneItemIsTrueTestCases =
            {
                new object[] { BuiltIn.ListOf(Value.Foo, Value.Bar), Value.Foo },
                new object[] { BuiltIn.ListOf(ClBool.False, Nil.Given, Value.One), Value.One },
                new object[] { BuiltIn.ListOf(ClBool.True, ClBool.False), ClBool.True }
            };

        [Test]
        public void EvalOr_ReturnFalse_WhenTailIsEmptyList()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Or);

            Assert.That(_evaluator.EvalOr(expr), Is.EqualTo(ClBool.False));
        }

        [Test]
        public void EvalOr_DoesNotEvaluateExpression_WhenTagIsWrong()
        {
            var expr = BuiltIn.ListOf(ClSymbol.And);

            Assert.That(_evaluator.EvalOr(expr), Is.Null);
        }
    }
}
