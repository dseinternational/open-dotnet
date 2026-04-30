// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using DSE.Open.Hashing;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Runtime.InteropServices;

namespace DSE.Open;

public static partial class MemoryExtensions
{
    /// <summary>
    /// Returns a new <see cref="AsciiString"/> wrapping <paramref name="memory"/>.
    /// </summary>
    public static AsciiString ToAsciiString(this ReadOnlyMemory<AsciiChar> memory)
    {
        return new(memory);
    }

    /// <summary>
    /// Returns a new <see cref="AsciiString"/> over a copy of <paramref name="span"/>.
    /// </summary>
    public static AsciiString ToAsciiString(this ReadOnlySpan<AsciiChar> span)
    {
        return new(span.ToArray());
    }

    /// <summary>
    /// Returns a copy of the underlying ASCII bytes of <paramref name="memory"/>.
    /// </summary>
    public static ReadOnlyMemory<byte> ToBytes(this ReadOnlyMemory<AsciiChar> memory)
    {
        return memory.Span.ToBytes();
    }

    /// <summary>
    /// Returns a copy of the underlying ASCII bytes of <paramref name="span"/>.
    /// </summary>
    public static ReadOnlyMemory<byte> ToBytes(this ReadOnlySpan<AsciiChar> span)
    {
        return ValuesMarshal.AsBytes(span).ToArray();
    }

    /// <summary>
    /// Decodes the ASCII bytes in <paramref name="memory"/> to UTF-16 characters.
    /// </summary>
    public static ReadOnlyMemory<char> ToChars(this ReadOnlyMemory<AsciiChar> memory)
    {
        return memory.Span.ToChars();
    }

    /// <summary>
    /// Decodes the ASCII bytes in <paramref name="span"/> to UTF-16 characters.
    /// </summary>
    [SkipLocalsInit]
    public static ReadOnlyMemory<char> ToChars(this ReadOnlySpan<AsciiChar> span)
    {
        char[]? rented = null;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(span.Length)
            ? stackalloc char[MemoryThresholds.StackallocCharThreshold]
            : (rented = ArrayPool<char>.Shared.Rent(span.Length));

        try
        {
            var status = Ascii.ToUtf16(ValuesMarshal.AsBytes(span), buffer, out var charsWritten);

            if (status != OperationStatus.Done)
            {
                throw new InvalidOperationException();
            }

            return buffer[..charsWritten].ToArray();
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    /// <summary>
    /// Converts the <see cref="ReadOnlyMemory{AsciiChar}"/> to a <see cref="string"/>.
    /// </summary>
    /// <param name="memory"></param>
    /// <returns></returns>
    public static string ToStringValue(this ReadOnlyMemory<AsciiChar> memory)
    {
        return memory.Span.ToString();
    }

    /// <summary>
    /// Converts the <see cref="ReadOnlySpan{AsciiChar}"/> to a <see cref="string"/>.
    /// </summary>
    /// <param name="span"></param>
    /// <returns></returns>
    [SkipLocalsInit]
    public static string ToStringValue(this ReadOnlySpan<AsciiChar> span)
    {
        char[]? rented = null;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(span.Length)
            ? stackalloc char[span.Length]
            : (rented = ArrayPool<char>.Shared.Rent(span.Length));

        try
        {
            var status = Ascii.ToUtf16(ValuesMarshal.AsBytes(span), buffer, out var charsWritten);

            if (status != OperationStatus.Done)
            {
                throw new InvalidOperationException();
            }

            return new(buffer[..charsWritten]);
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    /// <summary>
    /// Returns a deterministic, platform-stable 64-bit hash code for <paramref name="value"/>.
    /// </summary>
    public static ulong GetRepeatableHashCode(this ReadOnlySpan<AsciiChar> value)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every element of <paramref name="value"/> is an ASCII letter.
    /// </summary>
    public static bool ContainsOnlyAsciiLetters(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLetters();
    }

    /// <summary>
    /// Returns <see langword="true"/> if every element of <paramref name="value"/> is an ASCII letter or digit.
    /// </summary>
    public static bool ContainsOnlyAsciiLettersOrDigits(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLettersOrDigits();
    }

    /// <summary>
    /// Returns <see langword="true"/> if every element of <paramref name="value"/> is an upper-case ASCII letter or digit.
    /// </summary>
    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiUpperLettersOrDigits();
    }

    /// <summary>
    /// Returns <see langword="true"/> if every element of <paramref name="value"/> is a lower-case ASCII letter.
    /// </summary>
    public static bool ContainsOnlyAsciiLettersLower(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLettersLower();
    }

    /// <summary>
    /// Returns <see langword="true"/> if every element of <paramref name="value"/> is an upper-case ASCII letter.
    /// </summary>
    public static bool ContainsOnlyAsciiLettersUpper(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLettersUpper();
    }

    /// <summary>
    /// Determines whether the specified spans of ASCII are equal, ignoring case.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool SequenceEqualsIgnoreCase(this ReadOnlySpan<AsciiChar> a, ReadOnlySpan<AsciiChar> b)
    {
        return Ascii.EqualsIgnoreCase(ValuesMarshal.AsBytes(a), ValuesMarshal.AsBytes(b));
    }
}
