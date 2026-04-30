// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Runtime.Helpers;

/// <summary>
/// Provides thresholds for stack memory allocation.
/// </summary>
public static class MemoryThresholds
{
    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="length"/> elements of type
    /// <typeparamref name="T"/> can fit within <see cref="StackallocByteThreshold"/> bytes.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanStackalloc<T>(int length)
    {
        return Unsafe.SizeOf<T>() * length <= StackallocByteThreshold;
    }

    /// <summary>
    /// The maximum number of bytes considered safe to allocate on the stack.
    /// </summary>
    public const int StackallocByteThreshold = 512;

    /// <summary>
    /// The maximum number of <see cref="char"/> values considered safe to allocate on the stack.
    /// </summary>
    public const int StackallocCharThreshold = StackallocByteThreshold / 2;
}
