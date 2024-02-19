// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Language.Annotations.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

[JsonConverter(typeof(JsonStringWordFeatureCollectionConverter))]
[CollectionBuilder(typeof(WordFeatureCollection), nameof(Create))]
public sealed class WordFeatureCollection
    : IList<WordFeature>,
      ISpanParsable<WordFeatureCollection>,
      ISpanFormattable
{
    private readonly List<WordFeature> _features;

    public WordFeatureCollection()
    {
        _features = [];
    }

    public WordFeatureCollection(IEnumerable<WordFeature> features)
    {
        Guard.IsNotNull(features);

        _features = new List<WordFeature>(features);
    }

    public static WordFeatureCollection Create(ReadOnlySpan<WordFeature> items)
    {
        var list = new List<WordFeature>(items.Length);

        list.AddRange(items);

        return new WordFeatureCollection(list);
    }

    public static WordFeatureCollection Create(Span<WordFeature> items)
    {
        return Create((ReadOnlySpan<WordFeature>)items);
    }

    public int Count => _features.Count;

    public bool IsReadOnly => ((ICollection<WordFeature>)_features).IsReadOnly;

    public WordFeature this[int index]
    {
        get => _features[index];
        set => _features[index] = value;
    }

    public void Add(WordFeature item)
    {
        Guard.IsNotNull(item);
        EnsureDoesNotContainName(item.Name);
        _features.Add(item);
    }

    public void Clear()
    {
        _features.Clear();
    }

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

    public bool Contains(WordFeature item)
    {
        return _features.Contains(item);
    }

    public int IndexOf(WordFeature item)
    {
        return _features.IndexOf(item);
    }

    public void Insert(int index, WordFeature item)
    {
        _features.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _features.RemoveAt(index);
    }

    public void CopyTo(WordFeature[] array, int arrayIndex)
    {
        _features.CopyTo(array, arrayIndex);
    }

    public ReadOnlyWordFeatureCollection AsReadOnly()
    {
        return new ReadOnlyWordFeatureCollection(this);
    }

    public IEnumerator<WordFeature> GetEnumerator()
    {
        return _features.GetEnumerator();
    }

    public bool Remove(WordFeature item)
    {
        return _features.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _features.GetEnumerator();
    }

    public override string ToString()
    {
        return WordFeatureSerializer.SerializeToString(this);
    }

    public static WordFeatureCollection Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    public static WordFeatureCollection Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<WordFeatureCollection>(
            $"Cannot parse '{s}' as {typeof(WordFeatureCollection).Name}.");
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out WordFeatureCollection result)
    {
        if (WordFeatureSerializer.TryDeserialize(s, out var features))
        {
            result = new WordFeatureCollection(features);
            return true;
        };

        result = default;
        return false;
    }

    public static WordFeatureCollection Parse(string s)
    {
        return Parse(s, null);
    }

    public static WordFeatureCollection ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static WordFeatureCollection Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s, nameof(s));
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out WordFeatureCollection result)
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
