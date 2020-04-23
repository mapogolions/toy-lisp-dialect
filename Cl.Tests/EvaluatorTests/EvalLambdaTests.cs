using System.Collections.Generic;

using Cl.Extensions;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalLambdaTests
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
        [TestCaseSource(nameof(InvalidParameterTestCases))]
        public void EvalLambda_ThrowException_WhenAtLeastOneParameterIsNotSymbolPrimitive(IClObj parameter)
        {
            var parameters = BuiltIn.ListOf(parameter, ClBool.True);
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, parameters, Value.One);
            var errorMessage = Errors.BuiltIn.UnsupportBinding;

            Assert.That(() => _evaluator.Eval(expr),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }

        static IEnumerable<IClObj> InvalidParameterTestCases()
        {
            yield return Value.One;
            yield return Value.Foo;
            yield return BuiltIn.ListOf(ClSymbol.If, ClBool.True, Var.Foo, Var.Bar);
        }

        [Test]
        public void EvalLambda_BodyCanBeInvalidExpression()
        {
            var body = BuiltIn.ListOf(ClSymbol.Lambda, ClBool.True, ClBool.False);
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, body);

            var actual = _evaluator.Eval(expr).TypeOf<ClFn>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Body, Is.EqualTo(body));
        }

        [Test]
        [TestCaseSource(nameof(BodyExpressionTestCases))]
        public void EvalLambda_BodyCanBeAnyExpression(IClObj body)
        {
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, body);

            var actual = _evaluator.EvalLambda(expr).TypeOf<ClFn>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Body, Is.EqualTo(body));
        }

        static IEnumerable<IClObj> BodyExpressionTestCases()
        {
            yield return Value.One;
            yield return Value.Foo;
            yield return BuiltIn.ListOf(ClSymbol.And, ClBool.True, Nil.Given);
            yield return BuiltIn.ListOf(ClSymbol.Begin, ClBool.True, Nil.Given);
            yield return BuiltIn.ListOf(ClSymbol.Lambda, BuiltIn.ListOf(Var.Foo), Var.Bar);
        }

        [Test]
        public void EvalLambda_ThrowException_WhenLambdaSpecialFormHasInvalidBody()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, ClBool.True, ClBool.False);
            var errorMessage = Errors.Eval.InvalidLambdaBody;

            Assert.That(() => _evaluator.EvalLambda(expr),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void EvalLambda_ReturnProcedureWithoutParams()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, ClBool.True);

            var actual = _evaluator.EvalLambda(expr).TypeOf<ClFn>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Varargs, Is.EqualTo(Nil.Given));
        }

        [Test]
        public void EvalLambda_ThrowExceptionWhenParametersIsNotList()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, Var.Foo, Var.Foo);
            var errorMessage = Errors.Eval.InvalidLambdaParameters;

            Assert.That(() => _evaluator.EvalLambda(expr),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }
    }
}
