// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class Vector
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(ReadOnlyVector<T> x, ReadOnlyVector<T> y, Vector<T> destination)
        where T : struct, INumber<T>
    {
        VectorPrimitives.Add(x.Memory, y.Memory, destination.Memory);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPace<T>(Vector<T> x, ReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        VectorPrimitives.AddInPlace(x.Memory, y.Memory);
    }
}
