// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

// Each predicate follows the existing VectorPrimitives.IsNaN pattern:
//   - IsXxx(ReadOnlySpan<T>, Span<bool>)         — element-wise mask
//   - IsXxx(IReadOnlyVector<T>, Span<bool>)      — extension overload
//   - IsXxx(IReadOnlyVector<T>, IVector<bool>)   — extension overload
//   - IsXxxAll(ReadOnlySpan<T>) : bool           — all-true reduction
//   - IsXxxAll(IReadOnlyVector<T>) : bool
//   - IsXxxAny(ReadOnlySpan<T>) : bool           — any-true reduction
//   - IsXxxAny(IReadOnlyVector<T>) : bool
public static partial class VectorPrimitives
{
    // -------- IsFinite --------

    public static void IsFinite<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsFinite(x, destination);
    }

    public static void IsFinite<T>(this IReadOnlyVector<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        IsFinite(x.AsSpan(), destination);
    }

    public static void IsFinite<T>(this IReadOnlyVector<T> x, IVector<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        IsFinite(x, destination.AsSpan());
    }

    public static bool IsFiniteAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsFiniteAll(x);

    public static bool IsFiniteAll<T>(this IReadOnlyVector<T> x) where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IsFiniteAll(x.AsSpan());
    }

    public static bool IsFiniteAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsFiniteAny(x);

    public static bool IsFiniteAny<T>(this IReadOnlyVector<T> x) where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IsFiniteAny(x.AsSpan());
    }

    // -------- IsInfinity --------

    public static void IsInfinity<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsInfinity(x, destination);
    }

    public static void IsInfinity<T>(this IReadOnlyVector<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        IsInfinity(x.AsSpan(), destination);
    }

    public static bool IsInfinityAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsInfinityAll(x);

    public static bool IsInfinityAll<T>(this IReadOnlyVector<T> x) where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IsInfinityAll(x.AsSpan());
    }

    public static bool IsInfinityAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsInfinityAny(x);

    public static bool IsInfinityAny<T>(this IReadOnlyVector<T> x) where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IsInfinityAny(x.AsSpan());
    }

    public static void IsPositiveInfinity<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsPositiveInfinity(x, destination);
    }

    public static bool IsPositiveInfinityAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsPositiveInfinityAll(x);

    public static bool IsPositiveInfinityAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsPositiveInfinityAny(x);

    public static void IsNegativeInfinity<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsNegativeInfinity(x, destination);
    }

    public static bool IsNegativeInfinityAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNegativeInfinityAll(x);

    public static bool IsNegativeInfinityAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNegativeInfinityAny(x);

    // -------- IsNormal / IsSubnormal --------

    public static void IsNormal<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsNormal(x, destination);
    }

    public static bool IsNormalAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNormalAll(x);

    public static bool IsNormalAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNormalAny(x);

    public static void IsSubnormal<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsSubnormal(x, destination);
    }

    public static bool IsSubnormalAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsSubnormalAll(x);

    public static bool IsSubnormalAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsSubnormalAny(x);

    // -------- IsZero / IsPositive / IsNegative --------

    public static void IsZero<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsZero(x, destination);
    }

    public static bool IsZeroAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsZeroAll(x);

    public static bool IsZeroAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsZeroAny(x);

    public static void IsPositive<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsPositive(x, destination);
    }

    public static bool IsPositiveAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsPositiveAll(x);

    public static bool IsPositiveAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsPositiveAny(x);

    public static void IsNegative<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsNegative(x, destination);
    }

    public static bool IsNegativeAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNegativeAll(x);

    public static bool IsNegativeAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsNegativeAny(x);

    // -------- IsInteger / IsEvenInteger / IsOddInteger --------

    public static void IsInteger<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsInteger(x, destination);
    }

    public static bool IsIntegerAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsIntegerAll(x);

    public static bool IsIntegerAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsIntegerAny(x);

    public static void IsEvenInteger<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsEvenInteger(x, destination);
    }

    public static bool IsEvenIntegerAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsEvenIntegerAll(x);

    public static bool IsEvenIntegerAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsEvenIntegerAny(x);

    public static void IsOddInteger<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsOddInteger(x, destination);
    }

    public static bool IsOddIntegerAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsOddIntegerAll(x);

    public static bool IsOddIntegerAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsOddIntegerAny(x);

    // -------- IsRealNumber / IsComplexNumber / IsImaginaryNumber / IsCanonical / IsPow2 --------

    public static void IsRealNumber<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsRealNumber(x, destination);
    }

    public static bool IsRealNumberAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsRealNumberAll(x);

    public static bool IsRealNumberAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsRealNumberAny(x);

    public static void IsComplexNumber<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsComplexNumber(x, destination);
    }

    public static bool IsComplexNumberAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsComplexNumberAll(x);

    public static bool IsComplexNumberAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsComplexNumberAny(x);

    public static void IsImaginaryNumber<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsImaginaryNumber(x, destination);
    }

    public static bool IsImaginaryNumberAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsImaginaryNumberAll(x);

    public static bool IsImaginaryNumberAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsImaginaryNumberAny(x);

    public static void IsCanonical<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsCanonical(x, destination);
    }

    public static bool IsCanonicalAll<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsCanonicalAll(x);

    public static bool IsCanonicalAny<T>(in ReadOnlySpan<T> x) where T : INumberBase<T>
        => TensorPrimitives.IsCanonicalAny(x);

    public static void IsPow2<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : IBinaryNumber<T>
    {
        TensorPrimitives.IsPow2(x, destination);
    }

    public static bool IsPow2All<T>(in ReadOnlySpan<T> x) where T : IBinaryNumber<T>
        => TensorPrimitives.IsPow2All(x);

    public static bool IsPow2Any<T>(in ReadOnlySpan<T> x) where T : IBinaryNumber<T>
        => TensorPrimitives.IsPow2Any(x);
}
