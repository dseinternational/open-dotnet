// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void Subtract<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        GuardSequence.SameLength(x, y);
        TensorPrimitives.Subtract(x, y, destination);
    }

    public static void SubtractInPlace<T>(Span<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        GuardSequence.SameLength(x, y);
        TensorPrimitives.Subtract(x, y, x);
    }
}
