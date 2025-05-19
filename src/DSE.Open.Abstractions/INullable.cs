// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open;

/// <summary>
/// A value that may be 'null' (have no value).
/// </summary>
public interface INullable
{
    /// <summary>
    /// Indicates if the current value has been set.
    /// </summary>
    bool HasValue { get; }

    virtual bool IsUnknown => !HasValue;

    /// <summary>
    /// If the value is set (<see cref="HasValue"/>), then returns that value, otherwise throws
    /// a <see cref="UnknownValueException"/>.
    /// </summary>
    /// <exception cref="UnknownValueException">Thrown if no value is available
    /// (<see cref="HasValue"/> is false).</exception>
    object Value { get; }
}

/// <summary>
/// A value that may be 'null' (have no value).
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
public interface INullable<TSelf, T>
    : INullable,
      IEquatable<TSelf>,
      IEqualityOperators<TSelf, TSelf, bool>
    where T : IEquatable<T>
    where TSelf : INullable<TSelf, T>
{
    /// <summary>
    /// Gets a value that represents the 'null' or 'no value' state.
    /// </summary>
    static abstract TSelf Null { get; }

    /// <summary>
    /// If the value is set (<see cref="INullable.HasValue"/>), then returns that value, otherwise
    /// throws a <see cref="UnknownValueException"/>.
    /// </summary>
    /// <exception cref="UnknownValueException">Thrown if no value is available
    /// (<see cref="INullable.HasValue"/> is false).</exception>
    new T Value { get; }

    object INullable.Value => Value;
}
