// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class AsciiStringTests
{
    [Theory]
    [MemberData(nameof(AsciiTestData.ValidAsciiCharSequenceStrings), MemberType = typeof(AsciiTestData))]
    public void Cast_from_valid_value(string value)
    {
        var v = (AsciiString)value;
        Assert.Equal(value, v.ToString());
    }

    [Theory]
    [InlineData("£")]
    [InlineData("•")]
    [InlineData("abcdedfgsdjnhgfdlui34987trglih¦")]
    public void TryParse_returns_false_for_non_ascii_input(string value) => Assert.False(AsciiString.TryParse(value, out _));

    [Theory]
    [InlineData("a", "A", 0)]
    [InlineData("abcdEFG", "ABCdefg", 0)]
    [InlineData("abcdEFGb", "ABCdefg", 1)]
    [InlineData("100000", "100000", 0)]
    [InlineData("100000", "1000000", -1)]
    [InlineData("100000", "10000000", -2)]
    [InlineData("1000000", "100000", 1)]
    [InlineData("A", "a", 0)]
    [InlineData("A", "A", 0)]
    [InlineData("a", "a", 0)]
    [InlineData("a", "B", -1)]
    [InlineData("X", "B", 1)]
    [InlineData(":", ":", 0)]
    [InlineData("\t", "\t", 0)]
    public void Compare_case_insensitive(string a, string b, int expected)
    {
        var c = AsciiString.Parse(a).CompareToCaseInsensitive(AsciiString.Parse(b));
        Assert.Equal(expected, c);
    }

    [Theory]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
    public void ToString_returns_string(string value, string expected)
    {
        var c = AsciiString.Parse(value);
        Assert.Equal(expected, c.ToString());
    }

    [Theory]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz")]
    public void ToStringLower_returns_lower_string(string value, string expected)
    {
        var c = AsciiString.Parse(value);
        Assert.Equal(expected, c.ToStringLower());
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
    public void ToStringUpper_returns_upper_string(string value, string expected)
    {
        var c = AsciiString.Parse(value);
        Assert.Equal(expected, c.ToStringUpper());
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "xyz")]
    public void EndsWith(string value, string test)
    {
        var c = AsciiString.Parse(value);
        Assert.True(c.EndsWith(AsciiString.Parse(test)));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abc")]
    public void StartsWith(string value, string test)
    {
        var c = AsciiString.Parse(value);
        Assert.True(c.StartsWith(AsciiString.Parse(test)));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "z")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "vwxyz")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "bcdefghijklmnopqrstuvwxyz")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
    public void EndsWith_string(string value, string test)
    {
        var c = AsciiString.Parse(value);
        Assert.True(c.EndsWith(test));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abc")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
    public void StartsWith_string(string value, string test)
    {
        var c = AsciiString.Parse(value);
        Assert.True(c.StartsWith(test));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz")]
    public void Indexer_returns_indexed_asciichar(string value)
    {
        var c = AsciiString.Parse(value);

        for (var i = 0; i < value.Length; i++)
        {
            Assert.Equal(value[i], c[i]);
        }
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz")]
    public void GetEnumerator_returns_enumerator_that_enumerates(string value)
    {
        var c = AsciiString.Parse(value);

        var i = 0;
        foreach (var item in c)
        {
            Assert.Equal(value[i], item);
            i++;
        }
    }
}
