using Cl.Types;

namespace Cl
{
    public class Evaluator
    {
        private readonly IClObj _ast;

        public Evaluator(IClObj ast)
        {
            _ast = ast;
        }
    }
}
