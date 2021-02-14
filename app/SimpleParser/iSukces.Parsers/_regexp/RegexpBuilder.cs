namespace iSukces.Parsers
{
    public class RegexpBuilder
    {
        static RegexpBuilder()
        {
            Dot       = new RegexpChunk.RcChar(@"\.");
            Start     = new RegexpChunk.RcChar("^");
            Comma     = new RegexpChunk.RcChar(@",");
            Digit     = new RegexpChunk.RcChar(@"\d");
            WhiteChar = new RegexpChunk.RcChar("\\s");

            WhiteCharStar = WhiteChar.Star();
            WhiteCharPlus = WhiteChar.Plus();
            Digits        = Digit.Plus();

            PlusMinusOptional = new RegexpChunk.RcSquareBrackets(new[] {'+', '-'}, RegexpQuantifier.Question);
            var parts = new RegexpChunk.RcChar("_") + Digits;
            DigitsMixedWithUnderscores = Digits + parts.Star();
        }

        public static readonly RegexpChunk Start;
        public static readonly RegexpChunk WhiteChar;
        public static readonly RegexpChunk Dot;
        public static readonly RegexpChunk Comma;
        public static readonly RegexpChunk WhiteCharStar;
        public static readonly RegexpChunk WhiteCharPlus;

        public static readonly RegexpChunk Digit;
        public static readonly RegexpChunk Digits;
        public static readonly RegexpChunk DigitsMixedWithUnderscores;


        public static readonly RegexpChunk PlusMinusOptional;
    }
}