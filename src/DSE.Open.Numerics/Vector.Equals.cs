// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public partial class Vector
{
    public static bool Equals<T>(IReadOnlyVector<T>? v1, IReadOnlyVector<T>? v2)
        where T : IEquatable<T>
    {
        if (ReferenceEquals(v1, v2))
        {
            return true;
        }

        if (v1 is null || v2 is null)
        {
            return false;
        }

        if (v1.Length != v2.Length)
        {
            return false;
        }

        return v1.AsSpan().SequenceEqual(v2.AsSpan());
    }

    public static void ElementsEquals<T>(IReadOnlyVector<T> v1, IReadOnlyVector<T> v2, Span<bool> result)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(v1);
        ArgumentNullException.ThrowIfNull(v2);

        if (v1.Length != v2.Length)
        {
            throw new ArgumentException("Vectors must be of the same length.");
        }

        if (result.Length != v1.Length)
        {
            throw new ArgumentException("Result span must be of the same length as the vectors.");
        }

        for (var i = 0; i < v1.Length; i++)
        {
            result[i] = v1[i].Equals(v2[i]);
        }
    }
}
