using Cl.Input;
using static Cl.Extensions.FpUniverse;

namespace Cl.Extensions
{
    public static class IFilteredSourceOps
    {
        public static bool SkipWhitespacesAndComments(this IFilteredSource self,  string startsWith = ";",
            bool atLeastOne = false)
        {
            if (self.SkipWhitespaces())
                atLeastOne = true;
            if (!self.SkipMatched(startsWith)) return atLeastOne;
            Ignore(self.SkipLine());
            return self.SkipWhitespacesAndComments(startsWith, atLeastOne: true);
        }
    }
}
