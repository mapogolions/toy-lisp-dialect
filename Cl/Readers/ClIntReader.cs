using Cl.IO;
using Cl.Types;

namespace Cl.Readers
{
    public class ClIntReader : ClNumberReader, IReader<ClInt>
    {
        public ClInt Read(ISource source)
        {
            if (!TryReadAtLeastOneNumber(source, out var nums)) return null;
            return new ClInt(int.Parse(nums));
        }
    }
}
