// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// TODO
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class CategorySeries<T> : Series<T>
    where T : IEquatable<T>
{
    public CategorySeries(T[] vector, string? name = null) : base(vector, name)
    {
    }
}
