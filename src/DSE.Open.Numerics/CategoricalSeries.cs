// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// TODO
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class CategoricalSeries<T> : Series<T>, ICategoricalSeries<T>
    where T : IEquatable<T>
{
    internal CategoricalSeries(T[] vector, string? name = null, Index? index = null) : base(vector, name, index)
    {
    }
}
