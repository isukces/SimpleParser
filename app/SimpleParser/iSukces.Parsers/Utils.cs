using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace iSukces.Parsers
{
    internal static class Utils
    {
        public static T[] Concat<T>(T[] a, T[] b)
        {
            if (a == null || a.Length == 0)
                return b;
            if (b == null || b.Length == 0)
                return a;
            var aLength = a.Length;
            var result  = new T[aLength + b.Length];
            Array.Copy(a, 0, result, 0, aLength);
            Array.Copy(b, 0, result, aLength, b.Length);
            return result;
        }

        public static T[] Concat<T>(T a, T[] b)
        {
            if (b == null || b.Length == 0)
                return new[] {a};
            const int aLength = 1;
            var       result  = new T[aLength + b.Length];
            result[0] = a;
            Array.Copy(b, 0, result, aLength, b.Length);
            return result;
        }

        public static T[] Concat<T>(T[] a, T b)
        {
            if (a == null || a.Length == 0)
                return new[] {b};
            var aLength = a.Length;
            var result  = new T[aLength + 1];
            Array.Copy(a, 0, result, 0, aLength);
            result[aLength] = b;
            return result;
        }

        public static TTarget[] Map<TSrc, TTarget>(
            TSrc[] src, Func<TSrc, TTarget> map)
        {
            if (src is null)
                return new TTarget[0];
            var srcLength = src.Length;
            var result    = new TTarget[srcLength];
            if (srcLength == 0)
                return result;
            for (var index = 0; index < src.Length; index++)
            {
                var srcItem = src[index];
                result[index] = map(srcItem);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToInv(this int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}