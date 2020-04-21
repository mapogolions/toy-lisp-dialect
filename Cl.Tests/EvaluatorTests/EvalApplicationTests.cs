using System.Collections.Generic;
using Cl.Extensions;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalApplicationTests
    {
        private ClString _foo;
        private ClSymbol _x;
        private ClSymbol _f;

        [SetUp]
        public void BeforeClass()
        {
            _x = new ClSymbol("x");
            _f = new ClSymbol("f");
            _foo = new ClString("foo");
        }

        private IEnv _env;
        private Evaluator _evaluator;

        [SetUp]
        public void BeforeEach()
        {
            _env = new Env();
            _evaluator = new Evaluator(_env);
        }

        [Test]
        public void EvalApplication_SupportLexicalEnvironment()
        {
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, _x);
            var hof = BuiltIn.ListOf(ClSymbol.Lambda, BuiltIn.ListOf(_x), procedure);
            var definition = BuiltIn.ListOf(ClSymbol.Define, _f, hof);
            var application = BuiltIn.ListOf(_f, new ClFixnum(10));
            var expr = BuiltIn.ListOf(application);

            var actual = _evaluator.Eval(new List<IClObj> { definition, expr });

            Assert.That(actual, Is.EqualTo(new ClFixnum(10)));
        }

        [Test]
        public void EvalApplication_EvalCompoundArgumentsBeforeFunctionBody()
        {
            var compoundArg = BuiltIn.ListOf(ClSymbol.Define, _x, _foo);
            var parameters = BuiltIn.ListOf(_x);
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, parameters, _x);
            var expr = BuiltIn.ListOf(procedure, compoundArg);

            var actual = _evaluator.EvalApplication(expr);

            Assert.That(actual, Is.EqualTo(Nil.Given));
            Assert.That(_env.Lookup(_x), Is.EqualTo(_foo));
        }

        [Test]
        public void EvalApplication_ThrowException_WhenBodyContainsUnboundVariable()
        {
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, _x);
            var expr = BuiltIn.ListOf(procedure);
            var errorMessage = Errors.UnboundVariable(_x);

            Assert.That(() => _evaluator.EvalApplication(expr),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void EvalApplication_ApplyFunctionWithCompoundBody()
        {
            var body = BuiltIn.ListOf(ClSymbol.If, _x, ClBool.True, _foo);
            var parameters = BuiltIn.ListOf(_x);
            var compoundFn = BuiltIn.ListOf(ClSymbol.Lambda, parameters, body);
            var expr = BuiltIn.ListOf(compoundFn, ClBool.False);

            var actual = _evaluator.EvalApplication(expr);

            Assert.That(actual, Is.EqualTo(_foo));
        }

        [Test]
        public void EvalApplication_ApplyZeroArityFunction()
        {
            var constant = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, _foo);
            var expr = BuiltIn.ListOf(constant);

            var actual = _evaluator.EvalApplication(expr);

            Assert.That(actual, Is.EqualTo(_foo));
        }

        [Test]
        public void TryEvalAppliation_HaveAccessToGlobalScope()
        {
            _env.Bind(_x, _foo);
            var identity = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, _x);
            var expr = BuiltIn.ListOf(identity);

            var actual = _evaluator.EvalApplication(expr);

            Assert.That(actual, Is.EqualTo(_foo));
        }

        [Test]
        public void EvalApplication_JustReturnPassedArgument()
        {
            var parameters = BuiltIn.ListOf(_x);
            var identity = BuiltIn.ListOf(ClSymbol.Lambda, parameters, _x);
            var expr = BuiltIn.ListOf(identity, _foo);

            var actual = _evaluator.EvalApplication(expr);

            Assert.That(actual, Is.EqualTo(_foo));
        }
    }
}
