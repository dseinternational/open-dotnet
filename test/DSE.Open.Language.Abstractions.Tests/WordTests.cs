// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Language.Abstractions.Tests;

public class WordTests
{
    [Theory]
    [InlineData("television")]
    [InlineData("ball")]
    [InlineData("bat")]
    [InlineData("a")]
    public void SerializeDeserialize(string wordValue)
    {
        var word = WordText.Parse(wordValue, CultureInfo.InvariantCulture);
        AssertJson.Roundtrip(word);
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
    public void ParseSucceedsIfValid(string wordValue)
    {
        _ = WordText.Parse(wordValue, CultureInfo.InvariantCulture);
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
    public void ParseFailsIfInvalid(string wordValue)
    {
        _ = Assert.Throws<FormatException>(() => WordText.Parse(wordValue, CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData("{{ child_name }}")]
    [InlineData("{{child_name}}")]
    public void TemplatesAreIdentifiedAsTemplates(string wordValue)
    {
        var w = WordText.Parse(wordValue, CultureInfo.InvariantCulture);
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
    public void NonTemplatesAreNotIdentifiedAsTemplates(string wordValue)
    {
        var w = WordText.Parse(wordValue, CultureInfo.InvariantCulture);
        Assert.False(w.IsTemplate);
    }
}
