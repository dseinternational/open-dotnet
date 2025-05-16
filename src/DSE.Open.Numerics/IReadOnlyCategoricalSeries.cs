// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public interface IReadOnlyCategoricalSeries
{
    bool IsEmpty { get; }

    IReadOnlyCategorySet Categories { get; }
}

/// <summary>
/// TODO - a read-only series that may only contain values from a defined set.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IReadOnlyCategoricalSeries<T>
    : IReadOnlyCategoricalSeries,
      IReadOnlySeries<T>
    where T : struct, IBinaryNumber<T>
{
    /// <summary>
    /// Specifies the permitted values for a categorical series.
    /// </summary>
    new IReadOnlyCategorySet<T> Categories { get; }
}
