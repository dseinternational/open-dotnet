// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed class BinaryWordMeasure : Measure<BinaryWordObservation, bool, ulong>
{
    public BinaryWordMeasure(ulong id, Uri uri, string name, string statement)
        : base(id, uri, MeasurementLevel.Binary, name, statement)
    {
    }

    [JsonConstructor]
    private BinaryWordMeasure(ulong id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(measurementLevel, MeasurementLevel.Binary);
    }

#pragma warning disable CA1725 // Parameter names should match base declaration
    public override BinaryWordObservation CreateObservation(ulong wordId, bool value, DateTimeOffset timestamp)
#pragma warning restore CA1725 // Parameter names should match base declaration
    {
        return BinaryWordObservation.Create(this, wordId, value, TimeProvider.System);
    }
}
