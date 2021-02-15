using iSukces.Parsers;
using iSukces.Parsers.TokenParsers;

namespace SimpleParser.Console
{
    internal class Tokenizer : AbstractTokenizer
    {
        protected override ValueTokenizer[] GetTokenizers()
        {
            var candidates = new ValueTokenizer[]
            {
                new RegexpDoubleTokenizer(NumerFlags.AllowLedingSpaces, '.', ','),
                new IntegerTokenizer(),
                new StringNoSpaceTokenizer(),
                new SingleQuoteStringTokenizer()
            };
            return candidates;
        }
    }
}