namespace Cl
{
    public class Reader
    {
        private readonly ISourceCode _source;

        public Reader(ISourceCode source)
        {
            _source = source;
        }
    }
}
