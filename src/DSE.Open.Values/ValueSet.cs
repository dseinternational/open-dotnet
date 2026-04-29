// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// A <see cref="HashSet{T}"/> of <typeparamref name="TValue"/> instances that wrap an
/// underlying primitive of type <typeparamref name="T"/>.
/// </summary>
public class ValueSet<TValue, T> : HashSet<TValue>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>
{
    /// <summary>
    /// Initializes a new, empty <see cref="ValueSet{TValue, T}"/>.
    /// </summary>
    public ValueSet()
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ValueSet{TValue, T}"/> containing the elements of the specified collection.
    /// </summary>
    public ValueSet(IEnumerable<TValue> collection) : base(collection)
    {
    }

    /// <summary>
    /// Initializes a new, empty <see cref="ValueSet{TValue, T}"/> that uses the specified equality comparer.
    /// </summary>
    public ValueSet(IEqualityComparer<TValue>? comparer) : base(comparer)
    {
    }

    /// <summary>
    /// Initializes a new, empty <see cref="ValueSet{TValue, T}"/> with the specified initial capacity.
    /// </summary>
    public ValueSet(int capacity) : base(capacity)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ValueSet{TValue, T}"/> containing the elements of the specified
    /// collection and using the specified equality comparer.
    /// </summary>
    public ValueSet(IEnumerable<TValue> collection, IEqualityComparer<TValue>? comparer) : base(collection, comparer)
    {
    }

    /// <summary>
    /// Initializes a new, empty <see cref="ValueSet{TValue, T}"/> with the specified initial capacity
    /// and using the specified equality comparer.
    /// </summary>
    public ValueSet(int capacity, IEqualityComparer<TValue>? comparer) : base(capacity, comparer)
    {
    }
}
