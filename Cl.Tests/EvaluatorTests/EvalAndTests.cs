using Cl.Contracts;
using Cl.Errors;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalAndTests
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
        public void EvalAnd_MustBeLazy()
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, ClBool.False, define, ClBool.True);

            var ctx = expr.Reduce(_ctx);

            Assert.That(() => ctx.Env.Lookup(Var.Foo),
                Throws.Exception.TypeOf<UnboundVariableError>().With.Message.EqualTo("Unbound variable foo"));
        }

        [Test]
        public void EvalAnd_ReturnLastItem_WhenEachItemIsTrue()
        {
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, Value.Foo);
            var ctx = expr.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(Value.Foo));
        }

        [Test]
        [TestCaseSource(nameof(AtLeastOneItemIsFalseTestCases))]
        public void EvalAnd_ReturnFalsyItem_WhenAtLeastOneItemIsFalse(ClCell items, ClObj expected)
        {
            var expr = new ClCell(ClSymbol.And, items);
            var ctx = expr.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(expected));
        }

        static object[] AtLeastOneItemIsFalseTestCases =
            {
                new object[] { BuiltIn.ListOf(Value.One, ClCell.Nil, ClBool.True), ClCell.Nil },
                new object[] { BuiltIn.ListOf(ClBool.True, ClBool.False), ClBool.False }
            };

        [Test]
        public void EvalAnd_ReturnTrue_WhenTailIsEmptyList()
        {
            var expr = BuiltIn.ListOf(ClSymbol.And);
            var ctx = expr.Reduce(_ctx);
            Assert.That(ctx.Value, Is.EqualTo(ClBool.True));
        }
    }
}
