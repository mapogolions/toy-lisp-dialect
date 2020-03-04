using System;
using System.Collections.Generic;
using Cl.Types;

namespace Cl.Abs
{
    public interface IEnv
    {
        bool Bind(IClObj symbol, IClObj obj);
        IClObj Lookup(IClObj symbol);
        bool Assign(IClObj symbol, IClObj obj);
    }

    public class Env : IEnv
    {
        private readonly IEnv _frame;
        private readonly IDictionary<IClObj, IClObj> _bindings = new Dictionary<IClObj, IClObj>();

        public Env(IEnv frame = null)
        {
            _frame = frame;
        }

        public bool Bind(IClObj symbol, IClObj obj)
        {
            _bindings[symbol] = obj;
            return true;
        }

        public IClObj Lookup(IClObj symbol)
        {
            if (_bindings.TryGetValue(symbol, out var obj))
                return obj;
            var result = _frame?.Lookup(symbol);
            if (result is null)
                throw new InvalidOperationException("Unbound variable");
            return result;
        }

        public bool Assign(IClObj symbol, IClObj obj)
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
