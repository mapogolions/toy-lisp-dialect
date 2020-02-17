using System;

namespace Cl
{
    public interface ISource : IDisposable
    {
        int Read();
        void Buffer(int code);
        int Peek();
        bool Eof();
    }
}
