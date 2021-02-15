using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace iSukces.Parsers.TokenParsers
{
    public class RegexpDoubleTokenizer : AbstractRegexpTokenizer
    {
        public RegexpDoubleTokenizer(NumerFlags leadingSpaces, params char[] decimalSeparators)
        {
            var code = Builder.Build(leadingSpaces, decimalSeparators);
            _reg                = code.GetRegexp(RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _separatorsToChange = decimalSeparators.Where(a => a != '.').ToArray();
        }

        public RegexpDoubleTokenizer(NumerFlags leadingSpaces)
            : this(leadingSpaces, '.')
        {
        }

        protected override int GetPriority()
        {
            return 100; // najwyższy
        }

        protected override Regex GetRegex()
        {
            return _reg;
        }

        protected override object ParseValue(Match m)
        {
            var value = m.Groups[1].Value;
            for (var index = _separatorsToChange.Length - 1; index >= 0; index--)
                value = value.Replace(_separatorsToChange[index], '.');

            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        //private static readonly Regex DoubleRegex = new Regex(DoubleFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly Regex _reg;
        private readonly char[] _separatorsToChange;

        //private const string DoubleFilter = @"^\s*([+-]?\d+(?:(?:(?:\.\d+)(?:e[+-]?\d+)?)|(?:(?:e[+-]?\d+))))";

        private sealed class Builder : RegexpBuilder
        {
            static Builder()
            {
                ExponentPart = new RegexpChunk("e") + PlusMinusOptional + Digits;
            }

            public static RegexpChunk Build(NumerFlags flags, params char[] decimalSeparators)
            {
                RegexpChunk code1;
                {
                    code1 = Start;

                    if ((flags & NumerFlags.RequireAtLeastOneLeadingSpace) != 0)
                        code1 += WhiteCharPlus;
                    else if ((flags & NumerFlags.AllowLedingSpaces) != 0)
                        code1 += WhiteCharStar;
                }
                // 
                RegexpChunk code2;
                {
                    var fractionalWithOptionalExponent =
                        RegexpChunk.FromAlternateChars(decimalSeparators)
                        + Digits
                        + ExponentPart.Question();
                    ;

                    var digits = (flags & NumerFlags.AllowUndescores) != 0
                        ? DigitsMixedWithUnderscores
                        : Digits;

                    code2 = PlusMinusOptional
                            + Digits
                            + (
                                fractionalWithOptionalExponent
                                |
                                ExponentPart
                            );
                }

                var code = code1 + code2.Wrap();
                return code;
            }

            private static readonly RegexpChunk ExponentPart;
        }
    }
}
/*
 
 ^\s*
(
[+-]?
\d+
(?:
(?:(?:\.\d+)(?:e[+-]?\d+)?)
|
(?:(?:e[+-]?\d+))
)

)

 */