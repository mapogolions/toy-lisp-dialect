using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cl.Input
{
    public class Source : ISource
    {
        private readonly Stack<int> _buffer;
        private readonly Stream _stream;

        public Source(Stream stream)
        {
            _stream = stream;
            _buffer = new Stack<int>();
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
            if (_buffer.TryPop(out var code)) return (char) code;
            return _stream.ReadByte();
        }

        public void Buffer(int code) => _buffer.Push(code);

        public int Peek()
        {
            var code = Read();
            if (code == -1) return -1;
            _buffer.Push(code);
            return code;
        }

        public bool Eof() => Peek() == -1;

        public override string ToString()
        {
            var ctor = new StringBuilder();
            while (!Eof()) ctor.Append((char) Read());
            return ctor.ToString();
        }
    }
}
