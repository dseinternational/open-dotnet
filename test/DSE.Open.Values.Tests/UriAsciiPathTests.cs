// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;
using System.Text.Json;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Text.Json;

namespace DSE.Open.Values;

public class UriAsciiPathTests
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
        var p = (UriAsciiPath)path;
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
        var p = (UriAsciiPath)AsciiString.Parse(path, CultureInfo.InvariantCulture);
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
        var p = UriAsciiPath.Parse(path, CultureInfo.InvariantCulture);
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
    public void ParseValidBytes(string path)
    {
        var bytes = Encoding.UTF8.GetBytes(path);
        var p = UriAsciiPath.Parse(bytes, null);
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
        var p = UriAsciiPath.FromValue(AsciiString.Parse(path, CultureInfo.InvariantCulture));
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
    public void IsValidValueString_returns_true_for_valid_paths(string path)
    {
        Assert.True(UriAsciiPath.IsValidValue(path));
    }

    [Fact]
    public void IsValidValueString_WithByteStackallocBoundaryLength_ShouldReturnTrue()
    {
        var path = string.Create(MemoryThresholds.StackallocCharThreshold + 1, 'a', (span, value) => span.Fill(value));

        Assert.True(UriAsciiPath.IsValidValue(path));
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/home")]
    [InlineData("home/subdir/")]
    [InlineData("ungültig")]
    [InlineData("home/sub+dir")]
    public void IsValidValue_returns_false_for_invalid_paths(string path)
    {
        Assert.False(UriAsciiPath.IsValidValue(path));
    }

    [Fact]
    public void Serializes_to_string_value()
    {
        var json = JsonSerializer.Serialize(new UriAsciiPath("home"));
        Assert.Equal("\"home\"", json);
    }

    [Fact]
    public void Equals_UriAsciiPath()
    {
        Assert.True(((UriAsciiPath)"home").Equals((UriAsciiPath)"home"));
        Assert.True(UriAsciiPath.Empty.Equals(UriAsciiPath.Empty));
        Assert.True(UriAsciiPath.Empty.Equals((UriAsciiPath)string.Empty));
    }

    [Fact]
    public void Equality_operator()
    {
        Assert.True((UriAsciiPath)"home" == (UriAsciiPath)"home");
        Assert.True(UriAsciiPath.Empty == (UriAsciiPath)string.Empty);
    }

    [Fact]
    public void Inquality_operator()
    {
        Assert.True((UriAsciiPath)"home" != (UriAsciiPath)"homely");

        Assert.False((UriAsciiPath)"home" != (UriAsciiPath)"home");
        Assert.False(UriAsciiPath.Empty != (UriAsciiPath)string.Empty);
    }

    [Theory]
    [InlineData("", null)]
    [InlineData("home/sub", "home")]
    [InlineData("home/sub/sub", "home/sub")]
    public void GetParent_returns_parent(string path, string? parent)
    {
        var pathValue = (UriAsciiPath)path;
        var parentValue = pathValue.GetParent();
        Assert.Equal(parent, parentValue?.ToString());
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("home", 1)]
    [InlineData("home/sub", 2)]
    [InlineData("home/sub/child", 3)]
    public void GetSegmentCount_returns_number_of_path_segments(string path, int expected)
    {
        var pathValue = (UriAsciiPath)path;

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
        var pathValue = (UriAsciiPath)path;

        Assert.Equal(expected, pathValue.Subpath(startIndex).ToString());
        Assert.Equal(expected, pathValue.Substring(startIndex));
    }

    [Fact]
    public void StartsWith_and_EndsWith_return_expected_results()
    {
        var path = (UriAsciiPath)"Home/Sub-Dir_1~x.y";
        var homeBytes = Encoding.ASCII.GetBytes("Home");
        var suffixBytes = Encoding.ASCII.GetBytes("x.y");

        Assert.True(path.StartsWith("Home"));
        Assert.True(path.StartsWith("Home".AsSpan()));
        Assert.True(path.StartsWith(homeBytes));
        Assert.True(path.StartsWith((UriAsciiPath)"Home"));
        Assert.True(path.EndsWith("x.y"));
        Assert.True(path.EndsWith("x.y".AsSpan()));
        Assert.True(path.EndsWith(suffixBytes));
        Assert.True(path.EndsWith((UriAsciiPath)"x.y"));
        Assert.False(path.StartsWith("home"));
        Assert.False(path.EndsWith("X.Y"));
    }

    [Fact]
    public void Case_conversion_methods_return_expected_values()
    {
        var path = (UriAsciiPath)"Home/Sub-Dir_1~x.y";

        Assert.Equal("home/sub-dir_1~x.y", path.ToLower().ToString());
        Assert.Equal("HOME/SUB-DIR_1~X.Y", path.ToUpper().ToString());
        Assert.Equal("home/sub-dir_1~x.y", path.ToStringLower());
        Assert.Equal("HOME/SUB-DIR_1~X.Y", path.ToStringUpper());
        Assert.True(path.EqualsIgnoreCase((UriAsciiPath)"home/sub-dir_1~X.Y"));
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
        Assert.True(UriAsciiPath.TryParseSanitised(path, out var result));
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
        Assert.False(UriAsciiPath.TryParseSanitised(path, out _));
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("home", "", "home")]
    [InlineData("home", "sub", "home/sub")]
    [InlineData("home/sub", "sub", "home/sub/sub")]
    public void Append(string path, string append, string expected)
    {
        var pathValue = (UriAsciiPath)path;
        var appendValue = (UriAsciiPath)append;
        var appended = pathValue.Append(appendValue);
        Assert.Equal(expected, appended.ToString());
    }

    [Theory]
    [InlineData("", "", "", "")]
    [InlineData("home", "", "", "home")]
    [InlineData("home", "sub", "sub", "home/sub/sub")]
    public void Append2(string path, string append1, string append2, string expected)
    {
        var pathValue = (UriAsciiPath)path;
        var append1Value = (UriAsciiPath)append1;
        var append2Value = (UriAsciiPath)append2;
        var appended = pathValue.Append(append1Value, append2Value);
        Assert.Equal(expected, appended.ToString());
    }

    [Theory]
    [InlineData("", "", "", "", "")]
    [InlineData("home", "", "", "", "home")]
    [InlineData("home", "sub", "sub", "sub", "home/sub/sub/sub")]
    public void Append3(string path, string append1, string append2, string append3, string expected)
    {
        var pathValue = (UriAsciiPath)path;
        var append1Value = (UriAsciiPath)append1;
        var append2Value = (UriAsciiPath)append2;
        var append3Value = (UriAsciiPath)append3;
        var appended = pathValue.Append(append1Value, append2Value, append3Value);
        Assert.Equal(expected, appended.ToString());
    }

    [Fact]
    public void ToUriPath_WithEmpty_ShouldReturnEmptyUriPath()
    {
        // Arrange
        var path = UriAsciiPath.Empty;

        // Act
        var uriPath = path.ToUriPath();

        // Assert
        Assert.Equal(UriPath.Empty, uriPath);
    }

    [Theory]
    [InlineData("home")]
    [InlineData("home/sub")]
    public void ToUriPath_WithValue_ShouldReturnUriPathWithValue(string value)
    {
        // Arrange
        var path = UriAsciiPath.Parse(value, CultureInfo.InvariantCulture);

        // Act
        var uriPath = path.ToUriPath();

        // Assert
        Assert.Equal(UriPath.Parse(value, CultureInfo.InvariantCulture), uriPath);
    }

    [Theory]
    [InlineData("home", "/home/")]
    [InlineData("home/sub", "/home/sub/")]
    public void ToAbsolutePath_ShouldCorrectlyFormat(string value, string expected)
    {
        // Arrange
        var path = UriAsciiPath.Parse(value, CultureInfo.InvariantCulture);

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
        var path = UriAsciiPath.Parse(pathStr, CultureInfo.InvariantCulture);

        // Act
        var absolutePath = path.ToAbsolutePath();

        // Assert
        Assert.Equal($"/{pathStr}/", absolutePath);
    }

    [Fact]
    public void TryFormat_WithShortBuffer_ShouldReturnFalse()
    {
        // Arrange
        var path = UriAsciiPath.Parse("home", CultureInfo.InvariantCulture);
        Span<char> buffer = stackalloc char[3];

        // Act
        var success = path.TryFormat(buffer, out var charsWritten, default, default);

        // Assert
        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormatUtf8_WithShortBuffer_ShouldReturnFalse()
    {
        // Arrange
        var path = UriAsciiPath.Parse("home", CultureInfo.InvariantCulture);
        Span<byte> buffer = stackalloc byte[3];

        // Act
        var success = path.TryFormat(buffer, out var bytesWritten, default, default);

        // Assert
        Assert.False(success);
        Assert.Equal(0, bytesWritten);
    }

    [Fact]
    public void TryFormatUtf8_WithExactBuffer_ShouldWriteAsciiBytes()
    {
        // Arrange
        var path = UriAsciiPath.Parse("home", CultureInfo.InvariantCulture);
        Span<byte> buffer = stackalloc byte[4];

        // Act
        var success = path.TryFormat(buffer, out var bytesWritten, default, default);

        // Assert
        Assert.True(success);
        Assert.Equal(4, bytesWritten);
        Assert.True(buffer.SequenceEqual(Encoding.ASCII.GetBytes("home")));
    }

    [Fact]
    public void TryParseUtf8_WithNonAsciiBytes_ShouldReturnFalse()
    {
        byte[] bytes = [(byte)'h', (byte)'o', 0xC3, 0xBC];

        var success = UriAsciiPath.TryParse(bytes, CultureInfo.InvariantCulture, out var result);

        Assert.False(success);
        Assert.Equal(default, result);
    }

    [Fact]
    public void SerializeDeserialize()
    {
        const string value = "abcde";

        var path = UriAsciiPath.Parse(value, CultureInfo.InvariantCulture);

        var json = JsonSerializer.Serialize(path, JsonSharedOptions.RelaxedJsonEscaping);
        var result = JsonSerializer.Deserialize<UriAsciiPath>(json, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.Equal(path, result);
    }

    [Theory]
    [InlineData("\"/123/\"")]
    [InlineData("\"!abc!\"")]
    public void Deserialize_WithInvalidCode_Throws(string code)
    {
        _ = Assert.Throws<FormatException>(() => JsonSerializer.Deserialize<UriAsciiPath>(code, JsonSharedOptions.RelaxedJsonEscaping));
    }
}
