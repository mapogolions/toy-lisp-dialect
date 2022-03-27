using System.Globalization;
using System.Linq;
using Cl.Core.Extensions;
using Cl.Errors;
using Cl.IO;
using Cl.Types;

namespace Cl.Core.Readers
{
    public class ClDoubleReader : ClNumberReader, IClObjReader<ClDouble>
    {
        public ClDouble Read(ISource source)
        {
            if (!TryReadNumbersInRow(source, out var significand)) return null;
            if (!source.TryRewind("."))
            {
                significand.Reverse().ForEach(ch => source.Buffer(ch));
                return null;
            }
            if (!TryReadNumbersInRow(source, out var mantissa))
            {
                throw new SyntaxError($"Invalid format of the {nameof(ClDouble)} literal");
            }
            var number = double.Parse($"{significand}.{mantissa}", NumberStyles.Float,
                CultureInfo.InvariantCulture);
            return new ClDouble(number);
        }
    }
}
