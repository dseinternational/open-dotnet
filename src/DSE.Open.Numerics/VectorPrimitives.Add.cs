// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void Add<T>(T[]x, T[]y, T[] destination)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        TensorPrimitives.Add(x.AsSpan(), y.AsSpan(), destination.AsSpan());
    }

    public static void Add<T>(ImmutableArray<T> x, ImmutableArray<T> y, Span<T> destination)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        TensorPrimitives.Add(x.AsSpan(), y.AsSpan(), destination);
    }

    public static void Add<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        TensorPrimitives.Add(x, y, destination);
    }

    public static void AddInPlace<T>(Span<T> x, ReadOnlySpan<T> y)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        TensorPrimitives.Add(x, y, x);
    }
}
