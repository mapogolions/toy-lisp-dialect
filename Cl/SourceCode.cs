using System;
using System.IO;
using System.Text;

namespace Cl
{
    public class SourceCode : ISourceCode, IDisposable
    {
        private readonly Stream _stream;

        public SourceCode(Stream stream)
        {
            _stream = stream;
        }

        public SourceCode(string source) : this(new MemoryStream(Encoding.UTF8.GetBytes(source)))
        {

        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public int Read()
        {
            return _stream.ReadByte();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var ch = -1;
            while ((ch = Read()) != -1)
                builder.Append((char) ch);
            return builder.ToString();
        }
    }
}
