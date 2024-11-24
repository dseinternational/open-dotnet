// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// An annotation of a syntactic word.
/// </summary>
/// <remarks>
/// Out token/word model is based on the CoNLL-U format and Stanza NLP
/// data objects (<see href="https://stanfordnlp.github.io/stanza/data_objects.html#token"/>.
/// </remarks>
public record Word : ISpanFormattable, ISpanParsable<Word>, IRepeatableHash64
{
    /// <summary>
    /// Index of the token in the sentence.
    /// </summary>
    /// <remarks>Word index, integer starting at 1 for each new sentence.</remarks>
    [JsonPropertyName("id")]
    public required int Index { get; init; }

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
    public required UniversalPosTag Pos { get; init; }

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
    /// Head of the current word, which is either a value of <see cref="Index"/> or zero (0).
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
    /// Gets or sets misc annotations/attributes associated with the token.
    /// </summary>
    [JsonPropertyName("misc")]
    public ReadOnlyAttributeValueCollection Attributes { get; init; } = [];

    internal int GetCharCount()
    {
        var features = Features.Sum(f => f.GetCharCount() + 1);
        var attributes = Attributes.Sum(a => a.GetCharCount() + 1);

        var count = 9 // tabs
            + Form.Length
            + (Lemma?.Length ?? 1)
            + Pos.Length
            + (AltPos?.Length ?? 1)
            + features
            + 3 // at most
            + (Relation?.Length ?? 1)
            + attributes;

        return count;
    }

    public static Word Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    public static Word Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var token))
        {
            return token;
        }

        return ThrowHelper.ThrowFormatException<Word>(
            $"Failed to parse {s} as Token.");
    }

    public static Word Parse(string s)
    {
        return Parse(s, default);
    }

    public static Word Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    [SkipLocalsInit]
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Word result)
    {
        if (s.Length < 20)
        {
            return Fail(out result);
        }

        Span<Range> fields = stackalloc Range[10];

        var count = s.Split(fields, '\t', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (count != 10)
        {
            return Fail(out result);
        }

        if (!(int.TryParse(s[fields[ConlluFieldIndex.Id]], provider, out var index)
            && TokenText.TryParse(s[fields[ConlluFieldIndex.Form]], provider, out var word)))
        {
            return Fail(out result);
        }

        TokenText? lemma;
        var lemmaSpan = s[fields[ConlluFieldIndex.Lemma]];

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
                return Fail(out result);
            }
        }

        UniversalPosTag? pos;
        var posSpan = s[fields[ConlluFieldIndex.Pos]];

        if (UniversalPosTag.TryParse(posSpan, provider, out var posValue))
        {
            pos = posValue;
        }
        else
        {
            return Fail(out result);
        }

        PosTag? xpos;
        var xposSpan = s[fields[ConlluFieldIndex.AltPos]];

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
                return Fail(out result);
            }
        }

        var featuresSpan = s[fields[ConlluFieldIndex.Features]];
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
                return Fail(out result);
            }
        }

        int? head;
        var headSpan = s[fields[ConlluFieldIndex.Head]];

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
                return Fail(out result);
            }
        }

        UniversalRelationTag? relation;
        var relationSpan = s[fields[ConlluFieldIndex.Relation]];

        if (relationSpan.Length == 1 && relationSpan[0] == '_')
        {
            relation = null;
        }
        else if (relationSpan.Length == 4
            && (relationSpan[0] == 'R' || relationSpan[0] == 'r')
            && (relationSpan[1] == 'O' || relationSpan[1] == 'o')
            && (relationSpan[2] == 'O' || relationSpan[2] == 'o')
            && (relationSpan[3] == 'T' || relationSpan[3] == 't'))
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
                return Fail(out result);
            }
        }

        // TODO: DEPS

        var miscSpan = s[fields[ConlluFieldIndex.Misc]];
        ReadOnlyAttributeValueCollection misc;

        if (miscSpan.Length == 1 && miscSpan[0] == '_')
        {
            misc = [];
        }
        else
        {
            if (ReadOnlyAttributeValueCollection.TryParse(miscSpan, provider, out var miscValue))
            {
                misc = miscValue;
            }
            else
            {
                return Fail(out result);
            }
        }

        result = new()
        {
            Index = index,
            Form = word,
            Lemma = lemma,
            Pos = (UniversalPosTag)pos,
            AltPos = xpos,
            HeadIndex = head,
            Features = features,
            Relation = relation,
            Attributes = misc,
        };

        return true;

        static bool Fail(out Word? result)
        {
            result = default;
            return false;
        }
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Word result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public override string ToString()
    {
        return ToString(default, default);
    }

    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var charCount = GetCharCount();

        char[]? rented = null;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(charCount)
            ? stackalloc char[charCount]
            : (rented = ArrayPool<char>.Shared.Rent(charCount));

        try
        {
            if (TryFormat(buffer, out var charsWritten, format, formatProvider))
            {
                return buffer[..charsWritten].ToString();
            }
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }

        ThrowHelper.ThrowFormatException($"Failed to format {nameof(Word)} to string.");
        return null!; // unreachable
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        // ID

        if (!Index.TryFormat(destination, out charsWritten, format, provider))
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

        if (!Pos.Value.TryFormat(destination[charsWritten..], out var cwUpos, format, provider))
        {
            return false;
        }

        charsWritten += cwUpos;

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

    public ulong GetRepeatableHashCode()
    {
        var hash = RepeatableHash64Provider.Default.CombineHashCodes(
            RepeatableHash64Provider.Default.GetRepeatableHashCode(Index),
            RepeatableHash64Provider.Default.GetRepeatableHashCode(Form),
            Lemma?.GetRepeatableHashCode() ?? 0u,
            Pos.GetRepeatableHashCode(),
            AltPos?.GetRepeatableHashCode() ?? 0u,
            HeadIndex?.GetRepeatableHashCode() ?? 0u,
            Relation?.GetRepeatableHashCode() ?? 0u);

        foreach (var feature in Features)
        {
            hash = RepeatableHash64Provider.Default.CombineHashCodes(hash, feature.GetRepeatableHashCode());
        }

        foreach (var attribute in Attributes)
        {
            hash = RepeatableHash64Provider.Default.CombineHashCodes(hash, attribute.GetRepeatableHashCode());
        }

        return hash;
    }
}
