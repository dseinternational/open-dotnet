// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Values;

/// <summary>
/// Extension methods over collections of <see cref="IValue{TValue, T}"/> instances.
/// </summary>
public static class ValueCollectionExtensions
{
    /// <summary>
    /// Projects each <typeparamref name="TValue"/> in the source to its underlying
    /// primitive of type <typeparamref name="T"/> and returns the result as an array.
    /// </summary>
    public static T[] ToPrimitiveArray<TValue, T>(this IEnumerable<TValue> collection)
        where T : IEquatable<T>
        where TValue : struct, IValue<TValue, T>
    {
        return collection.Select(TValue.ConvertTo).ToArray();
    }

#pragma warning disable CA1002 // Do not expose generic lists
    /// <summary>
    /// Projects each <typeparamref name="TValue"/> in the source to its underlying
    /// primitive of type <typeparamref name="T"/> and returns the result as a <see cref="List{T}"/>.
    /// </summary>
    public static List<T> ToPrimitiveList<TValue, T>(this IEnumerable<TValue> collection)
#pragma warning restore CA1002 // Do not expose generic lists
        where T : IEquatable<T>
        where TValue : struct, IValue<TValue, T>
        => collection.Select(TValue.ConvertTo).ToList();

    // any value can be counted

    /// <summary>
    /// Returns the number of elements in the source collection.
    /// </summary>
    public static int Count<TValue, T>(this IEnumerable<TValue> collection)
        where T : IEquatable<T>
        where TValue : struct, IValue<TValue, T>
    {
        return collection.Count();
    }

    // ordinal values can be sorted

    // interval values can be summed

    /// <summary>
    /// Computes the sum of the underlying primitives in the source and returns the result
    /// as a <typeparamref name="TValue"/>.
    /// </summary>
    public static TValue Sum<TValue, T>(this IEnumerable<TValue> source)
        where T : struct, INumber<T>
        where TValue : struct, IAddableValue<TValue, T>
    {
        return TValue.FromValue(source.SumPrimitives<TValue, T>());
    }

    /// <summary>
    /// Computes the sum of the underlying primitives in the span and returns the result
    /// as a <typeparamref name="TValue"/>.
    /// </summary>
    public static TValue Sum<TValue, T>(ReadOnlySpan<TValue> span)
        where T : struct, INumber<T>
        where TValue : struct, IAddableValue<TValue, T>
    {
        return TValue.FromValue(SumPrimitives<TValue, T>(span));
    }

    /// <summary>
    /// Computes the sum of the underlying primitives in the source.
    /// </summary>
    public static T SumPrimitives<TValue, T>(this IEnumerable<TValue> source)
        where T : struct, INumber<T>
        where TValue : struct, IAddableValue<TValue, T>
    {
        return source.Select(TValue.ConvertTo).SumCore<T, T>();
    }

    /// <summary>
    /// Computes the sum of the underlying primitives in the span.
    /// </summary>
    public static T SumPrimitives<TValue, T>(ReadOnlySpan<TValue> span)
        where T : struct, INumber<T>
        where TValue : struct, IAddableValue<TValue, T>
    {
        var sum = T.Zero;

        foreach (var value in span)
        {
            checked
            {
                sum += TValue.ConvertTo(value);
            }
        }

        return sum;
    }

    /// <summary>
    /// Computes the average of the underlying primitives in the source, accumulating
    /// the running total in <typeparamref name="TAccumulator"/> to limit overflow risk.
    /// </summary>
    /// <exception cref="InvalidOperationException">The source is empty.</exception>
    public static T AveragePrimitives<TValue, T, TAccumulator>(this IEnumerable<TValue> source)
        where T : struct, INumber<T>
        where TValue : struct, IAddableValue<TValue, T>
        where TAccumulator : struct, INumber<TAccumulator>

