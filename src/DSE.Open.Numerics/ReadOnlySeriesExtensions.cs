// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.


namespace DSE.Open.Numerics;

public static class ReadOnlySeriesExtensions
{
    /// <summary>
    /// Gets the series with the specified name from the data frame.
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown when the series with the specified name does not exist in the data frame.</exception>
    public static ReadOnlySeries ExpectNamedSeries(this ReadOnlyDataFrame dataFrame, string name)
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
    public static ReadOnlySeries ExpectNamedSeriesWithElementType<T>(this ReadOnlyDataFrame dataFrame, string name)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(dataFrame);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return dataFrame.ExpectNamedSeries(name).ExpectElementOfType<T>();
    }

    /// <summary>
    /// Gets the series with elements typed to the specified <typeparamref name="T"/>.
    /// </summary>
    /// <param name="series">The series.</param>
    /// <typeparam name="T">The element type.</typeparam>
    /// <exception cref="InvalidDataException">The <paramref name="series"/> does not have elements of type <typeparamref name="T"/></exception>
    public static ReadOnlySeries<T> ExpectElementOfType<T>(this ReadOnlySeries series)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(series);

        if (series is not ReadOnlySeries<T> typedData)
        {
            throw new InvalidDataException(
                $"Expected series to be of type `{typeof(ReadOnlySeries<T>)}` but was `{series.GetType()}`.");
        }

        return typedData;
    }
}
