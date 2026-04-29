// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using OpenDocument = DSE.Open.Language.Annotations.Document;
using OpenSentence = DSE.Open.Language.Annotations.Sentence;
using OpenToken = DSE.Open.Language.Annotations.Token;
using OpenWord = DSE.Open.Language.Annotations.Word;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

/// <summary>
/// An <see cref="IAnnotator"/> implementation that uses a Stanza pipeline to annotate text.
/// </summary>
public class StanzaAnnotator : IAnnotator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StanzaAnnotator"/> class.
    /// </summary>
    /// <param name="stanza">The <see cref="StanzaService"/> used to create pipelines.</param>
    public StanzaAnnotator(StanzaService stanza)
    {
        ArgumentNullException.ThrowIfNull(stanza);

        Context = stanza;
    }

    /// <summary>
    /// Gets the <see cref="StanzaService"/> used by this annotator.
    /// </summary>
    public StanzaService Context { get; }

    /// <inheritdoc/>
    public Task<OpenDocument> AnnotateTextAsync(
        LanguageTag language,
        string text,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(text);
        cancellationToken.ThrowIfCancellationRequested();

        var model = language.GetLanguagePart().ToStringLower();

        using var nlp = Context.CreatePipeline(model);

        using var doc = nlp.ProcessText(text);

        OpenDocument result = new()
        {
            Sentences = [.. doc.Sentences.Select(s => new OpenSentence
            {
                Id = s.Id,
                Language = language,
                Text = s.Text,
                Tokens = [.. s.Tokens.Select(t => new OpenToken
                {
                    Text = (TokenText)t.Text,
                    Words = [.. t.Words.Select(MapWord)]
                })],
                Comments = [.. s.Comments]
            })]
        };

        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(result);
    }

    private static OpenWord MapWord(Word w)
    {
        if (!TokenText.TryParse(w.Text, out var word))
        {
            throw new StanzaException($"Failed to parse text '{w.Text}'");
        }

        TokenText? lemma = null;

        if (!string.IsNullOrWhiteSpace(w.Lemma) && w.Lemma != "_")
        {
            lemma = !TokenText.TryParse(w.Lemma, out var lemmaValue)
                ? throw new StanzaException($"Failed to parse lemma '{w.Lemma}'")
                : lemmaValue;
        }

        if (!UniversalPosTag.TryParse(w.Pos, out var pos))
        {
            throw new StanzaException($"Failed to parse UPOS '{w.Pos}'");
        }

        PosTag? altPos = null;

        if (!string.IsNullOrWhiteSpace(w.AltPos) && w.AltPos != "_")
        {
            if (!PosTag.TryParse(w.AltPos, out var altPosValue))
            {
                throw new StanzaException($"Failed to parse XPOS '{w.AltPos}'");
            }

            altPos = altPosValue;
        }

        var featuresSource = w.Features == "_" ? null : w.Features;

        if (!ReadOnlyWordFeatureValueCollection.TryParse(featuresSource, default, out var features))
        {
            throw new StanzaException($"Failed to parse features '{w.Features}'");
        }

        if (!UniversalRelationTag.TryParse(w.Relation, out var relation))
        {
            throw new StanzaException($"Failed to parse relation '{w.Relation}'");
        }

        var attributesSource = w.Attributes == "_" ? null : w.Attributes;

        if (!ReadOnlyAttributeValueCollection.TryParse(attributesSource, default, out var attributes))
        {
            throw new StanzaException($"Failed to parse attributes '{w.Attributes}'");
        }

        return new()
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
