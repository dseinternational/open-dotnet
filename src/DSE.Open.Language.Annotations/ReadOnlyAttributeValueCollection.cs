// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

[CollectionBuilder(typeof(ReadOnlyAttributeValueCollection), nameof(Create))]
public sealed class ReadOnlyAttributeValueCollection
    : ReadOnlyKeyedValueCollection<AlphaNumericCode, Attribute>,
      ISpanParsable<ReadOnlyAttributeValueCollection>,
      ISpanFormattable
{
    public static readonly ReadOnlyAttributeValueCollection Empty = new();

    public ReadOnlyAttributeValueCollection()
    {
    }

    public ReadOnlyAttributeValueCollection(IEnumerable<Attribute> list) : base(list)
    {
    }

    protected override AlphaNumericCode GetKeyForItem(Attribute item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.Name;
    }

    public static ReadOnlyAttributeValueCollection Create(ReadOnlySpan<Attribute> items)
    {
        if (items.IsEmpty)
        {
            return Empty;
        }

        var list = new List<Attribute>(items.Length);

        list.AddRange(items);

        return new ReadOnlyAttributeValueCollection(list);
    }

    public static ReadOnlyAttributeValueCollection Create(Span<Attribute> items)
    {
        return Create((ReadOnlySpan<Attribute>)items);
    }
    public override string ToString()
    {
        return AttributeSerializer.SerializeToString(this);
    }

    public static ReadOnlyAttributeValueCollection Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    public static ReadOnlyAttributeValueCollection Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<ReadOnlyAttributeValueCollection>(
            $"Cannot parse '{s}' as {typeof(ReadOnlyAttributeValueCollection).Name}.");
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyAttributeValueCollection result)
    {
        if (AttributeSerializer.TryDeserialize(s, out var features))
        {
            result = new ReadOnlyAttributeValueCollection(features);
            return true;
        };

        result = default;
        return false;
    }

    public static ReadOnlyAttributeValueCollection Parse(string s)
    {
        return Parse(s, null);
    }

    public static ReadOnlyAttributeValueCollection ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static ReadOnlyAttributeValueCollection Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s, nameof(s));
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyAttributeValueCollection result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        return AttributeSerializer.TrySerialize(destination, this, out charsWritten);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return AttributeSerializer.SerializeToString(this);
    }
}
