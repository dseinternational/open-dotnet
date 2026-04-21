// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public abstract class AsciiChar3Comparer : IComparer<AsciiChar3>, IEqualityComparer<AsciiChar3>
{
    public static readonly AsciiChar3Comparer CaseSensitive = new AsciiCharSequenceComparerCaseSensitive();
    public static readonly AsciiChar3Comparer IgnoreCase = new AsciiCharSequenceComparerIgnoreCase();

    protected AsciiChar3Comparer()
    {
    }

    public abstract int Compare(AsciiChar3 x, AsciiChar3 y);

    public abstract bool Equals(AsciiChar3 x, AsciiChar3 y);

    public abstract int GetHashCode(AsciiChar3 obj);

    private sealed class AsciiCharSequenceComparerCaseSensitive : AsciiChar3Comparer
    {
        public override int Compare(AsciiChar3 x, AsciiChar3 y)
        {
            return x.CompareTo(y);
        }

        public override bool Equals(AsciiChar3 x, AsciiChar3 y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode(AsciiChar3 obj)
        {
            return obj.GetHashCode();
        }
    }

    private sealed class AsciiCharSequenceComparerIgnoreCase : AsciiChar3Comparer
    {
        public override int Compare(AsciiChar3 x, AsciiChar3 y)
        {
            return x.CompareToIgnoreCase(y);
        }

        public override bool Equals(AsciiChar3 x, AsciiChar3 y)
        {
            return x.EqualsIgnoreCase(y);
        }

        public override int GetHashCode(AsciiChar3 obj)
        {
            return HashCode.Combine(obj._chars[0].ToUpper(), obj._chars[1].ToUpper(), obj._chars[2].ToUpper());
        }
    }
}
