// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class ReadOnlyVector
{
    public static ReadOnlyVector<T> Create<T>(ReadOnlyMemory<T> sequence)
        where T : struct, INumber<T>
    {
        return new ReadOnlyVector<T>(sequence);
    }

    public static ReadOnlyVector<T> Create<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumber<T>
    {
        return new ReadOnlyVector<T>(sequence.ToArray());
    }

}
