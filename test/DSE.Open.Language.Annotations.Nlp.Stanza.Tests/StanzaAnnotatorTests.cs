// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

[Collection("StanzaService")]
public class StanzaAnnotatorTests : StanzaTestsBase
{
    public StanzaAnnotatorTests(StanzaServiceFixture stanzaFixture, ITestOutputHelper output) : base(stanzaFixture, output)
    {
    }

    [Fact]
    public async Task CanAnnotateText()
    {
        var annotator = new StanzaAnnotator(Stanza);

        var doc = await annotator.AnnotateTextAsync(
            LanguageTag.EnglishUk,
            "Emma is eating her breakfast. She is sat at the table eating cereal.",
            TestContext.Current.CancellationToken);

        foreach (var sentence in doc.Sentences)
        {
            Output.WriteLine($"sentence {sentence.Index} ({sentence.Id})");

            foreach (var w in sentence.Tokens.SelectMany(t => t.Words))
            {
                Output.WriteLine($"{w.Index}\t{w.Form}\t{w.Lemma}\t{w.Pos}\t{w.AltPos?.ToString() ?? "_"}\t{w.Features?.ToString() ?? "_"}\t{w.Relation}\t_\t{w.Attributes}");
            }
        }
    }
}
