using Cl.Errors;
using Cl.Extensions;
using Cl.IO;
using Cl.Types;

namespace Cl.Readers
{
    public class ClBoolReader : IReader<ClBool>
    {
        public ClBool? Read(ISource source)
        {
            if (!source.Skip("#")) return null;
            if (source.Skip("t")) return ClBool.True;
            if (source.Skip("f")) return ClBool.False;
            throw new SyntaxError($"Invalid format of the {nameof(ClBool)} literal");
        }
    }
}
