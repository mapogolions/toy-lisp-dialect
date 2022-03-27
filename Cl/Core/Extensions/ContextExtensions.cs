using Cl.Core;
using Cl.Types;

namespace Cl.Core.Extensions
{
    public static class ContextExtensions
    {
        public static IContext FromValue(this IContext @this, ClObj value) => new Context(value, @this.Env);
        public static IContext FromEnv(this IContext @this, IEnv env) => new Context(@this.Value, env);
        public static void Deconstruct(this IContext @this, out ClObj value, out IEnv env)
        {
            value = @this.Value;
            env = @this.Env;
        }
    }
}
