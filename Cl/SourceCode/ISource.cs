using System;

namespace Cl.SourceCode
{
    public interface ISource : IDisposable
    {
        int Read();
        void Buffer(int code);
        int Peek();
        bool Eof();
    }
}
