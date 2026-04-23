// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Computes the cosine similarity of two sequences: <c>dot(x,y) / (|x| * |y|)</c>.
    /// </summary>
    public static T CosineSimilarity<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
        where T : struct, IRootFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        return TensorPrimitives.CosineSimilarity(x, y);
    }

    /// <summary>
    /// Computes the cosine similarity of two vectors: <c>dot(x,y) / (|x| * |y|)</c>.
    /// </summary>
    public static T CosineSimilarity<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return CosineSimilarity(x.AsSpan(), y.AsSpan());
    }
}
