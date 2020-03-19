using System;
using System.Collections.Generic;
using Cl.Types;

namespace Cl.Abs
{
    public interface IEnv
    {
        bool Bind(ClSymbol symbol, IClObj obj);
        IClObj Lookup(ClSymbol symbol);
        bool Assign(ClSymbol symbol, IClObj obj);
    }

    public class Env : IEnv
    {
        private readonly IEnv _frame;
        private readonly IDictionary<ClSymbol, IClObj> _bindings = new Dictionary<ClSymbol, IClObj>();

        public Env(IEnv frame = null)
        {
            _frame = frame;
        }

        public bool Bind(ClSymbol symbol, IClObj obj)
        {
            _bindings[symbol] = obj;
            return true;
        }

        public IClObj Lookup(ClSymbol symbol)
        {
            if (_bindings.TryGetValue(symbol, out var obj))
                return obj;
            var result = _frame?.Lookup(symbol);
            if (result is null)
                throw new InvalidOperationException("Unbound variable");
            return result;
        }

        public bool Assign(ClSymbol symbol, IClObj obj)
        {
            if (_bindings.ContainsKey(symbol))
                return Bind(symbol, obj);
            var result = _frame?.Assign(symbol, obj) ?? false;
            if (result is false)
                throw new InvalidOperationException("Unbound variable");
            return true;
        }
    }
}
