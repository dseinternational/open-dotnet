// Copyright (c) Down Syndrome Education Enterprises CIC. All Rights Reserved.
// Information contained herein is PROPRIETARY AND CONFIDENTIAL.

using DSE.Open.Testing.Xunit;
using DSE.Open.Testing.Xunit.Stanza;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

[Collection(nameof(StanzaContextCollection))]
public class PythonContextTests : LoggedTestsBase
{
    public PythonContextTests(StanzaContextFixture fixture, ITestOutputHelper output) : base(output)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        PythonContext = fixture.PythonContext;
        StanzaContext = fixture.StanzaContext;
    }

    public PythonContext PythonContext { get; }

    public StanzaContext StanzaContext { get; }

    [StanzaNlpFact("en")]
    public async Task CanCreatePipelineAndProcessText()
    {
        using (var nlp = StanzaContext.CreatePipeline("en"))
        {
            string[] sentences =
            [
                "Emma is eating ice cream.",
                "She likes ice cream.",
                "The dog is climbing the tree.",
            ];

            var doc = await nlp.ProcessTextAsync(sentences);

            foreach (var e in doc.Entities)
            {
                Output.WriteLine($"{e.Start,-4} {e.End,-4} {e.Text}");
            }

            foreach (var sentence in doc.Sentences)
            {
                Output.WriteLine($"sentence {sentence.Index} ({sentence.Id})");

                foreach (var c in sentence.Comments)
                {
                    Output.WriteLine(c);
                }

                foreach (var t in sentence.Tokens)
                {
                    Output.WriteLine($"{t.Id}\t{t.Text}\t{t.Attributes}\t{t.Start}\t{t.End}");
                }

                foreach (var w in sentence.Words)
                {
                    Output.WriteLine(w.ToString());
                }
            }

            Assert.True(doc.Sentences.Count == 3);
        }
    }
}
