using Cl.Input;
using static Cl.Extensions.FpUniverse;

namespace Cl.Extensions
{
    public static class IFilteredSourceOps
    {
        public static bool SkipUntilLiteral(this IFilteredSource self,  string startsWith = ";", bool skipAtLeastOne = false)
        {
            if (self.SkipWhitespaces())
                skipAtLeastOne = true;
            if (!self.SkipMatched(startsWith)) return skipAtLeastOne;
            Ignore(self.SkipLine());
            return self.SkipUntilLiteral(startsWith, skipAtLeastOne: true);
        }
    }
}
