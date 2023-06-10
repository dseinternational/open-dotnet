// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Values;

public static class ValueParser
{
    public static bool TryParse<TValue, T>(
        ReadOnlySpan<char> s,
        [MaybeNullWhen(false)] out TValue result)
        where T : IEquatable<T>, ISpanParsable<T>
        where TValue : struct, IValue<TValue, T>

        => TryParse<TValue, T>(s, null, out result);

    public static TValue Parse<TValue, T>(ReadOnlySpan<char> s, IFormatProvider? provider)
        where T : IEquatable<T>, ISpanParsable<T>
        where TValue : struct, IValue<TValue, T>

        => (TValue)T.Parse(s, provider);

    public static bool TryParse<TValue, T>(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out TValue result)
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

        => (TValue)T.Parse(s, provider);
}