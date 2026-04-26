// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

/// <summary>Extensions over <see cref="IReadOnlyDataFrame"/>.</summary>
public static class DataFrameExtensions
{
    /// <summary>
    /// Tries to find the column named <paramref name="name"/> in
    /// <paramref name="dataFrame"/>. Returns <see langword="true"/> when found
    /// (with <paramref name="vector"/> set to the column) and
    /// <see langword="false"/> otherwise.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="dataFrame"/> is <see langword="null"/>.</exception>
    public static bool TryGetVector(
        this IReadOnlyDataFrame dataFrame,
        string name,
        [NotNullWhen(true)] out IReadOnlySeries? vector)
    {
        ArgumentNullException.ThrowIfNull(dataFrame);

        vector = dataFrame[name];

        if (vector is null)
        {
            return false;
        }

        return true;
    }
}
