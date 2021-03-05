using Cl.Errors;

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
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {that.GetType().Name}")
            };

        public static ClObj operator *(ClDouble @this, ClObj that) =>
            that switch
            {
                ClInt fixnum => new ClDouble(@this.Value * fixnum.Value),
                ClDouble floatingPont => new ClDouble(@this.Value * floatingPont.Value),
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {that.GetType().Name}")
            };

        public static ClObj operator /(ClDouble @this, ClObj that) =>
            that switch
            {
                ClInt fixnum => new ClDouble(@this.Value / fixnum.Value),
                ClDouble floatingPont => new ClDouble(@this.Value / floatingPont.Value),
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {that.GetType().Name}")
            };
    }
}
