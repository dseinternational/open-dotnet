// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language;

public sealed class TokenTextTests
{
    [Fact]
    public void Constructor_from_string_preserves_characters()
    {
        var token = new TokenText("running");
        Assert.Equal(7, token.Length);
        Assert.Equal("running", token.ToStringInvariant());
    }

    [Fact]
    public void Constructor_from_memory_preserves_characters()
    {
        ReadOnlyMemory<char> memory = "running".AsMemory();
        var token = new TokenText(memory);
        Assert.Equal("running", token.ToStringInvariant());
    }

    [Fact]
    public void Equals_token_text_is_ordinal_by_default()
    {
        var lower = new TokenText("running");
        var upper = new TokenText("RUNNING");
        Assert.False(lower.Equals(upper, StringComparison.Ordinal));
        Assert.True(lower.Equals(upper, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Equals_word_text_compares_underlying_value()
    {
        var token = new TokenText("running");
        var word = new WordText("running");
        Assert.True(token.Equals(word, StringComparison.Ordinal));
    }

    [Fact]
    public void Equals_char_sequence_compares_underlying_value()
    {
        var token = new TokenText("running");
        var sequence = (CharSequence)"running";
        Assert.True(token.Equals(sequence, StringComparison.Ordinal));
        Assert.True(token == sequence);
        Assert.False(token != sequence);
    }

    [Fact]
    public void Equals_string_honours_comparison()
    {
        var token = new TokenText("Running");
        Assert.True(token.Equals("Running", StringComparison.Ordinal));
        Assert.False(token.Equals("running", StringComparison.Ordinal));
        Assert.True(token.Equals("running", StringComparison.OrdinalIgnoreCase));
        Assert.True(token == "Running");
        Assert.False(token != "Running");
    }

    [Fact]
    public void Operator_equals_with_null_string_returns_false()
    {
        var token = new TokenText("running");
        Assert.False(token == (string)null!);
        Assert.True(token != (string)null!);
    }

    [Fact]
    public void IsValidValue_rejects_empty_and_over_long_values()
    {
        Assert.False(TokenText.IsValidValue(default));
        Assert.False(TokenText.IsValidValue(new CharSequence(new string('a', 33))));
        Assert.True(TokenText.IsValidValue(new CharSequence(new string('a', 32))));
    }

    [Fact]
    public void Implicit_conversion_from_word_text_preserves_characters()
    {
        var word = new WordText("running");
        TokenText token = word;
        Assert.Equal("running", token.ToStringInvariant());
    }

    [Fact]
    public void Repeatable_hash_is_stable_for_equal_values()
    {
        var a = new TokenText("running");
        var b = new TokenText("running");
        Assert.Equal(a.GetRepeatableHashCode(), b.GetRepeatableHashCode());
    }
}
