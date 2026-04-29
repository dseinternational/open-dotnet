// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Ordinal comparer for <see cref="AsciiChar3"/> values. Use <see cref="CaseSensitive"/> for
/// ordinal comparison and <see cref="IgnoreCase"/> for ordinal, case-insensitive comparison.
/// </summary>
public abstract class AsciiChar3Comparer : IComparer<AsciiChar3>, IEqualityComparer<AsciiChar3>
{
    /// <summary>
    /// A comparer that distinguishes uppercase and lowercase ASCII letters.
    /// </summary>
    public static readonly AsciiChar3Comparer CaseSensitive = new AsciiCharSequenceComparerCaseSensitive();

    /// <summary>
    /// A comparer that treats corresponding uppercase and lowercase ASCII letters as equal.
    /// </summary>
    public static readonly AsciiChar3Comparer IgnoreCase = new AsciiCharSequenceComparerIgnoreCase();

    /// <summary>
    /// Initialises the base comparer. Intended for derived-class use only.
    /// </summary>
    protected AsciiChar3Comparer()
    {
    }

    /// <inheritdoc/>
    public abstract int Compare(AsciiChar3 x, AsciiChar3 y);

    /// <inheritdoc/>
    public abstract bool Equals(AsciiChar3 x, AsciiChar3 y);

    /// <inheritdoc/>
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
