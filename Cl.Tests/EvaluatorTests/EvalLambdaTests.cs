using System.Collections.Generic;
using Cl.Contracts;
using Cl.Errors;
using Cl.Extensions;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalLambdaTests
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
        [TestCaseSource(nameof(InvalidParameterTestCases))]
        public void EvalLambda_ThrowException_WhenAtLeastOneParameterIsNotSymbolPrimitive(ClObj parameter)
        {
            var parameters = BuiltIn.ListOf(parameter, ClBool.True);
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, parameters, Value.One);
            var errorMessage = $"Binding statement should have {nameof(ClSymbol)} on the left-hand-side";

            Assert.That(() => expr.Reduce(_ctx),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        static IEnumerable<ClObj> InvalidParameterTestCases()
        {
            yield return Value.One;
            yield return Value.Foo;
            yield return BuiltIn.ListOf(ClSymbol.If, ClBool.True, Var.Foo, Var.Bar);
        }

        [Test]
        public void EvalLambda_BodyCanBeInvalidExpression()
        {
            var body = BuiltIn.ListOf(ClSymbol.Lambda, ClBool.True, ClBool.False);
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, ClCell.Nil, body);

            var fn = expr
                .Reduce(_ctx)
                .Value
                .Cast<ClFn>();

            Assert.That(fn, Is.Not.Null);
            Assert.That(fn.Body, Is.EqualTo(body));
        }

        [Test]
        [TestCaseSource(nameof(BodyExpressionTestCases))]
        public void EvalLambda_BodyCanBeAnyExpression(ClObj body)
        {
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, ClCell.Nil, body);
            var fn = expr
                .Reduce(_ctx)
                .Value
                .Cast<ClFn>();

            Assert.That(fn.Body, Is.EqualTo(body));
        }

        static IEnumerable<ClObj> BodyExpressionTestCases()
        {
            yield return Value.One;
            yield return Value.Foo;
            yield return BuiltIn.ListOf(ClSymbol.And, ClBool.True, ClCell.Nil);
            yield return BuiltIn.ListOf(ClSymbol.Begin, ClBool.True, ClCell.Nil);
            yield return BuiltIn.ListOf(ClSymbol.Lambda, BuiltIn.ListOf(Var.Foo), Var.Bar);
        }

        [Test]
        public void EvalLambda_ThrowException_WhenLambdaSpecialFormHasInvalidBody()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, ClCell.Nil, ClBool.True, ClBool.False);
            var errorMessage = "Invalid function body format";

            Assert.That(() => expr.Reduce(_ctx),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }

        [Test]
        public void EvalLambda_ReturnProcedureWithoutParams()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, ClCell.Nil, ClBool.True);
            var fn = expr
                .Reduce(_ctx)
                .Value
                .Cast<ClFn>();

            Assert.That(fn.Varargs, Is.EqualTo(ClCell.Nil));
        }

        [Test]
        public void EvalLambda_ThrowExceptionWhenParametersIsNotList()
        {
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, Var.Foo, Var.Foo);
            var errorMessage = "Invalid function parameters format";

            Assert.That(() => expr.Reduce(_ctx),
                Throws.Exception.TypeOf<SyntaxError>().With.Message.EqualTo(errorMessage));
        }
    }
}
