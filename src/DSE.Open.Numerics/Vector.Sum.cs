// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public partial class Vector
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Sum<T>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
    {
        return Sum<T, T>(span);
    }

    /// <summary>
    /// Gets the sum of the elements in the sequence or
    /// <see cref="IAdditiveIdentity{TSelf, TResult}.AdditiveIdentity"/> if the sequence is empty, and
    /// returns the number of elements in the sequence. The sum is accumulated and returned in a
    /// <typeparamref name="TResult"/> value and if an overflow occurs an <see cref="OverflowException"/>
    /// is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TResult">The type to use for the calculation and the result.</typeparam>
    /// <param name="span">The sequence of elements to use for the calculation.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Sum<T, TResult>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        if (typeof(T) == typeof(TResult))
        {
            return TResult.CreateChecked(TensorPrimitives.Sum(span));
        }

        var result = TResult.AdditiveIdentity;

        foreach (var value in span)
        {
            result += TResult.CreateChecked(value);
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T SumFloatingPoint<T>(
        ReadOnlySpan<T> span,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
    {
        return SumFloatingPoint<T, T>(span, summation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult SumFloatingPoint<T, TResult>(
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TResult">The type to use for the calculation and the result.</typeparam>
    /// <param name="span">The sequence of elements to use for the calculation.</param>
    /// <returns></returns>
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
