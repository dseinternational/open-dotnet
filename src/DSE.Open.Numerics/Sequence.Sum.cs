// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using DSE.Open.Linq;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    public static T SumInteger<T>(IEnumerable<T> sequence)
        where T : struct, IBinaryInteger<T>
    {
        return SumInteger<T, T>(sequence);
    }

    public static TAcc SumInteger<T, TAcc>(IEnumerable<T> sequence)
        where T : struct, IBinaryInteger<T>
        where TAcc : struct, IBinaryInteger<TAcc>
    {
        if (sequence.TryGetSpan(out var span))
        {
            return SumInteger<T, TAcc>(span);
        }

        ArgumentNullException.ThrowIfNull(sequence);

        var result = TAcc.AdditiveIdentity;

        foreach (var value in sequence)
        {
            result += TAcc.CreateChecked(value);
        }

        return result;
    }

    public static T SumInteger<T>(ReadOnlySpan<T> sequence)
        where T : struct, IBinaryInteger<T>
    {
        return SumInteger<T,T>(sequence);
    }

    public static TAcc SumInteger<T, TAcc>(ReadOnlySpan<T> sequence)
        where T : struct, IBinaryInteger<T>
        where TAcc : struct, IBinaryInteger<TAcc>
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
            return SumFloatingPointIeee754KahanBabushkaNeumaier<T, TAcc>(sequence);
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
            return SumFloatingPointIeee754KahanBabushkaNeumaier<T, TAcc>(sequence);
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

    private static TAcc SumFloatingPointIeee754KahanBabushkaNeumaier<T, TAcc>(IEnumerable<T> sequence)
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

    private static TAcc SumFloatingPointIeee754KahanBabushkaNeumaier<T, TAcc>(ReadOnlySpan<T> sequence)
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
