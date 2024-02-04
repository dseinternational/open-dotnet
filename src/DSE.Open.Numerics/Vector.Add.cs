// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class Vector
{
    public static void Add<T>(ReadOnlyVector<T> x, ReadOnlyVector<T> y, Vector<T> destination)
        where T : struct, INumber<T>
    {
        VectorPrimitives.Add(x.Span, y.Span, destination.Span);
    }

    public static void AddInPace<T>(Vector<T> x, ReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        VectorPrimitives.AddInPlace(x.Span, y.Span);
    }
    /*
    public static void AddInPace<T, TVector>(TVector x, TVector y)
        where T : struct, INumber<T>
        where TVector : IVector<T, TVector>
    {
        VectorPrimitives.AddInPlace(x.Span, y.Span);
    }
    */
}
