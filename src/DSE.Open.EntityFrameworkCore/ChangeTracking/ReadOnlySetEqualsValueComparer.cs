// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DSE.Open.EntityFrameworkCore.ChangeTracking;

public class ReadOnlySetEqualsValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
TItem,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
TCollection>
    : ValueComparer<TCollection>
    where TCollection : IReadOnlySet<TItem>
{
    public ReadOnlySetEqualsValueComparer(Expression<Func<TCollection, TCollection>> snapshotExpression)
        : base((c1, c2) => CompareEquality(c1, c2), c => GenerateHash(c), snapshotExpression)
    {
    }

    private static bool CompareEquality(TCollection? c1, TCollection? c2)
    {
        return c1 is null ? c2 is null : c2 is not null && c1.SetEquals(c2);
    }

    private static int GenerateHash(TCollection values)
    {
        Guard.IsNotNull(values);

        var hash = 0;

        foreach (var element in values)
        {
            if (element is not null)
            {
                hash = unchecked(hash + EqualityComparer<TItem>.Default.GetHashCode(element));
            }
        }

        return hash;
    }
}

public sealed class ReadOnlySetEqualsValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
T>
    : ReadOnlySetEqualsValueComparer<T, IReadOnlySet<T>>
{
    public static readonly ReadOnlySetEqualsValueComparer<T> Default = new();

    public ReadOnlySetEqualsValueComparer() : base(c => GenerateSnapshot(c))
    {
    }

    private static HashSet<T> GenerateSnapshot(IReadOnlySet<T> values)
    {
        return [..values];
    }
}
