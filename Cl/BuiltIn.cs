using System;
using Cl.Types;

namespace Cl
{
    public static class BuiltIn
    {
        public static IClObj Car(IClObj expr) =>
            expr switch
            {
                ClPair cell => cell.Car,
                _ => throw new InvalidOperationException("Argument is no a cell")
            };

        public static IClObj Cdr(IClObj expr) =>
            expr switch
            {
                ClPair cell => cell.Cdr,
                _ => throw new InvalidOperationException("Argument is not a cell")
            };

        public static IClObj Cadr(IClObj expr) => Car(Cdr(expr));
        public static IClObj Cddr(IClObj expr) => Cdr(Cdr(expr));
        public static IClObj Caddr(IClObj expr) => Car(Cddr(expr));
        public static IClObj Cdddr(IClObj expr) => Cdr(Cddr(expr));
        public static IClObj Cadddr(IClObj expr) => Car(Cdddr(expr));

        public static Func<IClObj, IClObj> Head = Car;
        public static Func<IClObj, IClObj> Tail = Cdr;

        public static Func<IClObj, IClObj> First = Car;
        public static Func<IClObj, IClObj> Second = Cadr;
        public static Func<IClObj, IClObj> Third = Caddr;
        public static Func<IClObj, IClObj> Fourth = Cadddr;
    }
}
