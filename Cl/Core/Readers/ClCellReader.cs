using System;
using Cl.Errors;
using Cl.IO;
using Cl.Types;
using static Cl.Core.Helpers.FpUniverse;

namespace Cl.Core.Readers
{
    public class ClCellReader : IClObjReader<ClCell>
    {
        private ClObjReader _reader;

        public ClCellReader(ClObjReader reader)
        {
            _reader = reader;
        }

        public ClCell Read(ISource source)
        {
            if(TryReadNilOrNull(source, out var cell)) return cell;
            var car = _reader.Read(source);
            var wasDelimiter = source.TryRewindSpacesAndComments();
            if (!source.TryRewind(".")) return new ClCell(car, ReadList(source, wasDelimiter));
            var cdr = _reader.Read(source);
            Ignore(source.TryRewindSpacesAndComments());
            if (!source.TryRewind(")"))
            {
                throw new SyntaxError($"Invalid format of the {nameof(ClCell)} literal");
            }
            return new ClCell(car, cdr);
        }

        private bool TryReadNilOrNull(ISource source, out ClCell cell)
        {
            cell = default;
            if (!source.TryRewind("(")) return true;
            Ignore(source.TryRewindSpacesAndComments());
            if (source.TryRewind(")"))
            {
                cell = ClCell.Nil;
                return true;
            }
            return false;
        }

        private ClCell ReadList(ISource source, bool wasDelimiter)
        {
            if (source.TryRewind(")")) return ClCell.Nil;
            if (!wasDelimiter)
            {
                throw new SyntaxError($"Invalid format of the {nameof(ClCell)} literal");
            }
            var car = _reader.Read(source);
            wasDelimiter = source.TryRewindSpacesAndComments();
            if (source.TryRewind(")")) return new ClCell(car, ClCell.Nil);
            return new ClCell(car, ReadList(source, wasDelimiter));
        }
    }
}
