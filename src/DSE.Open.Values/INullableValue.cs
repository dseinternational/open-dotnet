// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Values;

// TODO: think more about nullability

/// <summary>
/// Defines a value that is encoded as a value of type <typeparamref name="T"/> and 
/// which may also represent a 'null' or unknown state.
/// </summary>
/// <typeparam name="TSelf">The type that implements the interface.</typeparam>
/// <typeparam name="T">The type that encodes the value.</typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
public interface INullableValue<TSelf, T> : IValue<TSelf, T>, INullable<TSelf, T>
    where T : IEquatable<T>
    where TSelf : struct, INullableValue<TSelf, T>
{
    new T Value => (T)this;

    // T INullable<TSelf, T>.Value => Value;

    new bool Equals(TSelf other) => TSelf.Equals((TSelf)this, other);

    static new virtual bool Equals(TSelf v1, TSelf v2)
        => v1.HasValue
        ? v2.HasValue && EqualityComparer<T>.Default.Equals(v1.Value, v2.Value)
        : !v2.HasValue;

    static virtual bool operator ==(TSelf left, TSelf right)
        => left.Equals(right);

    static virtual bool operator !=(TSelf left, TSelf right)
        => !left.Equals(right);
}
