// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open;

public abstract class AsciiChar3Comparer : IComparer<AsciiChar3>, IEqualityComparer<AsciiChar3>
{
    public static readonly AsciiChar3Comparer CaseSensitive = new AsciiCharSequenceComparerCaseSensitive();
    public static readonly AsciiChar3Comparer CaseInsensitive = new AsciiCharSequenceComparerCaseInsensitive();

    protected AsciiChar3Comparer()
    {
    }

    public abstract int Compare(AsciiChar3 x, AsciiChar3 y);

    public abstract bool Equals(AsciiChar3 x, AsciiChar3 y);

    public abstract int GetHashCode([DisallowNull] AsciiChar3 obj);

    private sealed class AsciiCharSequenceComparerCaseSensitive : AsciiChar3Comparer
    {
        public override int Compare(AsciiChar3 x, AsciiChar3 y) => x.CompareTo(y);

        public override bool Equals(AsciiChar3 x, AsciiChar3 y) => x.Equals(y);

        public override int GetHashCode([DisallowNull] AsciiChar3 obj) => obj.GetHashCode();
    }

    private sealed class AsciiCharSequenceComparerCaseInsensitive : AsciiChar3Comparer
    {
        public override int Compare(AsciiChar3 x, AsciiChar3 y) => x.CompareToCaseInsensitive(y);

        public override bool Equals(AsciiChar3 x, AsciiChar3 y) => x.EqualsCaseInsensitive(y);

        public override int GetHashCode([DisallowNull] AsciiChar3 obj)
            => HashCode.Combine(obj._c0.ToUpper(), obj._c1.ToUpper(), obj._c2.ToUpper());
    }
}
