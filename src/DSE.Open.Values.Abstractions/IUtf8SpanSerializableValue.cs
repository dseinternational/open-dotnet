// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Indicates that a <see cref="IValue{TSelf, T}"/> type can be written to and read from a span of bytes.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
public interface IUtf8SpanSerializableValue<TSelf, T> : IValue<TSelf, T>, IUtf8SpanParsableValue<TSelf, T>, IUtf8SpanFormattableValue<TSelf, T>
    where T : IEquatable<T>, IUtf8SpanParsable<T>, IUtf8SpanFormattable
    where TSelf : struct, IUtf8SpanSerializableValue<TSelf, T>
{
}
