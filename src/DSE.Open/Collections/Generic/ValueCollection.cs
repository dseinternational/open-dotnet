// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace DSE.Open.Collections.Generic;

public sealed class ValueCollection<T>
    : Collection<T>,
      IEquatable<ValueCollection<T>>
{
    public static readonly ValueCollection<T> Empty = new();

    public ValueCollection() : this(Enumerable.Empty<T>())
    {
    }

    public ValueCollection(IEnumerable<T> list) : base(new List<T>(list))
    {
    }

    public bool Equals(ValueCollection<T>? other)
    {
        return other is not null
            && Count == other.Count
            && this.SequenceEqual(other);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ValueCollection<T>);
    }

    public static bool operator ==(ValueCollection<T>? left, ValueCollection<T>? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(ValueCollection<T>? left, ValueCollection<T>? right)
    {
        return !(left == right);
    }

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
