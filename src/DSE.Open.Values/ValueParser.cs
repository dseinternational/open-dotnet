// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;

namespace DSE.Open.Values;

/// <summary>
/// Helpers for parsing <see cref="IValue{TValue, T}"/> instances from text.
/// </summary>
public static class ValueParser
{
    /// <summary>
    /// Attempts to parse a <typeparamref name="TValue"/> from the specified character span
    /// using the invariant format provider.
    /// </summary>
    public static bool TryParse<TValue, T>(
        ReadOnlySpan<char> s,
        out TValue result)
        where T : IEquatable<T>, ISpanParsable<T>
        where TValue : struct, IValue<TValue, T>
    {
        return TryParse<TValue, T>(s, null, out result);
    }

    /// <summary>
    /// Parses a <typeparamref name="TValue"/> from the specified character span,
    /// throwing a <see cref="FormatException"/> if the input cannot be parsed.
    /// </summary>
    public static TValue Parse<TValue, T>(ReadOnlySpan<char> s, IFormatProvider? provider)
        where T : IEquatable<T>, ISpanParsable<T>
        where TValue : struct, IValue<TValue, T>
    {
        if (TryParse<TValue, T>(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<TValue>($"Cannot parse '{s}' as {typeof(TValue).Name}.");
    }

    /// <summary>
    /// Attempts to parse a <typeparamref name="TValue"/> from the specified character span
    /// using the supplied format provider.
    /// </summary>
    public static bool TryParse<TValue, T>(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out TValue result)
        where T : IEquatable<T>, ISpanParsable<T>
        where TValue : struct, IValue<TValue, T>
    {
        if (T.TryParse(s, provider, out var valueResult) && TValue.TryFromValue(valueResult, out result))
        {
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Parses a <typeparamref name="TValue"/> from the specified string,
    /// throwing a <see cref="FormatException"/> if the input cannot be parsed.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    public static TValue Parse<TValue, T>(string s, IFormatProvider? provider)
        where T : IEquatable<T>, IParsable<T>
        where TValue : struct, IValue<TValue, T>
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TValue.TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<TValue>($"Cannot parse '{s}' as {typeof(TValue).Name}.");
    }

    /// <summary>
    /// Attempts to parse a <typeparamref name="TValue"/> from the specified UTF-8 byte span.
    /// </summary>
    public static bool TryParse<TValue, T>(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider,
        out TValue result)
        where T : IEquatable<T>, IUtf8SpanParsable<T>
        where TValue : struct, IValue<TValue, T>
    {
        if (T.TryParse(utf8Text, provider, out var valueResult) && TValue.TryFromValue(valueResult, out result))
        {
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Parses a <typeparamref name="TValue"/> from the specified UTF-8 byte span,
    /// throwing a <see cref="FormatException"/> if the input cannot be parsed.
    /// </summary>
    public static TValue Parse<TValue, T>(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider)
        where T : IEquatable<T>, IUtf8SpanParsable<T>
        where TValue : struct, IValue<TValue, T>
    {
        if (TryParse<TValue, T>(utf8Text, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<TValue>($"Cannot parse '{Encoding.UTF8.GetString(utf8Text)}' as {typeof(TValue).Name}.");
    }
}
