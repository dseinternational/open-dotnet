// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class Int32ExtensionsTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1000, 4)]
    [InlineData(-1000, 4)]
    [InlineData(123456789, 9)]
    [InlineData(-123456789, 9)]
    public void GetDigitCountReturnsCorrectCount(int number, int digits)
    {
        Assert.Equal(digits, number.GetDigitCount());
    }
}
