// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public class MeasureTests
{
    [Fact]
    public void CreateSetsMeasureId()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(uri, "Test measure", "[subject] does something");
        var expectedId = MeasureIdHelper.GetId(uri);
        Assert.Equal(expectedId, measure.Id);
    }

    [Fact]
    public void CreateSetsUri()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(uri, "Test measure", "[subject] does something");
        Assert.Equal(uri, measure.Uri);
    }

    [Fact]
    public void CreateSetsMeasurementLevel()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(uri, "Test measure", "[subject] does something");
        Assert.Equal(MeasurementLevel.Binary, measure.MeasurementLevel);
    }

    [Fact]
    public void CreateSetsName()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(uri, "Test measure", "[subject] does something");
        Assert.Equal("Test measure", measure.Name);
    }

    [Fact]
    public void CreateSetsStatement()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(uri, "Test measure", "[subject] does something");
        Assert.Equal("[subject] does something", measure.Statement);
    }

    [Fact]
    public void EqualUrisAreEqualMeasures()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(uri, "Test measure", "[subject] does something");
        var measure2 = new FakeBinaryMeasure(uri, "Test measure 2", "[subject] does something 2");
        Assert.Equal(measure.Id, measure2.Id);
        Assert.Equal(measure, measure2);
        Assert.Equal(measure.GetHashCode(), measure2.GetHashCode());
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
    private FakeBinaryObservation(ulong id, uint measureId, long timestamp, bool value)
        : base(id, measureId, timestamp, value)
    {
    }
}

public sealed class FakeBinaryMeasure : Measure<FakeBinaryObservation, bool>
{
    public FakeBinaryMeasure(Uri uri, string name, string statement)
        : base(uri, MeasurementLevel.Binary, name, statement)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public FakeBinaryMeasure(uint id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

    public override FakeBinaryObservation CreateObservation(bool value, DateTimeOffset timestamp)
    {
        return new FakeBinaryObservation(this, timestamp, value);
    }
}
