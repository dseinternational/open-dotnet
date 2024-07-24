// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed class BinaryMeasure : Measure<BinaryObservation, bool>
{
    public BinaryMeasure(Uri uri, string name, string statement)
        : base(uri, MeasurementLevel.Binary, name, statement)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public BinaryMeasure(ulong id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

    public override BinaryObservation CreateObservation(bool value, DateTimeOffset timestamp)
    {
        return BinaryObservation.Create(this, value, TimeProvider.System);
    }
}
