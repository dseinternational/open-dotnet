// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Linq;

public static class EnumerableExtensions
{
    // From: https://github.com/dotnet/runtime/blob/21b4a8585362c1bc12d545b63e62a0d9dd4e8673/src/libraries/System.Linq/src/System/Linq/Enumerable.cs#L27C18-L27C18

    /// <summary>
    /// Validates that source is not null and then tries to extract a span from the source.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // fast type checks that don't add a lot of overhead
    public static bool TryGetSpan<TSource>(this IEnumerable<TSource> source, out ReadOnlySpan<TSource> span)
        where TSource : struct
    {
        Guard.IsNotNull(source);

        var result = true;

        if (source.GetType() == typeof(TSource[]))
        {
            span = Unsafe.As<TSource[]>(source);
        }
        else if (source.GetType() == typeof(List<TSource>))
        {
            span = CollectionsMarshal.AsSpan(Unsafe.As<List<TSource>>(source));
        }
        else if (source.GetType() == typeof(ReadOnlyValueCollection<TSource>))
        {
            span = Unsafe.As<ReadOnlyValueCollection<TSource>>(source).AsSpan();
        }
        else
        {
            span = default;
            result = false;
        }

        return result;
    }
}
