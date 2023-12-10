using Cl.Errors;
using Cl.IO;
using Cl.Types;

namespace Cl.Readers
{
    public class ClObjReader : IReader<ClObj>
    {
        private readonly List<IReader<ClObj>> _readers;

        public ClObjReader()
        {
            _readers = new List<IReader<ClObj>>
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
            if (source.Eof()) return ClCell.Nil;
            foreach (var reader in _readers)
            {
                if (reader.Read(source) is ClObj obj) return obj;
            }
            throw new SyntaxError("Unknown literal");
        }
    }
}
