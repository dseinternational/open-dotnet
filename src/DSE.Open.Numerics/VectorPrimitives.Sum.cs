// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Computes the sum of the elements in the vector of numbers using the
    /// <see cref="IAdditionOperators{TSelf, TOther, TResult}"/> implementation provided
    /// by the type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="vector">The vector to sum.</param>
    /// <returns>The result of adding all elements in <paramref name="vector"/>, zero if
    /// <paramref name="vector"/> is empty, or NaN if any element is equal to
    /// <see cref="IFloatingPointIeee754{TSelf}.NaN"/>.</returns>
    /// <remarks>
    /// ⚠️ If any of the elements in the vector is equal to <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result is also NaN.
    /// <para>⚠️ Addition operations are unchecked and may overflow.</para>
    /// <para>⚠️ This method may call into the underlying C runtime or employ instructions specific
    /// to the current architecture. Exact results may differ between different operating systems
    /// or architectures.</para>
    /// </remarks>
    public static T Sum<T>(ReadOnlySpan<T> vector)
        where T : IEquatable<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        return TensorPrimitives.Sum(vector);
    }

    /// <summary>
    /// Computes the sum of the elements in the vector of numbers using the
    /// <see cref="IAdditionOperators{TSelf, TOther, TResult}"/> implementation provided
    /// by the type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="vector">The vector to sum.</param>
    /// <returns>The result of adding all elements in <paramref name="vector"/>, zero if
    /// <paramref name="vector"/> is empty, or NaN if any element is equal to
    /// <see cref="IFloatingPointIeee754{TSelf}.NaN"/>.</returns>
    /// <remarks>
    /// ⚠️ If any of the elements in the vector is equal to <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result is also NaN.
    /// <para>⚠️ Addition operations are unchecked and may overflow.</para>
    /// <para>⚠️ This method may call into the underlying C runtime or employ instructions specific
    /// to the current architecture. Exact results may differ between different operating systems
    /// or architectures.</para>
    /// </remarks>
    public static T Sum<T>(this IReadOnlyVector<T> vector)
        where T : IEquatable<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return Sum(vector.AsSpan());
    }

    /// <summary>
    /// Computes the sum of the elements in the vector of numbers, accumulating the sum
    /// in a value of type <typeparamref name="TResult"/> using the <c>+=</c> operator
    /// provided by the type <typeparamref name="TResult"/> and checking for overflow.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="span"></param>
    /// <returns></returns>
    /// <remarks>
    /// ⚠️ If any of the elements in the vector is equal to <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result is also NaN.
    /// </remarks>
    public static TResult SumChecked<T, TResult>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        var result = TResult.AdditiveIdentity;

        checked
        {
            foreach (var value in span)
            {
                if (T.IsNaN(value))
                {
                    return TResult.CreateChecked(value);
                }

                result += TResult.CreateChecked(value);
            }
        }

        return result;
    }

    public static T SumChecked<T>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
    {
        return SumChecked<T, T>(span);
    }

    public static TResult SumChecked<T, TResult>(this IReadOnlyVector<T> vector)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return SumChecked<T, TResult>(vector.AsSpan());
    }

    public static T SumChecked<T>(this IReadOnlyVector<T> vector)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return SumChecked<T, T>(vector.AsSpan());
    }

    public static T SumChecked<T>(
        ReadOnlySpan<T> span,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
    {
        return SumChecked<T, T>(span, summation);
    }

    public static TResult SumChecked<T, TResult>(
        ReadOnlySpan<T> span,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
        where TResult : struct, IFloatingPointIeee754<TResult>
    {
        if (summation == SummationCompensation.KahanBabushkaNeumaier)
        {
            return SumFloatingPointKahanBabushkaNeumaier<T, TResult>(span);
        }
        else if (summation == SummationCompensation.KahanBabushka)
        {
            throw new NotImplementedException();
        }
        else if (summation == SummationCompensation.Pairwise)
        {
            throw new NotImplementedException();
        }

        if (typeof(T) == typeof(TResult))
        {
            return TResult.CreateChecked(TensorPrimitives.Sum(span));
        }

        var result = TResult.AdditiveIdentity;

        foreach (var value in span)
        {
            if (T.IsNaN(value))
            {
                return TResult.NaN;
            }

            checked
            {
                result += TResult.CreateChecked(value);
            }
        }

        return result;
    }

    public static TResult SumChecked<T, TResult>(
        this IReadOnlyVector<T> vector,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
        where TResult : struct, IFloatingPointIeee754<TResult>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return SumChecked<T, TResult>(vector.AsSpan(), summation);
    }

    private static TResult SumFloatingPointKahanBabushkaNeumaier<T, TResult>(ReadOnlySpan<T> span)
        where T : struct, IFloatingPointIeee754<T>
        where TResult : struct, IFloatingPointIeee754<TResult>
    {
        var result = TResult.AdditiveIdentity;

        var c = TResult.AdditiveIdentity;

        foreach (var value in span)
        {
            if (T.IsNaN(value))
            {
                return TResult.NaN;
            }

            checked
            {
                var v = TResult.CreateChecked(value);
                var t = result + v;

                if (TResult.Abs(result) >= TResult.Abs(v))
                {
                    c += result - t + v;
                }
                else
                {
                    c += v - t + result;
                }

                result = t;
            }
        }

        return result + c;
    }
}
