using Cl.Input;
using static Cl.Helpers.FpUniverse;

namespace Cl.Extensions
{
    public static class FilteredSourceExtensions
    {
        public static bool SkipWhitespacesAndComments(this IFilteredSource @this,
            string startsWith = ";", bool atLeastOne = false)
        {
            if (@this.SkipWhitespaces())
                atLeastOne = true;
            if (!@this.SkipMatched(startsWith)) return atLeastOne;
            Ignore(@this.SkipLine());
            return @this.SkipWhitespacesAndComments(startsWith, atLeastOne: true);
        }
    }
}
