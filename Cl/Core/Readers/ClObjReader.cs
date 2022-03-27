using System.Collections.Generic;
using Cl.Errors;
using Cl.IO;
using Cl.Types;

namespace Cl.Core.Readers
{
    public class ClObjReader : IClObjReader<ClObj>
    {
        private readonly List<IClObjReader<ClObj>> _readers;

        public ClObjReader()
        {
            _readers = new List<IClObjReader<ClObj>>
                {
                    new ClCharReader(),
                    new ClBoolReader(),
                    new ClStringReader(),
                    new ClDoubleReader(),
                    new ClIntReader(),
                    new ClCellReader(this),
                    new ClSymbolReader()
                };
        }

        public ClObj Read(ISource source)
        {
            source.RewindSpacesAndComments();
            if (source.Eof()) return ClCell.Nil;
            foreach (var reader in _readers)
            {
                if (reader.Read(source) is ClObj obj) return obj;
            }
            throw new SyntaxError("Unknown literal");
        }
    }
}
