// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open;

public static class CharSequenceExtensions
{
    public static CharSequence ToCharSequence<T>(this T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
        where T : struct, ISpanFormattable
    {
        Span<char> buffer = stackalloc char[MemoryThresholds.StackallocCharThreshold];

        if (value.TryFormat(buffer, out var charsWritten, format, provider))
        {
            return new(buffer[..charsWritten].ToArray());
        }

        var poolBuffer = ArrayPool<char>.Shared.Rent(4096);

        try
        {
            if (value.TryFormat(poolBuffer, out var charsWritten2, format, provider))
            {
                return new(poolBuffer.AsMemory(0, charsWritten2));
            }

            ThrowHelper.ThrowInvalidOperationException("Unable to format value to char sequence: is the result > 4096 characters?");
            return default; // unreachable
        }
        finally
        {
            ArrayPool<char>.Shared.Return(poolBuffer);
        }
    }

    public static CharSequence ToCharSequence(this string value)
    {
        return new(value);
    }

    public static CharSequence ToCharSequence(this ReadOnlyMemory<char> value)
    {
        return new(value);
    }

    public static CharSequence ToCharSequence(this ReadOnlySpan<char> value)
    {
        return new(value.ToArray());
    }
}
