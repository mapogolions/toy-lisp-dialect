using System;
using System.Collections.Generic;
using Cl.Abs;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalIfElseTests
    {
        [Test]
        [TestCaseSource(nameof(FalsyTestCases))]
        public void EvalIfThenElse_EvalElseBranch_WhenConditionIsFalse(IClObj obj)
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.IfThenElse, obj, ClBool.False, ClBool.True);

            Assert.That(evaluator.EvalIfThenElse(expr), Is.EqualTo(ClBool.True));
        }

        static IEnumerable<IClObj> FalsyTestCases()
        {
            yield return ClBool.False;
            yield return Nil.Given;
        }

        [Test]
        [TestCaseSource(nameof(TruthyTestCases))]
        public void EvalIfThenElse_EvalThenBranch_WhenConditionIsTrue(IClObj obj)
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.IfThenElse, obj, ClBool.True, ClBool.False);

            Assert.That(evaluator.EvalIfThenElse(expr), Is.EqualTo(ClBool.True));
        }

        static IEnumerable<IClObj> TruthyTestCases()
        {
            yield return new ClFixnum(0);
            yield return new ClFloat(0.0);
            yield return new ClString(string.Empty);
            yield return ClBool.True;
            yield return new ClChar('\0');
            yield return BuiltIn.Quote(BuiltIn.ListOf(ClBool.False)); // (false . nil)
            yield return BuiltIn.Quote(BuiltIn.ListOf(Nil.Given)); // (nil . nil)
        }


        [Test]
        public void EvalIfThenElse_ReturnNil_WhenConditionIsFalseAndElseBranchIsSkipped()
        {
            var evaluator = new Evaluator(new Env());
            var expr = BuiltIn.ListOf(ClSymbol.IfThenElse, ClBool.False, new ClFixnum(1));

            Assert.That(evaluator.EvalIfThenElse(expr), Is.EqualTo(Nil.Given));
        }
    }
}
