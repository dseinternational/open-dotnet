// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record BehaviorFrequencyMeasure : Measure<BehaviorFrequencyObservation, BehaviorFrequency>
{
    public BehaviorFrequencyMeasure(MeasureId id, Uri uri, string name, string statement)
        : base(id, uri, MeasurementLevel.GradedMembership, name, statement)
    {
    }

    [JsonConstructor]
    internal BehaviorFrequencyMeasure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(measurementLevel, MeasurementLevel.GradedMembership);
    }

    public override BehaviorFrequencyObservation CreateObservation(BehaviorFrequency value, DateTimeOffset timestamp)
    {
        return BehaviorFrequencyObservation.Create(this, value, TimeProvider.System);
    }
}
