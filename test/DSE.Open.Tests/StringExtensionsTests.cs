// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("         ", true, true)]
    [InlineData("         ", false, true)]
    [InlineData(" ", true, true)]
    [InlineData(" ", false, true)]
    [InlineData("        -", true, false)]
    [InlineData("        -", false, false)]
    [InlineData("", true, true)]
    [InlineData("", false, false)]
    public void ContainsOnlyWhitespace(string source, bool allowEmpty, bool expected)
    {
        var result = source.ContainsOnlyWhitespace(allowEmpty);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Abcdefghijklm", true, true)]
    [InlineData("Abcdefghijklm", false, true)]
    [InlineData("a", true, true)]
    [InlineData("a", false, true)]
    [InlineData("Abcdefghijklm-", true, false)]
    [InlineData("Abcdefghijklm-", false, false)]
    [InlineData("123", true, false)]
    [InlineData("123", false, false)]
    [InlineData("", true, true)]
    [InlineData("", false, false)]
    public void ContainsOnlyLetters(string source, bool allowEmpty, bool expected)
    {
        var result = source.ContainsOnlyLetters(allowEmpty);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Abcdefghijklm", true, true)]
    [InlineData("Abcdefghijklm", false, true)]
    [InlineData("Abcdefghijklm123456", true, true)]
    [InlineData("Abcdefghijklm123456", false, true)]
    [InlineData("a", true, true)]
    [InlineData("a", false, true)]
    [InlineData("Abcdefghijklm-", true, false)]
    [InlineData("Abcdefghijklm-", false, false)]
    [InlineData("123", true, true)]
    [InlineData("123", false, true)]
    [InlineData("", true, true)]
    [InlineData("", false, false)]
    public void ContainsOnlyLettersOrDigits(string source, bool allowEmpty, bool expected)
    {
        var result = source.ContainsOnlyLettersOrDigits(allowEmpty);
        Assert.Equal(expected, result);
    }

}
