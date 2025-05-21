// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static T Sum<T>(ReadOnlySpan<T> vector)
        where T : IEquatable<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        return TensorPrimitives.Sum(vector);
    }

    public static T Sum<T>(this IReadOnlyVector<T> vector)
        where T : IEquatable<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return Sum(vector.AsSpan());
    }

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

    public static T SumFloatingPoint<T>(
        ReadOnlySpan<T> span,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
    {
        return SumFloatingPoint<T, T>(span, summation);
    }

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
