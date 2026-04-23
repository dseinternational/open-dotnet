// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class ComparisonsTests
{
    [Theory]
    [InlineData(1, 2, 2)]
    [InlineData(2, 1, 2)]
    [InlineData(-5, 5, 5)]
    [InlineData(int.MinValue, int.MaxValue, int.MaxValue)]
    [InlineData(7, 7, 7)]
    public void Max_Int_ReturnsGreater(int a, int b, int expected)
    {
        Assert.Equal(expected, Comparisons.Max(a, b));
    }

    [Theory]
    [InlineData(1, 2, 1)]
    [InlineData(2, 1, 1)]
    [InlineData(-5, 5, -5)]
    [InlineData(int.MinValue, int.MaxValue, int.MinValue)]
    [InlineData(7, 7, 7)]
    public void Min_Int_ReturnsLesser(int a, int b, int expected)
    {
        Assert.Equal(expected, Comparisons.Min(a, b));
    }

    [Fact]
    public void Max_String_ReturnsLexicographicallyGreater()
    {
        Assert.Equal("banana", Comparisons.Max("apple", "banana"));
    }

    [Fact]
    public void Min_String_ReturnsLexicographicallyLesser()
    {
        Assert.Equal("apple", Comparisons.Min("apple", "banana"));
    }

    [Fact]
    public void Max_EqualValues_ReturnsFirstArg()
    {
        var a = 10;
        var b = 10;

        Assert.Equal(a, Comparisons.Max(a, b));
    }

    [Fact]
    public void Min_EqualValues_ReturnsFirstArg()
    {
        var a = 10;
        var b = 10;

        Assert.Equal(a, Comparisons.Min(a, b));
    }
}
