using System.Linq;
using System;
using System.Collections.Generic;
using Cl.Types;
using Cl.Extensions;

namespace Cl.Abs
{
    public interface IEnv
    {
        bool Bind(ClSymbol identifier, IClObj obj);
        IClObj Lookup(ClSymbol indentifier);
        bool Assign(ClSymbol identifier, IClObj obj);
        IEnv Extend(ClCell identifiers, ClCell values);
        bool IsGlobal { get; }
    }

    public class Env : IEnv
    {
        private readonly IEnv _parent;
        private readonly IDictionary<ClSymbol, IClObj> _bindings = new Dictionary<ClSymbol, IClObj>();

        public Env(IEnv parent = null)
        {
            _parent = parent;
        }

        public bool IsGlobal => _parent is null;

        public bool Bind(ClSymbol symbol, IClObj obj)
        {
            _bindings[symbol] = obj;
            return true;
        }

        public IClObj Lookup(ClSymbol identifier)
        {
            if (_bindings.TryGetValue(identifier, out var obj))
                return obj;
            var result = _parent?.Lookup(identifier);
            if (result is null)
                throw new InvalidOperationException("Unbound variable");
            return result;
        }

        public bool Assign(ClSymbol identifier, IClObj obj)
        {
            if (_bindings.ContainsKey(identifier))
                return Bind(identifier, obj);
            var result = _parent?.Assign(identifier, obj) ?? false;
            if (result is false)
                throw new InvalidOperationException("Unbound variable");
            return true;
        }

        public IEnv Extend(ClCell identifiers, ClCell values)
        {
            var env = new Env(this);
            var pairs = BuiltIn.Seq(identifiers).Cast<ClSymbol>().BalancedZip(BuiltIn.Seq(values));
            pairs.ForEach(pair => env.Bind(pair.First, pair.Second));
            return env;
        }
    }
}
