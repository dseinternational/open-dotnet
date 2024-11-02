// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class MeasureIdTests
{
    [Fact]
    public void New_WithValueTooLarge_ShouldThrow()
    {
        // Arrange
        const ulong value = MeasureId.MaxIdValue + 1;

        // Act
        static void Act() => _ = new MeasureId(value);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void New_WithValueTooSmall_ShouldThrow()
    {
        // Arrange
        const ulong value = MeasureId.MinIdValue - 1;

        // Act
        static void Act() => _ = new MeasureId(value);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void New_WithValidValue_ShouldCreateNew()
    {
        // Arrange
        const ulong value1 = MeasureId.MaxIdValue;
        const ulong value2 = MeasureId.MinIdValue;
        const ulong value3 = MeasureId.MaxIdValue - ((MeasureId.MaxIdValue - MeasureId.MinIdValue) / 2);

        // Act
        var id1 = new MeasureId(value1);
        var id2 = new MeasureId(value2);
        var id3 = new MeasureId(value3);

        // Assert
        Assert.Equal(value1, id1.ToUInt64());
        Assert.Equal(value2, id2.ToUInt64());
        Assert.Equal(value3, id3.ToUInt64());
    }

    [Fact]
    public void IsValidValue_WithValueTooLarge_ShouldReturnFalse()
    {
        // Arrange
        const ulong value = MeasureId.MaxIdValue + 1;

        // Act
        var isValid = MeasureId.IsValidValue(value);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void IsValidValue_WithValueTooSmall_ShouldReturnFalse()
    {
        // Arrange
        const ulong value = MeasureId.MinIdValue - 1;

        // Act
        var isValid = MeasureId.IsValidValue(value);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void IsValidValue_WithValidValues_ShouldReturnTrue()
    {
        // Arrange
        const ulong value1 = MeasureId.MaxIdValue;
        const ulong value2 = MeasureId.MinIdValue;
        const ulong value3 = MeasureId.MaxIdValue - ((MeasureId.MaxIdValue - MeasureId.MinIdValue) / 2);

        // Act
        var isValid1 = MeasureId.IsValidValue(value1);
        var isValid2 = MeasureId.IsValidValue(value2);
        var isValid3 = MeasureId.IsValidValue(value3);

        // Assert
        Assert.True(isValid1);
        Assert.True(isValid2);
        Assert.True(isValid3);
    }

    [Fact]
    public void IsValidValue_WithValidValues_Long_ShouldReturnTrue()
    {
        // Arrange
        const long value1 = (long)MeasureId.MaxIdValue;
        const long value2 = (long)MeasureId.MinIdValue;
        const long value3 = (long)(MeasureId.MaxIdValue - ((MeasureId.MaxIdValue - MeasureId.MinIdValue) / 2));

        // Act
        var isValid1 = MeasureId.IsValidValue(value1);
        var isValid2 = MeasureId.IsValidValue(value2);
        var isValid3 = MeasureId.IsValidValue(value3);

        // Assert
        Assert.True(isValid1);
        Assert.True(isValid2);
        Assert.True(isValid3);
    }

    [Fact]
    public void TryFromInt64_WithInvalidValue_ShouldReturnFalse()
    {
        // Arrange
        const long value = (long)(MeasureId.MaxIdValue + 1);

        // Act
        var result = MeasureId.TryFromInt64(value, out _);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryFromInt64_WithValidValue_ShouldReturnTrueAndValue()
    {
        // Arrange
        const long value = (long)MeasureId.MaxIdValue;

        // Act
        var result = MeasureId.TryFromInt64(value, out var id);

        // Assert
        Assert.True(result);
        Assert.Equal((ulong)value, id.ToUInt64());
    }

    [Fact]
    public void FromInt64_WithInvalidValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        const long value = (long)(MeasureId.MinIdValue - 1);

        // Act
        static void Act() => _ = MeasureId.FromInt64(value);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void SerializesToNumber()
    {
        var id = MeasureId.FromInt64(667420532491);
        var json = JsonSerializer.Serialize(id);
        Assert.Equal("667420532491", json);
    }
}
