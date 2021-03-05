using Cl.Errors;

namespace Cl.Types
{
    public class ClInt : ClNumber<int>
    {
        public ClInt(int number) : base(number) { }

        public static explicit operator ClChar(ClInt obj) => new ClChar((char) obj.Value);
        public static ClInt operator -(ClInt obj) => new ClInt(-obj.Value);

        public static ClObj operator +(ClInt @this, ClObj that) =>
            that switch
            {
                ClInt fixnum => new ClInt(@this.Value + fixnum.Value),
                ClDouble floatingPont => new ClDouble(@this.Value + floatingPont.Value),
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {that.GetType().Name}")
            };

        public static ClObj operator *(ClInt @this, ClObj that) =>
            that switch
            {
                ClInt fixnum => new ClInt(@this.Value * fixnum.Value),
                ClDouble floatingPont => new ClDouble(@this.Value * floatingPont.Value),
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {that.GetType().Name}")
            };

        public static ClObj operator / (ClInt @this, ClObj that) =>
            that switch
            {
                ClInt fixnum => new ClInt(@this.Value / fixnum.Value),
                ClDouble floatingPont => new ClDouble(@this.Value / floatingPont.Value),
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {that.GetType().Name}")

            };
    }
}
