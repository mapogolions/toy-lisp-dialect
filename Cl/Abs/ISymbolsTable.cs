using System.Collections.Generic;
using Cl.Types;

namespace Cl.Abs
{
    public interface ISymbolsTable
    {
        ClSymbol CreateIfNotExists(string name);
    }

    public class DefaultSymbolsTable : ISymbolsTable
    {
        private readonly IDictionary<string, ClSymbol> _items = new Dictionary<string, ClSymbol>
            {
                ["quote"] = new ClSymbol("quote"),
                ["set!"] = new ClSymbol("set!"),
                ["define"] = new ClSymbol("define"),
                ["if"] = new ClSymbol("if"),
                ["else"] = new ClSymbol("else"),
                ["cond"] = new ClSymbol("cond"),
                ["lambda"] = new ClSymbol("lambda"),
                ["begin"] = new ClSymbol("begin"),
                ["let"] = new ClSymbol("let"),
                ["and"] = new ClSymbol("and"),
                ["or"] = new ClSymbol("or")
            };

        public ClSymbol CreateIfNotExists(string name)
        {
            if (_items.TryGetValue("name", out var symbol)) return symbol;
            var atom = new ClSymbol(name);
            _items.Add(name, atom);
            return atom;
        }
    }
}
