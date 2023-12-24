// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Indicates that a <see cref="IValue{TSelf, T}"/> type can be written to a span of characters.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
public interface ISpanFormattableValue<TSelf, T> : IFormattableValue<TSelf, T>, ISpanFormattable
    where T : IEquatable<T>, ISpanFormattable
    where TSelf : struct, ISpanFormattableValue<TSelf, T>
{
    new bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return ((T)this).TryFormat(destination, out charsWritten, format, provider);
    }
}
