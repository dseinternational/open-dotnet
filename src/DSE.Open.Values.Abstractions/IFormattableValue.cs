// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Defines a value type that wraps an underlying <see cref="IFormattable"/> value
/// of type <typeparamref name="T"/> and can be formatted using a format string
/// and format provider.
/// </summary>
/// <typeparam name="TSelf">The type that implements the interface.</typeparam>
/// <typeparam name="T">The underlying value type being wrapped.</typeparam>
public interface IFormattableValue<TSelf, T> : IValue<TSelf, T>, IFormattable
    where T : IEquatable<T>, IFormattable
    where TSelf : struct, IFormattableValue<TSelf, T>
{
    /// <inheritdoc/>
    new virtual string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ((T)this).ToString(format, formatProvider);
    }
}
