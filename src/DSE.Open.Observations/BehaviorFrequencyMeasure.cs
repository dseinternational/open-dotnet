// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public sealed record BehaviorFrequencyMeasure : Measure<BehaviorFrequencyObservation, BehaviorFrequency>
{
    [SetsRequiredMembers]
    public BehaviorFrequencyMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.GradedMembership;
        Name = name;
        Statement = statement;
    }

    public override BehaviorFrequencyObservation CreateObservation(BehaviorFrequency value, DateTimeOffset timestamp)
    {
        return BehaviorFrequencyObservation.Create(this, value, TimeProvider.System);
    }
}
