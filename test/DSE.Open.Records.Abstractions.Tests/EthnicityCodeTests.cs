// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Records.Abstractions.Tests;

public sealed class EthnicityCodeTests
{
    [Fact]
    public void ToString_WithValidCode_ShouldReturnString()
    {
        // Arrange
        var code = EthnicityCode.AsianBangladeshi;
        const string expected = "asian_bangladeshi";

        // Act
        var actual = code.ToString();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TryFormat_WithValidCode_ShouldReturnTrue()
    {
        // Arrange
        var code = EthnicityCode.AsianBangladeshi;
        const string expected = "asian_bangladeshi";
        Span<char> buffer = stackalloc char[64];

        // Act
        var result = code.TryFormat(buffer, out var charsWritten, null, null);

        // Assert
        Assert.True(result);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(buffer[..charsWritten], expected);
    }

    [Fact]
    public void TryFormat_WithBufferTooSmall_ShouldReturnFalse()
    {
        // Arrange
        var code = EthnicityCode.AsianBangladeshi;
        const string expected = "asian_bangladeshi";
        Span<char> buffer = stackalloc char[expected.Length - 1];

        // Act
        var result = code.TryFormat(buffer, out var charsWritten, null, null);

        // Assert
        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void Parse_WithValidCodeLabel_ShouldReturnCorrectCode()
    {
        // Arrange
        const string label = "asian_other";

        // Act
        var actual = EthnicityCode.Parse(label, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(EthnicityCode.AsianOther, actual);
    }

    [Fact]
    public void Parse_WithInvalidCodeLabel_ShouldThrowException()
    {
        // Arrange
        const string label = "invalid_code";

        // Assert
        _ = Assert.Throws<FormatException>(() => { _ = EthnicityCode.Parse(label, CultureInfo.InvariantCulture); });
    }

    [Fact]
    public void Parse_WithNullCodeLabel_ShouldThrowException()
    {
        // Arrange
        const string label = null!;

        // Assert
        _ = Assert.Throws<ArgumentNullException>(() =>
        {
            _ = EthnicityCode.Parse(label!, CultureInfo.InvariantCulture);
        });
    }

    [Fact]
    public void TryParse_WithValidCodeLabel_ShouldReturnTrue()
    {
        // Arrange
        const string label = "asian_other";

        // Act
        var actual = EthnicityCode.TryParse(label, out var result);

        // Assert
        Assert.True(actual);
        Assert.Equal(EthnicityCode.AsianOther, result);
    }

    [Fact]
    public void TryParse_WithInvalidCodeLabel_ShouldReturnFalse()
    {
        // Arrange
        const string label = "invalid_code";

        // Act
        var actual = EthnicityCode.TryParse(label, out _);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void TryParse_WithNullCodeLabel_ShouldReturnFalse()
    {
        // Arrange
        const string label = null!;

        // Act
        var actual = EthnicityCode.TryParse(label, out _);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void IsValidValue_WithUnknownValue_ShouldReturnFalse()
    {
        // Arrange
        const short value = 101;

        // Act
        var isValid = EthnicityCode.IsValidValue(value);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void IsValidValue_WithValueOutOfRange_ShouldReturnFalse()
    {
        // Arrange
        const short over = 1000;
        const short under = 100;

        // Act
        var overIsValid = EthnicityCode.IsValidValue(over);
        var underIsValid = EthnicityCode.IsValidValue(under);

        // Assert
        Assert.False(overIsValid);
        Assert.False(underIsValid);
    }

    [Fact]
    public void JsonRoundTrip()
    {
        // Arrange
        var code = EthnicityCode.OceaniaAndPacificAustralianAboriginal;

        // Act
        var json = JsonSerializer.Serialize(code);
        var deserialized = JsonSerializer.Deserialize<EthnicityCode>(json);

        // Assert
        Assert.Equal("""
                     "oceania_and_pacific_australian_aboriginal"
                     """,
            json);
        Assert.Equal(code, deserialized);
    }
}
