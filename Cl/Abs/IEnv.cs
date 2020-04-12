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

        public Env(params (ClSymbol, IClObj)[] pairs)
        {
            foreach (var pair in pairs)
            {
                Bind(pair.Item1, pair.Item2);
            }
        }

        public bool IsGlobal => _parent is null;

        public bool Bind(ClSymbol identifier, IClObj obj)
        {
            _bindings[identifier] = obj;
            return true;
        }

        public IClObj Lookup(ClSymbol identifier)
        {
            if (_bindings.TryGetValue(identifier, out var obj))
                return obj;
            var result = _parent?.Lookup(identifier);
            if (result is null)
                throw new InvalidOperationException(Errors.UnboundVariable(identifier));
            return result;
        }

        public bool Assign(ClSymbol identifier, IClObj obj)
        {
            if (_bindings.ContainsKey(identifier))
                return Bind(identifier, obj);
            var result = _parent?.Assign(identifier, obj) ?? false;
            if (result) return true;
            throw new InvalidOperationException(Errors.UnboundVariable(identifier));
        }

        public IEnv Extend(ClCell identifiers, ClCell values)
        {
            var env = new Env(this);
            BuiltIn.Seq(identifiers)
                .Cast<ClSymbol>()
                .BalancedZip(BuiltIn.Seq(values))
                .ForEach(pair => env.Bind(pair.First, pair.Second));
            return env;
        }
    }
}
