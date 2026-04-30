// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

/// <summary>
/// A mutable <see cref="Collection{T}"/> that defines equality based on its sequence of elements.
/// </summary>
/// <typeparam name="T">The type of element stored in the collection.</typeparam>
public sealed class ValueCollection<T>
    : Collection<T>,
      IEquatable<ValueCollection<T>>
{
    /// <summary>
    /// An empty <see cref="ValueCollection{T}"/>.
    /// </summary>
    public static new readonly ValueCollection<T> Empty = [];

    /// <summary>
    /// Initializes a new, empty <see cref="ValueCollection{T}"/>.
    /// </summary>
    public ValueCollection() : this([])
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ValueCollection{T}"/> containing the elements of <paramref name="list"/>.
    /// </summary>
    public ValueCollection(IEnumerable<T> list) : base([.. list])
    {
    }

    /// <inheritdoc/>
    public bool Equals(ValueCollection<T>? other)
    {
        return other is not null
            && Count == other.Count
            && this.SequenceEqual(other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(obj as ValueCollection<T>);
    }

    /// <summary>
    /// Indicates whether the two collections contain the same sequence of elements.
    /// </summary>
    public static bool operator ==(ValueCollection<T>? left, ValueCollection<T>? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    /// <summary>
    /// Indicates whether the two collections do not contain the same sequence of elements.
    /// </summary>
    public static bool operator !=(ValueCollection<T>? left, ValueCollection<T>? right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var i in this)
        {
            hash.Add(i);
        }

        return hash.ToHashCode();
    }
}
