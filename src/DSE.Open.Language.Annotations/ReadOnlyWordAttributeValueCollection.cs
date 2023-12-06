// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

[CollectionBuilder(typeof(ReadOnlyWordAttributeValueCollection), nameof(Create))]
public class ReadOnlyWordAttributeValueCollection
    : ReadOnlyKeyedValueCollection<AlphaNumericCode, WordAttribute>,
      ISpanParsable<ReadOnlyWordAttributeValueCollection>,
      ISpanFormattable
{
    public static readonly ReadOnlyWordAttributeValueCollection Empty = new();

    public ReadOnlyWordAttributeValueCollection()
    {
    }

    public ReadOnlyWordAttributeValueCollection(IEnumerable<WordAttribute> list) : base(list)
    {
    }

    protected override AlphaNumericCode GetKeyForItem(WordAttribute item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.Name;
    }

    public static ReadOnlyWordAttributeValueCollection Create(ReadOnlySpan<WordAttribute> items)
    {
        if (items.IsEmpty)
        {
            return Empty;
        }

        var list = new List<WordAttribute>(items.Length);

        list.AddRange(items);

        return new ReadOnlyWordAttributeValueCollection(list);
    }

    public static ReadOnlyWordAttributeValueCollection Create(Span<WordAttribute> items)
    {
        return Create((ReadOnlySpan<WordAttribute>)items);
    }
    public override string ToString()
    {
        return WordAttributeSerializer.SerializeToString(this);
    }

    public static ReadOnlyWordAttributeValueCollection Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    public static ReadOnlyWordAttributeValueCollection Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<ReadOnlyWordAttributeValueCollection>(
            $"Cannot parse '{s}' as {typeof(ReadOnlyWordAttributeValueCollection).Name}.");
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyWordAttributeValueCollection result)
    {
        if (WordAttributeSerializer.TryDeserialize(s, out var features))
        {
            result = new ReadOnlyWordAttributeValueCollection(features);
            return true;
        };

        result = default;
        return false;
    }

    public static ReadOnlyWordAttributeValueCollection Parse(string s)
    {
        return Parse(s, null);
    }

    public static ReadOnlyWordAttributeValueCollection ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static ReadOnlyWordAttributeValueCollection Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s, nameof(s));
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ReadOnlyWordAttributeValueCollection result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        return WordAttributeSerializer.TrySerialize(destination, this, out charsWritten);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return WordAttributeSerializer.SerializeToString(this);
    }
}
