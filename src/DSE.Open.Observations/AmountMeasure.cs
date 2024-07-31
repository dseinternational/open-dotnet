// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record AmountMeasure : Measure<AmountObservation, Amount>
{
    public AmountMeasure(ulong id, Uri uri, string name, string statement)
        : base(id, uri, MeasurementLevel.Amount, name, statement)
    {
    }

    [JsonConstructor]
    private AmountMeasure(ulong id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(measurementLevel, MeasurementLevel.Amount);
    }

    public override AmountObservation CreateObservation(Amount value, DateTimeOffset timestamp)
    {
        return AmountObservation.Create(this, value, TimeProvider.System);
    }
}
