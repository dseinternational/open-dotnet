// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IReadOnlyDataFrame
{
    string? Name { get; }

    int Count { get; }

    IReadOnlySeries this[int index] { get; }

    IReadOnlySeries? this[string name] { get; }

    IEnumerable<IReadOnlySeries> GetReadOnlySeriesEnumerable();
}
