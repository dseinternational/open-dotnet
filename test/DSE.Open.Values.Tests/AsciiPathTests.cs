// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Values.Tests;

public class AsciiPathTests
{
    [Theory]
    [InlineData("")]
    [InlineData("home")]
    [InlineData("home/subdir")]
    [InlineData("home-page")]
    public void IsValid_returns_true_for_valid_paths(string path) => Assert.True(AsciiPath.IsValidValue(path));

    [Theory]
    [InlineData("/")]
    [InlineData("/home")]
    [InlineData("home/subdir/")]
    [InlineData("ungültig")]
    public void IsValid_returns_false_for_invalid_paths(string path) => Assert.False(AsciiPath.IsValidValue(path));

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
}
