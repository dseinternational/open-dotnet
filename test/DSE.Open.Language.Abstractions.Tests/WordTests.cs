// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language.Tests;

public class WordTests
{
    [Theory]
    [InlineData("television")]
    [InlineData( "ball")]
    [InlineData( "bat")]
    [InlineData( "a")]
    public void SerializeDeserialize(string signValue)
    {
        var sign = Word.Parse(signValue);
        var json = JsonSerializer.Serialize(sign);
        var deserialized = JsonSerializer.Deserialize<Word>(json);
        Assert.Equal(sign, deserialized);
    }

    [Theory]
    [InlineData("television")]
    [InlineData( "ball")]
    [InlineData( "bat")]
    [InlineData( "Emma's")]
    [InlineData( "ice-cream")]
    [InlineData( "teachers'")]
    public void ParseSucceedsIfValid(string signValue)
    {
        _ = Word.Parse(signValue);
    }

    [Theory]
    [InlineData("-television")]
    [InlineData( "'ball")]
    [InlineData( "b:at")]
    [InlineData( "")]
    public void ParseFailsIfInvalid(string signValue)
    {
        Assert.Throws<FormatException>(() => Word.Parse(signValue));
    }
}
