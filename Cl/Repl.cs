using System;
using Cl.Abs;

namespace Cl
{
    public class Repl
    {
        private readonly IEnv _env;

        public Repl(IEnv env)
        {
            _env = env;
        }

        public Repl() : this(new Env()) { }

        public void Start(string sign = ">")
        {
            throw new NotImplementedException();
        }
    }
}
