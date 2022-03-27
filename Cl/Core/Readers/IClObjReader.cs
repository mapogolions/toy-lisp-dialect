using Cl.IO;
using Cl.Types;

namespace Cl.Core.Readers
{
    public interface IClObjReader<out TClObj> where TClObj : ClObj
    {
        TClObj Read(ISource source);
    }
}
