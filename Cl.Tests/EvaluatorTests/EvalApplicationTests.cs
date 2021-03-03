using Cl.Contracts;
using Cl.Exceptions;
using Cl.Extensions;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalApplicationTests
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
        public void EvalApplication_ThrowInvalidFunctionCall_WhenCarIsNotCallable()
        {
            _env.Bind(Var.Fn, Value.One);
            var expr = BuiltIn.ListOf(Var.Fn, Value.One);
            var errorMessage = Errors.Eval.InvalidFunctionCall;

            Assert.That(() => expr.Reduce(_ctx),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
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
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, ClCell.Nil, Var.Foo);
            var hof = BuiltIn.ListOf(ClSymbol.Lambda, BuiltIn.ListOf(Var.Foo), procedure);
            var definition = BuiltIn.ListOf(ClSymbol.Define, Var.Fn, hof);
            var application = BuiltIn.ListOf(BuiltIn.ListOf(Var.Fn, Value.One));

            var context = definition.Reduce(_ctx);
            var (actual, _) = application.Reduce(context);

            Assert.That(actual, Is.EqualTo(Value.One));
        }

        [Test]
        public void EvalApplication_SupportLexicalEnvironment()
        {
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, ClCell.Nil, Var.Foo);
            var hof = BuiltIn.ListOf(ClSymbol.Lambda, BuiltIn.ListOf(Var.Foo), procedure);
            var expr = BuiltIn.ListOf(BuiltIn.ListOf(hof, Value.One));

            var context = expr.Reduce(_ctx);

            Assert.That(context.Value, Is.EqualTo(Value.One));
        }

        /**
         * ((lambda (foo) foo)
         *   (define foo "foo")) // side effect
         */
        [Test]
        public void EvalApplication_EvalCompoundArgumentsBeforeFunctionBody()
        {
            var define = BuiltIn.ListOf(ClSymbol.Define, Var.Foo, Value.Foo);
            var lambda = BuiltIn.ListOf(ClSymbol.Lambda, BuiltIn.ListOf(Var.Foo), Var.Foo);
            var expr = BuiltIn.ListOf(lambda, define);

            var ctx = expr.Reduce(_ctx);

            Assert.That(ctx.Value, Is.EqualTo(ClCell.Nil));
            Assert.That(ctx.Env.Lookup(Var.Foo), Is.EqualTo(Value.Foo));
        }

        /**
         * ((lambda () bar))
         */
        [Test]
        public void EvalApplication_ThrowException_WhenBodyContainsUnboundVariable()
        {
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, ClCell.Nil, Var.Bar);
            var expr = BuiltIn.ListOf(procedure);
            Assert.That(() => expr.Reduce(_ctx),
                Throws.Exception.TypeOf<UnboundVariableException>().With.Message.EqualTo("Unbound variable bar"));
        }

        /**
         * ((lambda (foo)
         *   (if foo #t #f)) #f)
         */
        [Test]
        public void EvalApplication_FunctionWithCompoundBody()
        {
            var body = BuiltIn.ListOf(ClSymbol.If, Var.Foo, ClBool.True, ClBool.False);
            var parameters = BuiltIn.ListOf(Var.Foo);
            var compoundFn = BuiltIn.ListOf(ClSymbol.Lambda, parameters, body);
            var expr = BuiltIn.ListOf(compoundFn, ClBool.False);

            var context = expr.Reduce(_ctx);

            Assert.That(context.Value, Is.EqualTo(ClBool.False));
        }

        /**
         * ((lambda () 1))
         */
        [Test]
        public void EvalApplication_ZeroArityFunction()
        {
            var constantFn = BuiltIn.ListOf(ClSymbol.Lambda, ClCell.Nil, Value.One);
            var expr = BuiltIn.ListOf(constantFn);

            var context = expr.Reduce(_ctx);

            Assert.That(context.Value, Is.EqualTo(Value.One));
        }

        /**
         * (define foo "foo")
         * ((lambda () foo))
         */
        [Test]
        public void EvalAppliation_AccessToGlobalScope()
        {
            _env.Bind(Var.Foo, Value.Foo);
            var constantFn = BuiltIn.ListOf(ClSymbol.Lambda, ClCell.Nil, Var.Foo);
            var expr = BuiltIn.ListOf(constantFn);

            var context = expr.Reduce(_ctx);

            Assert.That(context.Value, Is.EqualTo(Value.Foo));
        }

        /**
         * ((lambda (foo) foo) "foo")
         */
        [Test]
        public void EvalApplication_IdentityFunction()
        {
            var parameters = BuiltIn.ListOf(Var.Foo);
            var identity = BuiltIn.ListOf(ClSymbol.Lambda, parameters, Var.Foo);
            var expr = BuiltIn.ListOf(identity, Value.Foo);

            var context = expr.Reduce(_ctx);

            Assert.That(context.Value, Is.EqualTo(Value.Foo));
        }
    }
}
