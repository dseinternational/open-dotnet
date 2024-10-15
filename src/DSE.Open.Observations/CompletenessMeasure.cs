// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record CompletenessMeasure : Measure<CompletenessObservation, Completeness>
{
    public CompletenessMeasure(MeasureId id, Uri uri, string name, string statement)
        : base(id, uri, MeasurementLevel.GradedMembership, name, statement)
    {
    }

    [JsonConstructor]
    internal CompletenessMeasure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(measurementLevel, MeasurementLevel.GradedMembership);
    }

    public override CompletenessObservation CreateObservation(Completeness value, DateTimeOffset timestamp)
    {
        return CompletenessObservation.Create(this, value, TimeProvider.System);
    }
}
