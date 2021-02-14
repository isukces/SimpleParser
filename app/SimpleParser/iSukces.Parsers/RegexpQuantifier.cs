using System;

namespace iSukces.Parsers
{
    public struct RegexpQuantifier : IEquatable<RegexpQuantifier>
    {
        public RegexpQuantifier(int min, int max)
        {
            if (min == 1 && max == 1)
            {
                _min = 0;
                _max = 0;
            }
            else
            {
                _min = min;
                _max = max;
            }
        }

        public static bool operator ==(RegexpQuantifier left, RegexpQuantifier right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RegexpQuantifier left, RegexpQuantifier right)
        {
            return !left.Equals(right);
        }

        public bool Equals(RegexpQuantifier other)
        {
            return _min == other._min && _max == other._max;
        }

        public override bool Equals(object obj)
        {
            return obj is RegexpQuantifier other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_min, _max);
        }

        public override string ToString()
        {
            if (_min == 0)
                return _max == 0
                    ? ""
                    : _max == 1
                        ? "?"
                        : _max < 0
                            ? "*"
                            : Curly;

            if (_min == 1) return _max < 0 ? "+" : Curly;

            return Curly;
        }


        private int Fix(int value)
        {
            if (_min == 0 && _max == 0)
                return 1;
            return value;
        }

        public string Curly
        {
            get
            {
                if (_min == _max)
                {
                    if (_min <= 1)
                        return "{1}";
                    return "{" + _min.ToInv() + "}";
                }

                if (_min <= 0)
                    return "{0," + _max.ToInv() + "}";
                return "{" + _min.ToInv() + "," + _max.ToInv() + "}";
            }
        }

        public int Min => Fix(_min);

        public int Max => Fix(_max);

        public static RegexpQuantifier One = new RegexpQuantifier();
        public static RegexpQuantifier Star = new RegexpQuantifier(0, -1);
        public static RegexpQuantifier Question = new RegexpQuantifier(0, 1);
        public static RegexpQuantifier Plus = new RegexpQuantifier(1, -1);
        private readonly int _min;
        private readonly int _max;
    }
}