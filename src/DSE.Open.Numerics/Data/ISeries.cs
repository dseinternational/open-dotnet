// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics.Data;

public interface ISeries : IReadOnlySeries
{
    new IVector Data { get; }

    new IDictionary<string, Variant>? Annotations { get; }
}

public interface ISeries<T, TVector> : ISeries, IReadOnlySeries<T, TVector>
    where TVector : IVector<T>
{
    new TVector Data { get; }
}
