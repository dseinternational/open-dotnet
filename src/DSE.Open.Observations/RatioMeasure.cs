// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record RatioMeasure : Measure<RatioObservation, Ratio>
{
    [JsonConstructor]
    public RatioMeasure(ulong id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

    public override RatioObservation CreateObservation(Ratio value, DateTimeOffset timestamp)
    {
        return RatioObservation.Create(this, value, TimeProvider.System);
    }
}
