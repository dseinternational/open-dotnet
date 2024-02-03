// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Linq;
using System.Numerics.Tensors;
using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Vector
{
    public static T Minimum<T>(ReadOnlySpan<T> values)
        where T : INumber<T>
    {
        return TensorPrimitives.Min(values);
    }

    public static T Minimum<T>(IEnumerable<T> values)
        where T : struct, INumber<T>
    {
        if (values.TryGetSpan(out var span))
        {
            return Minimum(span);
        }

        ArgumentNullException.ThrowIfNull(values);

        T result;

        using (var e = values.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                ThrowHelper.ThrowInvalidOperationException();
            }

            result = e.Current;

            while (e.MoveNext())
            {
                var x = e.Current;

                if (x < result)
                {
                    result = x;
                }
            }
        }

        return result;
    }

    public static T MinimumFloatingPoint<T>(ReadOnlySpan<T> values)
        where T : IFloatingPointIeee754<T>
    {
        return TensorPrimitives.Min(values);
    }

    public static T MinimumFloatingPoint<T>(this IEnumerable<T> values)
        where T : struct, IFloatingPointIeee754<T>
    {
        if (values.TryGetSpan(out var span))
        {
            return MinimumFloatingPoint(span);
        }

        ArgumentNullException.ThrowIfNull(values);

        var result = T.PositiveInfinity;

        using (var e = values.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                return T.NaN;
            }

            result = e.Current;

            while (e.MoveNext())
            {
                var x = e.Current;

                if (T.IsNaN(x))
                {
                    return T.NaN;
                }

                if (x < result)
                {
                    result = x;
                }
            }
        }

        return result;
    }
}
