using Cl.Extensions;
using Cl.IO;
using Cl.Types;

namespace Cl.Readers;

public class Reader(IReader<ClObj> reader) : IReader<ClObj>
{
    public Reader() : this (new ClObjReader()) { }

    public ClObj Read(ISource source)
    {
        var items = new List<ClObj> { ClSymbol.Begin };
        while (!source.Eof())
        {
            source.SkipComments();
            items.Add(reader.Read(source)!);
            source.SkipComments();
        }
        return BuiltIn.ListOf(items);
    }
}
