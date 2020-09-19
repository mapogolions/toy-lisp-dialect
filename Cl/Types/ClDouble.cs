using System;

namespace Cl.Types
{
    public class ClDouble : ClNumber<double>
    {
        public ClDouble(double number) : base(number) { }

        public static ClDouble operator -(ClDouble obj) => new ClDouble(-obj.Value);
        public static ClObj operator +(ClDouble @this, ClObj that) =>
            that switch
            {
                ClInt fixnum => new ClDouble(@this.Value + fixnum.Value),
                ClDouble floatingPont => new ClDouble(@this.Value + floatingPont.Value),
                _ => throw new InvalidOperationException()
            };

        public static ClObj operator *(ClDouble @this, ClObj that) =>
            that switch
            {
                ClInt fixnum => new ClDouble(@this.Value * fixnum.Value),
                ClDouble floatingPont => new ClDouble(@this.Value * floatingPont.Value),
                _ => throw new InvalidOperationException()
            };

        public static ClObj operator /(ClDouble @this, ClObj that) =>
            that switch
            {
                ClInt fixnum => new ClDouble(@this.Value / fixnum.Value),
                ClDouble floatingPont => new ClDouble(@this.Value / floatingPont.Value),
                _ => throw new InvalidOperationException()
            };
    }
}
