// Copyright (c) Down Syndrome Education Enterprises CIC. All Rights Reserved.
// Information contained herein is PROPRIETARY AND CONFIDENTIAL.

using DSE.Open.Globalization;
using DSE.Open.Testing.Xunit;
using DSE.Open.Testing.Xunit.Stanza;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

[Collection(nameof(StanzaContextCollection))]
public class StanzaAnnotatorTests : LoggedTestsBase
{
    public StanzaAnnotatorTests(StanzaContextFixture fixture, ITestOutputHelper output) : base(output)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        PythonContext = fixture.PythonContext;
        StanzaContext = fixture.StanzaContext;
    }

    public PythonContext PythonContext { get; }

    public StanzaContext StanzaContext { get; }

    [StanzaNlpFact("en")]
    public async Task CanAnnotateText()
    {
        var annotator = new StanzaAnnotator(StanzaContext);

        var doc = await annotator.AnnotateTextAsync(LanguageTag.EnglishUk, "Emma is eating her breakfast. She is sat at the table eating cereal.");

        foreach (var sentence in doc.Sentences)
        {
            Output.WriteLine($"sentence {sentence.Index} ({sentence.Id})");

            foreach (var w in sentence.Tokens.SelectMany(t => t.Words))
            {
                Output.WriteLine($"{w.Index}\t{w.Form}\t{w.Lemma}\t{w.Pos}\t{w.AltPos?.ToString() ?? "_"}\t{w.Features?.ToString() ?? "_"}\t{w.Relation}\t_\t{w.Attributes?.ToString() ?? "_"}");
            }
        }
    }
}
