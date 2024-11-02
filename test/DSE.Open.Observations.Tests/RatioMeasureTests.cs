// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class RatioMeasureTests
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

        AssertJson.Roundtrip(measure, JsonContext.Default);
    }
}
