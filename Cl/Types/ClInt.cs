using System;

namespace Cl.Types
{
    public class ClInt : ClNumber<int>
    {
        public ClInt(int number) : base(number) { }

        public static explicit operator ClChar(ClInt obj) => new ClChar((char) obj.Value);
        public static ClInt operator -(ClInt obj) => new ClInt(-obj.Value);

        public static IClObj operator +(ClInt @this, IClObj that) =>
            that switch
            {
                ClInt fixnum => new ClInt(@this.Value + fixnum.Value),
                ClDouble floatingPont => new ClDouble(@this.Value + floatingPont.Value),
                _ => throw new InvalidOperationException()
            };

        public static IClObj operator *(ClInt @this, IClObj that) =>
            that switch
            {
                ClInt fixnum => new ClInt(@this.Value * fixnum.Value),
                ClDouble floatingPont => new ClDouble(@this.Value * floatingPont.Value),
                _ => throw new InvalidOperationException()
            };

        public static IClObj operator / (ClInt @this, IClObj that) =>
            that switch
            {
                ClInt fixnum => new ClInt(@this.Value / fixnum.Value),
                ClDouble floatingPont => new ClDouble(@this.Value / floatingPont.Value),
                _ => throw new InvalidOperationException()
            };
    }
}
