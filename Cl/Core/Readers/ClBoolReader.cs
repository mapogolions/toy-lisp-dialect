using Cl.Errors;
using Cl.IO;
using Cl.Types;

namespace Cl.Core.Readers
{
    public class ClBoolReader : IClObjReader<ClBool>
    {
        public ClBool Read(ISource source)
        {
            if (!source.TryRewind("#")) return null;
            if (source.TryRewind("t")) return ClBool.True;
            if (source.TryRewind("f")) return ClBool.False;
            throw new SyntaxError($"Invalid format of the {nameof(ClBool)} literal");
        }
    }
}
