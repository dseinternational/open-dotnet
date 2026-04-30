// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Ordinal comparer for <see cref="AsciiChar2"/> values. Use <see cref="CaseSensitive"/> for
/// ordinal comparison and <see cref="IgnoreCase"/> for ordinal, case-insensitive comparison.
/// </summary>
public abstract class AsciiChar2Comparer : IComparer<AsciiChar2>, IEqualityComparer<AsciiChar2>
{
    /// <summary>
    /// A comparer that distinguishes uppercase and lowercase ASCII letters.
    /// </summary>
    public static readonly AsciiChar2Comparer CaseSensitive = new AsciiCharSequenceComparerCaseSensitive();

    /// <summary>
    /// A comparer that treats corresponding uppercase and lowercase ASCII letters as equal.
    /// </summary>
    public static readonly AsciiChar2Comparer IgnoreCase = new AsciiCharSequenceComparerIgnoreCase();

    /// <summary>
    /// Initialises the base comparer. Intended for derived-class use only.
    /// </summary>
    protected AsciiChar2Comparer()
    {
    }

    /// <inheritdoc/>
    public abstract int Compare(AsciiChar2 x, AsciiChar2 y);

    /// <inheritdoc/>
    public abstract bool Equals(AsciiChar2 x, AsciiChar2 y);

    /// <inheritdoc/>
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
