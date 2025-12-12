using Cl.Errors;
using Cl.Extensions;
using Cl.IO;
using Cl.Types;

namespace Cl.Readers;

public class ClCellReader(ClObjReader reader) : IReader<ClCell>
{
    public ClCell? Read(ISource source)
    {
        if(TryReadNilOrNull(source, out var cell))
        {
            return cell;
        }
        var car = reader.Read(source)!;
        var wasDelimiter = source.SkipComments();
        if (!source.Skip("."))
        {
            return new ClCell(car, ReadList(source, wasDelimiter));
        }
        source.SkipComments();
        var cdr = reader.Read(source)!;
        source.SkipComments();
        if (!source.Skip(")"))
        {
            throw new SyntaxError($"Invalid format of the {nameof(ClCell)} literal");
        }
        return new ClCell(car, cdr);
    }

    private static bool TryReadNilOrNull(ISource source, out ClCell? cell)
    {
        cell = default;
        if (!source.Skip("(")) return true;
        source.SkipComments();
        if (source.Skip(")"))
        {
            cell = ClCell.Nil;
            return true;
        }
        return false;
    }

    private ClCell ReadList(ISource source, bool wasDelimiter)
    {
        if (source.Skip(")")) return ClCell.Nil;
        if (!wasDelimiter)
        {
            throw new SyntaxError($"Invalid format of the {nameof(ClCell)} literal");
        }
        var car = reader.Read(source);
        wasDelimiter = source.SkipComments();
        if (source.Skip(")")) return new ClCell(car, ClCell.Nil);
        return new ClCell(car, ReadList(source, wasDelimiter));
    }
}
