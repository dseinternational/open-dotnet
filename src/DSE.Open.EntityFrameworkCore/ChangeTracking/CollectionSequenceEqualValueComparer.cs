// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.EntityFrameworkCore.ChangeTracking;

public sealed class CollectionSequenceEqualValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
    T>
    : SequenceEqualValueComparer<T, ICollection<T>>
{
    public static readonly CollectionSequenceEqualValueComparer<T> Default = new();

    public CollectionSequenceEqualValueComparer() : base(c => GenerateSnapshot(c))
    {
    }

    private static ICollection<T> GenerateSnapshot(ICollection<T> values) => new List<T>(values);
}
