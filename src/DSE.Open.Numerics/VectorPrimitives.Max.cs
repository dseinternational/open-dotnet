// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static T Max<T>([NotNull] IReadOnlyNumericVector<T> vector)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return Max(vector.AsReadOnlySpan());
    }

    public static T Max<T>(ReadOnlySpan<T> values)
        where T : INumber<T>
    {
        return TensorPrimitives.Max(values);
    }

    public static T MaxFloatingPoint<T>([NotNull] IReadOnlyNumericVector<T> vector)
        where T : struct, IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return MaxFloatingPoint(vector.AsReadOnlySpan());
    }

    public static T MaxFloatingPoint<T>(ReadOnlySpan<T> values)
        where T : IFloatingPointIeee754<T>
    {
        return TensorPrimitives.Max(values);
    }
}
