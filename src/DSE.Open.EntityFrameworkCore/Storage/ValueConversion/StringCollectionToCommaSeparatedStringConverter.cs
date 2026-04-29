// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="ICollection{T}"/> of <see cref="string"/> to and from a
/// comma-separated string.
/// </summary>
public sealed class StringCollectionToCommaSeparatedStringConverter : ValueConverter<ICollection<string>, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly StringCollectionToCommaSeparatedStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="StringCollectionToCommaSeparatedStringConverter"/>.
    /// </summary>
    public StringCollectionToCommaSeparatedStringConverter()
        : base(v => ConvertToString(v), v => ConvertToCollection(v))
    {
    }

    /// <summary>
    /// Joins the supplied collection of strings with a comma separator.
    /// </summary>
    /// <param name="values">The collection of strings. Must not be <see langword="null"/>.</param>
    /// <returns>The comma-separated string.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(ICollection<string> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        return string.Join(',', values);
    }

    /// <summary>
    /// Splits a comma-separated string back to a collection of strings, removing empty entries
    /// and trimming whitespace.
    /// </summary>
    /// <param name="value">The comma-separated string. Must not be <see langword="null"/>.</param>
    /// <returns>The split collection.</returns>
    // keep public for EF Core compiled models
    public static ICollection<string> ConvertToCollection(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return [.. value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
    }
}
