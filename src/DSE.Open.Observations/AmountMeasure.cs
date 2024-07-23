// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record class AmountMeasure : Measure<AmountObservation, Amount>
{
    public AmountMeasure(Uri uri, string name, string statement)
        : base(uri, MeasurementLevel.Amount, name, statement)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public AmountMeasure(uint id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

    public override AmountObservation CreateObservation(Amount value, DateTimeOffset timestamp)
    {
        return AmountObservation.Create(this, value, TimeProvider.System);
    }
}
