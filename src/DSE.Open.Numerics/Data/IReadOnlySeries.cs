// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics.Data;

public interface IReadOnlySeries
{
    string? Name { get; }

    IReadOnlyDictionary<string, Variant>? Annotations { get; }
}

public interface IReadOnlySeries<T, TVector> : ISeries
    where TVector : IReadOnlyVector<T>
{
}

