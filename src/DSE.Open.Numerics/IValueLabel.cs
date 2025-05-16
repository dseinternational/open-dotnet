// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IValueLabel
{
    object? Value { get; }

    string Label { get; }
}

public interface IValueLabel<T> : IValueLabel
    where T : IEquatable<T>
{
    new T Value { get; }
}
