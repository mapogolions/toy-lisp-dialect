using System;
namespace Cl
{
    public interface ISource : IDisposable
    {
        int Read();
        int Peek();
    }
}
