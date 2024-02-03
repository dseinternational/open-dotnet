// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class Vector
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

    public static void Add<T>(ReadOnlyVector<T> x, ReadOnlyVector<T> y, Vector<T> destination)
        where T : struct, INumber<T>
    {
        TensorPrimitives.Add(x.Span, y.Span, destination.Span);
    }

    public static void Add<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        TensorPrimitives.Add(x, y, destination);
    }

    public static void AddInPace<T>(Vector<T> x, Vector<T> y)
        where T : struct, INumber<T>
    {
        AddInPlace(x.Span, y.Span);
    }

    public static void AddInPace<T, TVector>(TVector x, TVector y)
        where T : struct, INumber<T>
        where TVector : IVector<T, TVector>
    {
        AddInPlace(x.Span, y.Span);
    }

    public static void AddInPlace<T>(Span<T> x, ReadOnlySpan<T> y)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        TensorPrimitives.Add(x, y, x);
    }
}
