using System.Collections.Generic;
using Cl.IO;
using Cl.Types;
using Cl.Errors;

namespace Cl.Core.Readers
{
    public class ClCharReader : IClObjReader<ClChar>
    {
        public ClChar Read(ISource source)
        {
            if (!source.TryRewind(@"#\")) return null;
            foreach (var (word, ch) in SpecialChars)
            {
                if (!source.TryRewind(word)) continue;
                return new ClChar(ch);
            }
            if (source.Eof())
                 throw new SyntaxError($"Invalid format of the {nameof(ClChar)} literal");
            return new ClChar((char) source.Read());
        }

        private IDictionary<string, char> SpecialChars = new Dictionary<string, char>
            {
                ["newline"] = '\n',
                ["tab"] = '\t',
                ["space"] = ' '
            };
    }
}
