// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open;

public static partial class MemoryExtensions
{
    public static AsciiString ToAsciiString(this ReadOnlyMemory<AsciiChar> memory)
    {
        return new(memory);
    }

    public static AsciiString ToAsciiString(this ReadOnlySpan<AsciiChar> span)
    {
        return new(span.ToArray());
    }

    public static ReadOnlyMemory<byte> ToBytes(this ReadOnlyMemory<AsciiChar> memory)
    {
        return memory.Span.ToBytes();
    }

    public static ReadOnlyMemory<byte> ToBytes(this ReadOnlySpan<AsciiChar> span)
    {
        return ValuesMarshal.AsBytes(span).ToArray();
    }

    public static ReadOnlyMemory<char> ToChars(this ReadOnlyMemory<AsciiChar> memory)
    {
        return memory.Span.ToChars();
    }

    public static ReadOnlyMemory<char> ToChars(this ReadOnlySpan<AsciiChar> span)
    {
        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(span.Length)
            ? stackalloc char[span.Length]
            : new char[span.Length];

        var status = Ascii.ToUtf16(ValuesMarshal.AsBytes(span), buffer, out var charsWritten);

        if (status != OperationStatus.Done)
        {
            throw new InvalidOperationException();
        }

        return buffer[..charsWritten].ToArray();
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
    public static string ToStringValue(this ReadOnlySpan<AsciiChar> span)
    {
        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(span.Length)
            ? stackalloc char[span.Length]
            : new char[span.Length];

        var status = Ascii.ToUtf16(ValuesMarshal.AsBytes(span), buffer, out var charsWritten);

        if (status != OperationStatus.Done)
        {
            throw new InvalidOperationException();
        }

        return new(buffer[..charsWritten]);
    }

    public static bool ContainsOnlyAsciiLetters(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLetters();
    }

    public static bool ContainsOnlyAsciiLettersOrDigits(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLettersOrDigits();
    }

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiUpperLettersOrDigits();
    }

    public static bool ContainsOnlyAsciiLettersLower(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLettersLower();
    }

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
