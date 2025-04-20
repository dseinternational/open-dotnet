// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

[CollectionBuilder(typeof(ReadOnlyWordFeatureValueCollection), nameof(Create))]
public sealed class ReadOnlyWordFeatureValueCollection
    : ReadOnlyKeyedValueCollection<AlphaNumericCode, WordFeature>,
      ISpanParsable<ReadOnlyWordFeatureValueCollection>,
      ISpanFormattable
{
    public static readonly ReadOnlyWordFeatureValueCollection Empty = new();

    public ReadOnlyWordFeatureValueCollection()
    {
    }

    public ReadOnlyWordFeatureValueCollection(IEnumerable<WordFeature> list) : base(list)
    {
    }

    protected override AlphaNumericCode GetKeyForItem(WordFeature item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.Name;
    }

    public static ReadOnlyWordFeatureValueCollection Create(ReadOnlySpan<WordFeature> items)
    {
        if (items.IsEmpty)
        {
            return Empty;
        }

        var list = new List<WordFeature>(items.Length);

        list.AddRange(items);

#pragma warning disable IDE0306 // Simplify collection initialization
        return new(list);
#pragma warning restore IDE0306 // Simplify collection initialization
    }

    public static ReadOnlyWordFeatureValueCollection Create(Span<WordFeature> items)
    {
        return Create((ReadOnlySpan<WordFeature>)items);
    }
    public override string ToString()
    {
        return WordFeatureSerializer.SerializeToString(this);
    }

    public static ReadOnlyWordFeatureValueCollection Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    public static ReadOnlyWordFeatureValueCollection Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<ReadOnlyWordFeatureValueCollection>(
            $"Cannot parse '{s}' as {nameof(ReadOnlyWordFeatureValueCollection)}.");
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyWordFeatureValueCollection result)
    {
        if (WordFeatureSerializer.TryDeserialize(s, out var features))
        {
            result = [.. features];
            return true;
        }

        result = default;
        return false;
    }

    public static ReadOnlyWordFeatureValueCollection Parse(string s)
    {
        return Parse(s, null);
    }

    public static ReadOnlyWordFeatureValueCollection ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static ReadOnlyWordFeatureValueCollection Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s, nameof(s));
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyWordFeatureValueCollection result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        return WordFeatureSerializer.TrySerialize(destination, this, out charsWritten);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return WordFeatureSerializer.SerializeToString(this);
    }
}
