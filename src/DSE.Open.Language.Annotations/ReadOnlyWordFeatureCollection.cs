// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Language.Annotations.Serialization;

namespace DSE.Open.Language.Annotations;

[JsonConverter(typeof(JsonStringReadOnlyWordFeatureCollectionConverter))]
[CollectionBuilder(typeof(ReadOnlyWordFeatureCollection), nameof(Create))]
public sealed class ReadOnlyWordFeatureCollection
    : IReadOnlyList<WordFeature>,
      ISpanParsable<ReadOnlyWordFeatureCollection>,
      ISpanFormattable
{
    public static readonly ReadOnlyWordFeatureCollection Empty = new();

    private readonly WordFeatureCollection _features;

    private ReadOnlyWordFeatureCollection()
    {
        _features = [];
    }

    public ReadOnlyWordFeatureCollection(IEnumerable<WordFeature> features)
    {
        ArgumentNullException.ThrowIfNull(features);
        _features = [.. features];
    }

    public static ReadOnlyWordFeatureCollection Create(ReadOnlySpan<WordFeature> items)
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

    public static ReadOnlyWordFeatureCollection Create(Span<WordFeature> items)
    {
        return Create((ReadOnlySpan<WordFeature>)items);
    }

    public int Count => _features.Count;

    public WordFeature this[int index] => _features[index];

    public IEnumerator<WordFeature> GetEnumerator()
    {
        return _features.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _features.GetEnumerator();
    }

    public override string ToString()
    {
        return WordFeatureSerializer.SerializeToString(this);
    }

    public static ReadOnlyWordFeatureCollection Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    public static ReadOnlyWordFeatureCollection Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<ReadOnlyWordFeatureCollection>(
            $"Cannot parse '{s}' as {nameof(ReadOnlyWordFeatureCollection)}.");
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyWordFeatureCollection result)
    {
        if (WordFeatureSerializer.TryDeserialize(s, out var features))
        {
            result = [.. features];
            return true;
        }

        result = default;
        return false;
    }

    public static ReadOnlyWordFeatureCollection Parse(string s)
    {
        return Parse(s, null);
    }

    public static ReadOnlyWordFeatureCollection ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static ReadOnlyWordFeatureCollection Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s, nameof(s));
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyWordFeatureCollection result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        return WordFeatureSerializer.TrySerialize(destination, _features, out charsWritten);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return WordFeatureSerializer.SerializeToString(_features);
    }
}
