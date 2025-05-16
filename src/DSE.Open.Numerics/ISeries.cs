// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous sequence of values of known length and data type.
/// Optionally named, labelled or categorised for use with a <see cref="DataFrame"/>.
/// </summary>
public interface ISeries : IReadOnlySeries
{
    new string? Name { get; set; }

    /// <summary>
    /// Gets a read-only view of the series.
    /// </summary>
    IReadOnlySeries AsReadOnly();
}
