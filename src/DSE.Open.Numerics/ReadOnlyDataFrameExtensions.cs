// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public static class ReadOnlyDataFrameExtensions
{
    /// <summary>
    /// Gets the series with the specified name from the data frame.
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown when the series with the specified name does not exist in the data frame.</exception>
    public static ReadOnlySeries GetRequiredNamedSeries(this ReadOnlyDataFrame dataFrame, string name)
    {
        ArgumentNullException.ThrowIfNull(dataFrame);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return dataFrame[name] ?? throw new KeyNotFoundException($"The series '{name}' was not found in the data frame.");
    }

    /// <summary>
    /// Gets the series with the specified name and ensures it has elements of type <typeparamref name="T"/>.
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown when the series with the specified name does not exist in the data frame.</exception>
    /// <exception cref="InvalidDataException">The series does not have elements of type <typeparamref name="T"/></exception>
    public static ReadOnlySeries<T> GetRequiredNamedSeriesWithElementType<T>(this ReadOnlyDataFrame dataFrame, string name)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(dataFrame);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return dataFrame.GetRequiredNamedSeries(name).CastToSeriesWithElementType<T>();
    }
}
