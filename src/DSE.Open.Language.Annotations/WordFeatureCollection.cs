// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Language.Annotations.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// A mutable collection of <see cref="WordFeature"/> values, indexed by feature name.
/// Each feature name may appear at most once in the collection.
/// </summary>
[JsonConverter(typeof(JsonStringWordFeatureCollectionConverter))]
[CollectionBuilder(typeof(WordFeatureCollection), nameof(Create))]
public sealed class WordFeatureCollection
    : IList<WordFeature>,
      ISpanParsable<WordFeatureCollection>,
      ISpanFormattable
{
    private readonly List<WordFeature> _features;

    /// <summary>
    /// Initializes a new, empty <see cref="WordFeatureCollection"/>.
    /// </summary>
    public WordFeatureCollection()
    {
        _features = [];
    }

    /// <summary>
    /// Initializes a new <see cref="WordFeatureCollection"/> containing the specified features.
    /// </summary>
    public WordFeatureCollection(IEnumerable<WordFeature> features)
    {
        ArgumentNullException.ThrowIfNull(features);

        _features = [.. features];
    }

    /// <summary>
    /// Creates a new <see cref="WordFeatureCollection"/> containing the specified items.
    /// </summary>
    public static WordFeatureCollection Create(ReadOnlySpan<WordFeature> items)
    {
        var list = new List<WordFeature>(items.Length);

        list.AddRange(items);
        return new(list);
    }

    /// <summary>
    /// Creates a new <see cref="WordFeatureCollection"/> containing the specified items.
    /// </summary>
    public static WordFeatureCollection Create(Span<WordFeature> items)
    {
        return Create((ReadOnlySpan<WordFeature>)items);
    }

    /// <inheritdoc/>
    public int Count => _features.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => ((ICollection<WordFeature>)_features).IsReadOnly;

    /// <inheritdoc/>
    public WordFeature this[int index]
    {
        get => _features[index];
        set => _features[index] = value;
    }

    /// <inheritdoc/>
    public void Add(WordFeature item)
    {
        ArgumentNullException.ThrowIfNull(item);
        EnsureDoesNotContainName(item.Name);
        _features.Add(item);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _features.Clear();
    }

    /// <summary>
    /// Returns <see langword="true"/> if the collection contains a feature with the specified name.
    /// </summary>
    public bool ContainsName(AlphaNumericCode name)
    {
        return _features.Any(f => f.Name == name);
    }

    private void EnsureDoesNotContainName(AlphaNumericCode name)
    {
        if (ContainsName(name))
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"{nameof(WordFeatureCollection)} already contains a feature named {name}.");
        }
    }

    /// <inheritdoc/>
    public bool Contains(WordFeature item)
    {
        return _features.Contains(item);
    }

    /// <inheritdoc/>
    public int IndexOf(WordFeature item)
    {
        return _features.IndexOf(item);
    }

    /// <inheritdoc/>
    public void Insert(int index, WordFeature item)
    {
        ArgumentNullException.ThrowIfNull(item);
        EnsureDoesNotContainName(item.Name);
        _features.Insert(index, item);
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        _features.RemoveAt(index);
    }

    /// <inheritdoc/>
    public void CopyTo(WordFeature[] array, int arrayIndex)
    {
        _features.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns a read-only snapshot of this collection.
    /// </summary>
    public ReadOnlyWordFeatureCollection AsReadOnly()
    {
        return [.. this];
    }

    /// <inheritdoc/>
    public IEnumerator<WordFeature> GetEnumerator()
    {
        return _features.GetEnumerator();
    }

    /// <inheritdoc/>
    public bool Remove(WordFeature item)
    {
        return _features.Remove(item);
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
    /// Parses a <see cref="WordFeatureCollection"/> from the specified character span.
    /// </summary>
    public static WordFeatureCollection Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static WordFeatureCollection Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<WordFeatureCollection>(
            $"Cannot parse '{s}' as {nameof(WordFeatureCollection)}.");
    }

    /// <inheritdoc/>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out WordFeatureCollection result)
    {
        if (WordFeatureSerializer.TryDeserialize(s, out var features))
        {
            result = [.. features];
            return true;
        };

        result = default;
        return false;
    }

    /// <summary>
    /// Parses a <see cref="WordFeatureCollection"/> from the specified string.
    /// </summary>
    public static WordFeatureCollection Parse(string s)
    {
        return Parse(s, null);
    }

    /// <summary>
    /// Parses a <see cref="WordFeatureCollection"/> from the specified string using the invariant culture.
    /// </summary>
    public static WordFeatureCollection ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public static WordFeatureCollection Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s, nameof(s));
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out WordFeatureCollection result)
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
