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
        public void TryEvalLambda_ThrowException_WhenLambdaSpecialFormHasInvalidBodyFormat()
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

            var actual = evaluator.Eval(expr).Cast<ClProc>();

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
