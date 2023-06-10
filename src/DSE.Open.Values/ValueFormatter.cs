// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

public static class ValueFormatter
{
    public static bool TryFormat<TValue, T>(
        TValue value,
        Span<char> destination,
        out int charsWritten)
        where T : IEquatable<T>, ISpanFormattable
        where TValue : struct, IValue<TValue, T>

        => TryFormat<TValue, T>(value, destination, out charsWritten, default, null);

    public static bool TryFormat<TValue, T>(
        TValue value,
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        where T : IEquatable<T>, ISpanFormattable
        where TValue : struct, IValue<TValue, T>

        => ((T)value).TryFormat(destination, out charsWritten, format, provider);

    public static string Format<TValue, T>(TValue value, string? format, IFormatProvider? formatProvider)
        where T : IEquatable<T>, IFormattable
        where TValue : struct, IValue<TValue, T>

        => ((T)value).ToString(format, formatProvider);
}