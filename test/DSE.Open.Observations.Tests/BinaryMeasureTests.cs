// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class BinaryMeasureTests
{
    private static readonly Uri s_measureUri = new("https://schema-test.dseapi.app/testing/measure");

    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var measure = new BinaryMeasure(MeasureId.GetRandomId(), s_measureUri, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure);
    }

    [Fact]
    public void JsonRoundtrip_WithContext()
    {
        var measure = new BinaryMeasure(MeasureId.GetRandomId(), s_measureUri, "Test measure", "[subject] does something");
        var typeInfo = ObservationsJsonSerializerContext.Default.BinaryMeasure;
        AssertJson.Roundtrip(measure, typeInfo);
    }

    [Fact]
    public void CanCreateObservation()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new BinaryMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");
        var obs = measure.CreateObservation(true, DateTimeOffset.UtcNow);
        Assert.Equal(measure.Id, obs.MeasureId);
        Assert.True(obs.Value);
    }
}
