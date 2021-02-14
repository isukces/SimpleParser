namespace iSukces.Parsers
{
    public partial class RegexpChunk
    {
        public abstract class RcQuantified : RegexpChunk
        {
            protected RcQuantified(string code, RegexpQuantifier quantifier) : base(code)
            {
                Quantifier = quantifier;
            }

            public RegexpQuantifier Quantifier { get; }
        }
    }
}