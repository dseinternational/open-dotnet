// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.


// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Values;

public class UriPathTests
{
    [Theory]
    [InlineData("/")]
    [InlineData("/a")]
    [InlineData("/a/")]
    [InlineData("/a/b")]
    [InlineData("a/b/")]
    [InlineData("/a/b/")]
    public void TryParse_WithInvalidValue_ShouldReturnFalseWithDefaultResult(string value)
    {
        // Arrange
        var expected = default(UriPath);

        // Act
        var actual = UriPath.TryParse(value, out var result);

        // Assert
        Assert.False(actual);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("home", "", "home")]
    [InlineData("home", "sub", "home/sub")]
    [InlineData("home/sub", "sub", "home/sub/sub")]
    public void Append(string path, string append, string expected)
    {
        var pathValue = (UriPath)path;
        var appendValue = (UriPath)append;
        var appended = pathValue.Append(appendValue);
        Assert.Equal(expected, appended.ToString());
    }

    [Theory]
    [InlineData("", "", "", "")]
    [InlineData("home", "", "", "home")]
    [InlineData("home", "sub", "sub", "home/sub/sub")]
    public void Append2(string path, string append1, string append2, string expected)
    {
        var pathValue = (UriPath)path;
        var append1Value = (UriPath)append1;
        var append2Value = (UriPath)append2;
        var appended = pathValue.Append(append1Value, append2Value);
        Assert.Equal(expected, appended.ToString());
    }

    [Theory]
    [InlineData("", "", "", "", "")]
    [InlineData("home", "", "", "", "home")]
    [InlineData("home", "sub", "sub", "sub", "home/sub/sub/sub")]
    public void Append3(string path, string append1, string append2, string append3, string expected)
    {
        var pathValue = (UriPath)path;
        var append1Value = (UriPath)append1;
        var append2Value = (UriPath)append2;
        var append3Value = (UriPath)append3;
        var appended = pathValue.Append(append1Value, append2Value, append3Value);
        Assert.Equal(expected, appended.ToString());
    }

    [Theory]
    [InlineData("home", "/home/")]
    [InlineData("home/sub", "/home/sub/")]
    public void ToAbsolutePath_ShouldCorrectlyFormat(string value, string expected)
    {
        // Arrange
        var path = UriPath.Parse(value, CultureInfo.InvariantCulture);

        // Act
        var absolutePath = path.ToAbsolutePath();

        // Assert
        Assert.Equal(expected, absolutePath);
    }

    [Fact]
    public void ToAbsolutePath_WithLongInput_ShouldCorrectlyFormat()
    {
        // Arrange
        var pathStr = string.Create(MemoryThresholds.StackallocCharThreshold + 1, 'a', (span, value) => span.Fill(value));
        var path = UriPath.Parse(pathStr, CultureInfo.InvariantCulture);

        // Act
        var absolutePath = path.ToAbsolutePath();

        // Assert
        Assert.Equal($"/{pathStr}/", absolutePath);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("/", "")]
    [InlineData("//", "")]
    [InlineData("home", "home")]
    [InlineData("home/SUB", "home/sub")]
    [InlineData("/HOME/", "home")]
    [InlineData("/HOME/sub", "home/sub")]
    [InlineData("/home/SUB/", "home/sub")]
    [InlineData("home/sub/", "home/sub")]
    public void TryParseSanitised_WithHandleablePath_ShouldReturnTrue(string path, string expected)
    {
        Assert.True(UriPath.TryParseSanitised(path, out var result));
        Assert.Equal(expected, result.ToString());
    }

    [Theory]
    // [InlineData("%home")]
    // [InlineData("HOME?")]
    // [InlineData("home/+SUB.html")]
    [InlineData("//home/sub/")]
    [InlineData("/home/sub//")]
    public void TryParseSanitisedWithInvalidPathShouldReturnFalse(string path)
    {
        Assert.False(UriPath.TryParseSanitised(path, out _));
    }
}
