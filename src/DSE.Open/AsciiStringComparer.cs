// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open;

public abstract class AsciiStringComparer : IComparer<AsciiString>, IEqualityComparer<AsciiString>
{
    public static readonly AsciiStringComparer CaseSensitive = new AsciiCharSequenceComparerCaseSensitive();
    public static readonly AsciiStringComparer IgnoreCase = new AsciiCharSequenceComparerIgnoreCase();

    protected AsciiStringComparer()
    {
    }

    public abstract int Compare(AsciiString x, AsciiString y);

    public abstract bool Equals(AsciiString x, AsciiString y);

    public abstract int GetHashCode(AsciiString obj);

    private sealed class AsciiCharSequenceComparerCaseSensitive : AsciiStringComparer
    {
        public override int Compare(AsciiString x, AsciiString y)
        {
            return x.CompareTo(y);
        }

        public override bool Equals(AsciiString x, AsciiString y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode(AsciiString obj)
        {
            return obj.GetHashCode();
        }
    }

    private sealed class AsciiCharSequenceComparerIgnoreCase : AsciiStringComparer
    {
        public override int Compare(AsciiString x, AsciiString y)
        {
            return x.CompareToIgnoreCase(y);
        }

        public override bool Equals(AsciiString x, AsciiString y)
        {
            return x.EqualsIgnoreCase(y);
        }

        [SkipLocalsInit]
        public override int GetHashCode(AsciiString obj)
        {
            var source = obj.AsSpan();

            var rented = SpanOwner<AsciiChar>.Empty;

            Span<AsciiChar> buffer = MemoryThresholds.CanStackalloc<AsciiChar>(source.Length)
                ? stackalloc AsciiChar[source.Length]
                : (rented = SpanOwner<AsciiChar>.Allocate(source.Length)).Span;

            using (rented)
            {
                for (var i = 0; i < source.Length; i++)
                {
                    buffer[i] = source[i].ToUpper();
                }

                var c = new HashCode();
                c.AddBytes(ValuesMarshal.AsBytes(buffer[..source.Length]));
                return c.ToHashCode();
            }
        }
    }
}
