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
/// <para>We limit <see cref="INaValue{TSelf, T}"/> implementations to be value types as they are
/// expected to be 'simple' wrappers over existing value types or (heap-allocated) object references.
/// When wrapping value types, it is expected that casting to spans of the underlying value type
/// will be a common requirement.</para>
/// </remarks>
public interface INaValue<TSelf, T>
    : INaValue,
      IEquatable<TSelf>,
      IEqualityOperators<TSelf, TSelf, bool>,
      ITernaryEquatable<TSelf>
    where T : notnull, IEquatable<T>
    where TSelf : struct, INaValue<TSelf, T>
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
}
