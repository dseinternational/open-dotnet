// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class AmountMeasureTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new AmountMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure);
    }

    [Fact]
    public void JsonRoundtrip_WithContext()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new AmountMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure, ObservationsJsonSerializerContext.RelaxedJsonEscaping);
    }

    [Fact]
    public void CanCreateObservation()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new AmountMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");
        var obs = measure.CreateObservation((Amount)42.123m, DateTimeOffset.UtcNow);
        Assert.Equal(measure.Id, obs.MeasureId);
        Assert.Equal((Amount)42.123m, obs.Value);
    }
}
