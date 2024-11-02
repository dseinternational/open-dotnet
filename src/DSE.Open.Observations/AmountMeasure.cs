// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record AmountMeasure : Measure<AmountObservation, Amount>
{
    [SetsRequiredMembers]
    public AmountMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.Amount;
        Name = name;
        Statement = statement;
    }

    public override AmountObservation CreateObservation(Amount value, DateTimeOffset timestamp)
    {
        return AmountObservation.Create(this, value, TimeProvider.System);
    }
}
