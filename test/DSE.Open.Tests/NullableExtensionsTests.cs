// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class NullableExtensionsTests
{
    [Fact]
    public void IsNullOrDefaultTrueForDefaultInt32()
    {
        int? i = 0;
        Assert.True(i.IsNullOrDefault());
    }

    [Fact]
    public void IsNullOrDefaultTrueForNullInt32()
    {
        int? i = null;
        Assert.True(i.IsNullOrDefault());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void IsNullOrDefaultFalseForNonZeroInt32(int value)
    {
        int? i = value;
        Assert.False(i.IsNullOrDefault());
    }

    [Fact]
    public void IsNullOrDefaultTrueForEmptyGuid()
    {
        Guid? value = Guid.Empty;
        Assert.True(value.IsNullOrDefault());
    }
}
