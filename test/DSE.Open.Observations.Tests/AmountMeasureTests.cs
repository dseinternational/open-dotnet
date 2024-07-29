// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class AmountMeasureTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new AmountMeasure(986168514, uri, "Test measure", "[subject] does something");
        var json = JsonSerializer.Serialize(measure, JsonSharedOptions.RelaxedJsonEscaping);
        var deserialized = JsonSerializer.Deserialize<AmountMeasure>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.NotNull(deserialized);
        Assert.Equal(measure.Id, deserialized.Id);
        Assert.Equal(measure.Uri, deserialized.Uri);
        Assert.Equal(measure.MeasurementLevel, deserialized.MeasurementLevel);
        Assert.Equal(measure.Name, deserialized.Name);
        Assert.Equal(measure.Statement, deserialized.Statement);
    }

    [Fact]
    public void CanCreateObservation()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new AmountMeasure(986168514, uri, "Test measure", "[subject] does something");
        var obs = measure.CreateObservation((Amount)42.123m, DateTimeOffset.UtcNow);
        Assert.Equal(measure.Id, obs.MeasureId);
        Assert.Equal((Amount)42.123m, obs.Value);
    }
}
