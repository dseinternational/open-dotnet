// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Extension methods for converting between primitive values and <see cref="IValue{TValue, T}"/> instances.
/// </summary>
public static class ValueExtensions
{
    // should this be editor hidden?

    /// <summary>
    /// Casts a primitive <typeparamref name="T"/> to a <typeparamref name="TValue"/>,
    /// throwing <see cref="InvalidCastException"/> if the primitive is not a valid value.
    /// </summary>
    /// <exception cref="InvalidCastException">The supplied value is not a valid <typeparamref name="TValue"/>.</exception>
    public static TValue CastToValue<TValue, T>(this T value)
        where T : IEquatable<T>
        where TValue : struct, IValue<TValue, T>
    {
        return TValue.TryFromValue(value, out var result)
           ? result
           : throw new InvalidCastException(
               $"The {nameof(T)} value '{value}' cannot be cast " +
               $"to a value of type {nameof(TValue)}");
    }
}
