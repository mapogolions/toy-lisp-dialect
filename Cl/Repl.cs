using System;
using System.Collections.Generic;
using Cl.Scope;
using Cl.Types;

namespace Cl
{
    public class Repl
    {
        private readonly IEnv _env;
        private readonly IDictionary<string, IClObj> _symbolsTable;

        public Repl(IEnv env, IDictionary<string, IClObj> symbolsTable)
        {
            _env = env;
            _symbolsTable = symbolsTable;
        }

        public Repl() : this(new Env(), new Dictionary<string, IClObj>()) { }

        public void Start(string sign = ">")
        {
            throw new NotImplementedException();
        }
    }
}
