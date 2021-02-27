using Cl.Contracts;
using Cl.Types;

namespace Cl.Extensions
{
    public static class ContextExtensions
    {
        public static IContext FromValue(this IContext context, ClObj value) => new Context(value, context.Env);
        public static IContext FromEnv(this IContext context, IEnv env) => new Context(context.Value, env);
        public static void Deconstruct(this IContext context, out ClObj value, out IEnv env)
        {
            value = context.Value;
            env = context.Env;
        }
    }
}
