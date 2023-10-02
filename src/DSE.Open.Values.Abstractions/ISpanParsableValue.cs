// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Values;

/// <summary>
/// Indicates that a <see cref="IValue{TSelf, T}"/> type can be read from a span of characters.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
public interface ISpanParsableValue<TSelf, T> : IParsableValue<TSelf, T>, ISpanParsable<TSelf>
    where T : IEquatable<T>, ISpanParsable<T>
    where TSelf : struct, ISpanParsableValue<TSelf, T>
{
    static new TSelf Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => (TSelf)T.Parse(s, provider);

    static new bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out TSelf result)
    {
        if (T.TryParse(s, provider, out var valueResult) && TSelf.TryFromValue(valueResult, out result))
        {
            return true;
        }

        result = default;
        return false;
    }
}
