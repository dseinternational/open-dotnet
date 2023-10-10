// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class CollectionToFormattableStringCollectionConverter<T> : ValueConverter<ICollection<T>, IEnumerable<string>>
    where T : IParsable<T>, IFormattable
{
    public static readonly CollectionToFormattableStringCollectionConverter<T> Default = new();

    public CollectionToFormattableStringCollectionConverter() : this(null, null)
    {
    }

    public CollectionToFormattableStringCollectionConverter(string? format, IFormatProvider? formatProvider)
        : base(v => ToCollection(v, format, formatProvider), v => FromCollection(v, formatProvider))
    {
        Format = format;
        FormatProvider = formatProvider;
    }

    public string? Format { get; }

    public IFormatProvider? FormatProvider { get; }

    private static IEnumerable<string> ToCollection(ICollection<T> set, string? format, IFormatProvider? formatProvider)
    {
        Guard.IsNotNull(set);
        return set.Select(p => p.ToString(format, formatProvider) ?? string.Empty);
    }

    private static List<T> FromCollection(IEnumerable<string> collection, IFormatProvider? formatProvider)
    {
        Guard.IsNotNull(collection);
        return collection.Select(s => T.Parse(s, formatProvider)).ToList();
    }
}
