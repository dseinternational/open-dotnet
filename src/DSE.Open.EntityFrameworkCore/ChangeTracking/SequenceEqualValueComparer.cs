// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DSE.Open.EntityFrameworkCore.ChangeTracking;

/// <summary>
/// A <see cref="ValueComparer{T}"/> that compares two enumerable collections by sequence equality
/// of their elements.
/// </summary>
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
    /// <summary>
    /// Initializes a new instance using the specified snapshot expression.
    /// </summary>
    /// <param name="snapshotExpression">An expression that produces a snapshot of the collection.</param>
    public SequenceEqualValueComparer(Expression<Func<TCollection, TCollection>> snapshotExpression)
        : base((c1, c2) => CompareEquality(c1, c2), c => GenerateHash(c), snapshotExpression)
    {
    }

    private static bool CompareEquality(TCollection? c1, TCollection? c2)
    {
        return c1 is null ? c2 is null : c2 is not null && c1.SequenceEqual(c2);
    }

    private static int GenerateHash(TCollection values)
    {
        ArgumentNullException.ThrowIfNull(values);
        return values.Aggregate(0, (a, v) => HashCode.Combine(a, v?.GetHashCode() ?? 0));
    }
}

/// <summary>
/// A <see cref="ValueComparer{T}"/> that compares two <see cref="IEnumerable{T}"/> instances
/// by sequence equality of their elements.
/// </summary>
public sealed class SequenceEqualValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
T>
    : SequenceEqualValueComparer<T, IEnumerable<T>>
{
    /// <summary>Gets a shared default instance.</summary>
    public static readonly SequenceEqualValueComparer<T> Default = new();

    /// <summary>Initializes a new instance.</summary>
    public SequenceEqualValueComparer() : base(c => GenerateSnapshot(c))
    {
    }
    private static List<T> GenerateSnapshot(IEnumerable<T> values)
    {
        return [.. values];
    }
}

