using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cl
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
            if (_buffer.TryPop(out var code)) return (char) code;
            return _stream.ReadByte();
        }

        public int Peek()
        {
            var code = Read();
            if (code == -1) return -1;
            _buffer.Push(code);
            return code;
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
