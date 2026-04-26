// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise conversion from degrees to radians.</summary>
    public static void DegreesToRadians<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.DegreesToRadians(x, destination);
    }

    /// <summary>Element-wise conversion from degrees to radians.</summary>

    public static Vector<T> DegreesToRadians<T>(this IReadOnlyVector<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        DegreesToRadians(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise conversion from radians to degrees.</summary>

    public static void RadiansToDegrees<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.RadiansToDegrees(x, destination);
    }

    /// <summary>Element-wise conversion from radians to degrees.</summary>

    public static Vector<T> RadiansToDegrees<T>(this IReadOnlyVector<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        RadiansToDegrees(x.AsSpan(), destination.AsSpan());
        return destination;
    }
}
