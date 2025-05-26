// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

[Collection("StanzaService")]
public class SentenceTests : StanzaTestsBase
{
    public SentenceTests(StanzaServiceFixture stanzaFixture, ITestOutputHelper output) : base(stanzaFixture, output)
    {
    }

    [Fact]
    public void Text_ReturnsText()
    {
        var doc = PipelineEnglish.ProcessText("The cat jumped over the dog.");
        Assert.NotNull(doc);
        var sentences = doc.Sentences;
        Assert.NotNull(sentences);
        _ = Assert.Single(sentences);
        Assert.Equal("The cat jumped over the dog.", sentences[0].Text);
    }

    [Fact]
    public void Tokens_ReturnsTokens()
    {
        var doc = PipelineEnglish.ProcessText("The cat jumped over the dog.");
        Assert.NotNull(doc);
        var sentences = doc.Sentences;
        Assert.NotNull(sentences);
        var sent = Assert.Single(sentences);
        var tokens = sent.Tokens;
        Assert.NotNull(tokens);
        Assert.Equal(7, tokens.Count); // "The", "cat", "jumped", "over", "the", "dog", "."
    }
}
