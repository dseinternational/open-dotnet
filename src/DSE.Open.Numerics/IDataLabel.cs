// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IDataLabel
{
    object? Data { get; }

    string Label { get; }
}

public interface IDataLabel<T> : IDataLabel
    where T : IEquatable<T>
{
    new T Data { get; }
}