    {
        if (source.TryGetSpan(out var span))
        {
            return AveragePrimitives<TValue, T>(span);
        }

        using (var e = source.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                ThrowHelper.ThrowInvalidOperationException();
            }

            var sum = TAccumulator.CreateChecked(TValue.ConvertTo(e.Current));

            long count = 1;

            while (e.MoveNext())
            {
                checked
                {
                    sum += TAccumulator.CreateChecked(TValue.ConvertTo(e.Current));
                }

                count++;
            }

            return T.CreateChecked(sum) / T.CreateChecked(count);
        }
    }

    /// <summary>
    /// Computes the average of the underlying primitives in the span.
    /// </summary>
    /// <exception cref="InvalidOperationException">The span is empty.</exception>
    public static T AveragePrimitives<TValue, T>(ReadOnlySpan<TValue> span)
        where T : struct, INumber<T>
        where TValue : struct, IAddableValue<TValue, T>
    {
        if (span.IsEmpty)
        {
            ThrowHelper.ThrowInvalidOperationException();
        }

        return T.CreateChecked(SumPrimitives<TValue, T>(span) / T.CreateChecked(span.Length));
    }

    // https://github.com/dotnet/runtime/blob/da1da02bbd2cb54490b7fc22f43ec32f5f302615/src/libraries/System.Linq/src/System/Linq/Average.cs#LL77C9-L77C9

    private static TResult AverageCore<TSource, TAccumulator, TResult>(this IEnumerable<TSource> source)
        where TSource : struct, INumber<TSource>
        where TAccumulator : struct, INumber<TAccumulator>
        where TResult : struct, INumber<TResult>
    {
        if (source.TryGetSpan(out var span))
        {
            if (span.IsEmpty)
            {
                ThrowHelper.ThrowInvalidOperationException();
            }

            return TResult.CreateChecked(SumCore<TSource, TAccumulator>(span)) / TResult.CreateChecked(span.Length);
        }

        using (var e = source.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                ThrowHelper.ThrowInvalidOperationException();
            }

            var sum = TAccumulator.CreateChecked(e.Current);

            long count = 1;

            while (e.MoveNext())
            {
                checked
                {
                    sum += TAccumulator.CreateChecked(e.Current);
                }

                count++;
            }

            return TResult.CreateChecked(sum) / TResult.CreateChecked(count);
        }
    }

    // https://github.com/dotnet/runtime/blob/da1da02bbd2cb54490b7fc22f43ec32f5f302615/src/libraries/System.Linq/src/System/Linq/Sum.cs#L24

    private static TResult SumCore<TSource, TResult>(this IEnumerable<TSource> source)
        where TSource : struct, INumber<TSource>
        where TResult : struct, INumber<TResult>
    {
        if (source.TryGetSpan(out var span))
        {
            return SumCore<TSource, TResult>(span);
        }

        var sum = TResult.Zero;

        foreach (var value in source)
        {
            checked
            {
                sum += TResult.CreateChecked(value);
            }
        }

        return sum;
    }

    // https://github.com/dotnet/runtime/blob/da1da02bbd2cb54490b7fc22f43ec32f5f302615/src/libraries/System.Linq/src/System/Linq/Sum.cs#L42

    private static TResult SumCore<T, TResult>(ReadOnlySpan<T> span)
        where T : struct, INumber<T>
        where TResult : struct, INumber<TResult>
    {
        var sum = TResult.Zero;

        foreach (var value in span)
        {
            checked
            {
                sum += TResult.CreateChecked(value);
            }
        }

        return sum;
    }

    // https://github.com/dotnet/runtime/blob/da1da02bbd2cb54490b7fc22f43ec32f5f302615/src/libraries/System.Linq/src/System/Linq/Enumerable.cs#L27

    /// <summary>Validates that source is not null and then tries to extract a span from the source.</summary> // fast type checks that don't add a lot of overhead
    private static bool TryGetSpan<TSource>(this IEnumerable<TSource> source, out ReadOnlySpan<TSource> span)
        // This constraint isn't required, but the overheads involved here can be more substantial when TSource
        // is a reference type and generic implementations are shared.  So for now we're protecting ourselves
        // and forcing a conscious choice to remove this in the future, at which point it should be paired with
        // sufficient performance testing.
        where TSource : struct
    {
        ArgumentNullException.ThrowIfNull(source);

        // Use `GetType() == typeof(...)` rather than `is` to avoid cast helpers.  This is measurably cheaper
        // but does mean we could end up missing some rare cases where we could get a span but don't (e.g. a uint[]
        // masquerading as an int[]).  That's an acceptable tradeoff.  The Unsafe usage is only after we've
        // validated the exact type; this could be changed to a cast in the future if the JIT starts to recognize it.
        // We only pay the comparison/branching costs here for super common types we expect to be used frequently
        // with LINQ methods.

        var result = true;

        if (source.GetType() == typeof(TSource[]))
        {
            span = Unsafe.As<TSource[]>(source);
        }
        else if (source.GetType() == typeof(List<TSource>))
        {
            span = CollectionsMarshal.AsSpan(Unsafe.As<List<TSource>>(source));
        }
        else
        {
            span = default;
            result = false;
        }

        return result;
    }
}
