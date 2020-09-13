namespace Cl.Types
{
    public class ClInt : ClNumber<int>
    {
        public ClInt(int number) : base(number) { }

        public static explicit operator ClChar(ClInt obj) => new ClChar((char) obj.Value);
        public static ClInt operator -(ClInt obj) => new ClInt(-obj.Value);
    }
}
