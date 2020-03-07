using Cl.Abs;
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
            return expr;
        }

        public IClObj EvalAssigment(IClObj expr)
        {
            var symbol = BuiltIn.Cadr(expr);
            var obj = Eval(BuiltIn.Caddr(expr));
            _env.Assign(symbol, obj);
            return Nil.Given;
        }
    }
}
