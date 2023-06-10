// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class AsciiCharSequenceTests
{
    [Theory]
    [MemberData(nameof(AsciiTestData.ValidAsciiCharSequenceStrings), MemberType = typeof(AsciiTestData))]
    public void Cast_from_valid_value(string value)
    {
        var v = (AsciiCharSequence)value;
        Assert.Equal(value, v.ToString());
    }

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
        var c = AsciiCharSequence.Parse(a).CompareToCaseInsensitive(AsciiCharSequence.Parse(b));
        Assert.Equal(expected, c);
    }
}
