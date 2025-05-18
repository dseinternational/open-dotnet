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
    static virtual TSelf Null => default!;

    /// <summary>
    /// If the value is set (<see cref="INullable.HasValue"/>), then returns that value, otherwise
    /// throws a <see cref="UnknownValueException"/>.
    /// </summary>
    /// <exception cref="UnknownValueException">Thrown if no value is available
    /// (<see cref="INullable.HasValue"/> is false).</exception>
    new T Value { get; }

    object INullable.Value => Value;

    new bool Equals(TSelf other)
    {
        return TSelf.Equals((TSelf)this, other);
    }

    // Note: as for Nullable<T> and consistent with IEquatable<TSelf>.Equals:
    // -----------------------
    // v1      v2    result
    // -----------------------
    // null    null  true
    // null    1     false
    // 1       null  false
    // 1       1     true

    static virtual bool Equals(TSelf v1, TSelf v2)
    {
        return v1.HasValue
            ? v2.HasValue && v1.Value.Equals(v2.Value)
            : !v2.HasValue;
    }

    static virtual bool operator ==(TSelf left, TSelf right)
    {
        return TSelf.Equals(left, right);
    }

    static virtual bool operator !=(TSelf left, TSelf right)
    {
        return !TSelf.Equals(left, right);
    }
}
