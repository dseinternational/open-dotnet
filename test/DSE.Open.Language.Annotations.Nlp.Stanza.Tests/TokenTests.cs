// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

[Collection("StanzaService")]
public class TokenTests : StanzaTestsBase
{
    public TokenTests(StanzaServiceFixture stanzaFixture, ITestOutputHelper output) : base(stanzaFixture, output)
    {
    }

    [Fact]
    public void Words_ReturnsWords()
    {
        var doc = PipelineEnglish.ProcessText("The cat jumped over the dog.");
        Assert.NotNull(doc);
        var sentences = doc.Sentences;
        Assert.NotNull(sentences);
        var sent = Assert.Single(sentences);
        var tokens = sent.Tokens;
        Assert.NotNull(tokens);
        var words = tokens[0].Words;
        Assert.NotNull(words);
        _ = Assert.Single(words); // "The"
    }
}
