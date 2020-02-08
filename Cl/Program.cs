using System;
using System.Collections.Generic;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public interface IEnv { }

    public sealed class EmptyEnv : IEnv
    {
        private static IEnv _instance = default;

        private EmptyEnv() { }

        public static IEnv Self
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
        private readonly IEnv _scope;
        private readonly IDictionary<IClObj, IClObj> _table;

        public Env(IDictionary<IClObj, IClObj> table, IEnv scope)
        {
            _table = table;
            _scope = scope;
        }

        public Env(IDictionary<IClObj, IClObj> table) : this(table, null) { }
    }

    public interface IClObj { }

    public class ClCarCdr
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
}
