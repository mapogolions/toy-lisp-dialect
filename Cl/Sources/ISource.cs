using System;

namespace Cl.Sources
{
    public interface ISource : IDisposable
    {
        int Read();
        void Buffer(int code);
        int Peek();
        bool Eof();
    }
}
