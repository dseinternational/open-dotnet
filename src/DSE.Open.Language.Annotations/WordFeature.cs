// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Language.Annotations.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// Features are additional pieces of information about the word, its part of speech and morphosyntactic
/// properties (<see href="https://universaldependencies.org/u/overview/morphology.html#features"/>).
/// Every feature has the form <c>Name=Value</c> and every word can have any number of features, separated by
/// the vertical bar, as in <c>Gender=Masc|Number=Sing</c>.
/// </summary>
/// <remarks>
/// Users can extend this set of universal features and add language-specific features when necessary.
/// Such features should be described in the language-specific documentation and follow the general principles
/// outlined here. Universal and language-specific features of a word are listed together in the FEATS column.
/// <list type="bullet">
/// <item>There are two types of identifiers:
///     <list type="bullet">
///         <item>feature names = features</item>
///         <item>feature values = values</item>
///     </list>
/// </item>
/// <item>All identifiers (both features and values) consist of English letters or, occasionally, digits 0-9.
/// The first letter is always uppercase. The other letters are generally lowercase, except for positions
/// where new internal words are marked for better readability (e.g. <c>NumType</c>). This makes features distinct
/// from the universal POS tags (all uppercase) and from the universal dependency relations (all lowercase).</item>
/// <item>A feature of a word should always be fully specified in the data, i.e. both the feature name and the value
/// should be identified: <c>PronType=Prs</c>. Note that the values are not guaranteed to be unique across features,
/// e.g. <c>Sup</c> could denote the superessive case, superlative degree of comparison or supine (a verb form).</item>
/// <item>Not mentioning a feature in the data implies the empty value, which means that the feature is either irrelevant
/// for this part of speech, or its value cannot be determined for this word form due to language-specific reasons.</item>
/// <item>It is possible to declare that a feature has two or more values for a given word: <c>Case=Acc,Dat</c>. The
/// interpretation is that the word may have one of these values but we cannot decide between them. Such multivalues
/// should be used sparingly. They should not be used if the value list would cover the whole value space, or the
/// subspace valid for the given language. That would mean that we cannot tell anything about this feature for the
/// given word, and then it is preferable to just leave the feature out.</item>
/// <item>Canonical ordering: features of one word (appearing on the same line) are always ordered alphabetically; if
/// a feature has multiple values, these are ordered alphabetically, too. This rule facilitates cases when it is
/// necessary to compare feature sets of two words. Alphabetical sorting means that uppercase letters are considered
/// identical to their lowercase counterparts. So for example, <c>Number</c> precedes <c>NumType</c></item>
/// </list>
/// </remarks>
[JsonConverter(typeof(JsonStringWordFeatureConverter))]
public sealed record WordFeature
    : ISpanFormattable,
      ISpanParsable<WordFeature>,
      ISpanSerializable<WordFeature>
{
    public static int MaxSerializedCharLength { get; } = (AlphaNumericCode.MaxSerializedCharLength * 2) + 1;

    private readonly ReadOnlyValueCollection<AlphaNumericCode> _values;

    public WordFeature(AlphaNumericCode name, IEnumerable<AlphaNumericCode> values)
        : this(name, [.. values])
    {
    }

    public WordFeature(AlphaNumericCode name, ReadOnlyValueCollection<AlphaNumericCode> values)
    {
        Guard.IsNotNull(values);

        Name = name;

        _values = values;

        if (_values.Count == 0)
        {
            ThrowHelper.ThrowArgumentException("Must have at least one value", nameof(values));
        }
    }

    /// <summary>
    /// Gets the name of the feature.
    /// </summary>
    public AlphaNumericCode Name { get; }

    /// <summary>
    /// Gets the value specified for the feature. If more than one value is specified, this only
    /// returns the first value.
    /// </summary>
    public AlphaNumericCode FirstValue => _values[0];

    /// <summary>
    /// Get the values specified for the feature.
    /// </summary>
    public ReadOnlyValueCollection<AlphaNumericCode> Values => _values;

    internal int GetCharCount()
    {
        return Name.Length + _values.Sum(v => v.Length + 1); // no -1 to accommodate = sign
    }

    public override string ToString()
    {
        return ToString(null, CultureInfo.InvariantCulture);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[MaxSerializedCharLength];
        _ = TryFormat(buffer, out var charsWritten, format, formatProvider);
        return buffer[..charsWritten].ToString();
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var charCount = GetCharCount();

        if (destination.Length >= charCount
            && Name.TryFormat(destination, out charsWritten, format, provider))
        {
            destination[charsWritten++] = '=';

            foreach (var value in _values)
            {
                if (!value.TryFormat(destination[charsWritten..], out var valueCharsWritten, format, provider))
                {
                    charsWritten = 0;
                    return false;
                }

                charsWritten += valueCharsWritten;

                if (charsWritten < charCount)
                {
                    destination[charsWritten++] = ',';
                }
            }

            return true;
        }

        charsWritten = 0;
        return false;
    }

    public static WordFeature Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static WordFeature Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var value))
        {
            return value;
        }

        return ThrowHelper.ThrowFormatException<WordFeature>($"Cannot parse '{s}' as {typeof(WordFeature).Name}.");
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        [MaybeNullWhen(false)] out WordFeature result)
    {
        return TryParse(s, CultureInfo.InvariantCulture, out result);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out WordFeature result)
    {
        if (s.Length >= 3)
        {
            var i = s.IndexOf('=');

            if (i > 0 && i <= s.Length - 2
                && AlphaNumericCode.TryParse(s[..i], provider, out var name))
            {
                var valuesSpan = s[(i + 1)..];

                var c = valuesSpan.IndexOf(',');

                if (c < 0)
                {
                    if (AlphaNumericCode.TryParse(valuesSpan, provider, out var value))
                    {
                        result = new(name, [value]);
                        return true;
                    }
                    else
                    {
                        result = default;
                        return false;
                    }
                }

                Span<Range> ranges = stackalloc Range[16];

                var l = valuesSpan.Split(ranges, ',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var values = new AlphaNumericCode[l];

                for (var r = 0; r < l; r++)
                {
                    var v = valuesSpan[ranges[r].Start.Value..ranges[r].End.Value];

                    if (AlphaNumericCode.TryParse(v, provider, out var value))
                    {
                        values[r] = value;
                    }
                    else
                    {
                        result = default;
                        return false;
                    }
                }

                result = new(name, values);
                return true;
            }
        }

        result = default;
        return false;
    }

    public static WordFeature Parse(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static WordFeature ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static WordFeature Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out WordFeature result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }
}
