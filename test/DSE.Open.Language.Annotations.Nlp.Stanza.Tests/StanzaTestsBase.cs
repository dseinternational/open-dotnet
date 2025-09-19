// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public abstract class StanzaTestsBase
{
    protected StanzaTestsBase(StanzaServiceFixture stanzaFixture, ITestOutputHelper output)
    {
        StanzaFixture = stanzaFixture;
        Output = output;
    }

    public StanzaServiceFixture StanzaFixture { get; }

    public StanzaService Stanza => StanzaFixture.Stanza;

    public Pipeline PipelineEnglish => StanzaFixture.PipelineEnglish;

    public ITestOutputHelper Output { get; }
}
