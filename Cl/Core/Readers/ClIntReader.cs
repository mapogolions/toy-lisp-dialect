using Cl.IO;
using Cl.Types;

namespace Cl.Core.Readers
{
    public class ClIntReader : ClNumberReader, IClObjReader<ClInt>
    {
        public ClInt Read(ISource source)
        {
            if (!TryReadNumbersInRow(source, out var nums)) return null;
            if (!int.TryParse(nums, out var integer)) return null;
            return new ClInt(integer);
        }
    }
}
