// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

[Collection("StanzaService")]
public class DocumentTests : StanzaTestsBase
{
    public DocumentTests(StanzaServiceFixture stanzaFixture, ITestOutputHelper output) : base(stanzaFixture, output)
    {
    }

    [Fact]
    public void Text_ReturnsText()
    {
        var doc = PipelineEnglish.ProcessText("The cat jumped over the dog.");
        Assert.NotNull(doc);
        Assert.Equal("The cat jumped over the dog.", doc.Text);
    }

    [Fact]
    public void Sentences_ReturnsSentence()
    {
        var doc = PipelineEnglish.ProcessText("The cat jumped over the dog.");
        Assert.NotNull(doc);
        var sentences = doc.Sentences;
        Assert.NotNull(sentences);
        _ = Assert.Single(sentences);
    }
}
