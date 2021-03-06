using System.Collections.Generic;
using System.Linq;
using Cl.Contracts;
using Cl.Errors;
using Cl.Extensions;
using Cl.Types;

namespace Cl
{
     public class Env : IEnv
    {
        private readonly IEnv _outer;
        private readonly IDictionary<ClSymbol, ClObj> _bindings = new Dictionary<ClSymbol, ClObj>();

        public Env(IEnv outer = null)
        {
            _outer = outer;
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
            var result = _outer?.Lookup(identifier);
            if (result is null)
                throw new UnboundVariableError($"{identifier}");
            return result;
        }

        public bool Assign(ClSymbol identifier, ClObj obj)
        {
            if (_bindings.ContainsKey(identifier))
                return Bind(identifier, obj);
            var result = _outer?.Assign(identifier, obj) ?? false;
            if (result) return true;
            throw new UnboundVariableError($"{identifier}");
        }

        public bool Bind(IEnumerable<ClObj> identifiers, IEnumerable<ClObj> values)
        {
            var arity = identifiers.Count();
            var passed = values.Count();
            if (arity != passed)
            {
                var s = arity == 0 ? string.Empty : "s";
                throw new TypeError($"Arity exception: function expects {arity} arg{s}, but passed {passed}");
            }
            var pairs = identifiers.ZipIfBalanced(values);
            pairs.ForEach(pair => Bind(pair.First.Cast<ClSymbol>(), pair.Second));
            return true;
        }
    }
}
