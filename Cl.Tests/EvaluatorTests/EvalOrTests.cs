using Cl.Contracts;
using Cl.Exceptions;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalOrTests
    {
        private IEnv _env;
        private IContext _ctx;

        [SetUp]
        public void BeforeEach()
        {
            _env = new Env();
            _ctx = new Context(_env);
        }

        [Test]
        public void EvalOr_MustBeLazy()
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.Foo);
            var expr = BuiltIn.ListOf(ClSymbol.Or, ClBool.True, define);

            var ctx = expr.Reduce(_ctx);

            Assert.That(ctx.Value, Is.EqualTo(ClBool.True));
            Assert.That(() => ctx.Env.Lookup(Var.Foo),
                Throws.Exception.TypeOf<UnboundVariableException>().With.Message.EqualTo("Unbound variable foo"));
        }

        [Test]
        [TestCaseSource(nameof(EachItemIsFalseTestCases))]
        public void EvalOr_ReturnLastItem_WhenEachItemIsFalse(ClCell items, ClObj expected)
        {
            var expr = new ClCell(ClSymbol.Or, items);
            var ctx = expr.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(expected));
        }

        static object[] EachItemIsFalseTestCases =
            {
                new object[] { BuiltIn.ListOf(ClCell.Nil, ClBool.False), ClBool.False },
                new object[] { BuiltIn.ListOf(ClBool.False, ClCell.Nil), ClCell.Nil },
            };


        [Test]
        [TestCaseSource(nameof(AtLeastOneItemIsTrueTestCases))]
        public void EvalOr_ReturnTruthyItem_WhenAtLeastOneItemIsTrue(ClCell items, ClObj expected)
        {
            var expr = new ClCell(ClSymbol.Or, items);
            var ctx = expr.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(expected));
        }

        static object[] AtLeastOneItemIsTrueTestCases =
            {
                new object[] { BuiltIn.ListOf(Value.Foo, Value.Bar), Value.Foo },
                new object[] { BuiltIn.ListOf(ClBool.False, ClCell.Nil, Value.One), Value.One },
                new object[] { BuiltIn.ListOf(ClBool.True, ClBool.False), ClBool.True }
            };

        [Test]
        public void EvalOr_ReturnFalse_WhenTailIsEmptyList()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Or);
            var ctx = expr.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(ClBool.False));
        }
    }
}
