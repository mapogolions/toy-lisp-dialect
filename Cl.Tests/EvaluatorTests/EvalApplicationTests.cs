using Cl.Abs;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalApplicationTests
    {
        [Test]
        public void TryEvalApplication_EvalCompoundArgumentsBeforeFunctionBody()
        {
            var env = new Env();
            var x = new ClSymbol("x");
            var evaluator = new Evaluator(env);
            var compoundArg = BuiltIn.ListOf(ClSymbol.Define, x, new ClString("foo"));
            var parameters = BuiltIn.ListOf(x);
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, parameters, x);
            var expr = BuiltIn.ListOf(procedure, compoundArg);

            var actual = evaluator.Eval(expr);

            Assert.That(actual, Is.EqualTo(Nil.Given));
            Assert.That(env.Lookup(x), Is.EqualTo(new ClString("foo")));
        }

        [Test]
        public void TryEvalApplication_ThrowException_WhenBodyContainsUnboundVariable()
        {
            var evaluator = new Evaluator(new Env());
            var unboundVariable = new ClSymbol("x");
            var procedure = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, unboundVariable);
            var expr = BuiltIn.ListOf(procedure);
            var errorMessage = Errors.UnboundVariable(unboundVariable);

            Assert.That(() => evaluator.Eval(expr),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void TryEvalApplication_ApplyFunctionWithCompoundBody()
        {
            var evaluator = new Evaluator(new Env());
            var parameters = BuiltIn.ListOf(new ClSymbol("x"));
            var body = BuiltIn.ListOf(ClSymbol.IfThenElse, new ClSymbol("x"), new ClString("foo"), new ClString("bar"));
            var compoundFn = BuiltIn.ListOf(ClSymbol.Lambda, parameters, body);
            var expr = BuiltIn.ListOf(compoundFn, ClBool.False);

            var actual = evaluator.Eval(expr);

            Assert.That(actual, Is.EqualTo(new ClString("bar")));
        }

        [Test]
        public void TryEvalApplication_ApplyZeroArityFunction()
        {
            var evaluator = new Evaluator(new Env());
            var zeroArityFn = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, new ClString("bar"));
            var expr = BuiltIn.ListOf(zeroArityFn); // ((lambda () true))

            var actual = evaluator.Eval(expr);

            Assert.That(actual, Is.EqualTo(new ClString("bar")));
        }

        [Test]
        public void TryEvalAppliation_HaveAccessToGlobalScope()
        {
            var global = new Env();
            global.Bind(new ClSymbol("x"), new ClString("foo"));
            var evaluator = new Evaluator(global);
            var identity = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, new ClSymbol("x"));
            var expr = BuiltIn.ListOf(identity);

            var actual = evaluator.Eval(expr);

            Assert.That(actual, Is.EqualTo(new ClString("foo")));
        }

        [Test]
        public void TryEvalApplication_JustReturnPassedArgument()
        {
            var evaluator = new Evaluator(new Env());
            var parameters = BuiltIn.ListOf(new ClSymbol("x"));
            var body = new ClSymbol("x");
            var identity = BuiltIn.ListOf(ClSymbol.Lambda, parameters, body);
            var expr = BuiltIn.ListOf(identity, new ClFixnum(10));

            var actual = evaluator.Eval(expr);

            Assert.That(actual, Is.EqualTo(new ClFixnum(10)));
        }
    }
}
