using System.Collections.Generic;
using Cl.Abs;
using Cl.Extensions;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalLambdaTests
    {
        [Test]
        public void TryEvalLambda_BodyCanBeInvalidExpression()
        {
            var evaluator = new Evaluator(new Env());
            var body = BuiltIn.ListOf(ClSymbol.Lambda, ClBool.True, ClBool.False);
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, body);

            var actual = evaluator.Eval(expr).TypeOf<ClProc>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Body, Is.EqualTo(body));
        }

        [Test]
        [TestCaseSource(nameof(BodyExpressionTestCases))]
        public void TryEvalLambda_BodyCanBeAnyExpression(IClObj body)
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, body);

            var actual = evaluator.Eval(expr).TypeOf<ClProc>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Body, Is.EqualTo(body));
        }

        static IEnumerable<IClObj> BodyExpressionTestCases()
        {
            yield return new ClFixnum(10);
            yield return new ClString("foo");
            yield return BuiltIn.ListOf(ClSymbol.And, ClBool.True, Nil.Given);
            yield return BuiltIn.ListOf(ClSymbol.Begin, ClBool.True, Nil.Given);
            yield return BuiltIn.ListOf(ClSymbol.Lambda, BuiltIn.ListOf(new ClSymbol("x")), new ClSymbol("x"));
        }

        [Test]
        public void TryEvalLambda_ThrowException_WhenLambdaSpecialFormHasInvalidBody()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, ClBool.True, ClBool.False);

            Assert.That(() => evaluator.Eval(expr),
                Throws.InvalidOperationException.With.Message.EqualTo("Invalid body"));
        }

        [Test]
        public void TryEvalLambda_ReturnProcedureWithoutParams()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, Nil.Given, ClBool.True);

            var actual = evaluator.Eval(expr).TypeOf<ClProc>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Varargs, Is.EqualTo(Nil.Given));
        }

        [Test]
        public void TryEvalLambda_ThrowExceptionWhenParametersIsNotList()
        {
            var evaluator = new Evaluator(new Env());
            var operands = new ClSymbol("x");
            var expr = BuiltIn.ListOf(ClSymbol.Lambda, operands, operands);

            Assert.That(() => evaluator.Eval(expr),
                Throws.InvalidOperationException.With.Message.EqualTo("Operands must be a cell"));
        }
    }
}