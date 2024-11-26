// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class MeasureTests
{
    private static readonly Uri s_measureUri = new("https://schema-test.dseapi.app/testing/measure");

    [Fact]
    public void CanSerializeAndDeserialize_Binary()
    {
        var measure = new Measure<Binary>(s_measureUri, MeasurementLevel.Binary, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_Binary()
    {
        var measure = new Measure<Binary>(s_measureUri, MeasurementLevel.Binary, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure, JsonContext.Default);
    }

    [Fact]
    public void CanCreateObservation_Binary()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new Measure<Binary>(uri, MeasurementLevel.Binary, "Test measure", "[subject] does something");
        var obs = measure.CreateObservation<Observation<Binary>, Binary>(true);
        Assert.Equal(measure.Id, obs.MeasureId);
        Assert.True(obs.Value);
    }
    [Fact]
    public void CanSerializeAndDeserialize_Amount()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new Measure<Amount>(uri, MeasurementLevel.Ordinal, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_Amount()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new Measure<Amount>(uri, MeasurementLevel.Ordinal, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure, JsonContext.Default);
    }

    [Fact]
    public void CanCreateObservation_Amount()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new Measure<Amount>(uri, MeasurementLevel.Ordinal, "Test measure", "[subject] does something");
        var obs = measure.CreateObservation<Observation<Amount>, Amount>((Amount)42.123m);
        Assert.Equal(measure.Id, obs.MeasureId);
        Assert.Equal((Amount)42.123m, obs.Value);
    }

    [Fact]
    public void JsonRoundtrip_BehaviorFrequency()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");

        var measure = new Measure<BehaviorFrequency>(uri, MeasurementLevel.Ordinal, "Test measure", "[subject] does something");

        AssertJson.Roundtrip(measure, JsonContext.Default);
    }
}
