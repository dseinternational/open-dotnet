// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using DSE.Open.Linq;

namespace DSE.Open.Numerics;

public static partial class Vector
{
    public static T Maximum<T>(ReadOnlySpan<T> values)
        where T : INumber<T>
    {
        return TensorPrimitives.Max(values);
    }

    public static T Maximum<T>(IEnumerable<T> values)
        where T : struct, INumber<T>
    {
        if (values.TryGetSpan(out var span))
        {
            return Maximum(span);
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

                if (x > result)
                {
                    result = x;
                }
            }
        }

        return result;
    }

    public static T MaximumFloatingPoint<T>(ReadOnlySpan<T> values)
        where T : IFloatingPointIeee754<T>
    {
        return TensorPrimitives.Max(values);
    }

    public static T MaximumFloatingPoint<T>(this IEnumerable<T> values)
        where T : struct, IFloatingPointIeee754<T>
    {
        if (values.TryGetSpan(out var span))
        {
            return MaximumFloatingPoint(span);
        }

        ArgumentNullException.ThrowIfNull(values);

        var result = T.NegativeInfinity;

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
                
                if (x > result)
                {
                    result = x;
                }
            }
        }

        return result;
    }
}
