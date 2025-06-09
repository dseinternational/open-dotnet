// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static class Resampler
{
    public const string PeriodSeriesName = "period";

    public const string ValueSeriesName = "value";

    /// <summary>
    /// Resamples the specified series in the data frame to a new frequency using the specified method.
    /// </summary>
    /// <param name="dates"></param>
    /// <param name="values"></param>
    /// <param name="frequency">The frequency to resample the series to.</param>
    /// <param name="method">The method to use for resampling the series.</param>
    /// <returns>A new <see cref="ReadOnlyDataFrame"/> containing the resampled series, with a <c>"period"</c> series of type <see cref="ReadOnlySeries{DateTimeOffset}"/> and a <c>"value"</c> series of type <see cref="ReadOnlySeries{T}"/> for the resampled values.</returns>
    public static ReadOnlyDataFrame Resample<T>(
        ReadOnlySpan<DateTimeOffset> dates,
        ReadOnlySpan<T> values,
        ResamplingFrequency frequency,
        ResamplingMethod method)
        where T : struct, INumber<T>
    {
        if (dates.Length != values.Length)
        {
            throw new ArgumentException($"`{nameof(dates)}` and `{nameof(values)}` must have the same length.");
        }

        var idx = 0;
        var n = dates.Length;

        List<(DateTimeOffset Period, T Value)> resampledData = [];

        while (idx < n)
        {
            // Determine the start of the current resampling period
            var periodStart = GetPeriodStart(dates[idx], frequency);

            // Find the end of this period
            var start = idx;
            idx++;
            while (idx < n && GetPeriodStart(dates[idx], frequency) == periodStart)
            {
                idx++;
            }

            var length = idx - start;
            var span = values.Slice(start, length);

            var result = method switch
            {
                ResamplingMethod.Mean => TensorPrimitives.Average(span),
                ResamplingMethod.Min => TensorPrimitives.Min(span),
                ResamplingMethod.Max => TensorPrimitives.Max(span),
                ResamplingMethod.First => span[0],
                ResamplingMethod.Last => span[^1],
                ResamplingMethod.Sum => TensorPrimitives.Sum(span),
                _ => throw new NotSupportedException($"Unsupported method: {method}"),
            };

            resampledData.Add((periodStart, result));
        }

        var dateSeries = Series.Create(resampledData.Select(d => d.Period).ToArray(), PeriodSeriesName);
        var valueSeries = Series.Create(resampledData.Select(d => d.Value).ToArray(), ValueSeriesName);

        return ReadOnlyDataFrame.Create([dateSeries, valueSeries]);
    }

    private static DateTimeOffset GetPeriodStart(DateTimeOffset date, ResamplingFrequency frequency)
    {
        return frequency switch
        {
            ResamplingFrequency.Daily => new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, date.Offset),
            ResamplingFrequency.Weekly => new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, date.Offset).AddDays(-(int)date.DayOfWeek),
            ResamplingFrequency.Monthly => new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, date.Offset),
            ResamplingFrequency.Yearly => new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, date.Offset),
            _ => throw new NotSupportedException($"Unsupported frequency: {frequency}"),
        };
    }
}
