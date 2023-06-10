// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class AsciiTests
{
    [Theory]
    [InlineData('A', 'a')]
    [InlineData('Z', 'z')]
    [InlineData('.', '.')]
    [InlineData('1', '1')]
    public void ToLower(byte b, byte expected) => Assert.Equal(expected, AsciiChar.ToLower(b));

    [Theory]
    [InlineData('a', 'A')]
    [InlineData('z', 'Z')]
    [InlineData('.', '.')]
    [InlineData('1', '1')]
    public void ToUpper(byte b, byte expected) => Assert.Equal(expected, AsciiChar.ToUpper(b));
}
