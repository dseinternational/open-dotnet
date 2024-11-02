// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record RatioMeasure : Measure<RatioObservation, Ratio>
{
    public override RatioObservation CreateObservation(Ratio value, DateTimeOffset timestamp)
    {
        return RatioObservation.Create(this, value, TimeProvider.System);
    }
}
