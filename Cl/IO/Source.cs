using System.Text;

namespace Cl.IO;

public class Source(Stream stream) : ISource
{
    private readonly Stack<int> _buffer = new();

    public Source(string source) : this(new MemoryStream(Encoding.UTF8.GetBytes(source))) { }

    public void Dispose()
    {
        _buffer.Clear();
        stream.Dispose();
    }

    public int Read()
    {
        if (_buffer.TryPop(out var code)) return code;
        return stream.ReadByte();
    }

    public void Buffer(int code) => _buffer.Push(code);

    public int Peek()
    {
        if (_buffer.TryPeek(out var code)) return code;
        code = stream.ReadByte();
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
