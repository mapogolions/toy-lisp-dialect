using Cl.IO;
using Cl.Types;

namespace Cl.Readers
{
    public interface IReader<out TClObj> where TClObj : ClObj
    {
        TClObj Read(ISource source);
    }
}
