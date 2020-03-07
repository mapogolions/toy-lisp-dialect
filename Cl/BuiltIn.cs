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
        public static IClObj Caddr(IClObj expr) => Car(Cddr(Cddr(expr)));
        public static IClObj Cdddr(IClObj expr) => Cdr(Cddr(expr));
        public static IClObj Cadddr(IClObj expr) => Car(Cdddr(expr));
    }
}
