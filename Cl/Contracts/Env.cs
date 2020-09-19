using System;
using System.Collections.Generic;
using Cl.Extensions;
using Cl.Types;

namespace Cl.Contracts
{
     public class Env : IEnv
    {
        private readonly IEnv _parent;
        private readonly IDictionary<ClSymbol, ClObj> _bindings = new Dictionary<ClSymbol, ClObj>();

        public Env(IEnv parent = null)
        {
            _parent = parent;
        }

        public Env(params (ClSymbol, ClObj)[] pairs)
        {
            foreach (var pair in pairs)
            {
                Bind(pair.Item1, pair.Item2);
            }
        }

        public bool Bind(ClSymbol identifier, ClObj obj)
        {
            _bindings[identifier] = obj;
            return true;
        }

        public ClObj Lookup(ClSymbol identifier)
        {
            if (_bindings.TryGetValue(identifier, out var obj))
                return obj;
            var result = _parent?.Lookup(identifier);
            if (result is null)
                throw new InvalidOperationException(Errors.UnboundVariable(identifier));
            return result;
        }

        public bool Assign(ClSymbol identifier, ClObj obj)
        {
            if (_bindings.ContainsKey(identifier))
                return Bind(identifier, obj);
            var result = _parent?.Assign(identifier, obj) ?? false;
            if (result) return true;
            throw new InvalidOperationException(Errors.UnboundVariable(identifier));
        }

        public bool Bind(IEnumerable<ClObj> identifiers, IEnumerable<ClObj> values)
        {
            var pairs = identifiers.ZipIfBalanced(values);
            pairs.ForEach(pair => Bind(pair.First as ClSymbol, pair.Second));
            return true;
        }
    }
}
