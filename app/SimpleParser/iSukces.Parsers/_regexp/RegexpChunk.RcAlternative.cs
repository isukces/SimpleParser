using System.Linq;

namespace iSukces.Parsers
{
    public partial class RegexpChunk
    {
        public class RcAlternative : RcCollection<RegexpChunk>, ISpecialConcatItem
        {
            private RcAlternative(RegexpChunk[] parts) :
                base(parts, GetCode(parts))
            {
            }

            public static RegexpChunk Make(RegexpChunk a, RegexpChunk b)
            {
                if (a is null)
                    return b;
                if (b is null)
                    return a;
                RegexpChunk[] regexpChunks;
                if (a is RcAlternative a1)
                {
                    if (b is RcAlternative b1)
                        regexpChunks = Utils.Concat(a1.Parts, b1.Parts);
                    else

                        regexpChunks = Utils.Concat(a1.Parts, b);
                }
                else
                {
                    if (b is RcAlternative b1)
                        regexpChunks = Utils.Concat(a, b1.Parts);
                    else
                        regexpChunks = new[] {a, b};
                }

                var sq = RcSquareBrackets.TryMake(regexpChunks);
                if (sq != null)
                    return sq;
                return new RcAlternative(regexpChunks);
            }

            private static string GetCode(RegexpChunk[] parts)
            {
                return string.Join("|", parts.Select(GetCodeFromChunk));
            }

            private static string GetCodeFromChunk(RegexpChunk a)
            {
                switch (a)
                {
                    case RcChar _:
                    case RcAlternative _:
                    case RcSquareBrackets _:
                        return a.Code;
                    case RcQuantified q when q.Quantifier != RegexpQuantifier.One:
                        return a.Code;
                    default:
                        return a.Wrap(true).Code;
                }
            }

            IRegexpChunk ISpecialConcatItem.GetSumItem()
            {
                if (Parts.Length != 1)
                    return Wrap(true);
                var singleItem = Parts[0];
                return singleItem is ISpecialConcatItem s
                    ? s.GetSumItem()
                    : singleItem;
            }
        }
    }
}