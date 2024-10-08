// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DSE.Open.EntityFrameworkCore.ChangeTracking;

public class SetEqualsValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
TItem,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
TCollection>
    : ValueComparer<TCollection>
    where TCollection : ISet<TItem>
{
    public SetEqualsValueComparer(Expression<Func<TCollection, TCollection>> snapshotExpression)
        : base((c1, c2) => CompareEquality(c1, c2), c => GenerateHash(c), snapshotExpression)
    {
    }

    private static bool CompareEquality(TCollection? c1, TCollection? c2)
    {
        return c1 is null ? c2 is null : c2 is not null && c1.SetEquals(c2);
    }

    private static int GenerateHash(TCollection values)
    {
        ArgumentNullException.ThrowIfNull(values);

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

public sealed class SetEqualsValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
T>
    : SetEqualsValueComparer<T, ISet<T>>
{
    public static readonly SetEqualsValueComparer<T> Default = new();

    public SetEqualsValueComparer() : base(c => GenerateSnapshot(c))
    {
    }

    private static HashSet<T> GenerateSnapshot(ISet<T> values)
    {
        return [.. values];
    }
}
