using Cl.IO;
using Cl.Types;

namespace Cl.Readers
{
    public class ClSymbolReader : IReader<ClSymbol>
    {
        public ClSymbol? Read(ISource source)
        {
            if (source.Eof()) return null;
            if (TryCheckSpecialBuiltInFunction(source, out var fun)) return new ClSymbol(fun);
            var ch = (char) source.Peek();
            if (!char.IsLetter(ch)) return null;
            string loop(string acc)
            {
                if (source.Eof()) return acc;
                var ch = (char) source.Peek();
                if (!char.IsLetterOrDigit(ch) && ch != '-' && ch != '?' && ch != '!') return acc;
                return loop($"{acc}{(char) source.Read()}");
            }
            return new ClSymbol(loop($"{(char) source.Read()}"));
        }

        private static bool TryCheckSpecialBuiltInFunction(ISource source, out string symbol)
        {
            symbol = string.Empty;
            var builinFuncitons = new [] { "+", "-", "*", "/" };
            foreach (var fun in builinFuncitons)
            {
                if (!source.Rewind(fun)) continue;
                symbol = fun;
                return true;
            }
            return false;
        }
    }
}
