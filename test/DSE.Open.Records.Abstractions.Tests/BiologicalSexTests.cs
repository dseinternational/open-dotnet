// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Records.Abstractions.Tests;

public class BiologicalSexTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(BiologicalSex value)
    {
        AssertJson.Roundtrip(value);
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
