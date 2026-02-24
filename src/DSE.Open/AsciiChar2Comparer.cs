// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public abstract class AsciiChar2Comparer : IComparer<AsciiChar2>, IEqualityComparer<AsciiChar2>
{
    public static readonly AsciiChar2Comparer CaseSensitive = new AsciiCharSequenceComparerCaseSensitive();
    public static readonly AsciiChar2Comparer IgnoreCase = new AsciiCharSequenceComparerIgnoreCase();

    protected AsciiChar2Comparer()
    {
    }

    public abstract int Compare(AsciiChar2 x, AsciiChar2 y);

    public abstract bool Equals(AsciiChar2 x, AsciiChar2 y);

    public abstract int GetHashCode(AsciiChar2 obj);

    private sealed class AsciiCharSequenceComparerCaseSensitive : AsciiChar2Comparer
    {
        public override int Compare(AsciiChar2 x, AsciiChar2 y)
        {
            return x.CompareTo(y);
        }

        public override bool Equals(AsciiChar2 x, AsciiChar2 y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode(AsciiChar2 obj)
        {
            return obj.GetHashCode();
        }
    }

    private sealed class AsciiCharSequenceComparerIgnoreCase : AsciiChar2Comparer
    {
        public override int Compare(AsciiChar2 x, AsciiChar2 y)
        {
            return x.CompareToIgnoreCase(y);
        }

        public override bool Equals(AsciiChar2 x, AsciiChar2 y)
        {
            return x.EqualsIgnoreCase(y);
        }

        public override int GetHashCode(AsciiChar2 obj)
        {
            return HashCode.Combine(obj._chars[0].ToUpper(), obj._chars[1].ToUpper());
        }
    }
}
