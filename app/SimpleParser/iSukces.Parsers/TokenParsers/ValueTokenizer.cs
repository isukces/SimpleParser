namespace iSukces.Parsers.TokenParsers
{
    public abstract class ValueTokenizer
    {
        public abstract TokenCandidate Parse(string text);
    }
}