using System;
using Cl.Abs;
using Cl.Extensions;
using Cl.Types;

namespace Cl
{
    public class Evaluator
    {
        private readonly IEnv _env;

        public Evaluator(IEnv env)
        {
            _env = env;
        }

        public IClObj Eval(IClObj expr)
        {
            if (expr.IsSelfEvaluating()) return expr;
            if (expr.IsAssignment()) return EvalAssigment(expr);
            throw new InvalidOperationException("Evaluation error");
        }

        public IClObj EvalAssigment(IClObj expr)
        {
            // (set! a 10) -> (set! . (a . (10 . nil)))
            var symbol = BuiltIn.Second(expr).Cast<ClSymbol>();
            var obj = Eval(BuiltIn.Third(expr));
            _env.Assign(symbol, obj);
            return Nil.Given;
        }
    }
}
