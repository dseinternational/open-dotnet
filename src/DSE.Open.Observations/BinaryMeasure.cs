// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record BinaryMeasure : Measure<BinaryObservation, bool>
{
    public BinaryMeasure(MeasureId id, Uri uri, string name, string statement)
        : base(id, uri, MeasurementLevel.Binary, name, statement)
    {
    }

    [JsonConstructor]
    internal BinaryMeasure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(measurementLevel, MeasurementLevel.Binary);
    }

    public override BinaryObservation CreateObservation(bool value, DateTimeOffset timestamp)
    {
        return BinaryObservation.Create(this, value, TimeProvider.System);
    }
}
