using System;
namespace Cl
{
    public interface ISource : IDisposable
    {
        int Read();
        void Buffer(int charCode);
        int Peek();
        bool Eof();
    }
}
