// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class CountMeasure : Measure<CountObservation, Count>
{
    public CountMeasure(Uri uri, string name, string statement)
        : this(uri, MeasurementLevel.Count, name, statement)
    {
    }

    public CountMeasure(Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(uri, measurementLevel, name, statement)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public CountMeasure(ulong id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

    public override CountObservation CreateObservation(Count value, DateTimeOffset timestamp)
    {
        return CountObservation.Create(this, value, TimeProvider.System);
    }
}
