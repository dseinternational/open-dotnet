// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static bool SequenceEqual<T>(
        [NotNull] IReadOnlyVector<T> vector1,
        [NotNull] IReadOnlyVector<T> vector2)
    {
        ArgumentNullException.ThrowIfNull(vector1);
        ArgumentNullException.ThrowIfNull(vector2);
        return vector1.AsReadOnlySpan().SequenceEqual(vector2.AsReadOnlySpan());
    }

    public static IReadOnlyVector<bool> Equals<T>(
        [NotNull] IReadOnlyVector<T> vector1,
        [NotNull] IReadOnlyVector<T> vector2)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector1);
        ArgumentNullException.ThrowIfNull(vector2);

        if (vector1.Length != vector2.Length)
        {
            throw new ArgumentException("Vectors must be the same length.");
        }

        var result = new bool[vector1.Length];

        for (var i = 0; i < vector1.Length; i++)
        {
            result[i] = vector1[i].Equals(vector2[i]);
        }

        return new Vector<bool>(result);
    }
}
