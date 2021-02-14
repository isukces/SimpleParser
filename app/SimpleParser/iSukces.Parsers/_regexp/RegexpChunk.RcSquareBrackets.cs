using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSukces.Parsers
{
    public partial class RegexpChunk
    {
        public class RcSquareBrackets : RcQuantified
        {
            public RcSquareBrackets(char[] chars, RegexpQuantifier quantifier, bool inverseCondition = false)
                : this(Map(chars), quantifier, inverseCondition)
            {
            }

            public RcSquareBrackets(IRcSquareBracketsItem[] items, RegexpQuantifier quantifier,
                bool inverseCondition = false)
                : base(MakeCode(items, quantifier, inverseCondition), quantifier)
            {
                Items            = items;
                InverseCondition = inverseCondition;
            }

            public static RcSquareBrackets TryMake(RegexpChunk[] regexpChunks)
            {
                var l = new List<IRcSquareBracketsItem>();
                foreach (var i in regexpChunks)
                    switch (i)
                    {
                        case RcQuantified q
                            when q.Quantifier != RegexpQuantifier.One:
                            return null;
                        case IRcSquareBracketsItem item:
                            l.Add(item);
                            break;
                        case RcSquareBrackets b
                            when b.InverseCondition:
                            return null;
                        case RcSquareBrackets b:
                            l.AddRange(b.Items);
                            break;
                        default:
                            return null;
                    }

                return new RcSquareBrackets(l.Distinct().ToArray(), RegexpQuantifier.One);
            }


            private static string MakeCode(IReadOnlyList<IRcSquareBracketsItem> chars, RegexpQuantifier quantifier,
                bool inverseCondition)
            {
                var sb = new StringBuilder();
                sb.Append("[");
                if (inverseCondition)
                    sb.Append("^");
                for (var index = 0; index < chars.Count; index++)
                {
                    var cod = chars[index]?.Code;
                    if (cod == "^")
                        sb.Append("\\" + cod);
                    else
                        sb.Append(cod);
                }

                sb.Append("]");
                sb.Append(quantifier);
                return sb.ToString();
            }

            private static IRcSquareBracketsItem[] Map(char[] chars)
            {
                return Utils.Map<char, IRcSquareBracketsItem>(chars, RcChar.Escape);
            }

            public override RegexpChunk WithQuantifier(RegexpQuantifier quantifier)
            {
                if (Quantifier == quantifier)
                    return this;
                return new RcSquareBrackets(Items, quantifier);
            }

            public IRcSquareBracketsItem[] Items            { get; }
            public bool                    InverseCondition { get; }
        }
    }
}