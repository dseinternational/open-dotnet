// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Memory;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

public static partial class ArrayExtensions
{
    /// <summary>
    /// Returns a reference to the first element within a given 2D <typeparamref name="T"/> array,
    /// with no bounds checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input 2D <typeparamref name="T"/> array
    /// instance.</typeparam>
    /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
    /// <returns>A reference to the first element within <paramref name="array"/>, or the location
    /// it would have used, if <paramref name="array"/> is empty.</returns>
    /// <remarks>
    /// This method does not do any bounds checks, therefore it is responsibility of the
    /// caller to perform checks in case the returned value is dereferenced.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReference<T>(this T[,] array)
    {
        return ref Unsafe.As<byte, T>(ref MemoryMarshal.GetArrayDataReference(array));
    }

    /// <summary>
    /// Returns a reference to the first element within a given 3D <typeparamref name="T"/> array,
    /// with no bounds checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input 3D <typeparamref name="T"/> array
    /// instance.</typeparam>
    /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
    /// <returns>A reference to the first element within <paramref name="array"/>, or the location
    /// it would have used, if <paramref name="array"/> is empty.</returns>
    /// <remarks>
    /// This method does not do any bounds checks, therefore it is responsibility of the
    /// caller to perform checks in case the returned value is dereferenced.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReference<T>(this T[,,] array)
    {
        return ref Unsafe.As<byte, T>(ref MemoryMarshal.GetArrayDataReference(array));
    }

    /// <summary>
    /// Returns a reference to the first element within a given 4D <typeparamref name="T"/> array,
    /// with no bounds checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input 4D <typeparamref name="T"/> array
    /// instance.</typeparam>
    /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
    /// <returns>A reference to the first element within <paramref name="array"/>, or the location
    /// it would have used, if <paramref name="array"/> is empty.</returns>
    /// <remarks>
    /// This method does not do any bounds checks, therefore it is responsibility of the
    /// caller to perform checks in case the returned value is dereferenced.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReference<T>(this T[,,,] array)
    {
        return ref Unsafe.As<byte, T>(ref MemoryMarshal.GetArrayDataReference(array));
    }

    /// <summary>
    /// Returns a reference to the first element within a given 5D <typeparamref name="T"/> array,
    /// with no bounds checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input 5D <typeparamref name="T"/> array
    /// instance.</typeparam>
    /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
    /// <returns>A reference to the first element within <paramref name="array"/>, or the location
    /// it would have used, if <paramref name="array"/> is empty.</returns>
    /// <remarks>
    /// This method does not do any bounds checks, therefore it is responsibility of the
    /// caller to perform checks in case the returned value is dereferenced.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReference<T>(this T[,,,,] array)
    {
        return ref Unsafe.As<byte, T>(ref MemoryMarshal.GetArrayDataReference(array));
    }
}
