using System;

namespace Cl.Types
{
    public class Proc<A, B> : IClObj
    {
        private readonly Func<A, B> _proc;

        public Proc(Func<A, B> proc)
        {
            _proc = proc;
        }

        public B Apply(A domain) => _proc.Invoke(domain);
    }
}
