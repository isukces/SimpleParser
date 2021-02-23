using System.Runtime.CompilerServices;

namespace iSukces.Parsers.TokenParsers
{
    public sealed class WhitecharsTokenizer : ValueTokenizer
    {
        private WhitecharsTokenizer(bool mustHaveAtLeastOne)
        {
            _mustHaveAtLeastOne = mustHaveAtLeastOne;
        }


        public override TokenCandidate Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
                return CreateTokenCandidate(0);
            for (var i = 0; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                    continue;
                return CreateTokenCandidate(i);
            }

            return CreateTokenCandidate(text.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TokenCandidate CreateTokenCandidate(int length)
        {
            if (_mustHaveAtLeastOne && length == 0)
                return null;
            return new TokenCandidate(null, length, length == 0 ? 0 : int.MaxValue);
        }

        public static WhitecharsTokenizer ZeroOrMoreWhitechars => InstanceHolder.MyZeroOrMoreWhitechars;
        public static WhitecharsTokenizer OneOrMoreWhitechars  => InstanceHolder.MyOneOrMoreWhitechars;
        private readonly bool _mustHaveAtLeastOne;

        private sealed class InstanceHolder
        {
            public static readonly WhitecharsTokenizer MyZeroOrMoreWhitechars = new WhitecharsTokenizer(false);
            public static readonly WhitecharsTokenizer MyOneOrMoreWhitechars = new WhitecharsTokenizer(true);
        }
    }
}