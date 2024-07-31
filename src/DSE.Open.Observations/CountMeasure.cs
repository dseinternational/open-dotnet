// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record CountMeasure : Measure<CountObservation, Count>
{
    public CountMeasure(ulong id, Uri uri, string name, string statement)
        : this(id, uri, MeasurementLevel.Count, name, statement)
    {
    }

    [JsonConstructor]
    private CountMeasure(ulong id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(measurementLevel, MeasurementLevel.Count);
    }

    public override CountObservation CreateObservation(Count value, DateTimeOffset timestamp)
    {
        return CountObservation.Create(this, value, TimeProvider.System);
    }
}
