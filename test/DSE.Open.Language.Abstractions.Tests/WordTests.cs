// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language.Tests;

public class WordTests
{
    [Theory]
    [InlineData("television")]
    [InlineData("ball")]
    [InlineData("bat")]
    [InlineData("a")]
    public void SerializeDeserialize(string signValue)
    {
        var sign = Word.Parse(signValue, CultureInfo.InvariantCulture);
        var json = JsonSerializer.Serialize(sign);
        var deserialized = JsonSerializer.Deserialize<Word>(json);
        Assert.Equal(sign, deserialized);
    }

    [Theory]
    [InlineData("television")]
    [InlineData("ball")]
    [InlineData("bat")]
    [InlineData("Emma's")]
    [InlineData("ice-cream")]
    [InlineData("ice cream")]
    [InlineData("teachers'")]
    [InlineData("{{ child_name }}")]
    [InlineData("{{child_name}}")]
    public void ParseSucceedsIfValid(string signValue)
    {
        _ = Word.Parse(signValue, CultureInfo.InvariantCulture);
    }

    [Theory]
    [InlineData("-television")]
    [InlineData("television ")]
    [InlineData(" television ")]
    [InlineData(" television")]
    [InlineData("'ball")]
    [InlineData("b:at")]
    [InlineData("")]
    [InlineData("{{  child_name  }}")]
    [InlineData("{{CHILD_NAME}}")]
    public void ParseFailsIfInvalid(string signValue)
    {
        Assert.Throws<FormatException>(() => Word.Parse(signValue, CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData("{{ child_name }}")]
    [InlineData("{{child_name}}")]
    public void TemplatesAreIdentifiedAsTemplates(string signValue)
    {
        var w = Word.Parse(signValue, CultureInfo.InvariantCulture);
        Assert.True(w.IsTemplate);
    }

    [Theory]
    [InlineData("television")]
    [InlineData("ball")]
    [InlineData("bat")]
    [InlineData("Emma's")]
    [InlineData("ice-cream")]
    [InlineData("ice cream")]
    [InlineData("teachers'")]
    public void NonTemplatesAreNotIdentifiedAsTemplates(string signValue)
    {
        var w = Word.Parse(signValue, CultureInfo.InvariantCulture);
        Assert.False(w.IsTemplate);
    }

}
