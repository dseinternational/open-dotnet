// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Ordinal comparer for <see cref="AsciiChar"/> values. Use <see cref="CaseSensitive"/> for
/// ordinal comparison and <see cref="IgnoreCase"/> for ordinal, case-insensitive comparison.
/// </summary>
public abstract class AsciiCharComparer : IComparer<AsciiChar>, IEqualityComparer<AsciiChar>
{
    /// <summary>
    /// A comparer that distinguishes uppercase and lowercase ASCII letters.
    /// </summary>
    public static readonly AsciiCharComparer CaseSensitive = new AsciiCharComparerCaseSensitive();

    /// <summary>
    /// A comparer that treats corresponding uppercase and lowercase ASCII letters as equal.
    /// </summary>
    public static readonly AsciiCharComparer IgnoreCase = new AsciiCharComparerIgnoreCase();

    /// <summary>
    /// Initialises the base comparer. Intended for derived-class use only.
    /// </summary>
    protected AsciiCharComparer()
    {
    }

    /// <inheritdoc/>
    public abstract int Compare(AsciiChar x, AsciiChar y);
    /// <inheritdoc/>
    public abstract bool Equals(AsciiChar x, AsciiChar y);
    /// <inheritdoc/>
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
    private sealed class AsciiCharComparerIgnoreCase : AsciiCharComparer
    {
        public override int Compare(AsciiChar x, AsciiChar y)
        {
            return AsciiChar.CompareToIgnoreCase(x, y);
        }

        public override bool Equals(AsciiChar x, AsciiChar y)
        {
            return AsciiChar.EqualsIgnoreCase(x, y);
        }

        public override int GetHashCode(AsciiChar obj)
        {
            return obj.ToUpper().GetHashCode();
        }
    }
}
