// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    private static T[] CreateUninitializedArray<T>(int length)
    {
        return GC.AllocateUninitializedArray<T>(length);
    }
}
