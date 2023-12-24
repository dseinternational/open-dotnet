// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// Specifies a range of numeric values.
/// </summary>
/// <typeparam name="T"></typeparam>
[StructLayout(LayoutKind.Auto)]
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
public readonly record struct Range<T> : ISpanFormattable, ISpanParsable<Range<T>>
    where T : INumber<T>
{
    public const int MaxLength = StackallocThresholds.MaxCharLength;

    public static readonly Range<T> Empty;

    [JsonConstructor]
    public Range(T start, T end)
    {
        Guard.IsLessThanOrEqualTo(start, end);

        Start = start;
        End = end;
    }

    /// <summary>
    /// The inclusive start of the range of values.
    /// </summary>
    public T Start { get; }

    /// <summary>
    /// The inclusive end of the range of values.
    /// </summary>
    public T End { get; }

    public T Length => End - Start;

    /// <summary>
    /// Indicates if the value is within the range specified.
    /// </summary>
    /// <param name="value"></param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is greater that or equal to
    /// <see cref="Start"/> and less than or equal to <see cref="End"/>, otherwise
    /// <see langword="false"/>.</returns>
    public bool Includes(T value)
    {
        return Start.CompareTo(value) <= 0 && End.CompareTo(value) >= 0;
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var buffer = new char[128];
        return TryFormat(buffer, out var charsWritten, format, formatProvider)
            ? buffer.AsSpan()[..charsWritten].ToString()
            : string.Empty;
    }

    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, default, default);
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
    {
        return TryFormat(destination, out charsWritten, format, null);
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (this == Empty)
        {
            charsWritten = 0;
            return true;
        }

        if (!Start.TryFormat(destination, out var charsWritten1, format, provider))
        {
            charsWritten = charsWritten1;
            return false;
        }

        destination[charsWritten1++] = ':';

        if (!End.TryFormat(destination[charsWritten1..], out var charsWritten2, format, provider))
        {
            charsWritten = charsWritten1 + charsWritten2;
            return false;
        }

        charsWritten = charsWritten1 + charsWritten2;
        return true;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Range<T> result)
    {
        if (s.IsEmpty)
        {
            result = Empty;
            return true;
        }

        var colonIndex = s.IndexOf(':');

        if (colonIndex == -1)
        {
            result = default;
            return false;
        }

        if (!T.TryParse(s[..colonIndex], provider, out var start) || !T.TryParse(s[(colonIndex + 1)..], provider, out var end))
        {
            result = default;
            return false;
        }

        result = new Range<T>(start, end);
        return true;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Range<T> result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static Range<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<Range<T>>($"Could not parse {nameof(Range)} from value: {s}");
    }

    public static Range<T> Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }
}
