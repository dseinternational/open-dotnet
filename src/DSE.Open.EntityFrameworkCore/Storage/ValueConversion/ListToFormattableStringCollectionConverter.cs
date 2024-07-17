// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class ListToFormattableStringCollectionConverter<T> : ValueConverter<IList<T>, IEnumerable<string>>
    where T : IParsable<T>, IFormattable
{
    public static readonly ListToFormattableStringCollectionConverter<T> Default = new();

    public ListToFormattableStringCollectionConverter() : this(null, null)
    {
    }

    public ListToFormattableStringCollectionConverter(string? format, IFormatProvider? formatProvider)
        : base(v => ToCollection(v, format, formatProvider), v => FromCollection(v, formatProvider))
    {
        Format = format;
        FormatProvider = formatProvider;
    }

    public string? Format { get; }

    public IFormatProvider? FormatProvider { get; }

    private static IEnumerable<string> ToCollection(IList<T> set, string? format, IFormatProvider? formatProvider)
    {
        ArgumentNullException.ThrowIfNull(set);
        return set.Select(p => p.ToString(format, formatProvider) ?? string.Empty);
    }

    private static List<T> FromCollection(IEnumerable<string> collection, IFormatProvider? formatProvider)
    {
        ArgumentNullException.ThrowIfNull(collection);
        return collection.Select(s => T.Parse(s, formatProvider)).ToList();
    }
}
