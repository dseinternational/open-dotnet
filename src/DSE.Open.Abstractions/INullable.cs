// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

using System.Numerics;

namespace DSE.Open;

/// <summary>
/// Encapsulates a value that may be 'null' (have no value).
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
public interface INullable<TSelf, T> : IEquatable<TSelf>, IEqualityOperators<TSelf, TSelf, bool>
    where T : IEquatable<T>
    where TSelf : struct, INullable<TSelf, T>
{
    /// <summary>
    /// Gets a value that represents the 'null' or 'no value' state.
    /// </summary>
    static virtual TSelf Null => default!;

    /// <summary>
    /// If the value can be represented as a value of type <typeparamref name="T"/> (<see cref="HasValue"/>),
    /// then returns that value, otherwise throws an <see cref="InvalidOperationException"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if no value is available
    /// (<see cref="HasValue"/> is false).</exception>
    T Value { get; }

    /// <summary>
    /// Indicates if the current value can be represented as a value of type <typeparamref name="T"/>.
    /// </summary>
    bool HasValue { get; }

    new bool Equals(TSelf other) => TSelf.Equals((TSelf)this, other);

    static virtual bool Equals(TSelf v1, TSelf v2)
        => v1.HasValue
        ? v2.HasValue && EqualityComparer<T>.Default.Equals(v1.Value, v2.Value)
        : !v2.HasValue;

    static virtual bool operator ==(TSelf left, TSelf right)
        => left.Equals(right);

    static virtual bool operator !=(TSelf left, TSelf right)
        => !left.Equals(right);
}
