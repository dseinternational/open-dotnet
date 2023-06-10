// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language;

public class PosTagComparer : IComparer<PosTag>, IEqualityComparer<PosTag>
{
    public static readonly PosTagComparer Ordinal = new(StringComparison.Ordinal);

    public static readonly PosTagComparer OrdinalIgnoreCase = new(StringComparison.OrdinalIgnoreCase);

    private PosTagComparer(StringComparison comparison)
    {
        Comparison = comparison;
    }

    public int Compare(PosTag x, PosTag y) => x.CompareTo(y, Comparison);

    public bool Equals(PosTag x, PosTag y) => x.Equals(y, Comparison);

    public int GetHashCode(PosTag obj) => string.GetHashCode(obj.AsSpan(), Comparison);

    public StringComparison Comparison { get; }
}
