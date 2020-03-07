using System;
using Cl.Types;

namespace Cl
{
    public static class Evaluator
    {
        public static Func<IClObj, IClObj> Var = BuiltIn.Cadr;
        public static Func<IClObj, IClObj> Val = BuiltIn.Caddr;
        public static Func<IClObj, IClObj> Operator = BuiltIn.Car;
        public static Func<IClObj, IClObj> Operandr = BuiltIn.Cdr;
        public static Func<IClObj, IClObj> IfPredicate = BuiltIn.Cadr;
        public static Func<IClObj, IClObj> IfConsequent = BuiltIn.Caddr;
    }
}
