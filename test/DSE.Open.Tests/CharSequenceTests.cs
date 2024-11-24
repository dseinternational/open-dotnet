// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class CharSequenceTests
{
    [Theory]
    [InlineData("Hello", "Hello", StringComparison.Ordinal)]
    [InlineData("Hello 123", "Hello 123", StringComparison.Ordinal)]
    [InlineData("Hello", "hello", StringComparison.OrdinalIgnoreCase)]
    [InlineData("Hello", "Hello", StringComparison.InvariantCulture)]
    [InlineData("Hello 123", "Hello 123", StringComparison.InvariantCulture)]
    [InlineData("Hello", "hello", StringComparison.InvariantCultureIgnoreCase)]
    public void EqualsReturnsTrueWhenEqualCharSequence(string value1, string value2, StringComparison stringComparison)
    {
        var c1 = CharSequence.ParseInvariant(value1);
        var c2 = CharSequence.ParseInvariant(value2);
        Assert.True(c1.Equals(c2, stringComparison));
    }

    [Theory]
    [InlineData("Hello", "Hello", StringComparison.Ordinal)]
    [InlineData("Hello 123", "Hello 123", StringComparison.Ordinal)]
    [InlineData("Hello", "hello", StringComparison.OrdinalIgnoreCase)]
    [InlineData("Hello", "Hello", StringComparison.InvariantCulture)]
    [InlineData("Hello 123", "Hello 123", StringComparison.InvariantCulture)]
    [InlineData("Hello", "hello", StringComparison.InvariantCultureIgnoreCase)]
    public void EqualsReturnsTrueWhenEqualString(string value1, string value2, StringComparison stringComparison)
    {
        var c1 = CharSequence.ParseInvariant(value1);
        Assert.True(c1.Equals(value2, stringComparison));
    }

    [Fact]
    public void GetRepeatableHashCode_ReturnsExpectedValue()
    {
        var c = (CharSequence)"Hello";
        Assert.Equal(6936088994997224939u, c.GetRepeatableHashCode());
    }
}
