// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics.Data;

public static class SeriesExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T, TVector>(this ISeries<T, TVector> series, T value)
        where T : IEquatable<T>
        where TVector : IVector<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return series.Data.IndexOf(value);
    }
}
