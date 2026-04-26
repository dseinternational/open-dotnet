// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

// All predicates expose the same core span-based surface:
//   - IsXxx(ReadOnlySpan<T>, Span<bool>)         — element-wise mask
//   - IsXxxAll(ReadOnlySpan<T>) : bool           — all-true reduction
//   - IsXxxAny(ReadOnlySpan<T>) : bool           — any-true reduction
//
// IReadOnlyVector<T> and IVector<bool> extension overloads (matching the
// existing VectorPrimitives.IsNaN pattern) are currently provided only for
// IsFinite and IsInfinity as representative coverage. Callers working with a
// vector and any other predicate should pass `vector.AsSpan()` directly.
public static partial class VectorPrimitives
{
    // -------- IsFinite --------

    /// <summary>Element-wise <c>IsFinite</c> predicate.</summary>

    public static void IsFinite<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsFinite(x, destination);
    }

    /// <summary>Element-wise <c>IsFinite</c> predicate.</summary>

    public static void IsFinite<T>(this IReadOnlyVector<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        IsFinite(x.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>IsFinite</c> predicate.</summary>

    public static void IsFinite<T>(this IReadOnlyVector<T> x, IVector<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        IsFinite(x, destination.AsSpan());
    }

    /// <summary>Returns <see langword="true"/> when every element is finite.</summary>

    public static bool IsFiniteAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsFiniteAll(x);

    /// <summary>Returns <see langword="true"/> when every element is finite.</summary>

    public static bool IsFiniteAll<T>(this IReadOnlyVector<T> x) where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IsFiniteAll(x.AsSpan());
    }

    /// <summary>Returns <see langword="true"/> when any element is finite.</summary>

    public static bool IsFiniteAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsFiniteAny(x);

    /// <summary>Returns <see langword="true"/> when any element is finite.</summary>

    public static bool IsFiniteAny<T>(this IReadOnlyVector<T> x) where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IsFiniteAny(x.AsSpan());
    }

    // -------- IsInfinity --------

    /// <summary>Element-wise <c>IsInfinity</c> predicate.</summary>

    public static void IsInfinity<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsInfinity(x, destination);
    }

    /// <summary>Element-wise <c>IsInfinity</c> predicate.</summary>

    public static void IsInfinity<T>(this IReadOnlyVector<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        IsInfinity(x.AsSpan(), destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is infinity.</summary>

    public static bool IsInfinityAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsInfinityAll(x);

    /// <summary>Returns <see langword="true"/> when every element is infinity.</summary>

    public static bool IsInfinityAll<T>(this IReadOnlyVector<T> x) where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IsInfinityAll(x.AsSpan());
    }

    /// <summary>Returns <see langword="true"/> when any element is infinity.</summary>

    public static bool IsInfinityAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsInfinityAny(x);

    /// <summary>Returns <see langword="true"/> when any element is infinity.</summary>

    public static bool IsInfinityAny<T>(this IReadOnlyVector<T> x) where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IsInfinityAny(x.AsSpan());
    }

    /// <summary>Element-wise <c>IsPositiveInfinity</c> predicate.</summary>

    public static void IsPositiveInfinity<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsPositiveInfinity(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is positive infinity.</summary>

    public static bool IsPositiveInfinityAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsPositiveInfinityAll(x);

    /// <summary>Returns <see langword="true"/> when any element is positive infinity.</summary>

    public static bool IsPositiveInfinityAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsPositiveInfinityAny(x);

    /// <summary>Element-wise <c>IsNegativeInfinity</c> predicate.</summary>

    public static void IsNegativeInfinity<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsNegativeInfinity(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is negative infinity.</summary>

    public static bool IsNegativeInfinityAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNegativeInfinityAll(x);

    /// <summary>Returns <see langword="true"/> when any element is negative infinity.</summary>

    public static bool IsNegativeInfinityAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNegativeInfinityAny(x);

    // -------- IsNormal / IsSubnormal --------

    /// <summary>Element-wise <c>IsNormal</c> predicate.</summary>

    public static void IsNormal<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsNormal(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is normal.</summary>

    public static bool IsNormalAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNormalAll(x);

    /// <summary>Returns <see langword="true"/> when any element is normal.</summary>

    public static bool IsNormalAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNormalAny(x);

    /// <summary>Element-wise <c>IsSubnormal</c> predicate.</summary>

    public static void IsSubnormal<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsSubnormal(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is subnormal.</summary>

    public static bool IsSubnormalAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsSubnormalAll(x);

    /// <summary>Returns <see langword="true"/> when any element is subnormal.</summary>

    public static bool IsSubnormalAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsSubnormalAny(x);

    // -------- IsZero / IsPositive / IsNegative --------

    /// <summary>Element-wise <c>IsZero</c> predicate.</summary>

    public static void IsZero<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsZero(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is zero.</summary>

    public static bool IsZeroAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsZeroAll(x);

    /// <summary>Returns <see langword="true"/> when any element is zero.</summary>

    public static bool IsZeroAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsZeroAny(x);

    /// <summary>Element-wise <c>IsPositive</c> predicate.</summary>

    public static void IsPositive<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsPositive(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is positive.</summary>

    public static bool IsPositiveAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsPositiveAll(x);

    /// <summary>Returns <see langword="true"/> when any element is positive.</summary>

    public static bool IsPositiveAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsPositiveAny(x);

    /// <summary>Element-wise <c>IsNegative</c> predicate.</summary>

    public static void IsNegative<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsNegative(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is negative.</summary>

    public static bool IsNegativeAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNegativeAll(x);

    /// <summary>Returns <see langword="true"/> when any element is negative.</summary>

    public static bool IsNegativeAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNegativeAny(x);

    // -------- IsInteger / IsEvenInteger / IsOddInteger --------

    /// <summary>Element-wise <c>IsInteger</c> predicate.</summary>

    public static void IsInteger<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsInteger(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is an integer.</summary>

    public static bool IsIntegerAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsIntegerAll(x);

    /// <summary>Returns <see langword="true"/> when any element is an integer.</summary>

    public static bool IsIntegerAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsIntegerAny(x);

    /// <summary>Element-wise <c>IsEvenInteger</c> predicate.</summary>

    public static void IsEvenInteger<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsEvenInteger(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is an even integer.</summary>

    public static bool IsEvenIntegerAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsEvenIntegerAll(x);

    /// <summary>Returns <see langword="true"/> when any element is an even integer.</summary>

    public static bool IsEvenIntegerAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsEvenIntegerAny(x);

    /// <summary>Element-wise <c>IsOddInteger</c> predicate.</summary>

    public static void IsOddInteger<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsOddInteger(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is an odd integer.</summary>

    public static bool IsOddIntegerAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsOddIntegerAll(x);

    /// <summary>Returns <see langword="true"/> when any element is an odd integer.</summary>

    public static bool IsOddIntegerAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsOddIntegerAny(x);

    // -------- IsRealNumber / IsComplexNumber / IsImaginaryNumber / IsCanonical / IsPow2 --------

    /// <summary>Element-wise <c>IsRealNumber</c> predicate.</summary>

    public static void IsRealNumber<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsRealNumber(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is real.</summary>

    public static bool IsRealNumberAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsRealNumberAll(x);

    /// <summary>Returns <see langword="true"/> when any element is real.</summary>

    public static bool IsRealNumberAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsRealNumberAny(x);

    /// <summary>Element-wise <c>IsComplexNumber</c> predicate.</summary>

    public static void IsComplexNumber<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsComplexNumber(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is complex.</summary>

    public static bool IsComplexNumberAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsComplexNumberAll(x);

    /// <summary>Returns <see langword="true"/> when any element is complex.</summary>

    public static bool IsComplexNumberAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsComplexNumberAny(x);

    /// <summary>Element-wise <c>IsImaginaryNumber</c> predicate.</summary>

    public static void IsImaginaryNumber<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsImaginaryNumber(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is imaginary.</summary>

    public static bool IsImaginaryNumberAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsImaginaryNumberAll(x);

    /// <summary>Returns <see langword="true"/> when any element is imaginary.</summary>

    public static bool IsImaginaryNumberAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsImaginaryNumberAny(x);

    /// <summary>Element-wise <c>IsCanonical</c> predicate.</summary>

    public static void IsCanonical<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsCanonical(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is canonical.</summary>

    public static bool IsCanonicalAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsCanonicalAll(x);

    /// <summary>Returns <see langword="true"/> when any element is canonical.</summary>

    public static bool IsCanonicalAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsCanonicalAny(x);

    /// <summary>Element-wise <c>IsPow2</c> predicate.</summary>

    public static void IsPow2<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : IBinaryNumber<T>
    {
        TensorPrimitives.IsPow2(x, destination);
    }

    /// <summary>Returns <see langword="true"/> when every element is a power of two.</summary>

    public static bool IsPow2All<T>(in ReadOnlySpan<T> x) where T : IBinaryNumber<T>
        => TensorPrimitives.IsPow2All(x);

    /// <summary>Returns <see langword="true"/> when any element is a power of two.</summary>

    public static bool IsPow2Any<T>(in ReadOnlySpan<T> x) where T : IBinaryNumber<T>
        => TensorPrimitives.IsPow2Any(x);
}
