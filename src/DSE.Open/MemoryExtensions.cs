// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.Intrinsics;

namespace DSE.Open;

public static partial class MemoryExtensions
{
    public static bool ContainsOnly<T>(this Span<T> value, Func<T, bool> predicate)
    {
        return ContainsOnly((ReadOnlySpan<T>)value, predicate);
    }

    public static bool ContainsOnly<T>(this ReadOnlySpan<T> values, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (values.IsEmpty)
        {
            return false;
        }

        foreach (var value in values)
        {
            if (!predicate(value))
            {
                return false;
            }
        }

        return true;
    }

    private static bool ContainsOnlyCore<T>(
        ReadOnlySpan<T> value,
        SearchValues<T> searchValues,
        Func<T, bool> filter)
        where T : unmanaged, IEquatable<T>
    {
        Debug.Assert(typeof(T) == typeof(char) || Vector128<T>.IsSupported);

        // If we can, and the value is long enough to be worth it, pass off to the vectorized SearchValues
        if (Vector128.IsHardwareAccelerated)
        {
            var vectorise = typeof(T) == typeof(char)
                ? value.Length >= Vector128<ushort>.Count
                : value.Length >= Vector128<T>.Count;

            if (vectorise)
            {
                return value.IndexOfAnyExcept(searchValues) == -1;
            }
        }

        foreach (var t in value)
        {
            if (!filter(t))
            {
                return false;
            }
        }

        return true;
    }

    private static bool ContainsOnlyInRangeCore<T>(
        ReadOnlySpan<T> value,
        T lowInclusive,
        T highInclusive,
        Func<T, bool> filter)
        where T : unmanaged, IComparable<T>
    {
        Debug.Assert(typeof(T) == typeof(char) || Vector128<T>.IsSupported);

        // If we can, and the value is long enough to be worth it, pass off to the vectorized SearchValues
        if (Vector128.IsHardwareAccelerated)
        {
            var vectorise = typeof(T) == typeof(char)
                ? value.Length >= Vector128<ushort>.Count
                : value.Length >= Vector128<T>.Count;

            if (vectorise)
            {
                return value.IndexOfAnyExceptInRange(lowInclusive, highInclusive) == -1;
            }
        }

        foreach (var t in value)
        {
            if (!filter(t))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Converts to the contents of a span to a <see cref="Memory{T}"/> using the specified converter.
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <param name="source"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Memory<TOut> ConvertTo<TIn, TOut>(this ReadOnlySpan<TIn> source, Func<TIn, TOut> converter)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (source.IsEmpty)
        {
            return Array.Empty<TOut>();
        }

        var result = new TOut[source.Length];

        for (var i = 0; i < source.Length; i++)
        {
            result[i] = converter(source[i]);
        }

        return result;
    }

    public static bool TryCopyWhere<T>(this Span<T> span, Span<T> buffer, Func<T, bool> predicate, out int itemsCopied)
    {
        return TryCopyWhere((ReadOnlySpan<T>)span, buffer, predicate, out itemsCopied);
    }

    /// <summary>
    /// Tries to copy values matching the specified predicate from a span of values to a buffer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span"></param>
    /// <param name="buffer"></param>
    /// <param name="predicate"></param>
    /// <param name="itemsCopied"></param>
    /// <returns></returns>
    public static bool TryCopyWhere<T>(
        this ReadOnlySpan<T> span,
        Span<T> buffer,
        Func<T, bool> predicate,
        out int itemsCopied)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var bufferIndex = 0;

        foreach (var item in span)
        {
            if (predicate(item))
            {
                if (bufferIndex >= buffer.Length)
                {
                    itemsCopied = bufferIndex;
                    return false;
                }

                buffer[bufferIndex++] = item;
            }
        }

        itemsCopied = bufferIndex;
        return true;
    }

    public static int CopyWhere<T>(this Span<T> span, Span<T> buffer, Func<T, bool> predicate)
    {
        return CopyWhere((ReadOnlySpan<T>)span, buffer, predicate);
    }

    /// <summary>
    /// Copies values matching the specified predicate from a span of values to a new span.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span"></param>
    /// <param name="buffer"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown if the buffer is too small to hold all of
    /// the values.</exception>
    public static int CopyWhere<T>(this ReadOnlySpan<T> span, Span<T> buffer, Func<T, bool> predicate)
    {
        if (span.TryCopyWhere(buffer, predicate, out var charsWritten))
        {
            return charsWritten;
        }

        ThrowHelper.ThrowInvalidOperationException("Buffer is too small to hold all of the values.");
        return charsWritten; // unreachable
    }

    public static int CopyExcluding<T>(this Span<T> span, Span<T> buffer, T value)
        where T : IEquatable<T>
    {
        return CopyExcluding((ReadOnlySpan<T>)span, buffer, value);
    }

    /// <summary>
    /// Copies all of the values from a span of values to a new span, except those equal to the specified value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span"></param>
    /// <param name="buffer"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int CopyExcluding<T>(this ReadOnlySpan<T> span, Span<T> buffer, T value)
        where T : IEquatable<T>
    {
        var bufferIndex = 0;

        foreach (var item in span)
        {
            if (!item.Equals(value))
            {
                if (bufferIndex >= buffer.Length)
                {
                    return ThrowHelper.ThrowInvalidOperationException<int>(
                        "Buffer is too small to hold all of the values");
                }

                buffer[bufferIndex++] = item;
            }
        }

        return bufferIndex;
    }

    /// <summary>
    /// Removes all instances of the specified value from a span of values and returns a span over the result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span"></param>
    /// <param name="value"></param>
    /// <returns>A span of values with the specified value removed.</returns>
    public static Span<T> Remove<T>(this Span<T> span, T value)
        where T : IEquatable<T>
    {
        if (span.IsEmpty)
        {
            return span;
        }

        var offset = 0;

        for (var i = 0; i < span.Length; i++)
        {
            if (offset != 0)
            {
                span[i + offset] = span[i];
            }

            if (span[i].Equals(value))
            {
                offset--;
            }
        }

        return span[..(span.Length + offset)];
    }

    /// <summary>
    /// Removes all instances of the values matching the specified prediacte from a span of values
    /// and returns a span over the result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Span<T> Remove<T>(this Span<T> span, Func<T, bool> predicate)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (span.IsEmpty)
        {
            return span;
        }

        var offset = 0;

        for (var i = 0; i < span.Length; i++)
        {
            if (offset != 0)
            {
                span[i + offset] = span[i];
            }

            if (predicate(span[i]))
            {
                offset--;
            }
        }

        return span[..(span.Length + offset)];
    }

    public static int Replace<T>(this Span<T> span, Func<T, bool> predicate, T replacement)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (span.IsEmpty)
        {
            return 0;
        }

        var count = 0;

        for (var i = 0; i < span.Length; i++)
        {
            if (predicate(span[i]))
            {
                span[i] = replacement;
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Computes the sum of the sequence of number values that are obtained by invoking a transform function
    /// on each element of the input sequence.
    /// </summary>
    public static TResult Sum<TSource, TResult>(this ReadOnlySpan<TSource> source, Func<TSource, TResult> selector)
        where TResult : struct, INumber<TResult>
    {
        ArgumentNullException.ThrowIfNull(selector);

        var accumulator = TResult.Zero;

        foreach (var source1 in source)
        {
            checked
            {
                accumulator += selector(source1);
            }
        }

        return accumulator;
    }
}
