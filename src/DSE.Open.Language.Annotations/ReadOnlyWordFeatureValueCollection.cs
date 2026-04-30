// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// A read-only keyed collection of <see cref="WordFeature"/>, indexed by feature name.
/// </summary>
[CollectionBuilder(typeof(ReadOnlyWordFeatureValueCollection), nameof(Create))]
public sealed class ReadOnlyWordFeatureValueCollection
    : ReadOnlyKeyedValueCollection<AlphaNumericCode, WordFeature>,
      ISpanParsable<ReadOnlyWordFeatureValueCollection>,
      ISpanFormattable
{
    /// <summary>
    /// An empty <see cref="ReadOnlyWordFeatureValueCollection"/>.
    /// </summary>
    public static readonly ReadOnlyWordFeatureValueCollection Empty = new();

    /// <summary>
    /// Initializes a new empty <see cref="ReadOnlyWordFeatureValueCollection"/>.
    /// </summary>
    public ReadOnlyWordFeatureValueCollection()
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlyWordFeatureValueCollection"/> with the items in <paramref name="list"/>.
    /// </summary>
    public ReadOnlyWordFeatureValueCollection(IEnumerable<WordFeature> list) : base(list)
    {
    }

    /// <inheritdoc/>
    protected override AlphaNumericCode GetKeyForItem(WordFeature item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.Name;
    }

    /// <summary>
    /// Creates a new <see cref="ReadOnlyWordFeatureValueCollection"/> containing the specified items.
    /// </summary>
    public static ReadOnlyWordFeatureValueCollection Create(ReadOnlySpan<WordFeature> items)
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
    /// Creates a new <see cref="ReadOnlyWordFeatureValueCollection"/> containing the specified items.
    /// </summary>
    public static ReadOnlyWordFeatureValueCollection Create(Span<WordFeature> items)
    {
        return Create((ReadOnlySpan<WordFeature>)items);
    }
    /// <inheritdoc/>
    public override string ToString()
    {
        return WordFeatureSerializer.SerializeToString(this);
    }

    /// <summary>
    /// Parses a <see cref="ReadOnlyWordFeatureValueCollection"/> from the specified character span.
    /// </summary>
    public static ReadOnlyWordFeatureValueCollection Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static ReadOnlyWordFeatureValueCollection Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<ReadOnlyWordFeatureValueCollection>(
            $"Cannot parse '{s}' as {nameof(ReadOnlyWordFeatureValueCollection)}.");
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Parses a <see cref="ReadOnlyWordFeatureValueCollection"/> from the specified string.
    /// </summary>
    public static ReadOnlyWordFeatureValueCollection Parse(string s)
    {
        return Parse(s, null);
    }

    /// <summary>
    /// Parses a <see cref="ReadOnlyWordFeatureValueCollection"/> from the specified string using the invariant culture.
    /// </summary>
    public static ReadOnlyWordFeatureValueCollection ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public static ReadOnlyWordFeatureValueCollection Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s, nameof(s));
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyWordFeatureValueCollection result)
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
        return WordFeatureSerializer.TrySerialize(destination, this, out charsWritten);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return WordFeatureSerializer.SerializeToString(this);
    }
}
