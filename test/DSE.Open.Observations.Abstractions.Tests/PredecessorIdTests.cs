// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Observations;

public sealed class PredecessorIdTests
{
    [Fact]
    public void New_WithValueTooLarge_ShouldThrow()
    {
        // Arrange
        const ulong value = PredecessorId.MaxIdValue + 1;

        // Act
        static void Act()
        {
            _ = new PredecessorId(value);
        }

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void New_WithValueTooSmall_ShouldThrow()
    {
        // Arrange
        const ulong value = PredecessorId.MinIdValue - 1;

        // Act
        static void Act()
        {
            _ = new PredecessorId(value);
        }

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void New_WithValidValue_ShouldCreateNew()
    {
        // Arrange
        const ulong value1 = PredecessorId.MaxIdValue;
        const ulong value2 = PredecessorId.MinIdValue;
        const ulong value3 = PredecessorId.MaxIdValue - ((PredecessorId.MaxIdValue - PredecessorId.MinIdValue) / 2);

        // Act
        var id1 = new PredecessorId(value1);
        var id2 = new PredecessorId(value2);
        var id3 = new PredecessorId(value3);

        // Assert
        Assert.Equal(value1, id1.ToUInt64());
        Assert.Equal(value2, id2.ToUInt64());
        Assert.Equal(value3, id3.ToUInt64());
    }

    [Fact]
    public void IsValidValue_WithValueTooLarge_ShouldReturnFalse()
    {
        // Arrange
        const ulong value = PredecessorId.MaxIdValue + 1;

        // Act
        var isValid = PredecessorId.IsValidValue(value);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void IsValidValue_WithValueTooSmall_ShouldReturnFalse()
    {
        // Arrange
        const ulong value = PredecessorId.MinIdValue - 1;

        // Act
        var isValid = PredecessorId.IsValidValue(value);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void IsValidValue_WithValidValues_ShouldReturnTrue()
    {
        // Arrange
        const ulong value1 = PredecessorId.MaxIdValue;
        const ulong value2 = PredecessorId.MinIdValue;
        const ulong value3 = PredecessorId.MaxIdValue - ((PredecessorId.MaxIdValue - PredecessorId.MinIdValue) / 2);

        // Act
        var isValid1 = PredecessorId.IsValidValue(value1);
        var isValid2 = PredecessorId.IsValidValue(value2);
        var isValid3 = PredecessorId.IsValidValue(value3);

        // Assert
        Assert.True(isValid1);
        Assert.True(isValid2);
        Assert.True(isValid3);
    }

    [Fact]
    public void IsValidValue_WithValidValues_Long_ShouldReturnTrue()
    {
        // Arrange
        const long value1 = (long)PredecessorId.MaxIdValue;
        const long value2 = (long)PredecessorId.MinIdValue;
        const long value3 = (long)(PredecessorId.MaxIdValue - ((PredecessorId.MaxIdValue - PredecessorId.MinIdValue) / 2));

        // Act
        var isValid1 = PredecessorId.IsValidValue(value1);
        var isValid2 = PredecessorId.IsValidValue(value2);
        var isValid3 = PredecessorId.IsValidValue(value3);

        // Assert
        Assert.True(isValid1);
        Assert.True(isValid2);
        Assert.True(isValid3);
    }

    [Fact]
    public void TryFromInt64_WithInvalidValue_ShouldReturnFalse()
    {
        // Arrange
        const long value = (long)(PredecessorId.MaxIdValue + 1);

        // Act
        var result = PredecessorId.TryFromInt64(value, out _);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryFromInt64_WithValidValue_ShouldReturnTrueAndValue()
    {
        // Arrange
        const long value = (long)PredecessorId.MaxIdValue;

        // Act
        var result = PredecessorId.TryFromInt64(value, out var id);

        // Assert
        Assert.True(result);
        Assert.Equal((ulong)value, id.ToUInt64());
    }

    [Fact]
    public void FromInt64_WithInvalidValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        const long value = (long)(PredecessorId.MinIdValue - 1);

        // Act
        static void Act()
        {
            _ = PredecessorId.FromInt64(value);
        }

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void SerializesToNumber()
    {
        var id = PredecessorId.FromInt64(667420532491);
        var json = JsonSerializer.Serialize(id);
        Assert.Equal("667420532491", json);
    }

    [Fact]
    public void JsonRoundtripDictionaryKey()
    {
        var dictionary = new Dictionary<PredecessorId, string>
        {
            { PredecessorId.GetRandomId(), "TestValue1" },
            { PredecessorId.GetRandomId(), "TestValue2" }
        };

        var json = JsonSerializer.Serialize(dictionary);
        var actual = JsonSerializer.Deserialize<Dictionary<PredecessorId, string>>(json);

        Assert.NotNull(actual);
        Assert.Equal(2, actual.Count);

        foreach (var kvp in dictionary)
        {
            Assert.True(actual.TryGetValue(kvp.Key, out var value));
            Assert.Equal(kvp.Value, value);
        }
    }

    [Fact]
    public void EqualForEqualUris()
    {
        var id1 = PredecessorId.FromMeasures(new Uri("urn:head"), new Uri("urn:head"));
        var id2 = PredecessorId.FromMeasures(new Uri("urn:head"), new Uri("urn:head"));

        Assert.Equal(id1, id2);
    }
}
