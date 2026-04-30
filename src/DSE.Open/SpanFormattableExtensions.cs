// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Extensions over <see cref="ISpanFormattable"/>.
/// </summary>
public static class SpanFormattableExtensions
{
    /// <summary>
    /// Tries to format <paramref name="formattable"/> into <paramref name="destination"/> using the
    /// default format and current culture.
    /// </summary>
    public static bool TryFormat<T>(this T formattable, Span<char> destination, out int charsWritten) where T : ISpanFormattable
    {
        ArgumentNullException.ThrowIfNull(formattable);
        return formattable.TryFormat(destination, out charsWritten, default, default);
    }

    /// <summary>
    /// Tries to format <paramref name="formattable"/> into <paramref name="destination"/> using the
    /// specified <paramref name="format"/> and the current culture.
    /// </summary>
    public static bool TryFormat<T>(this T formattable, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        where T : ISpanFormattable
    {
        ArgumentNullException.ThrowIfNull(formattable);
        return formattable.TryFormat(destination, out charsWritten, format, default);
    }

    /// <summary>
    /// Tries to format <paramref name="formattable"/> into <paramref name="destination"/> using the
    /// default format and the specified <paramref name="formatProvider"/>.
    /// </summary>
    public static bool TryFormat<T>(this T formattable, Span<char> destination, out int charsWritten, IFormatProvider? formatProvider)
        where T : ISpanFormattable
    {
        ArgumentNullException.ThrowIfNull(formattable);
        return formattable.TryFormat(destination, out charsWritten, default, formatProvider);
    }

    /// <summary>
    /// Tries to format <paramref name="formattable"/> into <paramref name="destination"/> using the
    /// default format and the invariant culture.
    /// </summary>
    public static bool TryFormatInvariant<T>(this T formattable, Span<char> destination, out int charsWritten) where T : ISpanFormattable
    {
        ArgumentNullException.ThrowIfNull(formattable);
        return formattable.TryFormat(destination, out charsWritten, default, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Tries to format <paramref name="formattable"/> into <paramref name="destination"/> using the
    /// specified <paramref name="format"/> and the invariant culture.
    /// </summary>
    public static bool TryFormatInvariant<T>(this T formattable, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        where T : ISpanFormattable
    {
        ArgumentNullException.ThrowIfNull(formattable);
        return formattable.TryFormat(destination, out charsWritten, format, CultureInfo.InvariantCulture);
    }
}
