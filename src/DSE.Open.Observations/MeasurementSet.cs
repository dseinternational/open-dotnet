// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// Provides a set of observations where no two observations reference the same measurement.
/// </summary>
/// <typeparam name="TObs"></typeparam>
public class MeasurementSet<TObs> : HashSet<TObs>
    where TObs : IObservation
{
    public MeasurementSet() : base(ObservationComparer<TObs>.Measurement)
    {
    }

    public MeasurementSet(IEnumerable<TObs> collection) : base(collection, ObservationComparer<TObs>.Measurement)
    {
    }

    public MeasurementSet(IEqualityComparer<TObs>? comparer) : base(comparer)
    {
    }

    public MeasurementSet(int capacity) : base(capacity, ObservationComparer<TObs>.Measurement)
    {
    }
}

public sealed class MeasurementSet : MeasurementSet<Observation>
{
    public MeasurementSet()
    {
    }

    public MeasurementSet(IEnumerable<Observation> collection) : base(collection)
    {
    }

    public MeasurementSet(IEqualityComparer<Observation>? comparer) : base(comparer)
    {
    }

    public MeasurementSet(int capacity) : base(capacity)
    {
    }
}
