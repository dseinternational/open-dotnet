// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Language;

public class TokenTextTests
{
    [Theory]
    [InlineData("the")]
    [InlineData("don't")]
    [InlineData("ice-cream")]
    [InlineData(",")]
    [InlineData(".")]
    public void SerializeDeserialize(string value)
    {
        var token = TokenText.Parse(value, CultureInfo.InvariantCulture);
        AssertJson.Roundtrip(token);
    }

    [Fact]
    public void Length_ReflectsValue()
    {
        var token = (TokenText)"hello";
        Assert.Equal(5, token.Length);
    }

    [Fact]
    public void Equals_TokenText_IgnoresCaseWhenRequested()
    {
        var lower = (TokenText)"hello";
        var upper = (TokenText)"HELLO";

        Assert.False(lower.Equals(upper, StringComparison.Ordinal));
        Assert.True(lower.Equals(upper, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Equals_String_IgnoresCaseWhenRequested()
    {
        var token = (TokenText)"hello";

        Assert.True(token.Equals("hello", StringComparison.Ordinal));
        Assert.False(token.Equals("HELLO", StringComparison.Ordinal));
        Assert.True(token.Equals("HELLO", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Equals_WordText_ComparesUnderlyingValue()
    {
        var token = (TokenText)"cat";
        var word = (WordText)"cat";

        Assert.True(token.Equals(word, StringComparison.Ordinal));
    }

    [Fact]
    public void ImplicitConversion_FromWordText_PreservesValue()
    {
        var word = (WordText)"hello";
        TokenText token = word;

        Assert.True(token.Equals("hello", StringComparison.Ordinal));
    }

    [Fact]
    public void IsValidValue_RejectsEmpty()
    {
        Assert.False(TokenText.IsValidValue(default));
    }
}
