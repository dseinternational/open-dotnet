// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public static class ReadOnlyDataFrameExtensions
{
    /// <summary>
    /// Gets the specified typed series from the <paramref name="dataFrame"/>, if present. Otherwise, throws.
    /// </summary>
    /// <param name="dataFrame">The data-frame.</param>
    /// <param name="name">The name of the series.</param>
    /// <typeparam name="T">The expected <em>element</em> type of the series.</typeparam>
    /// <exception cref="KeyNotFoundException">The <paramref name="dataFrame"/> does not contain a series named <paramref name="name"/>.</exception>
    /// <exception cref="InvalidDataException">The series does not have elements of type <typeparamref name="T"/></exception>
    public static ReadOnlySeries<T> Expect<T>(this ReadOnlyDataFrame dataFrame, string name)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(dataFrame);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var series = dataFrame[name] ?? throw new KeyNotFoundException($"The series '{name}' was not found in the data frame.");
        return series.ExpectElementOfType<T>();
    }
}
