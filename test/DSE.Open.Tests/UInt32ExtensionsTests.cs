// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class UInt32ExtensionsTests
{
    [Theory]
    [InlineData(0u, 1)]
    [InlineData(1000u, 4)]
    [InlineData(123456789u, 9)]
    [InlineData(1234567890u, 10)]
    public void GetDigitCountReturnsCorrectCount(uint number, int digits)
    {
        Assert.Equal(digits, number.GetDigitCount());
    }
}
