// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class ReadOnlyNumericVector
{
    public static ReadOnlyNumericVector<T> Create<T>(ReadOnlyMemory<T> sequence)
        where T : struct, INumber<T>
    {
        return new(sequence);
    }

    public static ReadOnlyNumericVector<T> Create<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumber<T>
    {
        return new(sequence.ToArray());
    }
}
