// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record CountMeasure : Measure<CountObservation, Count>
{
    [SetsRequiredMembers]
    public CountMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.Count;
        Name = name;
        Statement = statement;
    }

    public override CountObservation CreateObservation(Count value, DateTimeOffset timestamp)
    {
        return CountObservation.Create(this, value, TimeProvider.System);
    }
}
