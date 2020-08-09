using Cl.Contracts;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalOrTests
    {
        private IEnv _env;
        private IContext _context;

        [SetUp]
        public void BeforeEach()
        {
            _env = new Env();
            _context = new Context(_env);
        }

        [Test]
        public void EvalOr_MustBeLazy()
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.Foo);
            var expr = BuiltIn.ListOf(ClSymbol.Or, ClBool.True, define);

            var context = expr.Reduce(_context);

            Assert.That(context.Result, Is.EqualTo(ClBool.True));
            Assert.That(() => context.Env.Lookup(Var.Foo),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        [TestCaseSource(nameof(EachItemIsFalseTestCases))]
        public void EvalOr_ReturnLastItem_WhenEachItemIsFalse(ClCell items, IClObj expected)
        {
            var expr = new ClCell(ClSymbol.Or, items);
            var context = expr.Reduce(_context);
            Assert.That(context.Result, Is.EqualTo(expected));
        }

        static object[] EachItemIsFalseTestCases =
            {
                new object[] { BuiltIn.ListOf(Nil.Given, ClBool.False), ClBool.False },
                new object[] { BuiltIn.ListOf(ClBool.False, Nil.Given), Nil.Given },
            };


        [Test]
        [TestCaseSource(nameof(AtLeastOneItemIsTrueTestCases))]
        public void EvalOr_ReturnTruthyItem_WhenAtLeastOneItemIsTrue(ClCell items, IClObj expected)
        {
            var expr = new ClCell(ClSymbol.Or, items);
            var context = expr.Reduce(_context);
            Assert.That(context.Result, Is.EqualTo(expected));
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
            var context = expr.Reduce(_context);
            Assert.That(context.Result, Is.EqualTo(ClBool.False));
        }
    }
}
