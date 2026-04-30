// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an <see cref="ICollection{T}"/> to and from an <see cref="IEnumerable{T}"/> of
/// <see cref="string"/> using <see cref="IFormattable"/> formatting and <see cref="IParsable{T}"/> parsing.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
public sealed class CollectionToFormattableStringCollectionConverter<T> : ValueConverter<ICollection<T>, IEnumerable<string>>
    where T : IParsable<T>, IFormattable
{
    /// <summary>
    /// A shared default instance using a <see langword="null"/> format and format provider.
    /// </summary>
    public static readonly CollectionToFormattableStringCollectionConverter<T> Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="CollectionToFormattableStringCollectionConverter{T}"/>
    /// with no explicit format or format provider.
    /// </summary>
    public CollectionToFormattableStringCollectionConverter() : this(null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CollectionToFormattableStringCollectionConverter{T}"/>
    /// with the specified format and format provider.
    /// </summary>
    /// <param name="format">The format string used when formatting elements, or <see langword="null"/>.</param>
    /// <param name="formatProvider">The format provider used for formatting and parsing, or <see langword="null"/>.</param>
    public CollectionToFormattableStringCollectionConverter(string? format, IFormatProvider? formatProvider)
        : base(v => ToCollection(v, format, formatProvider), v => FromCollection(v, formatProvider))
    {
        Format = format;
        FormatProvider = formatProvider;
    }

    /// <summary>
    /// Gets the format string used when formatting elements, if any.
    /// </summary>
    public string? Format { get; }

    /// <summary>
    /// Gets the format provider used for formatting and parsing elements, if any.
    /// </summary>
    public IFormatProvider? FormatProvider { get; }

    /// <summary>
    /// Formats each element of the supplied collection using the given format and format provider.
    /// </summary>
    /// <param name="set">The collection to format. Must not be <see langword="null"/>.</param>
    /// <param name="format">The format string passed to each element's <see cref="IFormattable.ToString(string?, IFormatProvider?)"/>.</param>
    /// <param name="formatProvider">The format provider passed to each element's <see cref="IFormattable.ToString(string?, IFormatProvider?)"/>.</param>
    /// <returns>The formatted strings.</returns>
    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static IEnumerable<string> ToCollection(ICollection<T> set, string? format, IFormatProvider? formatProvider)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        ArgumentNullException.ThrowIfNull(set);
        return set.Select(p => p.ToString(format, formatProvider) ?? string.Empty);
    }

    /// <summary>
    /// Parses each string in the supplied collection using <see cref="IParsable{T}.Parse(string, IFormatProvider?)"/>.
    /// </summary>
    /// <param name="collection">The collection of strings to parse. Must not be <see langword="null"/>.</param>
    /// <param name="formatProvider">The format provider passed to <see cref="IParsable{T}.Parse(string, IFormatProvider?)"/>.</param>
    /// <returns>The parsed values.</returns>
    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA1002 // Do not expose generic lists
    public static List<T> FromCollection(IEnumerable<string> collection, IFormatProvider? formatProvider)
#pragma warning restore CA1002 // Do not expose generic lists
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        ArgumentNullException.ThrowIfNull(collection);
        return [.. collection.Select(s => T.Parse(s, formatProvider))];
    }
}
