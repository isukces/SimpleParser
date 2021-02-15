namespace iSukces.Parsers.TokenParsers
{
    /// <summary>
    /// Double number tokenizer - facade for ManualDoubleTokenizer
    /// </summary>
    public class DoubleTokenizer : ManualDoubleTokenizer
    {
        public DoubleTokenizer(NumerFlags leadingSpaces, params char[] decimalSeparators)
            : base(leadingSpaces, decimalSeparators)
        {
        }

        public DoubleTokenizer(NumerFlags leadingSpaces)
            : base(leadingSpaces)
        {
        }
    }
}