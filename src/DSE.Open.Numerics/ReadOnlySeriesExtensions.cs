// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.


namespace DSE.Open.Numerics;

public static class ReadOnlySeriesExtensions
{
    /// <summary>
    /// Gets the series with elements typed to the specified <typeparamref name="T"/>.
    /// </summary>
    /// <param name="series">The series.</param>
    /// <typeparam name="T">The element type.</typeparam>
    /// <exception cref="InvalidDataException">The <paramref name="series"/> does not have elements of type <typeparamref name="T"/></exception>
    public static ReadOnlySeries<T> Expect<T>(this ReadOnlySeries series)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(series);

        if (series is not ReadOnlySeries<T> typedData)
        {
            throw new InvalidDataException(
                $"Expected series to be of type {typeof(ReadOnlySeries<T>)} but was {series?.GetType()}.");
        }

        return typedData;
    }

    /// <summary>
    /// Creates a mutable copy of the specified read-only series.
    /// </summary>
    /// <param name="series">The series to copy.</param>
    public static Series<T> Copied<T>(this ReadOnlySeries<T> series)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(series);
        return new Series<T>(series.Vector.Copied(), series.Name, series.Categories.Copied(), series.ValueLabels.Copied());
    }
}
