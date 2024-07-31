// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public class MeasureTests
{
    [Fact]
    public void CreateSetsUri()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(11684189, uri, "Test measure", "[subject] does something");
        Assert.Equal(uri, measure.Uri);
    }

    [Fact]
    public void CreateSetsMeasurementLevel()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(11684189, uri, "Test measure", "[subject] does something");
        Assert.Equal(MeasurementLevel.Binary, measure.MeasurementLevel);
    }

    [Fact]
    public void CreateSetsName()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(11684189, uri, "Test measure", "[subject] does something");
        Assert.Equal("Test measure", measure.Name);
    }

    [Fact]
    public void CreateSetsStatement()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(11684189, uri, "Test measure", "[subject] does something");
        Assert.Equal("[subject] does something", measure.Statement);
    }
}

public sealed record FakeBinaryObservation : Observation<bool>
{
    public FakeBinaryObservation(Measure measure, DateTimeOffset time, bool value)
        : base(measure, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private FakeBinaryObservation(ulong id, ulong measureId, long timestamp, bool value)
        : base(id, measureId, timestamp, value)
    {
    }
}

public sealed record FakeBinaryMeasure : Measure<FakeBinaryObservation, bool>
{
    public FakeBinaryMeasure(ulong id, Uri uri, string name, string statement)
        : base(id, uri, MeasurementLevel.Binary, name, statement)
    {
    }

    [JsonConstructor]
    public FakeBinaryMeasure(ulong id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

    public override FakeBinaryObservation CreateObservation(bool value, DateTimeOffset timestamp)
    {
        return new FakeBinaryObservation(this, timestamp, value);
    }
}
