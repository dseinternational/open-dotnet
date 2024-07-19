// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public interface IObservation
{
    /// <summary>
    /// Identifies the measure that defines the observation.
    /// </summary>
    uint MeasureId { get; }

    /// <summary>
    /// The value that describes the observation.
    /// </summary>
    object Value { get; }

    /// <summary>
    /// The time at which the observation was made.
    /// </summary>
    DateTimeOffset Time { get; }
}

public interface IObservation<T> : IObservation
    where T : IEquatable<T>
{
    new T Value { get; }
}
