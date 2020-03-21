using System;

namespace Cl.Types
{
    public class ClProc : IClObj
    {
        private readonly ClCell _varargs;
        private readonly IClObj _body;

        public ClProc(ClCell varargs, IClObj body)
        {
            _varargs = varargs;
            _body = body;
        }
    }
}
