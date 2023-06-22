// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Values.Text;

namespace DSE.Open.Values.Tests.Text;

public class LikePatternTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("a", "a")]
    [InlineData("a*", "a")]
    [InlineData("a*", "abcde")]
    [InlineData("abcd*", "abcde")]
    [InlineData("abcde*", "abcde")]
    [InlineData("a[abc]", "aa")]
    [InlineData("a[abc]", "ab")]
    [InlineData("a[abc]", "ac")]
    [InlineData("a[abc][abc]", "aca")]
    public void IsMatch_returns_true_for_matches(string pattern, string value)
        => Assert.True(new LikePattern(pattern).IsMatch(value, StringComparison.Ordinal));

    [Theory]
    [InlineData("", " ")]
    [InlineData("a", "")]
    [InlineData("a*", "b")]
    [InlineData("a*", "ba")]
    [InlineData("abcd*", "abcf")]
    [InlineData("abcde*", "abcd")]
    [InlineData("a[abc]", "ad")]
    [InlineData("a[abc]", "adb")]
    [InlineData("a[abc]", "ca")]
    [InlineData("a[abc][abc]", "acd")]
    public void IsNotMatch_returns_false_for_nonmatches(string pattern, string value)
        => Assert.False(new LikePattern(pattern).IsMatch(value, StringComparison.Ordinal));

    [Theory]
    [InlineData("", "")]
    [InlineData("*", "%")]
    [InlineData("?", "_")]
    [InlineData("a", "a")]
    [InlineData("a*", "a%")]
    [InlineData("a?", "a_")]
    [InlineData("abcd*", "abcd%")]
    [InlineData("a[abc]", "a[abc]")]
    public void ToSqlLikePattern_returns_expected_pattern(string pattern, string sqlLikePattern)
        => Assert.Equal(new LikePattern(pattern).ToSqlLikePattern(), sqlLikePattern);

    [Fact]
    public void Serializes_to_string_value()
    {
        var pattern = new LikePattern("[Pp]attern[s] %");
        var seralized = JsonSerializer.Serialize(pattern);
        Assert.Equal("\"[Pp]attern[s] %\"", seralized);
    }

    [Fact]
    public void Serialized_and_deserialized_values_are_equal()
    {
        var pattern = new LikePattern("[Pp]attern[s] %");
        var seralized = JsonSerializer.Serialize(pattern);
        var deserialized = JsonSerializer.Deserialize<LikePattern>(seralized);
        Assert.Equal(pattern, deserialized);
    }
}

