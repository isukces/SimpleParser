namespace iSukces.Parsers.TokenParsers
{
    /// <summary>
    /// Double number tokenizer - facade for ManualDoubleTokenizer
    /// </summary>
    public class DoubleTokenizer : ManualDoubleTokenizer
    {
        public DoubleTokenizer(NumerFlags flags, params char[] decimalSeparators)
            : base(flags, decimalSeparators)
        {
        }

        public DoubleTokenizer(NumerFlags flags)
            : base(flags)
        {
        }
    }
}