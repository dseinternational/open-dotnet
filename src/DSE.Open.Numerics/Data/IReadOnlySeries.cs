// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics.Data;

public interface IReadOnlySeries
{
    string? Name { get; }

    IReadOnlyVector Data { get; }

    IReadOnlyDictionary<string, Variant>? Annotations { get; }
}

public interface IReadOnlySeries<T, TVector> : IReadOnlySeries
    where TVector : IReadOnlyVector<T>
{
    new TVector Data { get; }
}

