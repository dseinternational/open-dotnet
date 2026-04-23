// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Generic comparison helpers over <see cref="IComparable{T}"/>.
/// </summary>
public static class Comparisons
{
    /// <summary>
    /// Returns the greater of two comparable values. When the values compare equal,
    /// <paramref name="a"/> is returned.
    /// </summary>
    /// <typeparam name="T">The comparable element type.</typeparam>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    public static T Max<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) >= 0 ? a : b;
    }

    /// <summary>
    /// Returns the lesser of two comparable values. When the values compare equal,
    /// <paramref name="a"/> is returned.
    /// </summary>
    /// <typeparam name="T">The comparable element type.</typeparam>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    public static T Min<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) <= 0 ? a : b;
    }
}
