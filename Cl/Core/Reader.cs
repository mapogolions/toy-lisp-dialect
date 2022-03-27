using System.Collections.Generic;
using Cl.IO;
using Cl.Types;
using Cl.Core.Readers;

namespace Cl.Core
{
    public class Reader : IReader
    {
        private readonly ISource _source;
        private readonly IClObjReader<ClObj> _reader;

        public Reader(ISource source, IClObjReader<ClObj> reader)
        {
            _source = source;
            _reader = reader;
        }

        public Reader(string source) : this(new Source(source), new ClObjReader()) { }

        public IEnumerable<ClObj> Read()
        {
            var items = new List<ClObj>();
            while (!_source.Eof())
            {
                items.Add(_reader.Read(_source));
                _source.RewindSpacesAndComments();
            }
            return items;
        }

        public void Dispose()
        {
            _source.Dispose();
        }
    }
}
