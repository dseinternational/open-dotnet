// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Indicates that a <see cref="IValue{TSelf, T}"/> type can be written to and read from a span of characters.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
public interface ISpanSerializableValue<TSelf, T>
    : IValue<TSelf, T>,
      ISpanParsableValue<TSelf, T>,
      ISpanFormattableValue<TSelf, T>
    where T
    : IEquatable<T>,
      ISpanParsable<T>,
      ISpanFormattable
    where TSelf : struct, ISpanSerializableValue<TSelf, T>
{
}
