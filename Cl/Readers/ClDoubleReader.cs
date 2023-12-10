using System.Globalization;
using Cl.Extensions;
using Cl.Errors;
using Cl.IO;
using Cl.Types;

namespace Cl.Readers
{
    public class ClDoubleReader : ClNumberReader, IReader<ClDouble>
    {
        public ClDouble Read(ISource source)
        {
            if (!TryReadAtLeastOneNumber(source, out var significand)) return null;
            if (!source.Rewind("."))
            {
                significand.Reverse().ForEach(ch => source.Buffer(ch));
                return null;
            }
            if (!TryReadAtLeastOneNumber(source, out var mantissa))
                throw new SyntaxError($"Invalid format of the {nameof(ClDouble)} literal");
            var number = double.Parse($"{significand}.{mantissa}", NumberStyles.Float,
                CultureInfo.InvariantCulture);
            return new ClDouble(number);
        }
    }
}
