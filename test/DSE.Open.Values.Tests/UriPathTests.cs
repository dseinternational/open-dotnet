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
    [InlineData("A")]
    [InlineData("HOME")]
    [InlineData("home/sub_dir")]
    [InlineData("home.sub")]
    [InlineData("home~sub")]
    [InlineData("home+sub")]
    [InlineData("home sub")]
    [InlineData("über")]
    public void TryParse_WithInvalidCharacters_ShouldReturnFalseWithDefaultResult(string value)
    {
        var actual = UriPath.TryParse(value, out var result);

        Assert.False(actual);
        Assert.Equal(default, result);
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
    [InlineData("", null)]
    [InlineData("home", null)]
    [InlineData("home/sub", "home")]
    [InlineData("home/sub/child", "home/sub")]
    public void GetParent_returns_parent_path(string path, string? expected)
    {
        var pathValue = (UriPath)path;
        var parent = pathValue.GetParent();

        Assert.Equal(expected, parent?.ToString());
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("home", 1)]
    [InlineData("home/sub", 2)]
    [InlineData("home/sub/child", 3)]
    public void GetSegmentCount_returns_number_of_path_segments(string path, int expected)
    {
        var pathValue = (UriPath)path;

        Assert.Equal(expected, pathValue.GetSegmentCount());
    }

    [Theory]
    [InlineData("home/sub/child", 0, "home/sub/child")]
    [InlineData("home/sub/child", 4, "sub/child")]
    [InlineData("home/sub/child", 5, "sub/child")]
    [InlineData("home/sub/child", 8, "child")]
    [InlineData("home/sub/child", 9, "child")]
    public void Subpath_removes_leading_separator_from_result(string path, int startIndex, string expected)
    {
        var pathValue = (UriPath)path;

        Assert.Equal(expected, pathValue.Subpath(startIndex).ToString());
        Assert.Equal(expected, pathValue.Substring(startIndex));
    }

    [Fact]
    public void StartsWith_and_EndsWith_return_expected_results()
    {
        var path = (UriPath)"home/sub";

        Assert.True(path.StartsWith("home".AsSpan()));
        Assert.True(path.StartsWith((UriPath)"home"));
        Assert.True(path.StartsWith('h'));
        Assert.False(path.StartsWith('s'));
        Assert.True(path.EndsWith("sub".AsSpan()));
        Assert.True(path.EndsWith((UriPath)"sub"));
        Assert.True(path.EndsWith('b'));
        Assert.False(path.EndsWith('h'));
    }

    [Fact]
    public void FromUriAsciiPath_WithValidLowercasePath_ShouldCreateEquivalentUriPath()
    {
        var asciiPath = (UriAsciiPath)"home/sub";

        var path = UriPath.FromUriAsciiPath(asciiPath);

        Assert.Equal((UriPath)"home/sub", path);
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
    [InlineData("%home")]
    [InlineData("HOME?")]
    [InlineData("home/+SUB.html")]
    [InlineData("//home/sub/")]
    [InlineData("/home/sub//")]
    public void TryParseSanitisedWithInvalidPathShouldReturnFalse(string path)
    {
        Assert.False(UriPath.TryParseSanitised(path, out _));
    }
}
