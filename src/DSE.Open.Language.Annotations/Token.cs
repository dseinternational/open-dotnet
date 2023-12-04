// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// Carries data about a token in a sentence. Can be serialized to and from JSON
/// and CoNLL-U Format (<see href="https://universaldependencies.org/format.html"/>).
/// </summary>
public sealed record class Token
    : ISpanFormattable,
      ISpanParsable<Token>
{
    private const int IdIndex = 0;
    private const int FormIndex = 1;
    private const int LemmaIndex = 2;
    private const int PosIndex = 3;
    private const int AltPosIndex = 4;
    private const int FeaturesIndex = 5;
    private const int HeadIndexIndex = 6;
    private const int RelationIndex = 7;
    //private const int DepsIndex = 8;
    private const int MiscIndex = 9;


    /// <summary>
    /// Index of the token in the sentence.
    /// </summary>
    /// <remarks>Word index, integer starting at 1 for each new sentence; may be a range for
    /// multiword tokens; may be a decimal number for empty nodes (decimal numbers can be
    /// lower than 1 but must be greater than 0).</remarks>
    [JsonPropertyName("id")]
    public required TokenIndex Id { get; init; }

    [JsonPropertyName("form")]
    public required TokenText Form { get; init; }

    /// <summary>
    /// The canonical or base form of the word, which is the form typically found in dictionaries.
    /// </summary>
    [JsonPropertyName("lemma")]
    public TokenText? Lemma { get; init; }

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
    /// Universal dependency relation to the HEAD (root if HEAD = 0) or a defined language-specific
    /// subtype of one.
    /// </summary>
    [JsonPropertyName("deprel")]
    public UniversalRelationTag? Relation { get; init; }

    // TODO: DEPS: list of head-deprel pairs

    /// <summary>
    /// Gets or sets misc annotations/aatributes associated with the token.
    /// </summary>
    [JsonPropertyName("misc")]
    public ReadOnlyWordAttributeValueCollection Attributes { get; init; } = [];

    [JsonIgnore]
    public bool IsMultiwordToken => Id.IsMultiwordIndex;

    [JsonIgnore]
    public bool IsEmptyNode => Id.IsEmptyNode;

    public static Token Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    public static Token Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var token))
        {
            return token;
        }

        return ThrowHelper.ThrowFormatException<Token>(
            $"Failed to parse {s} as Token.");
    }

    public static Token Parse(string s)
    {
        return Parse(s, default);
    }

    public static Token Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Token result)
    {
        if (s.Length < 20)
        {
            goto Fail;
        }

        Span<Range> fields = stackalloc Range[10];

        var count = s.Split(fields, '\t', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (count != 10)
        {
            goto Fail;
        }

        if (!(TokenIndex.TryParse(s[fields[IdIndex]], provider, out var id)
            && TokenText.TryParse(s[fields[FormIndex]], provider, out var word)))
        {
            goto Fail;
        }

        TokenText? lemma;
        var lemmaSpan = s[fields[LemmaIndex]];

        if (lemmaSpan.Length == 1 && lemmaSpan[0] == '_')
        {
            lemma = null;
        }
        else
        {
            if (TokenText.TryParse(lemmaSpan, provider, out var lemmaValue))
            {
                lemma = lemmaValue;
            }
            else
            {
                goto Fail;
            }
        }

        UniversalPosTag? pos;
        var posSpan = s[fields[PosIndex]];

        if (posSpan.Length == 1 && posSpan[0] == '_')
        {
            pos = null;
        }
        else
        {
            if (UniversalPosTag.TryParse(posSpan, provider, out var posValue))
            {
                pos = posValue;
            }
            else
            {
                goto Fail;
            }
        }

        PosTag? xpos;
        var xposSpan = s[fields[AltPosIndex]];

        if (xposSpan.Length == 1 && xposSpan[0] == '_')
        {
            xpos = null;
        }
        else
        {
            if (PosTag.TryParse(xposSpan, provider, out var xposValue))
            {
                xpos = xposValue;
            }
            else
            {
                goto Fail;
            }
        }

        var featuresSpan = s[fields[FeaturesIndex]];
        ReadOnlyWordFeatureValueCollection features;

        if (featuresSpan.Length == 1 && featuresSpan[0] == '_')
        {
            features = [];
        }
        else
        {
            if (ReadOnlyWordFeatureValueCollection.TryParse(featuresSpan, provider, out var featuresValue))
            {
                features = featuresValue;
            }
            else
            {
                goto Fail;
            }
        }

        int? head;
        var headSpan = s[fields[HeadIndexIndex]];

        if (headSpan.Length == 1 && headSpan[0] == '_')
        {
            head = null;
        }
        else
        {
            if (int.TryParse(headSpan, provider, out var headValue))
            {
                head = headValue;
            }
            else
            {
                goto Fail;
            }
        }

        UniversalRelationTag? relation;
        var relationSpan = s[fields[RelationIndex]];

        if (relationSpan.Length == 1 && relationSpan[0] == '_')
        {
            relation = null;
        }
        else if (relationSpan.Length == 4
            && relationSpan[0] == 'R'
            && relationSpan[1] == 'O'
            && relationSpan[2] == 'O'
            && relationSpan[3] == 'T')
        {
            relation = null;
        }
        else
        {
            if (UniversalRelationTag.TryParse(relationSpan, provider, out var relationValue))
            {
                relation = relationValue;
            }
            else
            {
                goto Fail;
            }
        }

        // TODO: DEPS


        var miscSpan = s[fields[MiscIndex]];
        ReadOnlyWordAttributeValueCollection misc;

        if (miscSpan.Length == 1 && miscSpan[0] == '_')
        {
            misc = [];
        }
        else
        {
            if (ReadOnlyWordAttributeValueCollection.TryParse(miscSpan, provider, out var miscValue))
            {
                misc = miscValue;
            }
            else
            {
                goto Fail;
            }
        }

        result = new Token
        {
            Id = id,
            Form = word,
            Lemma = lemma,
            Pos = pos,
            AltPos = xpos,
            HeadIndex = head,
            Features = features,
            Relation = relation,
            Attributes = misc,
        };

        return true;

    Fail:
        result = default;
        return false;
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Token result)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return ToString(default, default);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        char[]? rented = null;

        Span<char> buffer = Features.Count + Attributes.Count > 2
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

        if (Attributes.Count > 0)
        {
            if (!Attributes.TryFormat(destination[charsWritten..], out var cwAnnotations, format, provider))
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
