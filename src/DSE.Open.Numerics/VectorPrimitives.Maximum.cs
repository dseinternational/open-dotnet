// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static T Maximum<T>(ReadOnlySpan<T> values)
        where T : INumber<T>
    {
        return TensorPrimitives.Max(values);
    }

    public static T MaximumFloatingPoint<T>(ReadOnlySpan<T> values)
        where T : IFloatingPointIeee754<T>
    {
        return TensorPrimitives.Max(values);
    }
}
