// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using DSE.Open.Linq;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    /// <summary>
    /// Gets the sum of the elements in the sequence or
    /// <see cref="IAdditiveIdentity{TSelf, TResult}.AdditiveIdentity"/> if the sequence is empty. The sum
    /// is accumulated in a <typeparamref name="T"/> value and if an overflow occurs an
    /// <see cref="OverflowException"/> is thrown.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sequence"></param>
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
    /// <typeparam name="T"></typeparam>
    /// <param name="sequence"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException">An arithmetic overflow occured.</exception>
    public static T Sum<T>(IEnumerable<T> sequence, out long size)
        where T : struct, INumberBase<T>
    {
        return Sum<T, T>(sequence, out size);
    }

    public static TAcc Sum<T, TAcc>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TAcc : struct, INumberBase<TAcc>
    {
        return Sum<T, TAcc>(sequence, out _);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TAcc"></typeparam>
    /// <param name="sequence"></param>
    /// <param name="size">The number of elements in the sequence.</param>
    /// <returns></returns>
    public static TAcc Sum<T, TAcc>(IEnumerable<T> sequence, out long size)
        where T : struct, INumberBase<T>
        where TAcc : struct, INumberBase<TAcc>
    {
        if (sequence.TryGetSpan(out var span))
        {
            size=span.Length;
            return Sum<T, TAcc>(span);
        }

        ArgumentNullException.ThrowIfNull(sequence);

        var result = TAcc.AdditiveIdentity;
        size = 0;

        foreach (var value in sequence)
        {
            size++;
            result += TAcc.CreateChecked(value);
        }

        return result;
    }

    public static T Sum<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
    {
        return Sum<T, T>(sequence);
    }

    public static TAcc Sum<T, TAcc>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
        where TAcc : struct, INumberBase<TAcc>
    {
        if (typeof(T) == typeof(TAcc))
        {
            return TAcc.CreateChecked(TensorPrimitives.Sum(sequence));
        }

        var result = TAcc.AdditiveIdentity;

        foreach (var value in sequence)
        {
            result += TAcc.CreateChecked(value);
        }

        return result;
    }

    public static T SumFloatingPoint<T>(
        IEnumerable<T> sequence,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
    {
        return SumFloatingPoint<T, T>(sequence, summation);
    }

    public static TAcc SumFloatingPoint<T, TAcc>(
        IEnumerable<T> sequence,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
        where TAcc : struct, IFloatingPointIeee754<TAcc>
    {
        if (sequence.TryGetSpan(out var span))
        {
            return SumFloatingPoint<T, TAcc>(span, summation);
        }

        ArgumentNullException.ThrowIfNull(sequence);

        if (summation == SummationCompensation.KahanBabushkaNeumaier)
        {
            return SumFloatingPointKahanBabushkaNeumaier<T, TAcc>(sequence);
        }
        else if (summation == SummationCompensation.KahanBabushka)
        {
            ThrowHelper.ThrowNotSupportedException();
        }
        else if (summation == SummationCompensation.Pairwise)
        {
            ThrowHelper.ThrowNotSupportedException();
        }

        var result = TAcc.AdditiveIdentity;

        foreach (var value in sequence)
        {
            if (T.IsNaN(value))
            {
                return TAcc.NaN;
            }

            checked
            {
                result += TAcc.CreateChecked(value);
            }
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

    public static TAcc SumFloatingPoint<T, TAcc>(
        ReadOnlySpan<T> sequence,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
        where TAcc : struct, IFloatingPointIeee754<TAcc>
    {
        if (summation == SummationCompensation.KahanBabushkaNeumaier)
        {
            return SumFloatingPointKahanBabushkaNeumaier<T, TAcc>(sequence);
        }
        else if (summation == SummationCompensation.KahanBabushka)
        {
            ThrowHelper.ThrowNotSupportedException();
        }
        else if (summation == SummationCompensation.Pairwise)
        {
            ThrowHelper.ThrowNotSupportedException();
        }

        if (typeof(T) == typeof(TAcc))
        {
            return TAcc.CreateChecked(TensorPrimitives.Sum(sequence));
        }

        var result = TAcc.AdditiveIdentity;

        foreach (var value in sequence)
        {
            if (T.IsNaN(value))
            {
                return TAcc.NaN;
            }

            checked
            {
                result += TAcc.CreateChecked(value);
            }
        }

        return result;
    }

    private static TAcc SumFloatingPointKahanBabushkaNeumaier<T, TAcc>(IEnumerable<T> sequence)
        where T : struct, IFloatingPointIeee754<T>
        where TAcc : struct, IFloatingPointIeee754<TAcc>
    {
        var result = TAcc.AdditiveIdentity;

        var c = TAcc.AdditiveIdentity;

        foreach (var value in sequence)
        {
            if (T.IsNaN(value))
            {
                return TAcc.NaN;
            }

            checked
            {
                var v = TAcc.CreateChecked(value);
                var t = result + v;

                if (TAcc.Abs(result) >= TAcc.Abs(v))
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

    private static TAcc SumFloatingPointKahanBabushkaNeumaier<T, TAcc>(ReadOnlySpan<T> sequence)
        where T : struct, IFloatingPointIeee754<T>
        where TAcc : struct, IFloatingPointIeee754<TAcc>
    {
        var result = TAcc.AdditiveIdentity;

        var c = TAcc.AdditiveIdentity;

        foreach (var value in sequence)
        {
            if (T.IsNaN(value))
            {
                return TAcc.NaN;
            }

            checked
            {
                var v = TAcc.CreateChecked(value);
                var t = result + v;

                if (TAcc.Abs(result) >= TAcc.Abs(v))
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
