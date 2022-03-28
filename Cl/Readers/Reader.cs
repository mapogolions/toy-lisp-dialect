using System.Collections.Generic;
using Cl.IO;
using Cl.Types;

namespace Cl.Readers
{
    public class Reader : IReader<ClObj>
    {
        private readonly IReader<ClObj> _reader;

        public Reader(IReader<ClObj> reader)
        {
            _reader = reader;
        }

        public Reader() : this (new ClObjReader()) { }

        public ClObj Read(ISource source)
        {
            var items = new List<ClObj> { ClSymbol.Begin };
            while (!source.Eof())
            {
                source.RewindSpacesAndComments();
                items.Add(_reader.Read(source));
                source.RewindSpacesAndComments();
            }
            return BuiltIn.ListOf(items);
        }
    }
}
