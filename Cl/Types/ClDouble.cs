namespace Cl.Types
{
    public class ClDouble : ClNumber<double>
    {
        public ClDouble(double number) : base(number) { }

        public static ClDouble operator -(ClDouble obj) => new ClDouble(-obj.Value);
    }
}
