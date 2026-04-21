// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Testing.Xunit;
using DSE.Open.Text.Json;

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
        var obs = measure.CreateObservation(true);
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
        var obs = measure.CreateObservation((Amount)42.123m);
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

    [Fact]
    public void Equals_SameId_ReturnsTrueAndMatchesHashCode()
    {
        var a = new Measure<Binary>(s_measureUri, MeasurementLevel.Binary, "A", "[subject] does A");
        var b = new Measure<Binary>(a.Id, s_measureUri, MeasurementLevel.Binary, "B (different name)", "[subject] does B", 0);

        Assert.True(a.Equals(b));
        Assert.True(a.Equals((object)b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentId_ReturnsFalse()
    {
        var a = new Measure<Binary>(s_measureUri, MeasurementLevel.Binary, "Test", "[subject] does something");
        var b = new Measure<Binary>(new Uri("https://schema-test.dseapi.app/testing/other"), MeasurementLevel.Binary, "Test", "[subject] does something");

        Assert.False(a.Equals(b));
        Assert.False(a.Equals((object)b));
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        var a = new Measure<Binary>(s_measureUri, MeasurementLevel.Binary, "Test", "[subject] does something");
        Assert.False(a.Equals("not a measure"));
    }

    [Fact]
    public void JsonRoundtrip_BehaviorFrequency_Polymorphic()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");

        Measure measure = new Measure<BehaviorFrequency>(uri, MeasurementLevel.Ordinal, "Test measure", "[subject] does something");

        var json = JsonSerializer.Serialize(measure, JsonSharedOptions.RelaxedJsonEscaping);

        var deserialized = JsonSerializer.Deserialize<Measure>(json, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.NotNull(deserialized);

        var measureDeserialized = Assert.IsType<Measure<BehaviorFrequency>>(deserialized);

        Assert.Equal(measure.Id, measureDeserialized.Id);
        Assert.Equal(measure.Uri, measureDeserialized.Uri);
        Assert.Equal(measure.MeasurementLevel, measureDeserialized.MeasurementLevel);
        Assert.Equal(measure.Name, measureDeserialized.Name);
        Assert.Equal(measure.Statement, measureDeserialized.Statement);
    }
}
