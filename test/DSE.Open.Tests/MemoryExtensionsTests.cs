// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public partial class MemoryExtensionsTests
{
    [Theory]
    [InlineData("AAAAAAAAAAAAA", 'A', true)]
    [InlineData("AAAAAAAAAAAAa", 'A', false)]
    public void ContainsOnlyChar(string source, char value, bool expected)
    {
        var result = source.AsSpan().ContainsOnly(v => v == value);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("999999999999999", 9, true)]
    [InlineData("99999999990", 9, false)]
    public void ContainsOnlyInt32(string source, int value, bool expected)
    {
        Span<int> sourceSpan = source.Select(x => int.Parse(x.ToString(), CultureInfo.InvariantCulture)).ToArray();

        var result = sourceSpan.ContainsOnly(v => v == value);
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRemoveData))]
    public void RemoveChar(string source, char value, string expected)
    {
        Span<char> span = source.ToCharArray();
        var result = span.Remove(value);
        Assert.True(expected.AsSpan().SequenceEqual(result));
    }

    [Theory]
    [InlineData("", 0, "")]
    [InlineData("01234567890123456789", 0, "123456789123456789")]
    public void RemoveInt32(string source, int value, string expected)
    {
        Span<int> sourceSpan = source.Select(x => int.Parse(x.ToString(), CultureInfo.InvariantCulture)).ToArray();
        Span<int> expectedSpan = expected.Select(x => int.Parse(x.ToString(), CultureInfo.InvariantCulture)).ToArray();

        var result = sourceSpan.Remove(value);
        Assert.True(expectedSpan.SequenceEqual(result));
    }

    [Theory]
    [MemberData(nameof(StringRemoveData))]
    public void RemoveCharToBuffer(string source, char value, string expected)
    {
        ReadOnlySpan<char> span = source.ToCharArray();
        Span<char> buffer = stackalloc char[span.Length];
        var count = span.CopyExcluding(buffer, value);
        Assert.True(expected.AsSpan().SequenceEqual(buffer[..count]));
    }

    [Theory]
    [MemberData(nameof(StringRemoveData))]
    public void Remove(string source, char value, string expected)
    {
        // Arrange
        Span<char> span = source.ToCharArray();

        // Act
        var removed = span.Remove(value);

        // Assert
        Assert.Equal(expected, removed.ToString());
    }

    [Theory]
    [MemberData(nameof(StringReplaceData))]
    public void ReplaceChar(string source, char target, char replacement, string expected)
    {
        // Arrange
        Span<char> span = source.ToCharArray();

        // Act
        _ = span.Replace(c => c.Equals(target), replacement);

        // Assert
        Assert.Equal(expected, span.ToString());
    }

    [Fact]
    public void RemoveCharToBufferInsufficientBufferThrows()
    {
        _ = Assert.Throws<InvalidOperationException>(() =>
        {
            ReadOnlySpan<char> span = "123456789ABC".ToCharArray();
            Span<char> buffer = stackalloc char[8];
            _ = span.CopyExcluding(buffer, 'D');
        });
    }

    [Theory]
    [InlineData(".", "")]
    [InlineData("(", "")]
    [InlineData("().,;:'", "")]
    [InlineData("ABCDEFG", "ABCDEFG")]
    [InlineData("ABC.,:DEFG", "ABCDEFG")]
    [InlineData("::ABCDEFG", "ABCDEFG")]
    [InlineData("ABCDEFG...", "ABCDEFG")]
    public void TryCopyWhereNotPunctuation(string source, string expected)
    {
        ReadOnlySpan<char> span = source.ToCharArray();
        Span<char> buffer = stackalloc char[span.Length];
        var success = span.TryCopyWhereNotPunctuation(buffer, out var charsWritten);
        Assert.True(success);
        Assert.True(expected.AsSpan().SequenceEqual(buffer[..charsWritten]));
    }

    [Theory]
    [InlineData(" ", "")]
    [InlineData("           ", "")]
    [InlineData("ABCDEFG", "ABCDEFG")]
    [InlineData("ABC          DEFG", "ABCDEFG")]
    [InlineData("  ABCDEFG", "ABCDEFG")]
    [InlineData("ABCDEFG  ", "ABCDEFG")]
    public void TryCopyWhereNotWhitespace(string source, string expected)
    {
        ReadOnlySpan<char> span = source.ToCharArray();
        Span<char> buffer = stackalloc char[span.Length];
        var success = span.TryCopyWhereNotWhitespace(buffer, out var charsWritten);
        Assert.True(success);
        Assert.True(expected.AsSpan().SequenceEqual(buffer[..charsWritten]));
    }

    [Fact]
    public void Sum_WithSpan_Int32_ShouldComputeCorrectSum()
    {
        // Arrange
        ReadOnlySpan<string> span = ["1", "2", "3", "4", "5"];

        // Act
        var sum = span.Sum(x => int.Parse(x, CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(15, sum);
    }

    [Fact]
    public void Sum_WithSpan_Int64_ShouldComputeCorrectSum()
    {
        // Arrange
        ReadOnlySpan<string> span = ["1", "2", "3", "4", "5"];

        // Act
        var sum = span.Sum(x => long.Parse(x, CultureInfo.InvariantCulture));

        // Assert
        Assert.Equal(15, sum);
    }

    [Fact]
    public void Sum_WithSpan_ShouldComputeCorrectSum()
    {
        // Arrange
        ReadOnlySpan<string> span = ["1", "2", "3", "4", "5"];

        // Act
        var sum = span.Sum(x => x.Length);

        // Assert
        Assert.Equal(5, sum);
    }

    public static TheoryData<string, char, string> StringRemoveData => new()
    {
        { "", ' ', "" },
        { "", 'Z', "" },
        { "A", 'A', "" },
        { "ABA", 'A', "B" },
        { "CCC", 'A', "CCC" },
        { "This is a string", ' ', "Thisisastring" },
        { "1024-2563-9874", '-', "102425639874" },
    };

    public static TheoryData<string, char, char, string> StringReplaceData => new()
    {
        { "", ' ', 'A', "" },
        { "", 'Z', 'A', "" },
        { "A", 'A', 'B', "B" },
        { "ABA", 'A', 'B', "BBB" },
        { "CCC", 'A', 'B', "CCC" },
        { "This is a string", ' ', 'A', "ThisAisAaAstring" },
        { "1024-2563-9874", '-', 'A', "1024A2563A9874" },
    };
}
