using System;
using System.Collections.Generic;
using System.Linq;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Scheme bootstrap");
        }
    }

    public interface IEnv { }

    public sealed class EmptyEnv : IEnv
    {
        private static IEnv _instance = default;

        private EmptyEnv() { }

        public static IEnv Given
        {
            get
            {
                if (_instance is null) _instance = new EmptyEnv();
                return _instance;
            }
        }
    }

    public class Env : IEnv
    {
        private readonly IEnv _frame;
        private readonly IDictionary<IClObj, IClObj> _bindings;

        public Env(IDictionary<IClObj, IClObj> bindings, IEnv frame)
        {
            _bindings = bindings;
            _frame = frame;
        }

        public IEnv Setup() => new Env(new Dictionary<IClObj, IClObj>(), EmptyEnv.Given);

        // TODO: check case when the key already taken
        public void Bind(IClObj key, IClObj value) => _bindings.Add(key, value);
    }

    public interface IClObj { }

    public sealed class Nil : IClObj
    {
        private static Nil _instance;

        private Nil () { }

        public Nil Given
        {
            get
            {
                if (_instance is null) _instance = new Nil();
                return _instance;
            }
        }
    }

    public class Proc<A, B> : IClObj
    {
        private readonly Func<A, B> _proc;

        public Proc(Func<A, B> proc)
        {
            _proc = proc;
        }

        public B Apply(A domain) => _proc.Invoke(domain);
    }

    public class ClPair
    {
        public IClObj Car { get; set; }
        public IClObj Cdr { get; set; }
    }

    public abstract class ClAtom<T> : IClObj
    {
        public T Value { get; }

        public ClAtom(T value) => Value = value;
    }

    public class ClFixnum : ClAtom<int>
    {
        public ClFixnum(int number) : base(number) { }
    }

    public class ClChar : ClAtom<char>
    {
        public ClChar(char ch) : base(ch) { }
    }

    public class ClSymbol : ClAtom<string>
    {
        public ClSymbol(string name) : base(name) { }
    }

    public class ClBool : ClAtom<bool>
    {
        public ClBool(bool flag) : base(flag) { }
    }

    public class BuiltIn
    {
        private readonly IDictionary<string, ClSymbol> _symbolsTable;

        public BuiltIn(IDictionary<string, ClSymbol> symbolsTable)
        {
            _symbolsTable = symbolsTable;
        }

        public BuiltIn() : this(new Dictionary<string, ClSymbol>()) { }

        public ClSymbol MakeSymbol(string name)
        {
            var foundSymbol = _symbolsTable.FirstOrDefault(it => it.Key.Equals(name)).Value;
            if (foundSymbol != null) return foundSymbol;
            var symbol = new ClSymbol(name);
            _symbolsTable.Add(name, symbol);
            return symbol;
        }

        public ClPair MakePair(IClObj car, IClObj cdr) => new ClPair { Car = car, Cdr = cdr };

        public IClObj Car(IClObj obj) =>
            obj switch
            {
                ClPair pair => pair.Car,
                _ => throw new ArgumentException(nameof(obj), $"Argument is not a {nameof(ClPair)}")
            };

        public IClObj Cdr(IClObj obj) =>
            obj switch
            {
                ClPair pair => pair.Cdr,
                _ => throw new ArgumentException(nameof(obj), $"Argument is not a {nameof(ClPair)}")
            };
    }
}
