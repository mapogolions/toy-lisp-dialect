using Cl.Contracts;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalAndTests
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
        public void EvalAnd_MustBeLazy()
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.One);
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, ClBool.False, define, ClBool.True);

            var context = expr.Reduce(_context);

            Assert.That(() => context.Env.Lookup(Var.Foo),
                Throws.InvalidOperationException.With.Message.StartWith("Unbound variable"));
        }

        [Test]
        public void EvalAnd_ReturnLastItem_WhenEachItemIsTrue()
        {
            var expr = BuiltIn.ListOf(ClSymbol.And, ClBool.True, Value.Foo);
            var context = expr.Reduce(_context);
            Assert.That(context.Result, Is.EqualTo(Value.Foo));
        }

        [Test]
        [TestCaseSource(nameof(AtLeastOneItemIsFalseTestCases))]
        public void EvalAnd_ReturnFalsyItem_WhenAtLeastOneItemIsFalse(ClCell items, IClObj expected)
        {
            var expr = new ClCell(ClSymbol.And, items);
            var context = expr.Reduce(_context);
            Assert.That(context.Result, Is.EqualTo(expected));
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
            var context = expr.Reduce(_context);
            Assert.That(context.Result, Is.EqualTo(ClBool.True));
        }
    }
}
