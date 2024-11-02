// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public class MeasureTests
{
    [Fact]
    public void CreateSetsUri()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");
        Assert.Equal(uri, measure.Uri);
    }

    [Fact]
    public void CreateSetsMeasurementLevel()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");
        Assert.Equal(MeasurementLevel.Binary, measure.MeasurementLevel);
    }

    [Fact]
    public void CreateSetsName()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");
        Assert.Equal("Test measure", measure.Name);
    }

    [Fact]
    public void CreateSetsStatement()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");

        Assert.Equal("[subject] does something", measure.Statement);
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new FakeBinaryMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");

        // Act
#pragma warning disable CA1508 // Always false - testing this!
        var result = measure.Equals(null);
#pragma warning restore CA1508

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithEqualMeasureId_ShouldReturnTrue()
    {
        // Arrange
        var sharedId = MeasureId.GetRandomId();

        var uri1 = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure1 = new FakeBinaryMeasure(sharedId, uri1, "Test measure 1", "[subject] does something 1");

        var uri2 = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure2 = new FakeBinaryMeasure(sharedId, uri2, "Test measure 2", "[subject] does something 2");

        // Act
        var result = measure1.Equals(measure2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WithDifferentMeasureId_ShouldReturnFalse()
    {
        // Arrange
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure1 = new FakeBinaryMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");
        var measure2 = new FakeBinaryMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");

        // Act
        var result = measure1.Equals(measure2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetHashCode_WithSameId_ShouldReturnSameHashCode()
    {
        // Arrange
        var sharedId = MeasureId.GetRandomId();

        var uri1 = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure1 = new FakeBinaryMeasure(sharedId, uri1, "Test measure 1", "[subject] does something 1");

        var uri2 = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure2 = new FakeBinaryMeasure(sharedId, uri2, "Test measure 2", "[subject] does something 2");

        // Act
        var hash1 = measure1.GetHashCode();
        var hash2 = measure2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_WithDifferentIds_ShouldReturnFalse()
    {
        // Arrange
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure1 = new FakeBinaryMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");
        var measure2 = new FakeBinaryMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");

        // Act
        var hash1 = measure1.GetHashCode();
        var hash2 = measure2.GetHashCode();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }
}

public sealed record FakeBinaryObservation : Observation<bool>
{
}

public sealed record FakeBinaryMeasure : Measure<FakeBinaryObservation, bool>
{
    [SetsRequiredMembers]
    public FakeBinaryMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.Binary;
        Name = name;
        Statement = statement;
    }

    public override FakeBinaryObservation CreateObservation(bool value, DateTimeOffset timestamp)
    {
        return new FakeBinaryObservation
        {
            Time = timestamp,
            MeasureId = Id,
            Value = value
        };
    }
}
