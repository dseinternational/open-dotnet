// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Language.Annotations.Serialization;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// A read-only list of <see cref="WordFeature"/>.
/// </summary>
[JsonConverter(typeof(JsonStringReadOnlyWordFeatureCollectionConverter))]
[CollectionBuilder(typeof(ReadOnlyWordFeatureCollection), nameof(Create))]
public sealed class ReadOnlyWordFeatureCollection
    : IReadOnlyList<WordFeature>,
      ISpanParsable<ReadOnlyWordFeatureCollection>,
      ISpanFormattable
{
    /// <summary>
    /// An empty <see cref="ReadOnlyWordFeatureCollection"/>.
    /// </summary>
    public static readonly ReadOnlyWordFeatureCollection Empty = new();

    private readonly WordFeatureCollection _features;

    private ReadOnlyWordFeatureCollection()
    {
        _features = [];
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlyWordFeatureCollection"/> with the specified features.
    /// </summary>
    public ReadOnlyWordFeatureCollection(IEnumerable<WordFeature> features)
    {
        ArgumentNullException.ThrowIfNull(features);
        _features = [.. features];
    }

    /// <summary>
    /// Creates a new <see cref="ReadOnlyWordFeatureCollection"/> containing the specified items.
    /// </summary>
    public static ReadOnlyWordFeatureCollection Create(ReadOnlySpan<WordFeature> items)
    {
        if (items.IsEmpty)
        {
            return Empty;
        }

        var list = new List<WordFeature>(items.Length);

        list.AddRange(items);
        return new(list);
    }

    /// <summary>
    /// Creates a new <see cref="ReadOnlyWordFeatureCollection"/> containing the specified items.
    /// </summary>
    public static ReadOnlyWordFeatureCollection Create(Span<WordFeature> items)
    {
        return Create((ReadOnlySpan<WordFeature>)items);
    }

    /// <inheritdoc/>
    public int Count => _features.Count;

    /// <inheritdoc/>
    public WordFeature this[int index] => _features[index];

    /// <inheritdoc/>
    public IEnumerator<WordFeature> GetEnumerator()
    {
        return _features.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _features.GetEnumerator();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return WordFeatureSerializer.SerializeToString(this);
    }

    /// <summary>
    /// Parses a <see cref="ReadOnlyWordFeatureCollection"/> from the specified character span.
    /// </summary>
    public static ReadOnlyWordFeatureCollection Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static ReadOnlyWordFeatureCollection Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<ReadOnlyWordFeatureCollection>(
            $"Cannot parse '{s}' as {nameof(ReadOnlyWordFeatureCollection)}.");
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Parses a <see cref="ReadOnlyWordFeatureCollection"/> from the specified string.
    /// </summary>
    public static ReadOnlyWordFeatureCollection Parse(string s)
    {
        return Parse(s, null);
    }

    /// <summary>
    /// Parses a <see cref="ReadOnlyWordFeatureCollection"/> from the specified string using the invariant culture.
    /// </summary>
    public static ReadOnlyWordFeatureCollection ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public static ReadOnlyWordFeatureCollection Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s, nameof(s));
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyWordFeatureCollection result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        return WordFeatureSerializer.TrySerialize(destination, _features, out charsWritten);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return WordFeatureSerializer.SerializeToString(_features);
    }
}
