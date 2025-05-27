// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public static class ReadOnlyVectorExtensions
{
    /// <summary>
    /// Creates a mutable copy of the <paramref name="vector"/>.
    /// </summary>
    /// <param name="vector">The read-only vector to copy.</param>
    public static Vector<T> Copied<T>(this ReadOnlyVector<T> vector)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return new Vector<T>(vector.ToArray());
    }
}
