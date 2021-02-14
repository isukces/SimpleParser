namespace iSukces.Parsers
{
    public partial class RegexpChunk
    {
        private class RcBrackets : RcQuantified
        {
            public RcBrackets(RegexpChunk source, bool ignoreResult, RegexpQuantifier quantifier)
                : base(GetCode(source, ignoreResult, quantifier), quantifier)
            {
                _source       = source;
                _ignoreResult = ignoreResult;
            }

            private static string GetCode(RegexpChunk source, bool ignoreResult, RegexpQuantifier quantifier)
            {
                return string.Format("({0}{1}){2}", ignoreResult ? "?:" : "", source.Code, quantifier);
            }

            public override RegexpChunk WithQuantifier(RegexpQuantifier quantifier)
            {
                if (Quantifier == quantifier)
                    return this;
                return new RcBrackets(_source, _ignoreResult, quantifier);
            }

            public override RegexpChunk Wrap(bool ignoreResult = false)
            {
                if (ignoreResult == _ignoreResult)
                    return this;
                return new RcBrackets(_source, ignoreResult, Quantifier);
            }

            private readonly RegexpChunk _source;
            private readonly bool _ignoreResult;
        }
    }
}