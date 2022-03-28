using Cl.Errors;
using Cl.IO;
using Cl.Types;

namespace Cl.Readers
{
    public class ClBoolReader : IReader<ClBool>
    {
        public ClBool Read(ISource source)
        {
            if (!source.Rewind("#")) return null;
            if (source.Rewind("t")) return ClBool.True;
            if (source.Rewind("f")) return ClBool.False;
            throw new SyntaxError($"Invalid format of the {nameof(ClBool)} literal");
        }
    }
}
