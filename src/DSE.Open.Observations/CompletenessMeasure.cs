// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public sealed record CompletenessMeasure : Measure<CompletenessObservation, Completeness>
{
    [SetsRequiredMembers]
    public CompletenessMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.GradedMembership;
        Name = name;
        Statement = statement;
    }

    public override CompletenessObservation CreateObservation(Completeness value, DateTimeOffset timestamp)
    {
        return CompletenessObservation.Create(this, value, TimeProvider.System);
    }
}
