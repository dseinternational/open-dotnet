// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Values.Tests;

public class AsciiPathTests
{
    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("home")]
    [InlineData("home/subdir")]
    [InlineData("home-page")]
    [InlineData("root/child/grandchild")]
    [InlineData("a/b/c/d/e/f/g/h")]
    public void CastStringWithValidStrings(string path)
    {
        var p = (AsciiPath)path;
        Assert.Equal(path, p.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("home")]
    [InlineData("home/subdir")]
    [InlineData("home-page")]
    [InlineData("root/child/grandchild")]
    [InlineData("a/b/c/d/e/f/g/h")]
    public void CastAsciiStringWithValidStrings(string path)
    {
        var p = (AsciiPath)AsciiString.Parse(path);
        Assert.Equal(path, p.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("home")]
    [InlineData("home/subdir")]
    [InlineData("home-page")]
    [InlineData("root/child/grandchild")]
    [InlineData("a/b/c/d/e/f/g/h")]
    public void ParseValidStrings(string path)
    {
        var p = AsciiPath.Parse(path);
        Assert.Equal(path, p.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("home")]
    [InlineData("home/subdir")]
    [InlineData("home-page")]
    [InlineData("root/child/grandchild")]
    [InlineData("a/b/c/d/e/f/g/h")]
    public void FromValueValidAsciiStrings(string path)
    {
        var p = AsciiPath.FromValue(AsciiString.Parse(path));
        Assert.Equal(path, p.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("home")]
    [InlineData("home/subdir")]
    [InlineData("home-page")]
    [InlineData("root/child/grandchild")]
    [InlineData("a/b/c/d/e/f/g/h")]
    public void IsValidValueString_returns_true_for_valid_paths(string path) => Assert.True(AsciiPath.IsValidValue(path));

    [Theory]
    [InlineData("/")]
    [InlineData("/home")]
    [InlineData("home/subdir/")]
    [InlineData("ungültig")]
    public void IsValidValue_returns_false_for_invalid_paths(string path) => Assert.False(AsciiPath.IsValidValue(path));

    [Fact]
    public void Serializes_to_string_value()
    {
        var json = JsonSerializer.Serialize(new AsciiPath("home"));
        Assert.Equal("\"home\"", json);
    }

    [Fact]
    public void Equals_AsciiPath()
    {
        Assert.True(((AsciiPath)"home").Equals((AsciiPath)"home"));
        Assert.True(AsciiPath.Empty.Equals(AsciiPath.Empty));
        Assert.True(AsciiPath.Empty.Equals((AsciiPath)string.Empty));
    }

    [Fact]
    public void Equality_operator()
    {
        Assert.True((AsciiPath)"home" == (AsciiPath)"home");
        Assert.True(AsciiPath.Empty == (AsciiPath)string.Empty);
    }

    [Fact]
    public void Inquality_operator()
    {
        Assert.True((AsciiPath)"home" != (AsciiPath)"homely");

        Assert.False((AsciiPath)"home" != (AsciiPath)"home");
        Assert.False(AsciiPath.Empty != (AsciiPath)string.Empty);
    }

    [Theory]
    [InlineData("", null)]
    [InlineData("home/sub", "home")]
    [InlineData("home/sub/sub", "home/sub")]
    public void GetParent_returns_parent(string path, string? parent)
    {
        var pathValue = (AsciiPath)path;
        var parentValue = pathValue.GetParent();
        Assert.Equal(parent, parentValue?.ToString());
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("home", "home")]
    [InlineData("home/sub", "home/sub")]
    [InlineData("/home/", "home")]
    [InlineData("/home/sub", "home/sub")]
    [InlineData("/home/sub/", "home/sub")]
    [InlineData("home/sub/", "home/sub")]
    public void TryParseSanitised_WithHandleablePath_ShouldReturnTrue(string path, string expected = "")
    {
        Assert.True(AsciiPath.TryParseSanitised(path, out var result));
        Assert.Equal(expected, result.ToString());
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("home", "sub", "home/sub")]
    [InlineData("home/sub", "sub", "home/sub/sub")]
    public void Append(string path, string append, string expected)
    {
        var pathValue = (AsciiPath)path;
        var appendValue = (AsciiPath)append;
        var appended = pathValue.Append(appendValue);
        Assert.Equal(expected, appended.ToString());
    }

    [Theory]
    [InlineData("", "", "", "")]
    [InlineData("home", "sub", "sub", "home/sub/sub")]
    public void Append2(string path, string append1, string append2, string expected)
    {
        var pathValue = (AsciiPath)path;
        var append1Value = (AsciiPath)append1;
        var append2Value = (AsciiPath)append2;
        var appended = pathValue.Append(append1Value, append2Value);
        Assert.Equal(expected, appended.ToString());
    }

    [Theory]
    [InlineData("", "", "", "", "")]
    [InlineData("home", "sub", "sub", "sub", "home/sub/sub/sub")]
    public void Append3(string path, string append1, string append2, string append3, string expected)
    {
        var pathValue = (AsciiPath)path;
        var append1Value = (AsciiPath)append1;
        var append2Value = (AsciiPath)append2;
        var append3Value = (AsciiPath)append3;
        var appended = pathValue.Append(append1Value, append2Value, append3Value);
        Assert.Equal(expected, appended.ToString());
    }
}
