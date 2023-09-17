// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;

namespace DSE.Open.Values;

public static class ValueParser
{
    public static bool TryParse<TValue, T>(
        ReadOnlySpan<char> s,
        out TValue result)
        where T : IEquatable<T>, ISpanParsable<T>
        where TValue : struct, IValue<TValue, T>
        => TryParse<TValue, T>(s, null, out result);

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

    public static TValue Parse<TValue, T>(string s, IFormatProvider? provider)
        where T : IEquatable<T>, IParsable<T>
        where TValue : struct, IValue<TValue, T>
    {
        Guard.IsNotNull(s);

        if (TValue.TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<TValue>($"Cannot parse '{s}' as {typeof(TValue).Name}.");
    }

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
