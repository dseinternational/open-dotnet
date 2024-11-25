// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public interface ISnapshot
{
    DateTimeOffset Time { get; }

    int GetMeasurementHashCode();
}

public interface ISnapshot<TObs> : ISnapshot
    where TObs : IObservation
{
    TObs Observation { get; }
}
