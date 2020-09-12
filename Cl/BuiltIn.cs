using System.Linq;
using System;
using System.Collections.Generic;
using Cl.Extensions;
using Cl.Types;
using Cl.Contracts;

namespace Cl
{
    public static class BuiltIn
    {
        public static IContext Eval(IEnumerable<IClObj> expressions) => expressions
            .Aggregate<IClObj, IContext>(new Context(Env), (ctx, expr) => expr.Reduce(ctx));

        public static IClObj Car(params IClObj[] obj) => obj.Unpack<ClCell>().Car;
        public static IClObj Cdr(params IClObj[] obj) => obj.Unpack<ClCell>().Cdr;

        public static IClObj Cadr(params IClObj[] obj) => Car(Cdr(obj));
        public static IClObj Cddr(params IClObj[] obj) => Cdr(Cdr(obj));
        public static IClObj Caddr(params IClObj[] obj) => Car(Cddr(obj));
        public static IClObj Cdddr(params IClObj[] obj) => Cdr(Cddr(obj));
        public static IClObj Cadddr(params IClObj[] obj) => Car(Cdddr(obj));
        public static ParamsFunc<IClObj, IClObj> Head = Car;
        public static ParamsFunc<IClObj, IClObj> Tail = Cdr;

        public static ParamsFunc<IClObj, IClObj> First = Car;
        public static ParamsFunc<IClObj, IClObj> Second = Cadr;
        public static ParamsFunc<IClObj, IClObj> Third = Caddr;
        public static ParamsFunc<IClObj, IClObj> Fourth = Cadddr;
        public static ClBool IsTrue(params IClObj[] obj)
        {
            var value = obj.Unpack<IClObj>();
            return ClBool.Of(value != ClCell.Nil && value != ClBool.False);
        }
        public static ClBool IsFalse(params IClObj[] obj) => Not(IsTrue(obj));
        public static ClBool Not(params IClObj[] obj) => ClBool.Of((!obj.Unpack<ClBool>().Value));

        public static ClCell ListOf(params IClObj[] items)
        {
            ClCell cell = ClCell.Nil;
            for (var i = items.Length - 1; i >= 0; i--)
            {
                cell = new ClCell(items[i], cell);
            }
            return cell;
        }

        public static ClCell Cons(params IClObj[] obj)
        {
            var (head, tail) = obj.Unpack<IClObj, IClObj>();
            return new ClCell(head, tail);
        }

        public static ClCell ListOf(IEnumerable<IClObj> items) => ListOf(items.ToArray());

        public static IEnumerable<IClObj> Seq(IClObj obj)
        {
            var cell = obj.TypeOf<ClCell>();
            if (cell is null) throw new InvalidOperationException();
            if (cell == ClCell.Nil) yield break;
            yield return cell.Car;
            var tail = cell.Cdr;
            while (tail is ClCell pair)
            {
                if (tail == ClCell.Nil) yield break;
                yield return pair.Car;
                tail = pair.Cdr;
            }
            yield return tail;
        }

        public static IClObj Quote(IClObj obj) => new ClCell(ClSymbol.Quote, obj);

        // Predicates
        public static ClBool IsNull(params IClObj[] obj) => ClBool.Of(obj.Unpack<IClObj>() == ClCell.Nil);
        public static ClBool HasType<T>(params IClObj[] obj) where T : IClObj  =>
            ClBool.Of(obj.Unpack<IClObj>().TypeOf<T>() != null);

        public static ClBool IsString(params IClObj[] obj) => HasType<ClString>(obj);
        public static ClBool IsSymbol(params IClObj[] obj) => HasType<ClSymbol>(obj);
        public static ClBool IsInteger(params IClObj[] obj) => HasType<ClInt>(obj);
        public static ClBool IsFloat(params IClObj[] obj) => HasType<ClFloat>(obj);
        public static ClBool IsChar(params IClObj[] obj) => HasType<ClChar>(obj);
        public static ClBool IsPair(params IClObj[] obj) => HasType<ClCell>(obj);
        public static ClBool IsCallable(params IClObj[] obj) => HasType<ClCallable>(obj);

        // Converts
        public static ClInt IntOfString(params IClObj[] obj) =>new ClInt(Convert.ToInt32(obj.Unpack<ClString>()));

        // Pervasives
        public static IEnv Env = new Env(
            (new ClSymbol("null?"), new NativeFn(IsNull)),
            (new ClSymbol("string?"), new NativeFn(IsString)),
            (new ClSymbol("symbol?"), new NativeFn(IsSymbol)),
            (new ClSymbol("int?"), new NativeFn(IsInteger)),
            (new ClSymbol("float?"), new NativeFn(IsFloat)),
            (new ClSymbol("char?"), new NativeFn(IsChar)),
            (new ClSymbol("callable?"), new NativeFn(IsCallable)),
            (new ClSymbol("head"), new NativeFn(Head)),
            (new ClSymbol("tail"), new NativeFn(Tail)),
            (new ClSymbol("car"), new NativeFn(Car)),
            (new ClSymbol("cdr"), new NativeFn(Cdr)),
            (new ClSymbol("cadr"), new NativeFn(Cadr)),
            (new ClSymbol("cddr"), new NativeFn(Cddr)),
            (new ClSymbol("caddr"), new NativeFn(Caddr)),
            (new ClSymbol("cadddr"), new NativeFn(Cadddr)),
            (new ClSymbol("first"), new NativeFn(First)),
            (new ClSymbol("second"), new NativeFn(Second)),
            (new ClSymbol("third"), new NativeFn(Third)),
            (new ClSymbol("fourth"), new NativeFn(Fourth)),
            (new ClSymbol("true?"), new NativeFn(IsTrue)),
            (new ClSymbol("false?"), new NativeFn(IsFalse)),
            (new ClSymbol("not"), new NativeFn(Not)),
            (new ClSymbol("list"), new NativeFn(ListOf)),
            (new ClSymbol("cons"), new NativeFn(Cons)),
            (new ClSymbol("int_of_string"), new NativeFn(IntOfString))
        );


    //     ("char->integer", char_to_integer_proc);
    //  ("integer->char", integer_to_char_proc);
    //  ("number->string", number_to_string_proc);
    //  ("string->number", string_to_symbol_proc);
    //  ("symbol->string", symbol_to_string_proc);
    //  ("string->symbol", string_to_symbol_proc);
    }
}
