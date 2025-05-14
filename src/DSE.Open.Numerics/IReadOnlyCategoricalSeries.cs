// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// TODO - a read-only series than may only contain values from a defined set.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IReadOnlyCategoricalSeries<T>
    : IReadOnlySeries<T>
    where T : IEquatable<T>
{
}
