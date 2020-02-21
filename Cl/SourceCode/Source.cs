using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cl.SourceCode
{
    public class Source : ISource
    {
        private readonly Stack<int> _buffer = new Stack<int>();
        private readonly Stream _stream;

        public Source(Stream stream)
        {
            _stream = stream;
        }

        public Source(string source) : this(new MemoryStream(Encoding.UTF8.GetBytes(source)))
        {
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public int Read()
        {
            if (_buffer.TryPop(out var charCode)) return (char) charCode;
            return _stream.ReadByte();
        }

        public void Buffer(int charCode) => _buffer.Push(charCode);

        public int Peek()
        {
            var charCode = Read();
            if (charCode == -1) return -1;
            _buffer.Push(charCode);
            return charCode;
        }

        public bool Eof() => Peek() == -1;

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
