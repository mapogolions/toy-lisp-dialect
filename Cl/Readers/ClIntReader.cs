using Cl.IO;
using Cl.Types;

namespace Cl.Readers
{
    public class ClIntReader : ClNumberReader, IReader<ClInt>
    {
        public ClInt Read(ISource source)
        {
            if (!TryReadNumbersInRow(source, out var nums)) return null;
            if (!int.TryParse(nums, out var integer)) return null;
            return new ClInt(integer);
        }
    }
}
