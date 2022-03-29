using Cl.Errors;
using Cl.IO;
using Cl.Types;

namespace Cl.Readers
{
    public class ClStringReader : IReader<ClString>
    {
        public ClString Read(ISource source)
        {
            if (!source.Rewind("'")) return null;
            string loop(string acc)
            {
                if (source.Eof())
                    throw new SyntaxError($"Invalid format of the {nameof(ClString)} literal");
                var ch = (char) source.Read();
                if (ch == '\'') return acc;
                return loop($"{acc}{ch}");
            }
            return new ClString(loop(string.Empty));
        }
    }
}
