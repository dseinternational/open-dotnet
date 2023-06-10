// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

public static class ValueExtensions
{
    // should this be editor hidden?

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