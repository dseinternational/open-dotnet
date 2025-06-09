// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

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

    /// <summary>
    /// Resamples the specified series in the data frame to a new frequency using the specified method.
    /// </summary>
    /// <typeparam name="T">The type of the values in the series to resample.</typeparam>
    /// <param name="dataFrame">The data frame containing the series to resample.</param>
    /// <param name="dateSeriesName">The name of the series containing the <see cref="DateTimeOffset"/> values.</param>
    /// <param name="valueSeriesName">The name of the series containing the values to resample.</param>
    /// <param name="frequency">The frequency to resample the series to.</param>
    /// <param name="method">The method to use for resampling the series.</param>
    /// <returns>A new <see cref="ReadOnlyDataFrame"/> containing the resampled series, with a <c>"period"</c> series of type <see cref="ReadOnlySeries{DateTimeOffset}"/> and a <c>"value"</c> series of type <see cref="ReadOnlySeries{T}"/> for the resampled values.</returns>
    public static ReadOnlyDataFrame Resample<T>(
        this ReadOnlyDataFrame dataFrame,
        string dateSeriesName,
        string valueSeriesName,
        ResamplingFrequency frequency,
        ResamplingMethod method = ResamplingMethod.Mean)
        where T : struct, INumber<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(dataFrame);

        var dates = dataFrame.GetRequiredNamedSeriesWithElementType<DateTimeOffset>(dateSeriesName);
        var values = dataFrame.GetRequiredNamedSeriesWithElementType<T>(valueSeriesName);

        return Resampler.Resample(dates.Vector.AsSpan(), values.Vector.AsSpan(), frequency, method);
    }
}
