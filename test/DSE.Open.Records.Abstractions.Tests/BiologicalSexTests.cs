// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Records.Tests;

public class BiologicalSexTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(BiologicalSex value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<BiologicalSex>(json);
        Assert.Equal(value, deserialized);
    }

    [Fact]
    public void IsValidValue_WithUnknownValue_ShouldReturnFalse()
    {
        // Arrange
        var value = AsciiString.Parse("notkwn", null);

        // Act
        var result = BiologicalSex.IsValidValue(value);

        // Assert
        Assert.False(result);
    }

    public static TheoryData<BiologicalSex> Values { get; } = new()
    {
        BiologicalSex.Female,
        BiologicalSex.Male,
    };
}
