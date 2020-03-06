using Cl.Input;
using static Cl.Extensions.FpUniverse;

namespace Cl.Extensions
{
    public static class IFilteredSourceOps
    {
        public static void SkipUntilToken(this IFilteredSource self,  string startsWith = ";")
        {
            Ignore(self.SkipWhitespaces());
            if (!self.SkipMatched(startsWith)) return;
            Ignore(self.SkipLine());
            self.SkipUntilToken(startsWith);
        }
    }
}
