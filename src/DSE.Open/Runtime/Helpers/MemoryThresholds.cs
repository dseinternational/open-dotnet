// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Runtime.Helpers;

/// <summary>
/// Provides thresholds for stack memory allocation.
/// </summary>
public static class MemoryThresholds
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanStackalloc<T>(int length)
    {
        return Unsafe.SizeOf<T>() * length <= StackallocByteThreshold;
    }

    public const int StackallocByteThreshold = 512;

    public const int StackallocCharThreshold = StackallocByteThreshold / 2;
}
