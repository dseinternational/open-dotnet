// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Observations;

public interface IObservationSet
{
    Identifier TrackerReference { get; }

    Identifier ObserverReference { get; }

    GroundPoint? Location { get; }

    Uri Source { get; }
}

public interface IObservationSet<T> : IObservationSet
    where T : IObservation
{
    /// <summary>
    /// The observations in the set.
    /// </summary>
    IReadOnlyCollection<T> Observations { get; }
}
