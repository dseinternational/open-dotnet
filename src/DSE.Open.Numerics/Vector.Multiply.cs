// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class Vector
{
    public static void Multiply<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        GuardSequence.SameLength(x, y);
        TensorPrimitives.Multiply(x, y, destination);
    }

    public static void MultiplyInPlace<T>(Span<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        GuardSequence.SameLength(x, y);
        TensorPrimitives.Multiply(x, y, x);
    }
}
