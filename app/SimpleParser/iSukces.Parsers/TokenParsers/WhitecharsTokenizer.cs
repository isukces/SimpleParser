using System.Runtime.CompilerServices;

namespace iSukces.Parsers.TokenParsers
{
    public sealed class WhitecharsTokenizer : ValueTokenizer
    {
        private WhitecharsTokenizer(WhitecharsTokenizerKind kind)
        {
            _zeroIsNotAcceptable = kind == WhitecharsTokenizerKind.All
                                   || kind == WhitecharsTokenizerKind.AtLeastOne;
            _whole = kind == WhitecharsTokenizerKind.AllOrNone || kind == WhitecharsTokenizerKind.All;
        }


        public override TokenCandidate Parse(string text)
        {
            if (string.IsNullOrEmpty(text)) return CreateTokenCandidate(0);

            for (var i = 0; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                    continue;
                return _whole ? null : CreateTokenCandidate(i);
            }

            return CreateTokenCandidate(text.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TokenCandidate CreateTokenCandidate(int length)
        {
            var isZeroLength = length == 0;
            if (_zeroIsNotAcceptable && isZeroLength)
                return null;
            return new TokenCandidate(null, length, isZeroLength ? 0 : int.MaxValue);
        }

        /// <summary>
        ///  Texts starts from zero or any number of white chars 
        /// </summary>
        public static WhitecharsTokenizer ZeroOrMore => InstanceHolder.MyZeroOrMore;
        
        
        /// <summary>
        /// Texts starts from at least one white char
        /// </summary>
        public static WhitecharsTokenizer OneOrMore  => InstanceHolder.MyOneOrMore;
        
        /// <summary>
        /// Text contains only white chars or is empty
        /// </summary>
        public static WhitecharsTokenizer AllOrNone  => InstanceHolder.MyAllOrNone;
        
        /// <summary>
        /// Text contains only white chars
        /// </summary>
        public static WhitecharsTokenizer All        => InstanceHolder.MyAll;


        private readonly bool _zeroIsNotAcceptable;
        private readonly bool _whole;

        private sealed class InstanceHolder
        {
            public static readonly WhitecharsTokenizer MyZeroOrMore
                = new WhitecharsTokenizer(WhitecharsTokenizerKind.ZeroOrMore);

            public static readonly WhitecharsTokenizer MyOneOrMore =
                new WhitecharsTokenizer(WhitecharsTokenizerKind.AtLeastOne);

            public static readonly WhitecharsTokenizer MyAllOrNone =
                new WhitecharsTokenizer(WhitecharsTokenizerKind.AllOrNone);

            public static readonly WhitecharsTokenizer MyAll =
                new WhitecharsTokenizer(WhitecharsTokenizerKind.All);
        }
    }

    public enum WhitecharsTokenizerKind
    {
        /// <summary>
        /// Texts starts from zero or any number of white chars 
        /// </summary>
        ZeroOrMore,
        
        /// <summary>
        /// Texts starts from at least one white char
        /// </summary>
        AtLeastOne,
        
        /// <summary>
        /// Text contains only white chars or is empty
        /// </summary>
        AllOrNone,
        
        /// <summary>
        /// Text contains only white chars
        /// </summary>
        All
    }
}