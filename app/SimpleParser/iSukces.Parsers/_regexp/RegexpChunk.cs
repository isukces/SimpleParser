using System;
using System.Text.RegularExpressions;

namespace iSukces.Parsers
{
    public partial class RegexpChunk : IRegexpChunk
    {
        public RegexpChunk(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException(nameof(code));
            Code = code;
        }

        public static RegexpChunk FromAlternateChars(char[] decimalSeparators)
        {
            if (decimalSeparators.Length == 1)
                return new RcChar(decimalSeparators[0]);
            return new RcSquareBrackets(decimalSeparators, RegexpQuantifier.One);
        }

        public static RegexpChunk operator +(RegexpChunk a, RegexpChunk b)
        {
            return RcConcat.MakeSum(a, b);
        }

        public static RegexpChunk operator |(RegexpChunk a, RegexpChunk b)
        {
            return RcAlternative.Make(a, b);
        }

        public Regex GetRegexp(RegexOptions options)
        {
            return new Regex(Code, options);
        }

        public RegexpChunk Plus()
        {
            return WithQuantifier(RegexpQuantifier.Plus);
        }

        public RegexpChunk Question()
        {
            return WithQuantifier(RegexpQuantifier.Question);
        }

        public RegexpChunk Star()
        {
            return WithQuantifier(RegexpQuantifier.Star);
        }


        public override string ToString()
        {
            return Code;
        }

        public virtual RegexpChunk WithQuantifier(RegexpQuantifier quantifier)
        {
            return new RcBrackets(this, true, quantifier);
        }

        public virtual RegexpChunk Wrap(bool ignoreResult = false)
        {
            return new RcBrackets(this, ignoreResult, RegexpQuantifier.One);
        }

        public string Code { get; }
    }
}