namespace Cl.Types
{
    public sealed class Nil : ClPair
    {
        private static Nil _instance;

        private Nil() : base(null, null) { }

        public static Nil Given
        {
            get
            {
                if (_instance is null) _instance = new Nil();
                return _instance;
            }
        }
    }
}
