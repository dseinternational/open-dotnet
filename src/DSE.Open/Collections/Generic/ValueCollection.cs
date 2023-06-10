// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace DSE.Open.Collections.Generic;

public sealed class ValueCollection<T>
    : Collection<T>,
      IEquatable<ValueCollection<T>>
    where T
    : IEquatable<T>
{
    public static readonly ValueCollection<T> Empty = new();

    public ValueCollection() : this(Enumerable.Empty<T>())
    {
    }

    public ValueCollection(IEnumerable<T> list) : base(new List<T>(list))
    {
    }

    public bool Equals(ValueCollection<T>? other)
        => other is not null && this.SequenceEqual(other);

    public override bool Equals(object? obj) => Equals(obj as ValueCollection<T>);

    public static bool operator ==(ValueCollection<T>? left, ValueCollection<T>? right)
        => left is null ? right is null : left.Equals(right);

    public static bool operator !=(ValueCollection<T>? left, ValueCollection<T>? right)
        => !(left == right);

    public override int GetHashCode()
    {
        var hashCode = -7291863;

        foreach (var i in this)
        {
            hashCode = HashCode.Combine(hashCode, i);
        }

        return hashCode;
    }
}
