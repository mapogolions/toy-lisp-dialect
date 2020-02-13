using System;
using System.Collections.Generic;
using Cl.Types;

namespace Cl
{
    public interface IEnv
    {
        bool Bind(IClObj symbol, IClObj obj);
        IClObj Lookup(IClObj symbol);
        bool Assign(IClObj symbol, IClObj obj);
    }

    public class Env : IEnv
    {
        private readonly IEnv _parent;
        private readonly IDictionary<IClObj, IClObj> _bindings;

        public Env(IDictionary<IClObj, IClObj> bindings, IEnv parent)
        {
            _bindings = bindings;
            _parent = parent;
        }

        public Env() : this(new Dictionary<IClObj, IClObj>(), null) { }

        public bool Bind(IClObj symbol, IClObj obj)
        {
            _bindings[symbol] = obj;
            return true;
        }

        public IClObj Lookup(IClObj symbol)
        {
            if (_bindings.TryGetValue(symbol, out var obj))
                return obj;
            throw new InvalidOperationException("Unbound variable");
        }

        public bool Assign(IClObj symbol, IClObj obj)
        {
            if (_bindings.ContainsKey(symbol))
                return Bind(symbol, obj);
            var result = _parent?.Assign(symbol, obj) ?? false;
            if (result is false)
                throw new InvalidOperationException("Unbound variable");
            return true;
        }
    }
}
