// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// A read-only keyed collection of <see cref="AttributeValue"/>, indexed by the attribute name.
/// </summary>
[CollectionBuilder(typeof(ReadOnlyAttributeValueCollection), nameof(Create))]
public sealed class ReadOnlyAttributeValueCollection
    : ReadOnlyKeyedValueCollection<AlphaNumericCode, AttributeValue>,
      ISpanParsable<ReadOnlyAttributeValueCollection>,
      ISpanFormattable
{
    /// <summary>
    /// An empty <see cref="ReadOnlyAttributeValueCollection"/>.
    /// </summary>
    public static readonly ReadOnlyAttributeValueCollection Empty = new();

    /// <summary>
    /// Initializes a new empty <see cref="ReadOnlyAttributeValueCollection"/>.
    /// </summary>
    public ReadOnlyAttributeValueCollection()
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ReadOnlyAttributeValueCollection"/> with the items in <paramref name="list"/>.
    /// </summary>
    public ReadOnlyAttributeValueCollection(IEnumerable<AttributeValue> list) : base(list)
    {
    }

    /// <inheritdoc/>
    protected override AlphaNumericCode GetKeyForItem(AttributeValue item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.Name;
    }

    /// <summary>
    /// Creates a new <see cref="ReadOnlyAttributeValueCollection"/> containing the specified items.
    /// </summary>
    public static ReadOnlyAttributeValueCollection Create(ReadOnlySpan<AttributeValue> items)
    {
        if (items.IsEmpty)
        {
            return Empty;
        }

        var list = new List<AttributeValue>(items.Length);

        list.AddRange(items);
        return new(list);
    }

    /// <summary>
    /// Creates a new <see cref="ReadOnlyAttributeValueCollection"/> containing the specified items.
    /// </summary>
    public static ReadOnlyAttributeValueCollection Create(Span<AttributeValue> items)
    {
        return Create((ReadOnlySpan<AttributeValue>)items);
    }
    /// <inheritdoc/>
    public override string ToString()
    {
        return AttributeValueSerializer.SerializeToString(this);
    }

    /// <summary>
    /// Parses a <see cref="ReadOnlyAttributeValueCollection"/> from the specified character span.
    /// </summary>
    public static ReadOnlyAttributeValueCollection Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static ReadOnlyAttributeValueCollection Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<ReadOnlyAttributeValueCollection>(
            $"Cannot parse '{s}' as {nameof(ReadOnlyAttributeValueCollection)}.");
    }

    /// <inheritdoc/>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyAttributeValueCollection result)
    {
        if (AttributeValueSerializer.TryDeserialize(s, out var features))
        {
            result = [.. features];
            return true;
        };

        result = default;
        return false;
    }

    /// <summary>
    /// Parses a <see cref="ReadOnlyAttributeValueCollection"/> from the specified string.
    /// </summary>
    public static ReadOnlyAttributeValueCollection Parse(string s)
    {
        return Parse(s, null);
    }

    /// <summary>
    /// Parses a <see cref="ReadOnlyAttributeValueCollection"/> from the specified string using the invariant culture.
    /// </summary>
    public static ReadOnlyAttributeValueCollection ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public static ReadOnlyAttributeValueCollection Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s, nameof(s));
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyAttributeValueCollection result)
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
        return AttributeValueSerializer.TrySerialize(destination, this, out charsWritten);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return AttributeValueSerializer.SerializeToString(this);
    }
}
