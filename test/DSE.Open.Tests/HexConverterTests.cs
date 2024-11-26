// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;

namespace DSE.Open;

public class HexConverterTests
{
    [Fact]
    public void TryEncodeToHex_WithValidBytesData_ShouldReturnTrue_Upper()
    {
        // Arrange
        ReadOnlySpan<byte> data = stackalloc byte[] { 0x0 };
        var buffer = new byte[2];

        // Act
        var result = HexConverter.TryEncodeToHexUpper(data, buffer, out var bytesWritten);

        // Assert
        Assert.True(result);
        Assert.Equal(2, bytesWritten);
        Assert.Equal("00", Encoding.ASCII.GetString(buffer));
    }

    [Fact]
    public void TryEncodeToHex_WithValidCharsData_ShouldReturnTrue_Upper()
    {
        // Arrange
        ReadOnlySpan<byte> data = stackalloc byte[] { 0x0 };
        var buffer = new char[2];

        // Act
        var result = HexConverter.TryEncodeToHexUpper(data, buffer, out var charsWritten);

        // Assert
        Assert.True(result);
        Assert.Equal(2, charsWritten);
        Assert.Equal("00", new(buffer));
    }

    [Fact]
    public void TryEncodeToHex_WithValidBytesData_ShouldReturnTrue_Lower()
    {
        // Arrange
        ReadOnlySpan<byte> data = stackalloc byte[] { 0x0 };
        var buffer = new byte[2];

        // Act
        var result = HexConverter.TryEncodeToHexLower(data, buffer, out var bytesWritten);

        // Assert
        Assert.True(result);
        Assert.Equal(2, bytesWritten);
        Assert.Equal("00", Encoding.ASCII.GetString(buffer));
    }

    [Fact]
    public void TryEncodeToHex_WithValidCharsData_ShouldReturnTrue_Lower()
    {
        // Arrange
        ReadOnlySpan<byte> data = stackalloc byte[] { 0x0 };
        var buffer = new char[2];

        // Act
        var result = HexConverter.TryEncodeToHexLower(data, buffer, out var charsWritten);

        // Assert
        Assert.True(result);
        Assert.Equal(2, charsWritten);
        Assert.Equal("00", new(buffer));
    }
}
