// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// TODO - a series that may only contain values from a defined set.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICategoricalSeries<T>
    : ISeries<T>,
      IReadOnlyCategoricalSeries<T>
    where T : struct, IBinaryNumber<T>
{
    /// <summary>
    /// Specifies the permitted values for a categorical series.
    /// </summary>
    new ICategorySet<T> Categories { get; }
}
