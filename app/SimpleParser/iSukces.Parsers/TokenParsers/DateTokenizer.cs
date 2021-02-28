using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace iSukces.Parsers.TokenParsers
{
    public sealed class DateTokenizer : RegexpDateTokenizer
    {
    }

    public class ManualDateTokenizer : ValueTokenizer
    {
        public override TokenCandidate Parse(string text)
        {
            if (text is null || text.Length < 10)
                return null;
            if (!char.IsDigit(text[0])) return null;
            if (!char.IsDigit(text[1])) return null;
            if (!char.IsDigit(text[2])) return null;
            if (!char.IsDigit(text[3])) return null;

            if (text[4] != '-') return null;
            if (!char.IsDigit(text[5])) return null;
            if (!char.IsDigit(text[6])) return null;
            if (text[7] != '-') return null;
            if (!char.IsDigit(text[8])) return null;
            if (!char.IsDigit(text[9])) return null;

            var year  = int.Parse(text.Substring(0, 4), CultureInfo.InvariantCulture);
            var month = int.Parse(text.Substring(5, 2), CultureInfo.InvariantCulture);
            var day   = int.Parse(text.Substring(8, 2), CultureInfo.InvariantCulture);
            try
            {
                var dt = new DateTime(year, month, day);
                return new TokenCandidate(dt, 10, 10);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }

    public class RegexpDateTokenizer : AbstractRegexpTokenizer
    {
        protected override int GetPriority()
        {
            return 10;
        }

        protected override Regex GetRegex()
        {
            return DateRegex;
        }

        protected override object ParseValue(Match m)
        {
            var year  = int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
            var month = int.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);
            var day   = int.Parse(m.Groups[3].Value, CultureInfo.InvariantCulture);
            try
            {
                return new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        private static readonly Regex
            DateRegex = new Regex(DateFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private const string DateFilter = @"^(\d{4})-(\d{2})-(\d{2})";
    }
}