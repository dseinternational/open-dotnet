// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

[Collection("StanzaService")]
public class PipelineTests : StanzaTestsBase
{
    public PipelineTests(StanzaServiceFixture stanzaFixture, ITestOutputHelper output) : base(stanzaFixture, output)
    {
    }

    [Fact]
    public void GetLoadedProcessorsDescriptions_ReturnsDescriptions()
    {
        var processors = PipelineEnglish.GetLoadedProcessorsDescriptions();
        Assert.NotEmpty(processors);
    }

    [Fact]
    public void ProcessText()
    {
        var doc = PipelineEnglish.ProcessText("The cat jumped over the dog.");
        Assert.NotNull(doc);
    }
}
