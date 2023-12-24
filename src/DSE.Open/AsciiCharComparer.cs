// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public abstract class AsciiCharComparer : IComparer<AsciiChar>, IEqualityComparer<AsciiChar>
{
    public static readonly AsciiCharComparer CaseSensitive = new AsciiCharComparerCaseSensitive();
    public static readonly AsciiCharComparer CaseInsensitive = new AsciiCharComparerCaseInsensitive();

    protected AsciiCharComparer()
    {
    }

    public abstract int Compare(AsciiChar x, AsciiChar y);
    public abstract bool Equals(AsciiChar x, AsciiChar y);
    public abstract int GetHashCode(AsciiChar obj);

    private sealed class AsciiCharComparerCaseSensitive : AsciiCharComparer
    {
        public override int Compare(AsciiChar x, AsciiChar y)
        {
            return x.CompareTo(y);
        }

        public override bool Equals(AsciiChar x, AsciiChar y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode(AsciiChar obj)
        {
            return obj.GetHashCode();
        }
    }
    private sealed class AsciiCharComparerCaseInsensitive : AsciiCharComparer
    {
        public override int Compare(AsciiChar x, AsciiChar y)
        {
            return AsciiChar.CompareToCaseInsensitive(x, y);
        }

        public override bool Equals(AsciiChar x, AsciiChar y)
        {
            return AsciiChar.EqualsCaseInsensitive(x, y);
        }

        public override int GetHashCode(AsciiChar obj)
        {
            return obj.GetHashCode();
        }
    }
}
