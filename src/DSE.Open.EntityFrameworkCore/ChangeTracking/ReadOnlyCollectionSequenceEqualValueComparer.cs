// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.EntityFrameworkCore.ChangeTracking;

public sealed class ReadOnlyCollectionSequenceEqualValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
T>
    : SequenceEqualValueComparer<T, IReadOnlyCollection<T>>
{
    public static readonly ReadOnlyCollectionSequenceEqualValueComparer<T> Default = new();

    public ReadOnlyCollectionSequenceEqualValueComparer() : base(c => GenerateSnapshot(c))
    {
    }

    private static List<T> GenerateSnapshot(IReadOnlyCollection<T> values)
    {
        return new(values);
    }
}
