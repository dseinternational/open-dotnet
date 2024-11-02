// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public sealed record BinaryMeasure : Measure<BinaryObservation, bool>
{
    [SetsRequiredMembers]
    public BinaryMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.Binary;
        Name = name;
        Statement = statement;
    }


    public override BinaryObservation CreateObservation(bool value, DateTimeOffset timestamp)
    {
        return BinaryObservation.Create(this, value, TimeProvider.System);
    }
}
