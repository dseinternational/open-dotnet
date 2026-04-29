// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.EntityFrameworkCore.ChangeTracking;

/// <summary>
/// A <see cref="SequenceEqualValueComparer{TItem, TCollection}"/> for <see cref="IReadOnlyList{T}"/>
/// values that compares by sequence equality and snapshots to a list.
/// </summary>
public sealed class ReadOnlyListSequenceEqualValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
T>
    : SequenceEqualValueComparer<T, IReadOnlyList<T>>
{
    /// <summary>Gets a shared default instance.</summary>
    public static readonly ReadOnlyListSequenceEqualValueComparer<T> Default = new();

    /// <summary>Initializes a new instance.</summary>
    public ReadOnlyListSequenceEqualValueComparer() : base(c => GenerateSnapshot(c))
    {
    }

    private static List<T> GenerateSnapshot(IReadOnlyList<T> values)
    {
        return [.. values];
    }
}
