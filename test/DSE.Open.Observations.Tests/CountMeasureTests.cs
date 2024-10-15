// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class CountMeasureTests
{
    public static readonly Uri s_measureUri = new("https://schema-test.dseapi.app/testing/measure");

    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var measure = new CountMeasure(MeasureId.GetRandomId(), s_measureUri, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure);
    }

    [Fact]
    public void JsonRoundtrip_WithContext()
    {
        var measure = new CountMeasure(MeasureId.GetRandomId(), s_measureUri, "Test measure", "[subject] does something");
        var typeInfo = ObservationsJsonSerializerContext.Default.CountMeasure;
        AssertJson.Roundtrip(measure, typeInfo);
    }

    [Fact]
    public void CanCreateObservation()
    {
        var measure = new CountMeasure(MeasureId.GetRandomId(), s_measureUri, "Test measure", "[subject] does something");
        var obs = measure.CreateObservation((Count)42, DateTimeOffset.UtcNow);
        Assert.Equal(measure.Id, obs.MeasureId);
        Assert.Equal((Count)42, obs.Value);
    }
}
