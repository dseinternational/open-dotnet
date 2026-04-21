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

    [Fact]
    public void TryEncodeToHexUpper_BytesDestination_TooSmall_ReturnsFalse()
    {
        ReadOnlySpan<byte> data = stackalloc byte[] { 0xAB, 0xCD };
        var buffer = new byte[3]; // needs 4

        var result = HexConverter.TryEncodeToHexUpper(data, buffer, out var bytesWritten);

        Assert.False(result);
        Assert.Equal(0, bytesWritten);
    }

    [Fact]
    public void TryEncodeToHexLower_BytesDestination_TooSmall_ReturnsFalse()
    {
        ReadOnlySpan<byte> data = stackalloc byte[] { 0xAB, 0xCD };
        var buffer = new byte[3]; // needs 4

        var result = HexConverter.TryEncodeToHexLower(data, buffer, out var bytesWritten);

        Assert.False(result);
        Assert.Equal(0, bytesWritten);
    }

    [Fact]
    public void TryEncodeToHexUpper_CharsDestination_TooSmall_ReturnsFalse()
    {
        ReadOnlySpan<byte> data = stackalloc byte[] { 0xAB, 0xCD };
        var buffer = new char[3]; // needs 4

        var result = HexConverter.TryEncodeToHexUpper(data, buffer, out var charsWritten);

        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryEncodeToHexLower_CharsDestination_TooSmall_ReturnsFalse()
    {
        ReadOnlySpan<byte> data = stackalloc byte[] { 0xAB, 0xCD };
        var buffer = new char[3]; // needs 4

        var result = HexConverter.TryEncodeToHexLower(data, buffer, out var charsWritten);

        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryConvertFromUtf8_DestinationTooSmall_ReturnsFalse()
    {
        ReadOnlySpan<byte> source = "ABCD"u8; // decodes to 2 bytes
        var buffer = new byte[1]; // needs 2

        var result = HexConverter.TryConvertFromUtf8(source, buffer, out var bytesWritten);

        Assert.False(result);
        Assert.Equal(0, bytesWritten);
    }

    [Fact]
    public void TryConvertFromUtf8_OddSourceLength_ReturnsFalse()
    {
        ReadOnlySpan<byte> source = "ABC"u8;
        var buffer = new byte[2];

        var result = HexConverter.TryConvertFromUtf8(source, buffer, out var bytesWritten);

        Assert.False(result);
        Assert.Equal(0, bytesWritten);
    }

    [Fact]
    public void TryConvertFromUtf8_WithValidInput_ReturnsTrue()
    {
        ReadOnlySpan<byte> source = "ABCD"u8;
        var buffer = new byte[2];

        var result = HexConverter.TryConvertFromUtf8(source, buffer, out var bytesWritten);

        Assert.True(result);
        Assert.Equal(2, bytesWritten);
        Assert.Equal(0xAB, buffer[0]);
        Assert.Equal(0xCD, buffer[1]);
    }
}
