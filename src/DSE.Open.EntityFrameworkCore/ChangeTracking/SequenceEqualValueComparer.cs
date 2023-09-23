// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DSE.Open.EntityFrameworkCore.ChangeTracking;

public class SequenceEqualValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
    TItem,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
    TCollection>
    : ValueComparer<TCollection>
    where TCollection : IEnumerable<TItem>
{
    public SequenceEqualValueComparer(Expression<Func<TCollection, TCollection>> snapshotExpression)
        : base((c1, c2) => CompareEquality(c1, c2), c => GenerateHash(c), snapshotExpression)
    {
    }

    private static bool CompareEquality(TCollection? c1, TCollection? c2)
        => c1 is null ? c2 is null : c2 is not null && c1.SequenceEqual(c2);

    private static int GenerateHash(TCollection values)
    {
        Guard.IsNotNull(values);
        return values.Aggregate(0, (a, v) => HashCode.Combine(a, v?.GetHashCode() ?? 0));
    }
}

public class SequenceEqualValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
    T>
    : SequenceEqualValueComparer<T, IEnumerable<T>>
{
    public static readonly SequenceEqualValueComparer<T> Default = new();

    public SequenceEqualValueComparer() : base(c => GenerateSnapshot(c))
    {
    }
    private static IEnumerable<T> GenerateSnapshot(IEnumerable<T> values) => new List<T>(values);
}

