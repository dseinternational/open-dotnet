// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open;

public abstract class AsciiCharSequenceComparer : IComparer<AsciiCharSequence>, IEqualityComparer<AsciiCharSequence>
{
    public static readonly AsciiCharSequenceComparer CaseSensitive = new AsciiCharSequenceComparerCaseSensitive();
    public static readonly AsciiCharSequenceComparer CaseInsensitive = new AsciiCharSequenceComparerCaseInsensitive();

    protected AsciiCharSequenceComparer()
    {
    }

    public abstract int Compare(AsciiCharSequence x, AsciiCharSequence y);

    public abstract bool Equals(AsciiCharSequence x, AsciiCharSequence y);

    public abstract int GetHashCode([DisallowNull] AsciiCharSequence obj);

    private sealed class AsciiCharSequenceComparerCaseSensitive : AsciiCharSequenceComparer
    {
        public override int Compare(AsciiCharSequence x, AsciiCharSequence y) => x.CompareTo(y);
        public override bool Equals(AsciiCharSequence x, AsciiCharSequence y) => x.Equals(y);
        public override int GetHashCode([DisallowNull] AsciiCharSequence obj) => obj.GetHashCode();
    }

    private sealed class AsciiCharSequenceComparerCaseInsensitive : AsciiCharSequenceComparer
    {
        public override int Compare(AsciiCharSequence x, AsciiCharSequence y) => x.CompareToCaseInsensitive(y);
        public override bool Equals(AsciiCharSequence x, AsciiCharSequence y) => x.EqualsCaseInsensitive(y);
        public override int GetHashCode([DisallowNull] AsciiCharSequence obj)
        {
            byte[]? rented = null;

            var source = obj.AsSpan();

            try
            {
                Span<byte> buffer = source.Length > StackallocThresholds.MaxByteLength
                    ? (rented = ArrayPool<byte>.Shared.Rent(StackallocThresholds.MaxByteLength))
                    : stackalloc byte[source.Length];

                for (var i = 0; i < source.Length; i++)
                {
                    buffer[i] = AsciiChar.ToUpper(source[i]);
                }

                var c = new HashCode();
                c.AddBytes(buffer);
                return c.ToHashCode();
            }
            finally
            {
                if (rented is not null)
                {
                    ArrayPool<byte>.Shared.Return(rented);
                }
            }
        }
    }
}
