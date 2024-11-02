// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Testing.Xunit;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class RatioSnapshotSetTests
{
    [Fact]
    public void JsonRoundtrip()
    {
        var measure = new RatioMeasure
        {
            Id = MeasureId.GetRandomId(),
            Uri = new Uri("https://schema-test.dseapi.app/testing/ratio-measure"),
            MeasurementLevel = MeasurementLevel.Absolute,
            Name = "Test measure description",
            Statement = "Test measure unit"
        };

        var obs = measure.CreateObservation(Ratio.FromValue(0.2m));

        var set = RatioSnapshotSet.Create(Identifier.New(), [RatioSnapshot.ForUtcNow(obs)]);

        AssertJson.Roundtrip(set, JsonContext.Default);
    }
}
