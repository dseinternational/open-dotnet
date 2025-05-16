// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IReadOnlyDataFrame : IReadOnlyList<IReadOnlySeries>
{
    IReadOnlySeries? this[string name] { get; }

    string? Name { get; }
}
