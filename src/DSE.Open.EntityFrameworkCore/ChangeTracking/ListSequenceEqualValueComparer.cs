// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.EntityFrameworkCore.ChangeTracking;

public sealed class ListSequenceEqualValueComparer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.PublicProperties)]
    T>
    : SequenceEqualValueComparer<T, IList<T>>
{
    public static readonly ListSequenceEqualValueComparer<T> Default = new();

    public ListSequenceEqualValueComparer() : base(c => GenerateSnapshot(c))
    {
    }

    private static IList<T> GenerateSnapshot(IList<T> values) => new List<T>(values);
}
