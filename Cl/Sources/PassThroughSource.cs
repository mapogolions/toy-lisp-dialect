namespace Cl.Sources
{
    // like Kotlin by-construction
    public abstract class PassThroughSource : IFilteredSource
    {
        protected readonly ISource _source;

        public PassThroughSource(ISource source)
        {
            _source = source;
        }

        public override string ToString() => _source.ToString();
        public void Buffer(int code) => _source.Buffer(code);
        public void Dispose() => _source.Dispose();
        public bool Eof() => _source.Eof();
        public int Peek() => _source.Peek();
        public int Read() => _source.Read();
        public abstract bool SkipEol();
        public abstract bool SkipLine();
        public abstract bool SkipMatched(string pattern);
        public abstract bool SkipWhitespaces();
    }
}
