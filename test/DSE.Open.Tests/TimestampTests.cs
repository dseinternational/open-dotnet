// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class TimestampTests
{
    [Fact]
    public void Default_Initialized_Timestamps_Are_Equal()
    {
        var t1 = new Timestamp();
        var t2 = new Timestamp();
        Assert.True(t1.Equals(t2));
        Assert.True(t2.Equals(t1));
        Assert.Equal(t1, t2);
    }

    [Fact]
    public void Empty_Timestamps_Are_Equal()
    {
        var t1 = Timestamp.Empty;
        var t2 = Timestamp.Empty;
        Assert.True(t1.Equals(t2));
        Assert.True(t2.Equals(t1));
        Assert.Equal(t1, t2);
    }

    [Theory]
    [MemberData(nameof(EqualValues))]
    public void Equal_Value_Timestamps_Are_Equal(byte[] value1, byte[] value2)
    {
        var t1 = new Timestamp(value1);
        var t2 = new Timestamp(value2);
        Assert.True(t1.Equals(t2));
        Assert.True(t2.Equals(t1));
        Assert.Equal(t1, t2);
    }

    public static IEnumerable<object[]> EqualValues =>
        new List<object[]>
        {
            new object[] { new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 }, new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 } },
            new object[] { new byte[] { 0x6, 0x5, 0x2, 0x0, 0x0, 0x0, 0x0, 0x0 }, new byte[] { 0x6, 0x5, 0x2, 0x0, 0x0, 0x0, 0x0, 0x0 } },
            new object[] { new byte[] { 0x8, 0x4, 0x3, 0x9, 0x0, 0x0, 0x0, 0x0 }, new byte[] { 0x8, 0x4, 0x3, 0x9, 0x0, 0x0, 0x0, 0x0 } },
            new object[] { new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x6, 0x5, 0x2 }, new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x6, 0x5, 0x2 } },
            new object[] { new byte[] { 0x8, 0x4, 0x3, 0x9, 0x8, 0x4, 0x3, 0x9 }, new byte[] { 0x8, 0x4, 0x3, 0x9, 0x8, 0x4, 0x3, 0x9 } },
            new object[] { new byte[] { 0x8, 0x4, 0x7, 0x9, 0x2, 0x5, 0x3, 0x1 }, new byte[] { 0x8, 0x4, 0x7, 0x9, 0x2, 0x5, 0x3, 0x1 } },
        };

    [Fact]
    public void Parse_WithEmptySpan_ShouldReturnDefault()
    {
        var a = Timestamp.Parse(Span<char>.Empty, null);
        Assert.Equal(Timestamp.Empty, a);
    }

    [Fact]
    public void Parse_WithEmptyString_ShouldReturnDefault()
    {
        var a = Timestamp.Parse(string.Empty, null);
        Assert.Equal(Timestamp.Empty, a);
    }

    [Fact]
    public void Parse_WithNullString_ShouldThrowArgumentNull() => Assert.Throws<ArgumentNullException>(() => Timestamp.Parse(null!, null));

    [Fact]
    public void TryParse_WithEmptySpan_ShouldReturnTrueAndDefaultResult()
    {
        var success = Timestamp.TryParse(Span<char>.Empty, null, out var result);
        Assert.True(success);
        Assert.Equal(Timestamp.Empty, result);
    }

    [Fact]
    public void TryParse_WithNullString_ShouldReturnFalseAndDefaultResult()
    {
        var success = Timestamp.TryParse(null, null, out var result);
        Assert.False(success);
        Assert.Equal(Timestamp.Empty, result);
    }

    [Fact]
    public void TryParse_WithEmptyString_ShouldReturnTrueAndDefaultResult()
    {
        var success = Timestamp.TryParse(string.Empty, null, out var result);
        Assert.True(success);
        Assert.Equal(Timestamp.Empty, result);
    }

    [Fact]
    public void TryFormat_WithEmptyCode_ShouldReturnTrueWithNothingWritten()
    {
        // Arrange
        var code = Timestamp.Empty;
        var buffer = new char[1];

        // Act
        var success = code.TryFormat(buffer, out var charsWritten, default, default);

        // Assert
        Assert.True(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void EqualValuesAreEqual()
    {
        var v1 = Timestamp.Parse("MTIzNDU2Nzg=", null);
        var v2 = Timestamp.Parse(v1.ToString(), null);
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void EqualValuesAsObjectsAreEqual()
    {
        var v1 = (object)Timestamp.Parse("MTIzNDU2Nzg=", null);
        var v2 = (object)Timestamp.Parse(v1.ToString()!, null);
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void ToString_WithCode_ShouldReturnExpected()
    {
        // Arrange
        var code = Timestamp.Parse("MTIzNDU2Nzg=", null);

        // Act
        var result = code.ToString();

        // Assert
        Assert.Equal("MTIzNDU2Nzg=", result);
    }

    [Fact]
    public void New_WithInvalidCode_ShouldThrow()
    {
        // Act
        static void Act()
        {
            _ = new Timestamp("MTIzNDU2Nzg"u8);
        }

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Fact]
    public void GetBytes_ShouldReturnCorrectBytes()
    {
        // Arrange
        Span<byte> expected = stackalloc byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7 };
        var code = new Timestamp(expected);

        // Act
        var result = code.GetBytes();

        // Assert
        for (var i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], result[i]);
        }
    }

    [Fact]
    public void ToBase64String_ShouldReturnCorrectTimestamp()
    {
        // Arrange
        const string expected = "MTIzNDU2Nzg=";
        var code = Timestamp.Parse(expected, null);

        // Act
        var result = code.ToBase64String();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryCreate_WithValidBytes_ShouldCreate()
    {
        // Arrange
        Span<byte> bytes = stackalloc byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7 };

        // Act
        var success = Timestamp.TryCreate(bytes, out var result);

        // Assert
        Assert.True(success);
        var resultBytes = result.GetBytes();
        for (var i = 0; i < bytes.Length; i++)
        {
            Assert.Equal(bytes[i], resultBytes[i]);
        }
    }

    [Fact]
    public void TryCreate_WithEmpty_ShouldReturnEmpty()
    {
        // Arrange
        var bytes = Span<byte>.Empty;

        // Act
        var success = Timestamp.TryCreate(bytes, out var result);

        // Assert
        Assert.True(success);
        Assert.Equal(Timestamp.Empty, result);
    }

    [Fact]
    public void TryCreate_WithInvalidBytes_ShouldReturnFalse()
    {
        // Arrange
        Span<byte> bytes = stackalloc byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6 };

        // Act
        var success = Timestamp.TryCreate(bytes, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(Timestamp.Empty, result);
    }

    [Fact]
    public void GetHashCode_WithIdentical_ShouldEqual()
    {
        // Arrange
        const string expected = "MTIzNDU2Nzg=";
        var code1 = Timestamp.Parse(expected, null);
        var code2 = Timestamp.Parse(expected, null);

        // Act
        var hash1 = code1.GetHashCode();
        var hash2 = code2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }
}
