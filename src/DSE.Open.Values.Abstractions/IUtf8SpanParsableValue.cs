// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Values;

/// <summary>
/// Indicates that a <see cref="IValue{TSelf, T}"/> type can be read from a span of bytes
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
public interface IUtf8SpanParsableValue<TSelf, T> : IValue<TSelf, T>, IUtf8SpanParsable<TSelf>
    where T : IEquatable<T>, IUtf8SpanParsable<T>
    where TSelf : struct, IUtf8SpanParsableValue<TSelf, T>
{
    static new TSelf Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        return (TSelf)T.Parse(utf8Text, provider);
    }

    static new bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider,
        out TSelf result)
    {
        if (T.TryParse(utf8Text, provider, out var valueResult) && TSelf.TryFromValue(valueResult, out result))
        {
            return true;
        }

        result = default;
        return false;
    }
}
