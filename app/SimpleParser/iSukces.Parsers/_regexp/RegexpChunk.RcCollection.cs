namespace iSukces.Parsers
{
    public partial class RegexpChunk
    {
        public abstract class RcCollection<T> : RegexpChunk
        {
            protected RcCollection(T[] parts, string code)
                : base(code)
            {
                Parts = parts;
            }


            public T[] Parts { get; }
        }
    }
}