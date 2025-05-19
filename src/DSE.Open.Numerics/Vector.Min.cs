// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

public partial class Vector
{
    public static T Min<T>([NotNull] IReadOnlyVector<T> vector)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return Min(vector.AsSpan());
    }

    public static T Min<T>(ReadOnlySpan<T> span)
        where T : INumber<T>
    {
        return TensorPrimitives.Min(span);
    }
}
