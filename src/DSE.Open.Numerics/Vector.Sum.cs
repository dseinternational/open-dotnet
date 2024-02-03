// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using DSE.Open.Linq;

namespace DSE.Open.Numerics;

public static partial class Vector
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sequence"></param>
    /// <returns></returns>
    public static T Sum<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
    {
        return Sum<T, T>(sequence);
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
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <returns></returns>
    public static TResult Sum<T, TResult>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        if (typeof(T) == typeof(TResult))
        {
            return TResult.CreateChecked(TensorPrimitives.Sum(sequence));
        }

        var result = TResult.AdditiveIdentity;

        foreach (var value in sequence)
        {
            result += TResult.CreateChecked(value);
        }

        return result;
    }

    public static T SumFloatingPoint<T>(
        ReadOnlySpan<T> sequence,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
    {
        return SumFloatingPoint<T, T>(sequence, summation);
    }

    public static TResult SumFloatingPoint<T, TResult>(
        ReadOnlySpan<T> sequence,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
        where TResult : struct, IFloatingPointIeee754<TResult>
    {
        if (summation == SummationCompensation.KahanBabushkaNeumaier)
        {
            return SumFloatingPointKahanBabushkaNeumaier<T, TResult>(sequence);
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
            return TResult.CreateChecked(TensorPrimitives.Sum(sequence));
        }

        var result = TResult.AdditiveIdentity;

        foreach (var value in sequence)
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
    /// Gets the sum of the elements in the sequence or
    /// <see cref="IAdditiveIdentity{TSelf, TResult}.AdditiveIdentity"/> if the sequence is empty. The sum
    /// is accumulated in a <typeparamref name="T"/> value and if an overflow occurs an
    /// <see cref="OverflowException"/> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <returns></returns>
    /// <exception cref="OverflowException">An arithmetic overflow occured.</exception>
    public static T Sum<T>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
    {
        return Sum<T, T>(sequence, out _);
    }

    /// <summary>
    /// Gets the sum of the elements in the sequence or
    /// <see cref="IAdditiveIdentity{TSelf, TResult}.AdditiveIdentity"/> if the sequence is empty, and
    /// returns the number of elements in the sequence. The sum is accumulated in a <typeparamref name="T"/>
    /// value and if an overflow occurs an <see cref="OverflowException"/> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <param name="size"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException">An arithmetic overflow occured.</exception>
    public static T Sum<T>(IEnumerable<T> sequence, out long size)
        where T : struct, INumberBase<T>
    {
        return Sum<T, T>(sequence, out size);
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
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <returns></returns>
    public static TResult Sum<T, TResult>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        return Sum<T, TResult>(sequence, out _);
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
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <param name="size">The number of elements in the sequence.</param>
    /// <returns></returns>
    public static TResult Sum<T, TResult>(IEnumerable<T> sequence, out long size)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        if (sequence.TryGetSpan(out var span))
        {
            size=span.Length;
            return Sum<T, TResult>(span);
        }

        ArgumentNullException.ThrowIfNull(sequence);

        var result = TResult.AdditiveIdentity;
        size = 0;

        foreach (var value in sequence)
        {
            size++;
            result += TResult.CreateChecked(value);
        }

        return result;
    }

    /// <summary>
    /// Gets the sum of the elements in the sequence or
    /// <see cref="IAdditiveIdentity{TSelf, TResult}.AdditiveIdentity"/> if the sequence is empty, and
    /// returns the number of elements in the sequence. The sum is accumulated and returned in a
    /// <typeparamref name="T"/> value and if an overflow occurs an <see cref="OverflowException"/>
    /// is thrown. It the sequence includes a <see cref="IFloatingPointIeee754{T}.NaN"/> value then
    /// Nan is returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sequence"></param>
    /// <param name="summation"></param>
    /// <returns></returns>
    public static T SumFloatingPoint<T>(
        IEnumerable<T> sequence,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
    {
        return SumFloatingPoint<T, T>(sequence, summation);
    }

    /// <summary>
    /// Gets the sum of the elements in the sequence or
    /// <see cref="IAdditiveIdentity{TSelf, TResult}.AdditiveIdentity"/> if the sequence is empty, and
    /// returns the number of elements in the sequence. The sum is accumulated and returned in a
    /// <typeparamref name="TResult"/> value and if an overflow occurs an <see cref="OverflowException"/>
    /// is thrown. It the sequence includes a <see cref="IFloatingPointIeee754{T}.NaN"/> value then
    /// Nan is returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="sequence"></param>
    /// <param name="summation"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static TResult SumFloatingPoint<T, TResult>(
        IEnumerable<T> sequence,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
        where TResult : struct, IFloatingPointIeee754<TResult>
    {
        if (sequence.TryGetSpan(out var span))
        {
            return SumFloatingPoint<T, TResult>(span, summation);
        }

        ArgumentNullException.ThrowIfNull(sequence);

        if (summation == SummationCompensation.KahanBabushkaNeumaier)
        {
            return SumFloatingPointKahanBabushkaNeumaier<T, TResult>(sequence);
        }
        else if (summation == SummationCompensation.KahanBabushka)
        {
            throw new NotImplementedException();
        }
        else if (summation == SummationCompensation.Pairwise)
        {
            throw new NotImplementedException();
        }

        var result = TResult.AdditiveIdentity;

        foreach (var value in sequence)
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
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <returns></returns>
    private static TResult SumFloatingPointKahanBabushkaNeumaier<T, TResult>(IEnumerable<T> sequence)
        where T : struct, IFloatingPointIeee754<T>
        where TResult : struct, IFloatingPointIeee754<TResult>
    {
        var result = TResult.AdditiveIdentity;

        var c = TResult.AdditiveIdentity;

        foreach (var value in sequence)
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TResult">The type to use for the calculation and the result.</typeparam>
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <returns></returns>
    private static TResult SumFloatingPointKahanBabushkaNeumaier<T, TResult>(ReadOnlySpan<T> sequence)
        where T : struct, IFloatingPointIeee754<T>
        where TResult : struct, IFloatingPointIeee754<TResult>
    {
        var result = TResult.AdditiveIdentity;

        var c = TResult.AdditiveIdentity;

        foreach (var value in sequence)
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
