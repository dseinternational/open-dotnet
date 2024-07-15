// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public interface IWordObservation : IObservation
{
    /// <summary>
    /// Identifies the word involved in the observation.
    /// </summary>
    uint WordId { get; init; }
}
