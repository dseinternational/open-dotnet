// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Indicates that a <see cref="IValue{TSelf, T}"/> type can be written to a span of characters.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
public interface IUtf8SpanFormattableValue<TSelf, T> : IUtf8SpanFormattable
    where T : IEquatable<T>, IUtf8SpanFormattable
    where TSelf : struct, IUtf8SpanFormattableValue<TSelf, T>
{
    new bool TryFormat(Span<byte> utf8Destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return ((T)this).TryFormat(utf8Destination, out charsWritten, format, provider);
    }
}
