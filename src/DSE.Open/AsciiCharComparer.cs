// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

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
    public abstract int GetHashCode([DisallowNull] AsciiChar obj);

    private sealed class AsciiCharComparerCaseSensitive : AsciiCharComparer
    {
        public override int Compare(AsciiChar x, AsciiChar y) => x.CompareTo(y);
        public override bool Equals(AsciiChar x, AsciiChar y) => x.Equals(y);
        public override int GetHashCode([DisallowNull] AsciiChar obj) => obj.GetHashCode();
    }
    private sealed class AsciiCharComparerCaseInsensitive : AsciiCharComparer
    {
        public override int Compare(AsciiChar x, AsciiChar y) => AsciiChar.CompareToCaseInsensitive(x, y);
        public override bool Equals(AsciiChar x, AsciiChar y) => AsciiChar.EqualsCaseInsensitive(x, y);
        public override int GetHashCode([DisallowNull] AsciiChar obj) => obj.GetHashCode();
    }
}
