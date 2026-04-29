// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.EntityFrameworkCore.ChangeTracking;

/// <summary>
/// A <see cref="SequenceEqualValueComparer{TItem, TCollection}"/> for <see cref="IList{T}"/>
/// values that compares by sequence equality and snapshots to a list.
/// </summary>
public sealed class ListSequenceEqualValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
T>
    : SequenceEqualValueComparer<T, IList<T>>
{
    /// <summary>Gets a shared default instance.</summary>
    public static readonly ListSequenceEqualValueComparer<T> Default = new();

    /// <summary>Initializes a new instance.</summary>
    public ListSequenceEqualValueComparer() : base(c => GenerateSnapshot(c))
    {
    }

    private static List<T> GenerateSnapshot(IList<T> values)
    {
        return [.. values];
    }
}
