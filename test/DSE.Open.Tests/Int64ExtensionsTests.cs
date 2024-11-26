// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class Int64ExtensionsTests
{
    [Theory]
    [InlineData(0L, 1)]
    [InlineData(1000L, 4)]
    [InlineData(-1000L, 4)]
    [InlineData(123456789L, 9)]
    [InlineData(-123456789L, 9)]
    [InlineData(1234567890123L, 13)]
    [InlineData(-1234567890123L, 13)]
    public void GetDigitCountReturnsCorrectCount(long number, int digits)
    {
        Assert.Equal(digits, number.GetDigitCount());
    }
}
