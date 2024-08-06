// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Values.Text;

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
    [InlineData("a[[]", "a[")]
    [InlineData("[[]a*", "[abcde")]
    [InlineData(@"a\[abc\]", "a[abc]")]
    [InlineData(@"a\[\[\]\]", "a[[]]")]
    [InlineData("a[[]]", "a[]")] // The outer brackets mean the inner are treated as literals
    [InlineData(@"\\a", @"\a")]
    [InlineData(@"\*", "*")]
    [InlineData(@"\?", "?")]
    [InlineData(@"\[]", "[]")]
    [InlineData(@"\[\]", "[]")]
    [InlineData("a*?b", "abb")]
    [InlineData("[az]", "z")]
    [InlineData("[a-z]", "b")]
    [InlineData("[^a-c]", "d")]
    [InlineData("[a-c]", "b")]
    [InlineData("[0-9]", "1")]
    public void IsMatch_returns_true_for_matches(string pattern, string value)
    {
        Assert.True(new LikePattern(pattern).IsMatch(value, StringComparison.Ordinal));
    }

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
    [InlineData("a[[]", "abc[")]
    [InlineData("[[]a*", "a[abcde")]
    [InlineData(@"a\[abc\]", "aa")]
    [InlineData("a*?b", "ab")] // `?` requires exactly 1 character, this pattern requires 2 'b's
    [InlineData("a*?", "a")] // `?` requires exactly 1 character, this pattern requires 1 more character
    [InlineData("a?", "a")]
    [InlineData("[a-z]", "A")]
    [InlineData("[0-9]", "a")]
    public void IsNotMatch_returns_false_for_nonmatches(string pattern, string value)
    {
        Assert.False(new LikePattern(pattern).IsMatch(value, StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("*", "%")]
    [InlineData("?", "_")]
    [InlineData("a", "a")]
    [InlineData("a*", "a%")]
    [InlineData("a?", "a_")]
    [InlineData("abcd*", "abcd%")]
    [InlineData("a[abc]", "a[abc]")]
    [InlineData("a[[][abc]", "a[[][abc]")]
    [InlineData(@"a\[abc\]", "a[[]abc]")]
    [InlineData(@"\*", "*")]
    [InlineData(@"\?", "?")]
    public void ToSqlLikePattern_returns_expected_pattern(string pattern, string sqlLikePattern)
    {
        // Arrange
        var likePattern = new LikePattern(pattern);

        // Act
        var result = likePattern.ToSqlLikePattern();

        // Assert
        Assert.Equal(sqlLikePattern, result);
    }

    [Fact]
    public void Serializes_to_string_value()
    {
        var pattern = new LikePattern("[Pp]attern[s] %");
        var serialized = JsonSerializer.Serialize(pattern);
        Assert.Equal("\"[Pp]attern[s] %\"", serialized);
    }

    [Fact]
    public void Serialized_and_deserialized_values_are_equal()
    {
        var pattern = new LikePattern("[Pp]attern[s] %");
        var serialized = JsonSerializer.Serialize(pattern);
        var deserialized = JsonSerializer.Deserialize<LikePattern>(serialized);
        Assert.Equal(pattern, deserialized);
    }
}
