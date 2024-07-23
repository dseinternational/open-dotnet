// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed class BinaryWordMeasure : Measure<BinaryWordObservation, bool, uint>
{
    public BinaryWordMeasure(Uri uri, string name, string statement)
        : base(uri, MeasurementLevel.Binary, name, statement)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public BinaryWordMeasure(uint id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

#pragma warning disable CA1725 // Parameter names should match base declaration
    public override BinaryWordObservation CreateObservation(uint wordId, bool value, DateTimeOffset timestamp)
#pragma warning restore CA1725 // Parameter names should match base declaration
    {
        return BinaryWordObservation.Create(this, wordId, value, TimeProvider.System);
    }
}
