// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// TODO
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ReadOnlyCategoricalSeries<T> : ReadOnlySeries<T>, IReadOnlyCategoricalSeries<T>
    where T : IEquatable<T>
{
    internal ReadOnlyCategoricalSeries(T[] vector, string? name = null, Index? index = null) : base(vector, name, index)
    {
    }
}
