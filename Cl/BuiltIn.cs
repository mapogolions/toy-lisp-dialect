using System;
using Cl.Types;

namespace Cl
{
    public static class BuiltIn
    {
        public static IClObj Car(IClObj expr) =>
            expr switch
            {
                ClCell cell => cell.Car,
                _ => throw new InvalidOperationException("Argument is no a cell")
            };

        public static IClObj Cdr(IClObj expr) =>
            expr switch
            {
                ClCell cell => cell.Cdr,
                _ => throw new InvalidOperationException("Argument is not a cell")
            };

        public static IClObj Cadr(IClObj expr) => Car(Cdr(expr));
        public static IClObj Cddr(IClObj expr) => Cdr(Cdr(expr));
        public static IClObj Caddr(IClObj expr) => Car(Cddr(expr));
        public static IClObj Cdddr(IClObj expr) => Cdr(Cddr(expr));
        public static IClObj Cadddr(IClObj expr) => Car(Cdddr(expr));

        public static ClCell ListOf(params IClObj[] items)
        {
            ClCell cell = Nil.Given;
            for (var i = items.Length - 1; i >= 0; i--)
            {
                cell = new ClCell(items[i], cell);
            }
            return cell;
        }

        public static ClCell Quote(ClCell cell) => new ClCell(ClSymbol.Quote, cell);

        public static Func<IClObj, IClObj> Head = Car;
        public static Func<IClObj, IClObj> Tail = Cdr;

        public static Func<IClObj, IClObj> First = Car;
        public static Func<IClObj, IClObj> Second = Cadr;
        public static Func<IClObj, IClObj> Third = Caddr;
        public static Func<IClObj, IClObj> Fourth = Cadddr;
    }
}
