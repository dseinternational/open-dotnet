// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public sealed class ValueCollection<T>
    : Collection<T>,
      IEquatable<ValueCollection<T>>
{
    public static new readonly ValueCollection<T> Empty = [];

    public ValueCollection() : this([])
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
