// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Helpers for formatting <see cref="IValue{TValue, T}"/> instances by delegating to their
/// underlying representation.
/// </summary>
public static class ValueFormatter
{
    /// <summary>
    /// Attempts to format the underlying value into the destination span using default format and provider.
    /// </summary>
    public static bool TryFormat<TValue, T>(
        TValue value,
        Span<char> destination,
        out int charsWritten)
        where T : IEquatable<T>, ISpanFormattable
        where TValue : struct, IValue<TValue, T>
    {
        return TryFormat<TValue, T>(value, destination, out charsWritten, default, null);
    }

    /// <summary>
    /// Attempts to format the underlying value into the destination span using the specified
    /// format string and format provider.
    /// </summary>
    public static bool TryFormat<TValue, T>(
        TValue value,
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        where T : IEquatable<T>, ISpanFormattable
        where TValue : struct, IValue<TValue, T>
    {
        return ((T)value).TryFormat(destination, out charsWritten, format, provider);
    }

    /// <summary>
    /// Formats the underlying value as a string using the specified format string and format provider.
    /// </summary>
    public static string Format<TValue, T>(TValue value, string? format, IFormatProvider? formatProvider)
        where T : IEquatable<T>, IFormattable
        where TValue : struct, IValue<TValue, T>
    {
        return ((T)value).ToString(format, formatProvider);
    }
}
