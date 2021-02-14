using System.Text;

namespace iSukces.Parsers
{
    public partial class RegexpChunk
    {
        public class RcConcat : RcCollection<RegexpChunk>
        {
            private RcConcat(params RegexpChunk[] chunks) : base(chunks, MakeCode(chunks))
            {
            }

            public static RegexpChunk MakeSum(RegexpChunk a, RegexpChunk b)
            {
                if (a is null)
                    return b;
                if (b is null)
                    return a;
                if (a is RcConcat a1)
                {
                    if (b is RcConcat b1)
                        return new RcConcat(Utils.Concat(a1.Parts, b1.Parts));
                    return new RcConcat(Utils.Concat(a1.Parts, b));
                }
                else
                {
                    if (b is RcConcat b1)
                        return new RcConcat(Utils.Concat(a, b1.Parts));
                }

                return new RcConcat(a, b);
            }

            private static string GetCodeForSum(IRegexpChunk chunk)
            {
                if (chunk is null)
                    return null;
                if (chunk is ISpecialConcatItem al)
                    chunk = al.GetSumItem();
                return chunk.Code;
            }

            private static string MakeCode(RegexpChunk[] chunks)
            {
                var sb = new StringBuilder();
                for (var index = 0; index < chunks.Length; index++)
                    sb.Append(GetCodeForSum(chunks[index]));

                return sb.ToString();
            }
        }
    }
}