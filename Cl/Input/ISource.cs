using System;

namespace Cl.Input
{
    public interface ISource : IDisposable
    {
        int Read();
        void Buffer(int code);
        int Peek();
        bool Eof();
    }
}
