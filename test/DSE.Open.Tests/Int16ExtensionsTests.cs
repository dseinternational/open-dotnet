// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class Int16ExtensionsTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1000, 4)]
    [InlineData(-1000, 4)]
    [InlineData(12345, 5)]
    [InlineData(-12345, 5)]
    public void GetDigitCountReturnsCorrectCount(short number, int digits) => Assert.Equal(digits, number.GetDigitCount());
}
