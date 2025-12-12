using Cl.Errors;

namespace Cl.Types;

public class ClInt(int number) : ClNumber<int>(number)
{
    public static explicit operator ClChar(ClInt obj) => new((char) obj.Value);
    public static ClInt operator -(ClInt obj) => new(-obj.Value);

    public static ClObj operator +(ClInt @this, ClObj other) =>
        other switch
        {
            ClInt fixnum => new ClInt(@this.Value + fixnum.Value),
            ClDouble floatingPont => new ClDouble(@this.Value + floatingPont.Value),
            _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {other.GetType().Name}")
        };

    public static ClObj operator *(ClInt @this, ClObj other) =>
        other switch
        {
            ClInt fixnum => new ClInt(@this.Value * fixnum.Value),
            ClDouble floatingPont => new ClDouble(@this.Value * floatingPont.Value),
            _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {other.GetType().Name}")
        };

    public static ClObj operator / (ClInt @this, ClObj other) =>
        other switch
        {
            ClInt fixnum => new ClInt(@this.Value / fixnum.Value),
            ClDouble floatingPont => new ClDouble(@this.Value / floatingPont.Value),
            _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {other.GetType().Name}")

        };

    public static ClObj operator %(ClInt @this, ClObj other) =>
        other switch
        {
            ClInt fixnum => new ClInt(@this.Value % fixnum.Value),
            ClDouble floatingPont => new ClDouble(@this.Value % floatingPont.Value),
            _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {other.GetType().Name}")
        };
}
