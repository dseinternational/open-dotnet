// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using OpenDocument = DSE.Open.Language.Annotations.Document;
using OpenSentence = DSE.Open.Language.Annotations.Sentence;
using OpenToken = DSE.Open.Language.Annotations.Token;
using OpenWord = DSE.Open.Language.Annotations.Word;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class StanzaAnnotator : IAnnotator
{
    public StanzaAnnotator(StanzaContext stanzaContext)
    {
        Context = stanzaContext;
    }

    public StanzaContext Context { get; }

    public async Task<OpenDocument> AnnotateTextAsync(
        LanguageTag language,
        string text,
        CancellationToken cancellationToken = default)
    {
        var model = language.GetLanguagePart().ToStringLower();

        using var nlp = Context.CreatePipeline(model);

        var stanzaDoc = await nlp.ProcessTextAsync(text, cancellationToken).ConfigureAwait(false);

        return new OpenDocument
        {
            Sentences = [.. stanzaDoc.Sentences.Select(s => new OpenSentence
            {
                Id = s.Id,
                Language = language,
                Text = s.Text,
                Tokens = [.. s.Tokens.Select(t => new OpenToken
                {
                    Index = new TokenIndex(t.Id.Item1, t.Id.Item2),
                    Text = (TokenText)t.Text,
                    Words = [..t.Words.Select(MapWord)]
                })],
                Comments = [..s.Comments]
            })]
        };

    }

    private static OpenWord MapWord(Word w)
    {
        if (!TokenText.TryParse(w.Text, out var word))
        {
            throw new StanzaException($"Failed to parse text '{w.Text}'");
        }

        var lemma = !TokenText.TryParse(w.Lemma, out var lemmaValue)
            ? throw new StanzaException($"Failed to parse lemma '{w.Lemma}'")
            : (TokenText?)lemmaValue;

        if (!UniversalPosTag.TryParse(w.Pos, out var pos))
        {
            throw new StanzaException($"Failed to parse UPOS '{w.Pos}'");
        }

        if (!PosTag.TryParse(w.AltPos, out var altPos))
        {
            throw new StanzaException($"Failed to parse XPOS '{w.AltPos}'");
        }

        if (!ReadOnlyWordFeatureValueCollection.TryParse(w.Features, default, out var features))
        {
            throw new StanzaException($"Failed to parse features '{w.Features}'");
        }

        if (!UniversalRelationTag.TryParse(w.Relation, out var relation))
        {
            throw new StanzaException($"Failed to parse relation '{w.Relation}'");
        }

        if (!ReadOnlyAttributeValueCollection.TryParse(w.Attributes, default, out var attributes))
        {
            throw new StanzaException($"Failed to parse features '{w.Features}'");
        }

        return new OpenWord
        {
            Index = w.Index,
            Form = word,
            Lemma = lemma,
            Pos = pos,
            AltPos = altPos,
            Features = features,
            HeadIndex = w.Head,
            Relation = relation,
            Attributes = attributes
        };
    }
}
