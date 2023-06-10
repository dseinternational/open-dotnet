// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class AsciiCharTests
{
    [Theory]
    [MemberData(nameof(AsciiTestData.ValidAsciiCharBytes), MemberType = typeof(AsciiTestData))]
    public void Cast_from_valid_value(byte value)
    {
        var v = (AsciiChar)value;
        Assert.Equal(value, (byte)v);
    }

    [Theory]
    [InlineData('a', 'A', 0)]
    [InlineData('A', 'a', 0)]
    [InlineData('A', 'A', 0)]
    [InlineData('a', 'a', 0)]
    [InlineData('a', 'B', -1)]
    [InlineData('X', 'B', 1)]
    [InlineData('0', '0', 0)]
    [InlineData('1', '1', 0)]
    [InlineData('0', '1', -1)]
    [InlineData('1', '0', 1)]
    [InlineData('6', '2', 1)]
    [InlineData(':', ':', 0)]
    [InlineData('\t', '\t', 0)]
    [InlineData('?', '?', 0)]
    public void Compare_case_insensitive(char a, char b, int expected)
    {
        var c = ((AsciiChar)a).CompareToCaseInsensitive((AsciiChar)b);
        Assert.Equal(expected, c);
    }
}
