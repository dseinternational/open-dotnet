// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open;

/// <summary>
/// A value that may be 'not a value', missing, or not available, such as <see langword="null"/> or
/// <see cref="IFloatingPointIeee754{TSelf}.NaN"/>, that provides consistent semantics for handling
/// 'not a value' values and null/missing values.
/// </summary>
/// <remarks>
/// See remarks on <see cref="INaValue{TSelf, T}"/> for more details.
/// </remarks>
public interface INaValue
{
    /// <summary>
    /// Indicates if <see cref="Value"/> is not missing and is known to be a legitimate value.
    /// </summary>
    bool HasValue { get; }

    /// <summary>
    /// Indicates if <see cref="Value"/> is missing, not known or is 'not a value'.
    /// </summary>
    virtual bool IsNa => !HasValue;

    /// <summary>
    /// If the value is known (<see cref="HasValue"/>), then returns that value, otherwise throws
    /// an <see cref="NaValueException"/>.
    /// </summary>
    /// <exception cref="NaValueException">Thrown if no value is available
    /// (<see cref="HasValue"/> is false).</exception>
    object Value { get; }
}

/// <summary>
/// A value that may be 'not a value', missing, or not available, such as <see langword="null"/> or
/// <see cref="IFloatingPointIeee754{TSelf}.NaN"/>, that provides consistent semantics for handling
/// 'not a value' values and null/missing values.
/// </summary>
/// <typeparam name="TSelf">The NaValue type.</typeparam>
/// <typeparam name="T">The underlying data type.</typeparam>
/// <remarks>
/// <see cref="INaValue{TSelf, T}"/> defines an interface that is similar to <see cref="Nullable{T}"/>
/// in that it wraps an underlying value and provides a way to determine if that value is missing.
/// However, it can be applied to both reference and value types.
/// </remarks>
public interface INaValue<TSelf, T>
    : INaValue,
      IEquatable<TSelf>,
      IEqualityOperators<TSelf, TSelf, bool>,
      ITernaryEquatable<TSelf>
    where T : notnull, IEquatable<T>
    where TSelf : notnull, INaValue<TSelf, T>
{
    /// <summary>
    /// Gets a value that represents the missing, <see langword="null"/> or 'not a value' state.
    /// </summary>
    static abstract TSelf Na { get; }

    /// <summary>
    /// If the value is known (<see cref="INaValue.HasValue"/>), then returns that value, otherwise
    /// throws a <see cref="NaValueException"/>.
    /// </summary>
    /// <exception cref="NaValueException">Thrown if no value is available
    /// (<see cref="INaValue.HasValue"/> is false).</exception>
    new T Value { get; }

    object INaValue.Value => Value;

    static virtual bool EqualAndNotNa(INaValue<TSelf, T> n1, INaValue<TSelf, T> n2)
    {
        if (n1 is null || n2 is null)
        {
            return false;
        }

        return n2.HasValue && n1.HasValue && n2.Value.Equals(n1.Value);
    }

    static virtual bool EqualOrBothNa(INaValue<TSelf, T> n1, INaValue<TSelf, T> n2)
    {
        if (n1 is null)
        {
            return n2 is null;
        }

        if (n2 is null)
        {
            return false;
        }

        if (n1.HasValue && n2.HasValue)
        {
            return true;
        }

        return n2.Value.Equals(n1.Value);
    }

    static virtual bool EqualOrEitherNa(INaValue<TSelf, T> n1, INaValue<TSelf, T> n2)
    {
        if (n1 is null || n2 is null)
        {
            return true;
        }

        if (n1.HasValue || n2.HasValue)
        {
            return true;
        }

        return n2.Value.Equals(n1.Value);
    }
}
