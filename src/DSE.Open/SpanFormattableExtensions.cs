// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class SpanFormattableExtensions
{
    public static bool TryFormat(this ISpanFormattable formattable, Span<char> destination, out int charsWritten)
    {
        Guard.IsNotNull(formattable);
        return formattable.TryFormat(destination, out charsWritten, default, default);
    }

    public static bool TryFormat(this ISpanFormattable formattable, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
    {
        Guard.IsNotNull(formattable);
        return formattable.TryFormat(destination, out charsWritten, format, default);
    }

    public static bool TryFormat(this ISpanFormattable formattable, Span<char> destination, out int charsWritten, IFormatProvider? formatProvider)
    {
        Guard.IsNotNull(formattable);
        return formattable.TryFormat(destination, out charsWritten, default, formatProvider);
    }

    public static bool TryFormatInvariant(this ISpanFormattable formattable, Span<char> destination, out int charsWritten)
    {
        Guard.IsNotNull(formattable);
        return formattable.TryFormat(destination, out charsWritten, default, CultureInfo.InvariantCulture);
    }

    public static bool TryFormatInvariant(this ISpanFormattable formattable, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
    {
        Guard.IsNotNull(formattable);
        return formattable.TryFormat(destination, out charsWritten, format, CultureInfo.InvariantCulture);
    }
}
