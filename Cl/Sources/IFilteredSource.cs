namespace Cl.Sources
{
    public interface IFilteredSource : ISource
    {
        bool SkipEol();
        bool SkipWhitespaces();
        bool SkipLine();
        bool  SkipMatched(string pattern);
    }
}
