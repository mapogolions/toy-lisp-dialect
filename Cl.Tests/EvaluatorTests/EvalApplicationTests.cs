using System.Collections.Generic;
using Cl.Contracts;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalApplicationTests
    {
        private IEnv _env;
        private IContext _context;
        private Evaluator _evaluator;

        [SetUp]
        public void BeforeEach()
        {
            _env = new Env();
            _context = new Context(_env);
            _evaluator = new Evaluator(_env);
        }

        /**
         * (define fn
         *   (lambda (foo)
         *     (lambda () foo)))
         * (fn 1)
         */
        [Test]
        public void EvalApplication_SupportLexicalEnvironmentAndGlobalDefinition()
        {
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, Var.Foo);
            var hof = BuiltIn.ListOf(ClSymbol.Lambda, BuiltIn.ListOf(Var.Foo), procedure);
            var definition = BuiltIn.ListOf(ClSymbol.Define, Var.Fn, hof);
            var application = BuiltIn.ListOf(Var.Fn, Value.One);
            var expr = BuiltIn.ListOf(application);

            var actual = _evaluator.Eval(new List<IClObj> { definition, expr });

            Assert.That(actual, Is.EqualTo(Value.One));
        }

        /**
         * ((lambda (foo)
         *   (lambda () foo)) 1)
         */
        [Test]
        public void EvalApplication_SupportLexicalEnvironment()
        {
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, Var.Foo);
            var hof = BuiltIn.ListOf(ClSymbol.Lambda, BuiltIn.ListOf(Var.Foo), procedure);
            var expr = BuiltIn.ListOf(BuiltIn.ListOf(hof, Value.One));

            var actual = _evaluator.EvalApplication(expr);

            Assert.That(actual, Is.EqualTo(Value.One));
        }

        [Test]
        public void EvalApplication_EvalCompoundArgumentsBeforeFunctionBody()
        {
            var compoundArg = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.Foo);
            var parameters = BuiltIn.ListOf(Var.Foo);
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, parameters, Var.Foo);
            var expr = BuiltIn.ListOf(procedure, compoundArg);

            var actual = _evaluator.EvalApplication(expr);

            Assert.That(actual, Is.EqualTo(Nil.Given));
            Assert.That(_env.Lookup(Var.Foo), Is.EqualTo(Value.Foo));
        }

        [Test]
        public void EvalApplication_ThrowException_WhenBodyContainsUnboundVariable()
        {
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, Var.Bar);
            var expr = BuiltIn.ListOf(procedure);
            var errorMessage = Errors.UnboundVariable(Var.Bar);

            Assert.That(() => _evaluator.EvalApplication(expr),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void EvalApplication_FunctionWithCompoundBody()
        {
            var body = BuiltIn.ListOf(ClSymbol.If, Var.Foo, ClBool.True, Value.Foo);
            var parameters = BuiltIn.ListOf(Var.Foo);
            var compoundFn = BuiltIn.ListOf(ClSymbol.Lambda, parameters, body);
            var expr = BuiltIn.ListOf(compoundFn, ClBool.False);

            var actual = _evaluator.EvalApplication(expr);

            Assert.That(actual, Is.EqualTo(Value.Foo));
        }

        [Test]
        public void EvalApplication_ZeroArityFunction()
        {
            var constant = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, Value.One);
            var expr = BuiltIn.ListOf(constant);

            var actual = _evaluator.EvalApplication(expr);

            Assert.That(actual, Is.EqualTo(Value.One));
        }

        [Test]
        public void EvalAppliation_AccessToGlobalScope()
        {
            _env.Bind(Var.Foo, Value.Foo);
            var identity = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, Var.Foo);
            var expr = BuiltIn.ListOf(identity);

            var actual = _evaluator.EvalApplication(expr);

            Assert.That(actual, Is.EqualTo(Value.Foo));
        }

        [Test]
        public void EvalApplication_IdentityFunction()
        {
            var parameters = BuiltIn.ListOf(Var.Foo);
            var identity = BuiltIn.ListOf(ClSymbol.Lambda, parameters, Var.Foo);
            var expr = BuiltIn.ListOf(identity, Value.Foo);

            var actual = _evaluator.EvalApplication(expr);

            Assert.That(actual, Is.EqualTo(Value.Foo));
        }
    }
}
