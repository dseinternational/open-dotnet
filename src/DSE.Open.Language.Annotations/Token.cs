// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// Carries data about a token in a sentence. Can be serialized to and from JSON
/// and CoNLL-U Format (<see href="https://universaldependencies.org/format.html"/>).
/// </summary>
public sealed record class Token : ISpanFormattable
{
    /// <summary>
    /// Index of the token in the sentence.
    /// </summary>
    /// <remarks>Word index, integer starting at 1 for each new sentence; may be a range for
    /// multiword tokens; may be a decimal number for empty nodes (decimal numbers can be
    /// lower than 1 but must be greater than 0).</remarks>
    [JsonPropertyName("id")]
    public required TokenIndex Id { get; init; }

    [JsonPropertyName("form")]
    public required Word Form { get; init; }

    /// <summary>
    /// The canonical or base form of the word, which is the form typically found in dictionaries.
    /// </summary>
    [JsonPropertyName("lemma")]
    public Word? Lemma { get; init; }

    /// <summary>
    /// The universal POS tag (<see href="https://universaldependencies.org/u/pos/index.html"/>).
    /// </summary>
    [JsonPropertyName("upos")]
    public UniversalPosTag? Pos { get; init; }

    /// <summary>
    /// An optional additional POS tag. May be a <see cref="TreebankPosTag"/>
    /// (<see href="https://www.ling.upenn.edu/courses/Fall_2003/ling001/penn_treebank_pos.html"/>)
    /// </summary>
    [JsonPropertyName("xpos")]
    public PosTag? AltPos { get; init; }

    /// <summary>
    /// List of morphological features from the universal feature inventory or from a defined
    /// language-specific extension.
    /// </summary>
    [JsonPropertyName("feats")]
    public ReadOnlyWordFeatureValueCollection Features { get; init; } = [];

    /// <summary>
    /// Head of the current word, which is either a value of <see cref="Id"/> or zero (0).
    /// </summary>
    [JsonPropertyName("head")]
    public int? HeadIndex { get; init; }

    /// <summary>
    /// Universal dependency relation to the HEAD (root iff HEAD = 0) or a defined language-specific
    /// subtype of one.
    /// </summary>
    [JsonPropertyName("deprel")]
    public UniversalRelationTag? Relation { get; init; }

    // TODO: DEPS: list of head-deprel pairs

    /// <summary>
    /// Gets or sets other annotations.
    /// </summary>
    [JsonPropertyName("misc")]
    public ReadOnlyWordFeatureValueCollection Annotations { get; init; } = [];

    [JsonIgnore]
    public bool IsMultiwordToken => Id.IsMultiwordIndex;

    [JsonIgnore]
    public bool IsEmptyNode => Id.IsEmptyNode;

    public override string ToString()
    {
        return ToString(default, default);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        char[]? rented = null;

        Span<char> buffer = Features.Count + Annotations.Count > 2
            ? (rented = ArrayPool<char>.Shared.Rent(512))
            : stackalloc char[128];

        if (TryFormat(buffer, out var charsWritten, format, formatProvider))
        {
            return buffer[..charsWritten].ToString();
        }

        ThrowHelper.ThrowFormatException("Failed to format Token to string.");
        return null!; // unreachable
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        // ID

        if (!Id.TryFormat(destination, out charsWritten, format, provider))
        {
            return false;
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '\t';
        }
        else
        {
            return false;
        }

        // FORM

        if (!Form.TryFormat(destination[charsWritten..], out var cwForm, format, provider))
        {
            return false;
        }

        charsWritten += cwForm;

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '\t';
        }
        else
        {
            return false;
        }

        // LEMMA

        if (Lemma is not null)
        {
            if (!Lemma.Value.TryFormat(destination[charsWritten..], out var cwLemma, format, provider))
            {
                return false;
            }

            charsWritten += cwLemma;
        }
        else
        {
            if (destination.Length > charsWritten)
            {
                destination[charsWritten++] = '_';
            }
            else
            {
                return false;
            }
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '\t';
        }
        else
        {
            return false;
        }

        // UPOS

        if (Pos is not null)
        {
            if (!Pos.Value.TryFormat(destination[charsWritten..], out var cwUpos, format, provider))
            {
                return false;
            }

            charsWritten += cwUpos;
        }
        else
        {
            if (destination.Length > charsWritten)
            {
                destination[charsWritten++] = '_';
            }
            else
            {
                return false;
            }
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '\t';
        }
        else
        {
            return false;
        }

        // XPOS

        if (AltPos is not null)
        {
            if (!AltPos.Value.TryFormat(destination[charsWritten..], out var cwAltPos, format, provider))
            {
                return false;
            }

            charsWritten += cwAltPos;
        }
        else
        {
            if (destination.Length > charsWritten)
            {
                destination[charsWritten++] = '_';
            }
            else
            {
                return false;
            }
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '\t';
        }
        else
        {
            return false;
        }

        // FEATS

        if (Features.Count > 0)
        {
            if (!Features.TryFormat(destination[charsWritten..], out var cwFeats, format, provider))
            {
                return false;
            }

            charsWritten += cwFeats;
        }
        else
        {
            if (destination.Length > charsWritten)
            {
                destination[charsWritten++] = '_';
            }
            else
            {
                return false;
            }
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '\t';
        }
        else
        {
            return false;
        }

        // HEAD

        if (HeadIndex is not null)
        {
            if (!HeadIndex.Value.TryFormat(destination[charsWritten..], out var cwHead, format, provider))
            {
                return false;
            }

            charsWritten += cwHead;
        }
        else
        {
            if (destination.Length > charsWritten)
            {
                destination[charsWritten++] = '_';
            }
            else
            {
                return false;
            }
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '\t';
        }
        else
        {
            return false;
        }

        // DEPREL

        if (Relation is not null)
        {
            if (!Relation.Value.TryFormat(destination[charsWritten..], out var cwRelation, format, provider))
            {
                return false;
            }

            charsWritten += cwRelation;
        }
        else
        {
            if (destination.Length > charsWritten)
            {
                destination[charsWritten++] = '_';
            }
            else
            {
                return false;
            }
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '\t';
        }
        else
        {
            return false;
        }

        // DEPS (TODO)

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '_';
        }
        else
        {
            return false;
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '\t';
        }
        else
        {
            return false;
        }

        // MISC

        if (Annotations.Count > 0)
        {
            if (!Annotations.TryFormat(destination[charsWritten..], out var cwAnnotations, format, provider))
            {
                return false;
            }

            charsWritten += cwAnnotations;
        }
        else
        {
            if (destination.Length > charsWritten)
            {
                destination[charsWritten++] = '_';
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}
