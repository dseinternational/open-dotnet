// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace DSE.Open.Values;

/// <summary>
/// A <see cref="Collection{T}"/> of <typeparamref name="TValue"/> instances that wrap an
/// underlying primitive of type <typeparamref name="T"/>.
/// </summary>
public class ValueCollection<TValue, T> : Collection<TValue>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>
{
    /// <summary>
    /// Initializes a new, empty <see cref="ValueCollection{TValue, T}"/>.
    /// </summary>
    public ValueCollection()
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ValueCollection{TValue, T}"/> containing the elements of the specified collection.
    /// </summary>
    public ValueCollection(IEnumerable<TValue> collection) : base(collection.ToList())
    {
    }
}
