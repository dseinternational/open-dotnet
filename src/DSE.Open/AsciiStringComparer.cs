// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open;

public abstract class AsciiStringComparer : IComparer<AsciiString>, IEqualityComparer<AsciiString>
{
    public static readonly AsciiStringComparer CaseSensitive = new AsciiCharSequenceComparerCaseSensitive();
    public static readonly AsciiStringComparer CaseInsensitive = new AsciiCharSequenceComparerCaseInsensitive();

    protected AsciiStringComparer()
    {
    }

    public abstract int Compare(AsciiString x, AsciiString y);

    public abstract bool Equals(AsciiString x, AsciiString y);

    public abstract int GetHashCode([DisallowNull] AsciiString obj);

    private sealed class AsciiCharSequenceComparerCaseSensitive : AsciiStringComparer
    {
        public override int Compare(AsciiString x, AsciiString y) => x.CompareTo(y);
        public override bool Equals(AsciiString x, AsciiString y) => x.Equals(y);
        public override int GetHashCode([DisallowNull] AsciiString obj) => obj.GetHashCode();
    }

    private sealed class AsciiCharSequenceComparerCaseInsensitive : AsciiStringComparer
    {
        public override int Compare(AsciiString x, AsciiString y) => x.CompareToCaseInsensitive(y);
        public override bool Equals(AsciiString x, AsciiString y) => x.EqualsCaseInsensitive(y);
        public override int GetHashCode([DisallowNull] AsciiString obj)
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
