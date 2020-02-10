using System;
using System.Collections.Generic;
using System.Linq;
using Cl.Types;

namespace Cl
{
    public class BuiltIn
    {
        private readonly IDictionary<string, ClSymbol> _symbolsTable;

        public BuiltIn(IDictionary<string, ClSymbol> symbolsTable)
        {
            _symbolsTable = symbolsTable;
        }

        public BuiltIn() : this(new Dictionary<string, ClSymbol>()) { }

        public ClSymbol MakeSymbol(string name)
        {
            var foundSymbol = _symbolsTable.FirstOrDefault(it => it.Key.Equals(name)).Value;
            if (foundSymbol != null) return foundSymbol;
            var symbol = new ClSymbol(name);
            _symbolsTable.Add(name, symbol);
            return symbol;
        }

        public ClPair MakePair(IClObj car, IClObj cdr) => new ClPair { Car = car, Cdr = cdr };

        public IClObj Car(IClObj obj) =>
            obj switch
            {
                ClPair pair => pair.Car,
                _ => throw new ArgumentException(nameof(obj), $"Argument is not a {nameof(ClPair)}")
            };

        public IClObj Cdr(IClObj obj) =>
            obj switch
            {
                ClPair pair => pair.Cdr,
                _ => throw new ArgumentException(nameof(obj), $"Argument is not a {nameof(ClPair)}")
            };
    }
}
