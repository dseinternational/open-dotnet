// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class NumericVector
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Sum<T>(ReadOnlyNumericVector<T> elements)
        where T : struct, INumber<T>
    {
        return VectorPrimitives.Sum(elements.Span);
    }
}
