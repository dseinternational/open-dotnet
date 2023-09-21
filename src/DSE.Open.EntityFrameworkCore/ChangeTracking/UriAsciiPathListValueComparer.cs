// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DSE.Open.EntityFrameworkCore.ChangeTracking;

public sealed class ListSequenceEqualValueComparer<T> : ValueComparer<IList<T>>
{
    public static readonly ListSequenceEqualValueComparer<T> Default = new();

    public ListSequenceEqualValueComparer()
        : base(
            (c1, c2) => (c1 != null && c2 != null && c1.SequenceEqual(c2)) || (c1 == null && c2 == null),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
            c => c.ToList())
    {
    }
}
