// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using DSE.Open.Values;

namespace DSE.Open.Web.Tests;

public class AsciiPathExtensionsTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("a", "/a")]
    [InlineData("a/b/c/d", "/a/b/c/d")]
    public void CanConvertToPathString(string pathStr, string expected)
    {
        var asciiPath = new AsciiPath(pathStr);
        var pathString = asciiPath.ToPathString();
        Assert.Equal(expected, pathString.Value);
    }

    [Theory]
    [InlineData("", "/en-us")]
    [InlineData("a", "/en-us/a")]
    [InlineData("a/b/c/d", "/en-us/a/b/c/d")]
    public void CanConvertToLanguagePrefixedPathString(string pathStr, string expected)
    {
        var asciiPath = new AsciiPath(pathStr);
        var pathString = asciiPath.ToLanguagePrefixedPathString(LanguageTag.EnglishUs);
        Assert.Equal(expected, pathString.Value);
    }
}
