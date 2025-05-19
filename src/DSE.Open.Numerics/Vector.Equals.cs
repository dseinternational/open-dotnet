// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public partial class Vector
{
    /// <summary>
    /// Determines whether two vectors are equal by comparing the elements using
    /// <see cref="IEquatable{T}.Equals(T)"/>. This comparison considers corresponding
    /// unknown values (<see langword="null"/> or <see cref="INaValue{TSelf, T}.Na"/> or
    /// <see cref="IFloatingPointIeee754{TSelf}.NaN"/>) to be equal. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns><see langword="true"/> if the vectors are the same length and each corresponding
    /// element is determined to be equal using <see cref="IEquatable{T}.Equals(T)"/>.</returns>
    public static bool SequenceEqual<T>(IReadOnlyVector<T> v1, IReadOnlyVector<T> v2)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(v1);
        ArgumentNullException.ThrowIfNull(v2);

        return SequenceEqual(v1, v2.AsSpan());
    }

    /// <summary>
    /// Determines whether two vectors are equal by comparing the elements using
    /// <see cref="IEquatable{T}.Equals(T)"/>. This comparison considers corresponding
    /// unknown values (<see langword="null"/> or <see cref="INaValue{TSelf, T}.Na"/> or
    /// <see cref="IFloatingPointIeee754{TSelf}.NaN"/>) to be equal. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static bool SequenceEqual<T>(IReadOnlyVector<T> v1, ReadOnlySpan<T> v2)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(v1);

        return SequenceEqual(v1, v2, true);
    }

    public static bool SequenceEqual<T>(IReadOnlyVector<T> v1, ReadOnlySpan<T> v2, bool unknownsEqual)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(v1);

        if (v1.Length != v2.Length)
        {
            return false;
        }

        if (v1.IsEmpty) // already established that lengths are equal
        {
            return true;
        }

        if (unknownsEqual)
        {

            // TODO: if nullable number, cast to underlying primitive type span

            return v1.AsSpan().SequenceEqual(v2);
        }

        // TODO: optimise

        for (var i = 0; i < v1.Length; i++)
        {
            var e1 = v1[i];
            var e2 = v2[i];

            if (e1 is null || e2 is null
                || (e1 is INaValue n1 && n1.IsNa)
                || (e2 is INaValue n2 && n2.IsNa)
                || !e1.Equals(e2))
            {
                return false;
            }
        }

        return true;
    }

    public static void ElementsEqual<T>(IReadOnlyVector<T> v1, IReadOnlyVector<T> v2, Span<bool> result)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(v1);
        ArgumentNullException.ThrowIfNull(v2);

        if (v1.Length != v2.Length)
        {
            throw new ArgumentException("Vectors must be of the same length.");
        }

        if (result.Length != v1.Length)
        {
            throw new ArgumentException("Result span must be of the same length as the vectors.");
        }

        for (var i = 0; i < v1.Length; i++)
        {
            result[i] = v1[i].Equals(v2[i]);
        }
    }
}
