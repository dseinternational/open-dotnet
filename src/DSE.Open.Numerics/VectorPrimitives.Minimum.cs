// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;
using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static T Minimum<T>(ReadOnlySpan<T> span)
        where T : INumber<T>
    {
        return TensorPrimitives.Min(span);
    }
}
