namespace Cl.Types
{
    public sealed class Nil : IClObj
    {
        private static Nil _instance;

        private Nil() { }

        public Nil Given
        {
            get
            {
                if (_instance is null) _instance = new Nil();
                return _instance;
            }
        }
    }
}
