using System.Collections.Generic;
using Cl.Abs;
using Cl.Extensions;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalApplicationTests
    {
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
