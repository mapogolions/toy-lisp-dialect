using System.Collections.Generic;
using Cl.Types;

namespace Cl
{
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

        // TODO: check case when the key already exists
        public void Bind(IClObj key, IClObj value) => _bindings.Add(key, value);
    }
}
