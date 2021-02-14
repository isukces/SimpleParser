using System;

namespace iSukces.Parsers
{
    public partial class RegexpChunk
    {
        public sealed class RcChar : RcQuantified, IRcSquareBracketsItem, IEquatable<RcChar>
        {
            public RcChar(char source, RegexpQuantifier quantifier)
                : this(ToRegexpCode(source), quantifier)
            {
            }


            public RcChar(string source, RegexpQuantifier quantifier)
                : base(source + quantifier, quantifier)
            {
                _source = source;
            }

            public RcChar(string code)
                : this(code, RegexpQuantifier.One)
            {
            }

            public RcChar(char ch)
                : this(ch, RegexpQuantifier.One)
            {
            }

            public static RcChar Escape(char arg)
            {
                switch (arg)
                {
                    case '.':
                    case '(':
                        return new RcChar("\\" + arg);
                    default:
                        return new RcChar(arg.ToString());
                }
            }

            public static bool operator ==(RcChar left, RcChar right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(RcChar left, RcChar right)
            {
                return !Equals(left, right);
            }

            public static string ToRegexpCode(char arg)
            {
                switch (arg)
                {
                    case '.':
                    case '(':
                        return "\\" + arg;
                    default:
                        return arg.ToString();
                }
            }

            public bool Equals(RcChar other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return _source == other._source && Quantifier.Equals(other.Quantifier);
            }

            public bool Equals(IRcSquareBracketsItem other)
            {
                return other is RcChar o2 && Equals(o2);
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj) || obj is RcChar other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(_source, Quantifier);
            }

            public override RegexpChunk WithQuantifier(RegexpQuantifier quantifier)
            {
                if (Quantifier == RegexpQuantifier.One)
                    return new RcChar(_source, quantifier);
                return base.WithQuantifier(quantifier);
            }

            private readonly string _source;
        }
    }
}